//#define USE_CALLBACK 1 //ͼ��ץȡ��ʽ�궨�壬������ûص�������ʽ���رգ���ʹ�ö��߳�����ץȡ��ʽ��



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using MVSDK;//ʹ��MindVision .net SDK�ӿ�
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
        protected CameraHandle m_hCamera = 0;             // ���
        protected IntPtr       m_ImageBuffer;             // Ԥ��ͨ��RGBͼ�񻺴�
        protected IntPtr       m_ImageBufferSnapshot;     // ץ��ͨ��RGBͼ�񻺴�
        protected tSdkCameraCapbility tCameraCapability;  // �����������
        protected int          m_iDisplayedFrames = 0;    //�Ѿ���ʾ����֡��
        protected IntPtr       m_iCaptureCallbackCtx;     //ͼ��ص������������Ĳ���
        protected Thread       m_tCaptureThread;          //ͼ��ץȡ�߳�
        protected bool         m_bExitCaptureThread = false;//�����̲߳ɼ�ʱ�����߳��˳��ı�־
        protected IntPtr       m_iSettingPageMsgCallbackCtx; //������ý�����Ϣ�ص������������Ĳ���   
        protected tSdkFrameHead m_tFrameHead;
        protected bool          m_bEraseBk = false;
        #endregion

        public BasicForm()
        {
            InitializeComponent();

            //��ʼ��SDK
            MvApi.CameraSdkInit(1);//1:SDK��ʹ�����Ľ�����ʾ�ʹ���������ô���  0��Ӣ��
            // ����Ƿ����������������Ѿ����ӣ���ֱ�ӳ�ʼ���������ʼԤ��
            if (InitCamera() == true)
            {
                MvApi.CameraPlay(m_hCamera);
            }

        }
       

#if USE_CALL_BACK
        public void ImageCaptureCallback(CameraHandle hCamera, uint pFrameBuffer, ref tSdkFrameHead pFrameHead, uint pContext)
        {
            //ͼ������ԭʼ���ת��ΪRGB��ʽ��λͼ���ݣ�ͬʱ���Ӱ�ƽ�⡢���Ͷȡ�LUT��ISP����
            MvApi.CameraImageProcess(hCamera, pFrameBuffer, (IntPtr)((int)m_ImageBuffer&(~0xf)), ref pFrameHead);
            //����ʮ���ߡ��Զ��عⴰ�ڡ���ƽ�ⴰ����Ϣ(����������Ϊ�ɼ�״̬��)��   
            MvApi.CameraImageOverlay(hCamera, (IntPtr)((int)m_ImageBuffer & (~0xf)), ref pFrameHead);
            //����SDK��װ�õĽӿڣ���ʾԤ��ͼ��
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
            uint uRawBuffer;//rawbuffer��SDK�ڲ����롣Ӧ�ò㲻Ҫ����delete֮����ͷź���
  
            while(m_bExitCaptureThread == false)
            {
                //500���볬ʱ,ͼ��û����ǰ���̻߳ᱻ����,�ͷ�CPU�����Ը��߳����������sleep
                eStatus = MvApi.CameraGetImageBuffer(m_hCamera, out FrameHead, out uRawBuffer, 500);
                
                if (eStatus == CameraSdkStatus.CAMERA_STATUS_SUCCESS)//����Ǵ���ģʽ�����п��ܳ�ʱ
                {
                    //ͼ������ԭʼ���ת��ΪRGB��ʽ��λͼ���ݣ�ͬʱ���Ӱ�ƽ�⡢���Ͷȡ�LUT��ISP����
                    MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBuffer, ref FrameHead);
                    //����ʮ���ߡ��Զ��عⴰ�ڡ���ƽ�ⴰ����Ϣ(����������Ϊ�ɼ�״̬��)��    
                    MvApi.CameraImageOverlay(m_hCamera, m_ImageBuffer, ref FrameHead);
                    //����SDK��װ�õĽӿڣ���ʾԤ��ͼ��
                    MvApi.CameraDisplayRGB24(m_hCamera, m_ImageBuffer, ref FrameHead);
                    //�ɹ�����CameraGetImageBuffer������ͷţ��´β��ܼ�������CameraGetImageBuffer����ͼ��
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

        /*������ô��ڵ���Ϣ�ص�����
        hCamera:��ǰ����ľ��
        MSG:��Ϣ���ͣ�
	    SHEET_MSG_LOAD_PARAM_DEFAULT	= 0,//����Ĭ�ϲ����İ�ť�����������Ĭ�ϲ�����ɺ󴥷�����Ϣ,
	    SHEET_MSG_LOAD_PARAM_GROUP		= 1,//�л���������ɺ󴥷�����Ϣ,
	    SHEET_MSG_LOAD_PARAM_FROMFILE	= 2,//���ز�����ť��������Ѵ��ļ��м�����������󴥷�����Ϣ
	    SHEET_MSG_SAVE_PARAM_GROUP		= 3//���������ť���������������󴥷�����Ϣ
	    ����μ�CameraDefine.h��emSdkPropSheetMsg����

        uParam:��Ϣ�����Ĳ�������ͬ����Ϣ���������岻ͬ��
	    �� MSG Ϊ SHEET_MSG_LOAD_PARAM_DEFAULTʱ��uParam��ʾ�����س�Ĭ�ϲ�����������ţ���0��ʼ���ֱ��ӦA,B,C,D����
	    �� MSG Ϊ SHEET_MSG_LOAD_PARAM_GROUPʱ��uParam��ʾ�л���Ĳ�����������ţ���0��ʼ���ֱ��ӦA,B,C,D����
	    �� MSG Ϊ SHEET_MSG_LOAD_PARAM_FROMFILEʱ��uParam��ʾ���ļ��в������ǵĲ�����������ţ���0��ʼ���ֱ��ӦA,B,C,D����
	    �� MSG Ϊ SHEET_MSG_SAVE_PARAM_GROUPʱ��uParam��ʾ��ǰ����Ĳ�����������ţ���0��ʼ���ֱ��ӦA,B,C,D����
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
            int iCameraCounts = 12;//����ж�����ʱ����ʾ���ֻ��ȡ���12���������Ϣ�б��ñ��������ʼ�������Ҵ���1
            if (m_hCamera > 0)
            {
                //�Ѿ���ʼ������ֱ�ӷ��� true

                return true;
            }
            if (MvApi.CameraEnumerateDevice(ptr, ref iCameraCounts) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                for (i = 0; i < 12; i++)
                {
                    tCameraDevInfoList[i] = (tSdkCameraDevInfo)Marshal.PtrToStructure((IntPtr)((int)ptr + i * Marshal.SizeOf(new tSdkCameraDevInfo())), typeof(tSdkCameraDevInfo));
                }
                Marshal.FreeHGlobal(ptr); 

                if(iCameraCounts >= 1)//��ʱiCameraCounts������ʵ�����ӵ�����������������1�����ʼ����һ�����
                {
                    if (MvApi.CameraInit(ref tCameraDevInfoList[0], -1,-1, ref m_hCamera) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                    {
                        //��������������
                        ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraCapbility()));
                        MvApi.CameraGetCapability(m_hCamera, ptr);
                        tCameraCapability = (tSdkCameraCapbility)Marshal.PtrToStructure(ptr, typeof(tSdkCameraCapbility));
                        Marshal.FreeHGlobal(ptr);
                        Marshal.FreeHGlobal(m_ImageBuffer);
                        m_ImageBuffer = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax*3 + 1024);
                        m_ImageBufferSnapshot = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);
                        
                        //��ʼ����ʾģ�飬ʹ��SDK�ڲ���װ�õ���ʾ�ӿ�
                        MvApi.CameraDisplayInit(m_hCamera, PreviewBox.Handle);
                        MvApi.CameraSetDisplaySize(m_hCamera, PreviewBox.Width, PreviewBox.Height);

                        //����ץ��ͨ���ķֱ��ʡ�
                        tSdkImageResolution tResolution;
                        tResolution.fZoomScale = 1.0F;
                        tResolution.iVOffset = 0;
                        tResolution.iHOffset = 0;
                        tResolution.uBinMode = 0;
                        tResolution.uSkipMode = 0;
                        tResolution.iWidth  = tCameraCapability.sResolutionRange.iWidthMax;
                        tResolution.iHeight = tCameraCapability.sResolutionRange.iHeightMax;
                        //tResolution.iIndex = 0xff;��ʾ�Զ���ֱ���,���tResolution.iWidth��tResolution.iHeight
                        //����Ϊ0�����ʾ����Ԥ��ͨ���ķֱ��ʽ���ץ�ġ�ץ��ͨ���ķֱ��ʿ��Զ�̬���ġ�
                        //�����н�ץ�ķֱ��ʹ̶�Ϊ���ֱ��ʡ�
                        tResolution.iIndex = 0xff;
                        tResolution.acDescription = new byte[32];//������Ϣ���Բ�����
                        MvApi.CameraSetResolutionForSnap(m_hCamera, ref tResolution);

                        //��SDK������������ͺŶ�̬��������������ô��ڡ�
                        MvApi.CameraCreateSettingPage(m_hCamera,this.Handle,tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null,0);

                        //���ַ�ʽ�����Ԥ��ͼ�����ûص���������ʹ�ö�ʱ�����߶����̵߳ķ�ʽ��
                        //��������CameraGetImageBuffer�ӿ���ץͼ��
                        //�����н���ʾ�����ֵķ�ʽ,ע�⣬���ַ�ʽҲ����ͬʱʹ�ã������ڻص������У�
                        //��Ҫʹ��CameraGetImageBuffer������������������
#if USE_CALL_BACK
                        MvApi.CameraSetCallbackFunction(m_hCamera, ImageCaptureCallback, m_iCaptureCallbackCtx, ref pCaptureCallOld);
#else //�����Ҫ���ö��̣߳�ʹ������ķ�ʽ
                        m_bExitCaptureThread = false;
                        m_tCaptureThread = new Thread(new ThreadStart(CaptureThreadProc));
                        m_tCaptureThread.Start();

#endif
                        return true;

                    }
                    else
                    {
                        m_hCamera = 0;
                        StateLabel.Text = "�����ʼ��ʧ��";
                        return false;
                    }

                   
                }
            }

            return false;
        
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (m_hCamera < 1)//��δ��ʼ�����
            {
                if (InitCamera() == true)
                {
                    MvApi.CameraPlay(m_hCamera);
                }
            }
            else//�Ѿ���ʼ��
            {
                MvApi.CameraPlay(m_hCamera);
            }
        }

        private void BasicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_hCamera > 0)
            {
#if !USE_CALL_BACK //ʹ�ûص������ķ�ʽ����Ҫֹͣ�߳�
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

        //1�����һ����Ƶ��Ϣ
        private void timer1_Tick(object sender, EventArgs e)
        {
            tSdkFrameStatistic tFrameStatistic;
            if (m_hCamera > 0)
            {
                //���SDK��ͼ��֡ͳ����Ϣ������֡������֡�ȡ�
                MvApi.CameraGetFrameStatistic(m_hCamera, out tFrameStatistic);
                //��ʾ֡����Ӧ�ó����Լ���¼��
                string sFrameInfomation = String.Format("| ͼ��ֱ���:{0}*{1} | ��ʾ֡��{2} | ����֡��{3} |", m_tFrameHead.iWidth, m_tFrameHead.iHeight, m_iDisplayedFrames, tFrameStatistic.iCapture);
                StateLabel.Text = sFrameInfomation;
                
            }
            else
            {
                StateLabel.Text = "";
            }
        }

        //���ڷֱ����л�ʱ��ˢ�±�����ͼ
        private void timer2_Tick(object sender, EventArgs e)
        {
            //�л��ֱ��ʺ󣬲���һ�α���
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
            uint uRawBuffer;//��SDK�и�RAW���ݷ����ڴ棬���ͷ�
         
           
                          
            if (m_hCamera <= 0)
            {
                return;//�����δ��ʼ���������Ч
            }
            
            if (MvApi.CameraSnapToBuffer(m_hCamera, out tFrameHead, out uRawBuffer,500) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                //��ʱ��uRawBufferָ�������ԭʼ���ݵĻ�������ַ��Ĭ�������Ϊ8bitλ���Bayer��ʽ�����
                //����Ҫ����bayer���ݣ���ʱ�Ϳ���ֱ�Ӵ����ˣ������Ĳ�����ʾ����ν�ԭʼ����ת��ΪRGB��ʽ
                //����ʾ�ڴ����ϡ�

                //����������ԭʼ����ת��ΪRGB��ʽ���ڴ�m_ImageBufferSnapshot��
                MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBufferSnapshot, ref tFrameHead);
                //CameraSnapToBuffer�ɹ����ú������CameraReleaseImageBuffer�ͷ�SDK�з����RAW���ݻ�����
                //���򣬽������������Ԥ��ͨ����ץ��ͨ���ᱻһֱ������ֱ������CameraReleaseImageBuffer�ͷź������
                MvApi.CameraReleaseImageBuffer(m_hCamera, uRawBuffer);
                //����ץ����ʾ���ڡ�

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
            uint uRawBuffer;//��SDK�и�RAW���ݷ����ڴ棬���ͷ�

            //Bitmap image = new Bitmap(10, 10);
           // image.Save(save_path+'\\'+"sampe_" + i.ToString()+ '_' + DateTime.Now.ToString("hh_mm_ss") + ".jpg", ImageFormat.Jpeg);

            if (m_hCamera <= 0)
            {
                image = null;
                fileName = null;
                return;//�����δ��ʼ���������Ч
            }

            if (MvApi.CameraSnapToBuffer(m_hCamera, out tFrameHead, out uRawBuffer, 500) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                //��ʱ��uRawBufferָ�������ԭʼ���ݵĻ�������ַ��Ĭ�������Ϊ8bitλ���Bayer��ʽ�����
                //����Ҫ����bayer���ݣ���ʱ�Ϳ���ֱ�Ӵ����ˣ������Ĳ�����ʾ����ν�ԭʼ����ת��ΪRGB��ʽ
                //����ʾ�ڴ����ϡ�

                //����������ԭʼ����ת��ΪRGB��ʽ���ڴ�m_ImageBufferSnapshot��
                MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBufferSnapshot, ref tFrameHead);
                //CameraSnapToBuffer�ɹ����ú������CameraReleaseImageBuffer�ͷ�SDK�з����RAW���ݻ�����
                //���򣬽������������Ԥ��ͨ����ץ��ͨ���ᱻһֱ������ֱ������CameraReleaseImageBuffer�ͷź������
                MvApi.CameraReleaseImageBuffer(m_hCamera, uRawBuffer);
                //����ץ����ʾ���ڡ�


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
            uint uRawBuffer;//��SDK�и�RAW���ݷ����ڴ棬���ͷ�

            //Bitmap image = new Bitmap(10, 10);
            // image.Save(save_path+'\\'+"sampe_" + i.ToString()+ '_' + DateTime.Now.ToString("hh_mm_ss") + ".jpg", ImageFormat.Jpeg);

            if (m_hCamera <= 0)
            {
                return;//�����δ��ʼ���������Ч
            }

            if (MvApi.CameraSnapToBuffer(m_hCamera, out tFrameHead, out uRawBuffer, 500) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                //��ʱ��uRawBufferָ�������ԭʼ���ݵĻ�������ַ��Ĭ�������Ϊ8bitλ���Bayer��ʽ�����
                //����Ҫ����bayer���ݣ���ʱ�Ϳ���ֱ�Ӵ����ˣ������Ĳ�����ʾ����ν�ԭʼ����ת��ΪRGB��ʽ
                //����ʾ�ڴ����ϡ�

                //����������ԭʼ����ת��ΪRGB��ʽ���ڴ�m_ImageBufferSnapshot��
                MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBufferSnapshot, ref tFrameHead);
                //CameraSnapToBuffer�ɹ����ú������CameraReleaseImageBuffer�ͷ�SDK�з����RAW���ݻ�����
                //���򣬽������������Ԥ��ͨ����ץ��ͨ���ᱻһֱ������ֱ������CameraReleaseImageBuffer�ͷź������
                MvApi.CameraReleaseImageBuffer(m_hCamera, uRawBuffer);
                //����ץ����ʾ���ڡ�


                Bitmap image1 = new Bitmap(convertImage(ref tFrameHead, m_ImageBufferSnapshot));
                string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                string imgPath = @Application.StartupPath + @"/photo/" + filename;
                image1.Save(imgPath, ImageFormat.Jpeg);

                // image1.Save(save_path + '\\' + "sampe_" + i.ToString() + '_' + DateTime.Now.ToString("HH_mm_ss") + ".jpg", ImageFormat.Jpeg);

            }
        }

        private void PreviewBox_MouseClick(object sender, MouseEventArgs e)
        {
            // ���ʱ��¼����
            int x = e.X;
            int y = e.Y;
            int w = PreviewBox.Width;
            int h = PreviewBox.Height;
            int ox = w / 2;
            int oy = h / 2;
            // ִ����ķ���,����ģ����궯��
            if (m_hCamera <= 0)
            {
                return;//�����δ��ʼ���������Ч
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