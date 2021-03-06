//#define USE_CALLBACK 1 //图像抓取方式宏定义，打开则采用回调函数方式，关闭，则使用多线程主动抓取方式。



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using MVSDK;//使用MindVision .net SDK接口
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.IO;
using System.Drawing.Imaging;
using Protein_Crystallization;
using Meteroi;

namespace Basic
{
    

    public partial class BasicForm : Form
    {
        
        public Detector parent_window;
        public string save_path = "";
        #region variable
        protected CameraHandle m_hCamera = 0;             // 句柄
        protected IntPtr       m_ImageBuffer;             // 预览通道RGB图像缓存
        protected IntPtr       m_ImageBufferSnapshot;     // 抓拍通道RGB图像缓存
        protected tSdkCameraCapbility tCameraCapability;  // 相机特性描述
        protected int          m_iDisplayedFrames = 0;    //已经显示的总帧数
        protected IntPtr       m_iCaptureCallbackCtx;     //图像回调函数的上下文参数
        protected Thread       m_tCaptureThread;          //图像抓取线程
        protected bool         m_bExitCaptureThread = false;//采用线程采集时，让线程退出的标志
        protected IntPtr       m_iSettingPageMsgCallbackCtx; //相机配置界面消息回调函数的上下文参数   
        protected tSdkFrameHead m_tFrameHead;
        protected bool          m_bEraseBk = false;
        #endregion

        public BasicForm()
        {
            InitializeComponent();

            //初始化SDK
            MvApi.CameraSdkInit(1);//1:SDK中使用中文进行提示和创建相机配置窗口  0：英文
            // 检测是否有连接相机，如果已经连接，则直接初始化相机并开始预览
            if (InitCamera() == true)
            {
                MvApi.CameraPlay(m_hCamera);
            }

        }
       

#if USE_CALL_BACK
        public void ImageCaptureCallback(CameraHandle hCamera, uint pFrameBuffer, ref tSdkFrameHead pFrameHead, uint pContext)
        {
            //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
            MvApi.CameraImageProcess(hCamera, pFrameBuffer, (IntPtr)((int)m_ImageBuffer&(~0xf)), ref pFrameHead);
            //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。   
            MvApi.CameraImageOverlay(hCamera, (IntPtr)((int)m_ImageBuffer & (~0xf)), ref pFrameHead);
            //调用SDK封装好的接口，显示预览图像
            MvApi.CameraDisplayRGB24(hCamera, (IntPtr)((int)m_ImageBuffer & (~0xf)), ref pFrameHead);
            m_tFrameHead = pFrameHead;
            m_iDisplayedFrames++;

            if (pFrameHead.iWidth != m_tFrameHead.iWidth || pFrameHead.iHeight != m_tFrameHead.iHeight)
            {
                timer2.Enabled = true;
                timer2.Start();
                m_tFrameHead = pFrameHead;
            }
            
        }
#else
        public void CaptureThreadProc()
        {
            CameraSdkStatus eStatus;
            tSdkFrameHead FrameHead;
            uint uRawBuffer;//rawbuffer由SDK内部申请。应用层不要调用delete之类的释放函数
  
            while(m_bExitCaptureThread == false)
            {
                //500毫秒超时,图像没捕获到前，线程会被挂起,释放CPU，所以该线程中无需调用sleep
                eStatus = MvApi.CameraGetImageBuffer(m_hCamera, out FrameHead, out uRawBuffer, 500);
                
                if (eStatus == CameraSdkStatus.CAMERA_STATUS_SUCCESS)//如果是触发模式，则有可能超时
                {
                    //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
                    MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBuffer, ref FrameHead);
                    //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。    
                    MvApi.CameraImageOverlay(m_hCamera, m_ImageBuffer, ref FrameHead);
                    //调用SDK封装好的接口，显示预览图像
                    MvApi.CameraDisplayRGB24(m_hCamera, m_ImageBuffer, ref FrameHead);
                    //成功调用CameraGetImageBuffer后必须释放，下次才能继续调用CameraGetImageBuffer捕获图像。
                    MvApi.CameraReleaseImageBuffer(m_hCamera,uRawBuffer);

                    if (FrameHead.iWidth != m_tFrameHead.iWidth || FrameHead.iHeight != m_tFrameHead.iHeight)
                    {
                        m_bEraseBk = true;
                        m_tFrameHead = FrameHead;  
                    }
                    m_iDisplayedFrames++;
                }
           
            }
           
        }
#endif

        /*相机配置窗口的消息回调函数
        hCamera:当前相机的句柄
        MSG:消息类型，
	    SHEET_MSG_LOAD_PARAM_DEFAULT	= 0,//加载默认参数的按钮被点击，加载默认参数完成后触发该消息,
	    SHEET_MSG_LOAD_PARAM_GROUP		= 1,//切换参数组完成后触发该消息,
	    SHEET_MSG_LOAD_PARAM_FROMFILE	= 2,//加载参数按钮被点击，已从文件中加载相机参数后触发该消息
	    SHEET_MSG_SAVE_PARAM_GROUP		= 3//保存参数按钮被点击，参数保存后触发该消息
	    具体参见CameraDefine.h中emSdkPropSheetMsg类型

        uParam:消息附带的参数，不同的消息，参数意义不同。
	    当 MSG 为 SHEET_MSG_LOAD_PARAM_DEFAULT时，uParam表示被加载成默认参数组的索引号，从0开始，分别对应A,B,C,D四组
	    当 MSG 为 SHEET_MSG_LOAD_PARAM_GROUP时，uParam表示切换后的参数组的索引号，从0开始，分别对应A,B,C,D四组
	    当 MSG 为 SHEET_MSG_LOAD_PARAM_FROMFILE时，uParam表示被文件中参数覆盖的参数组的索引号，从0开始，分别对应A,B,C,D四组
	    当 MSG 为 SHEET_MSG_SAVE_PARAM_GROUP时，uParam表示当前保存的参数组的索引号，从0开始，分别对应A,B,C,D四组
        */
        public void SettingPageMsgCalBack(CameraHandle hCamera, uint MSG, uint uParam, uint pContext)
        {

        }

        private bool InitCamera()
        {
            tSdkCameraDevInfo[] tCameraDevInfoList = new tSdkCameraDevInfo[12];
            IntPtr ptr;
            int i;
#if USE_CALL_BACK
            CAMERA_SNAP_PROC pCaptureCallOld = null;
#endif
            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraDevInfo())*12);
            int iCameraCounts = 12;//如果有多个相机时，表示最大只获取最多12个相机的信息列表。该变量必须初始化，并且大于1
            if (m_hCamera > 0)
            {
                //已经初始化过，直接返回 true

                return true;
            }
            if (MvApi.CameraEnumerateDevice(ptr, ref iCameraCounts) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                for (i = 0; i < 12; i++)
                {
                    tCameraDevInfoList[i] = (tSdkCameraDevInfo)Marshal.PtrToStructure((IntPtr)((int)ptr + i * Marshal.SizeOf(new tSdkCameraDevInfo())), typeof(tSdkCameraDevInfo));
                }
                Marshal.FreeHGlobal(ptr); 

                if(iCameraCounts >= 1)//此时iCameraCounts返回了实际连接的相机个数。如果大于1，则初始化第一个相机
                {
                    if (MvApi.CameraInit(ref tCameraDevInfoList[0], -1,-1, ref m_hCamera) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                    {
                        //获得相机特性描述
                        ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraCapbility()));
                        MvApi.CameraGetCapability(m_hCamera, ptr);
                        tCameraCapability = (tSdkCameraCapbility)Marshal.PtrToStructure(ptr, typeof(tSdkCameraCapbility));
                        Marshal.FreeHGlobal(ptr);
                        Marshal.FreeHGlobal(m_ImageBuffer);
                        m_ImageBuffer = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax*3 + 1024);
                        m_ImageBufferSnapshot = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);
                        
                        //初始化显示模块，使用SDK内部封装好的显示接口
                        MvApi.CameraDisplayInit(m_hCamera, PreviewBox.Handle);
                        MvApi.CameraSetDisplaySize(m_hCamera, PreviewBox.Width, PreviewBox.Height);

                        //设置抓拍通道的分辨率。
                        tSdkImageResolution tResolution;
                        tResolution.fZoomScale = 1.0F;
                        tResolution.iVOffset = 0;
                        tResolution.iHOffset = 0;
                        tResolution.uBinMode = 0;
                        tResolution.uSkipMode = 0;
                        tResolution.iWidth  = tCameraCapability.sResolutionRange.iWidthMax;
                        tResolution.iHeight = tCameraCapability.sResolutionRange.iHeightMax;
                        //tResolution.iIndex = 0xff;表示自定义分辨率,如果tResolution.iWidth和tResolution.iHeight
                        //定义为0，则表示跟随预览通道的分辨率进行抓拍。抓拍通道的分辨率可以动态更改。
                        //本例中将抓拍分辨率固定为最大分辨率。
                        tResolution.iIndex = 0xff;
                        tResolution.acDescription = new byte[32];//描述信息可以不设置
                        MvApi.CameraSetResolutionForSnap(m_hCamera, ref tResolution);

                        //让SDK来根据相机的型号动态创建该相机的配置窗口。
                        MvApi.CameraCreateSettingPage(m_hCamera,this.Handle,tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null,0);

                        //两种方式来获得预览图像，设置回调函数或者使用定时器或者独立线程的方式，
                        //主动调用CameraGetImageBuffer接口来抓图。
                        //本例中仅演示了两种的方式,注意，两种方式也可以同时使用，但是在回调函数中，
                        //不要使用CameraGetImageBuffer，否则会造成死锁现象。
#if USE_CALL_BACK
                        MvApi.CameraSetCallbackFunction(m_hCamera, ImageCaptureCallback, m_iCaptureCallbackCtx, ref pCaptureCallOld);
#else //如果需要采用多线程，使用下面的方式
                        m_bExitCaptureThread = false;
                        m_tCaptureThread = new Thread(new ThreadStart(CaptureThreadProc));
                        m_tCaptureThread.Start();

#endif
                        return true;

                    }
                    else
                    {
                        m_hCamera = 0;
                        StateLabel.Text = "相机初始化失败";
                        return false;
                    }

                   
                }
            }

            return false;
        
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (m_hCamera < 1)//还未初始化相机
            {
                if (InitCamera() == true)
                {
                    MvApi.CameraPlay(m_hCamera);
                }
            }
            else//已经初始化
            {
                MvApi.CameraPlay(m_hCamera);
            }
        }

        private void BasicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_hCamera > 0)
            {
#if !USE_CALL_BACK //使用回调函数的方式则不需要停止线程
                m_bExitCaptureThread = true;
                while (m_tCaptureThread.IsAlive)
                {
                    Thread.Sleep(10);
                }
#endif
                MvApi.CameraUnInit(m_hCamera);
                Marshal.FreeHGlobal(m_ImageBuffer);
                Marshal.FreeHGlobal(m_ImageBufferSnapshot);
                m_hCamera = 0;
            }
            parent_window.set_show_picture();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            if (m_hCamera > 0)
            {
                MvApi.CameraShowSettingPage(m_hCamera, 1);//1 show ; 0 hide
            }
        }

        //1秒更新一次视频信息
        private void timer1_Tick(object sender, EventArgs e)
        {
            tSdkFrameStatistic tFrameStatistic;
            if (m_hCamera > 0)
            {
                //获得SDK中图像帧统计信息，捕获帧、错误帧等。
                MvApi.CameraGetFrameStatistic(m_hCamera, out tFrameStatistic);
                //显示帧率有应用程序自己记录。
                string sFrameInfomation = String.Format("| 图像分辨率:{0}*{1} | 显示帧数{2} | 捕获帧数{3} |", m_tFrameHead.iWidth, m_tFrameHead.iHeight, m_iDisplayedFrames, tFrameStatistic.iCapture);
                StateLabel.Text = sFrameInfomation;
                
            }
            else
            {
                StateLabel.Text = "";
            }
        }

        //用于分辨率切换时，刷新背景绘图
        private void timer2_Tick(object sender, EventArgs e)
        {
            //切换分辨率后，擦除一次背景
            if (m_bEraseBk == true)
            {
                m_bEraseBk = false;
                PreviewBox.Refresh();
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPFILEHEADER
        {
            public ushort bfType;
            public uint bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfOffBits;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
            public const int BI_RGB = 0;
        } 
        private Image convertImage(ref tSdkFrameHead tFrameHead, IntPtr pRgbBuffer)
        {
            BITMAPINFOHEADER bmi;
            BITMAPFILEHEADER bmfi;

            bmfi.bfType = ((int)'M' << 8) | ((int)'B');
            bmfi.bfOffBits = 54;
            bmfi.bfSize = (uint)(54 + tFrameHead.iWidth * tFrameHead.iHeight * 3);
            bmfi.bfReserved1 = 0;
            bmfi.bfReserved2 = 0;

            bmi.biBitCount = 24;
            bmi.biClrImportant = 0;
            bmi.biClrUsed = 0;
            bmi.biCompression = 0;
            bmi.biPlanes = 1;
            bmi.biSize = 40;
            bmi.biHeight = tFrameHead.iHeight;
            bmi.biWidth = tFrameHead.iWidth;
            bmi.biXPelsPerMeter = 0;
            bmi.biYPelsPerMeter = 0;
            bmi.biSizeImage = 0;

            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);
            byte[] data = new byte[14];
            IntPtr ptr = Marshal.AllocHGlobal(54);
            Marshal.StructureToPtr((object)bmfi, ptr, false);
            Marshal.Copy(ptr, data, 0, data.Length);
            bw.Write(data);
            data = new byte[40];
            Marshal.StructureToPtr((object)bmi, ptr, false);
            Marshal.Copy(ptr, data, 0, data.Length);
            bw.Write(data);
            data = new byte[tFrameHead.iWidth * tFrameHead.iHeight * 3];
            Marshal.Copy(pRgbBuffer, data, 0, data.Length);
            bw.Write(data);
            Marshal.FreeHGlobal(ptr);
            return Image.FromStream(stream);
        }


        private void BtnSnapshot_Click(object sender, EventArgs e)
        {
            tSdkFrameHead tFrameHead;
            uint uRawBuffer;//由SDK中给RAW数据分配内存，并释放
         
           
                          
            if (m_hCamera <= 0)
            {
                return;//相机还未初始化，句柄无效
            }
            
            if (MvApi.CameraSnapToBuffer(m_hCamera, out tFrameHead, out uRawBuffer,500) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                //此时，uRawBuffer指向了相机原始数据的缓冲区地址，默认情况下为8bit位宽的Bayer格式，如果
                //您需要解析bayer数据，此时就可以直接处理了，后续的操作演示了如何将原始数据转换为RGB格式
                //并显示在窗口上。

                //将相机输出的原始数据转换为RGB格式到内存m_ImageBufferSnapshot中
                MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBufferSnapshot, ref tFrameHead);
                //CameraSnapToBuffer成功调用后必须用CameraReleaseImageBuffer释放SDK中分配的RAW数据缓冲区
                //否则，将造成死锁现象，预览通道和抓拍通道会被一直阻塞，直到调用CameraReleaseImageBuffer释放后解锁。
                MvApi.CameraReleaseImageBuffer(m_hCamera, uRawBuffer);
                //更新抓拍显示窗口。

                Bitmap image1 = new Bitmap(convertImage(ref tFrameHead, m_ImageBufferSnapshot));
                saveFileDialog1.Filter = "JPEG files (*.jpg)|*.jpg";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)//Save setting file
                {
                    image1.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
                }

            }
        }

        private void BasicForm_Load(object sender, EventArgs e)
        {

        }


        public void Record_the_picture(out Bitmap image, out string fileName )
        {
            tSdkFrameHead tFrameHead;
            uint uRawBuffer;//由SDK中给RAW数据分配内存，并释放

            //Bitmap image = new Bitmap(10, 10);
           // image.Save(save_path+'\\'+"sampe_" + i.ToString()+ '_' + DateTime.Now.ToString("hh_mm_ss") + ".jpg", ImageFormat.Jpeg);

            if (m_hCamera <= 0)
            {
                image = null;
                fileName = null;
                return;//相机还未初始化，句柄无效
            }

            if (MvApi.CameraSnapToBuffer(m_hCamera, out tFrameHead, out uRawBuffer, 500) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                //此时，uRawBuffer指向了相机原始数据的缓冲区地址，默认情况下为8bit位宽的Bayer格式，如果
                //您需要解析bayer数据，此时就可以直接处理了，后续的操作演示了如何将原始数据转换为RGB格式
                //并显示在窗口上。

                //将相机输出的原始数据转换为RGB格式到内存m_ImageBufferSnapshot中
                MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBufferSnapshot, ref tFrameHead);
                //CameraSnapToBuffer成功调用后必须用CameraReleaseImageBuffer释放SDK中分配的RAW数据缓冲区
                //否则，将造成死锁现象，预览通道和抓拍通道会被一直阻塞，直到调用CameraReleaseImageBuffer释放后解锁。
                MvApi.CameraReleaseImageBuffer(m_hCamera, uRawBuffer);
                //更新抓拍显示窗口。


                Bitmap image1 = new Bitmap(convertImage(ref tFrameHead, m_ImageBufferSnapshot));
                string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                string imgPath = @Application.StartupPath + @"/photo/" + filename;
                image1.Save(imgPath, ImageFormat.Jpeg);

                image = image1;
                fileName = filename;

               // image1.Save(save_path + '\\' + "sampe_" + i.ToString() + '_' + DateTime.Now.ToString("HH_mm_ss") + ".jpg", ImageFormat.Jpeg);

            }
            
                image = null;
                fileName = null;
                return;
        }
        public void Record_the_picture()
        {
            tSdkFrameHead tFrameHead;
            uint uRawBuffer;//由SDK中给RAW数据分配内存，并释放

            //Bitmap image = new Bitmap(10, 10);
            // image.Save(save_path+'\\'+"sampe_" + i.ToString()+ '_' + DateTime.Now.ToString("hh_mm_ss") + ".jpg", ImageFormat.Jpeg);

            if (m_hCamera <= 0)
            {
                return;//相机还未初始化，句柄无效
            }

            if (MvApi.CameraSnapToBuffer(m_hCamera, out tFrameHead, out uRawBuffer, 500) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                //此时，uRawBuffer指向了相机原始数据的缓冲区地址，默认情况下为8bit位宽的Bayer格式，如果
                //您需要解析bayer数据，此时就可以直接处理了，后续的操作演示了如何将原始数据转换为RGB格式
                //并显示在窗口上。

                //将相机输出的原始数据转换为RGB格式到内存m_ImageBufferSnapshot中
                MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBufferSnapshot, ref tFrameHead);
                //CameraSnapToBuffer成功调用后必须用CameraReleaseImageBuffer释放SDK中分配的RAW数据缓冲区
                //否则，将造成死锁现象，预览通道和抓拍通道会被一直阻塞，直到调用CameraReleaseImageBuffer释放后解锁。
                MvApi.CameraReleaseImageBuffer(m_hCamera, uRawBuffer);
                //更新抓拍显示窗口。


                Bitmap image1 = new Bitmap(convertImage(ref tFrameHead, m_ImageBufferSnapshot));
                string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                string imgPath = @Application.StartupPath + @"/photo/" + filename;
                image1.Save(imgPath, ImageFormat.Jpeg);

                // image1.Save(save_path + '\\' + "sampe_" + i.ToString() + '_' + DateTime.Now.ToString("HH_mm_ss") + ".jpg", ImageFormat.Jpeg);

            }
        }

        private void PreviewBox_MouseClick(object sender, MouseEventArgs e)
        {
            // 点击时记录坐标
            int x = e.X;
            int y = e.Y;
            int w = PreviewBox.Width;
            int h = PreviewBox.Height;
            int ox = w / 2;
            int oy = h / 2;
            // 执行你的方法,无须模拟鼠标动作
            if (m_hCamera <= 0)
            {
                return;//相机还未初始化，句柄无效
            }
            int picture_w = m_tFrameHead.iWidth;
            int picture_h = m_tFrameHead.iHeight;
            int delta_x = (x - ox) * 1150 / 10 / w; // 2 * 500 / 710;
            int delta_y = (y - oy) * 800 / 10 / h;  // 2 * 300 / 420;
            PCAS.micoscope_x(delta_x);
            PCAS.micoscope_y(delta_y);
        }
    }
}