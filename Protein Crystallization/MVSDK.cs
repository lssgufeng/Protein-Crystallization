/*
 * MindVision��ҵ���C# SDK��������
 * �������ϣ���ο�SDK�����ֲ�
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

//��������SDK֧��ͬʱ�򿪶��������øþ�����ֶ����
using CameraHandle    = System.Int32;


namespace MVSDK
{

    //SDK�ӿڵķ���ֵ����������
    public enum CameraSdkStatus
    {
        CAMERA_STATUS_SUCCESS                   = 0,   // �����ɹ�
        CAMERA_STATUS_FAILED                    =-1,   // ����ʧ��
        CAMERA_STATUS_intERNAL_ERROR            =-2,   // �ڲ�����
        CAMERA_STATUS_UNKNOW                    =-3,   // δ֪����
        CAMERA_STATUS_NOT_SUPPORTED             =-4,   // ��֧�ָù���
        CAMERA_STATUS_NOT_INITIALIZED           =-5,   // ��ʼ��δ���
        CAMERA_STATUS_PARAMETER_INVALID         =-6,   // ������Ч
        CAMERA_STATUS_PARAMETER_OUT_OF_BOUND    =-7,   // ����Խ��
        CAMERA_STATUS_UNENABLED                 =-8,   // δʹ��
        CAMERA_STATUS_USER_CANCEL               =-9,   // �û��ֶ�ȡ���ˣ�����roi�����ȡ��������
        CAMERA_STATUS_PATH_NOT_FOUND            =-10,  // ע�����û���ҵ���Ӧ��·��
        CAMERA_STATUS_SIZE_DISMATCH             =-11,  // ���ͼ�����ݳ��ȺͶ���ĳߴ粻ƥ��
        CAMERA_STATUS_TIME_OUT                  =-12,  // ��ʱ����
        CAMERA_STATUS_IO_ERROR                  =-13,  // Ӳ��IO����
        CAMERA_STATUS_COMM_ERROR                =-14,  // ͨѶ����
        CAMERA_STATUS_BUS_ERROR                 =-15,  // ���ߴ���
        CAMERA_STATUS_NO_DEVICE_FOUND           =-16,  // û�з����豸
        CAMERA_STATUS_NO_LOGIC_DEVICE_FOUND     =-17,  // δ�ҵ��߼��豸
        CAMERA_STATUS_DEVICE_IS_OPENED          =-18,  // �豸�Ѿ���
        CAMERA_STATUS_DEVICE_IS_CLOSED          =-19,  // �豸�Ѿ��ر�
        CAMERA_STATUS_DEVICE_VEDIO_CLOSED       =-20,  // û�д��豸��Ƶ������¼����صĺ���ʱ����������Ƶû�д򿪣���ط��ظô���
        CAMERA_STATUS_NO_MEMORY                 =-21,  // û���㹻ϵͳ�ڴ�
        CAMERA_STATUS_FILE_CREATE_FAILED        =-22,  // �����ļ�ʧ��
        CAMERA_STATUS_FILE_INVALID              =-23,  // �ļ���ʽ��Ч
        CAMERA_STATUS_WRITE_PROTECTED           =-24,  // д����������д
        CAMERA_STATUS_GRAB_FAILED               =-25,  // ���ݲɼ�ʧ��
        CAMERA_STATUS_LOST_DATA                 =-26,  // ���ݶ�ʧ��������
        CAMERA_STATUS_EOF_ERROR                 =-27,  // δ���յ�֡������
        CAMERA_STATUS_BUSY                      =-28,  // ��æ(��һ�β������ڽ�����)���˴β������ܽ���
        CAMERA_STATUS_WAIT                      =-29,  // ��Ҫ�ȴ�(���в���������������)�������ٴγ���
        CAMERA_STATUS_IN_PROCESS                =-30,  // ���ڽ��У��Ѿ���������
        CAMERA_STATUS_IIC_ERROR                 =-31,  // IIC�������
        CAMERA_STATUS_SPI_ERROR                 =-32,  // SPI�������
        CAMERA_STATUS_USB_CONTROL_ERROR         =-33,  // USB���ƴ������
        CAMERA_STATUS_USB_BULK_ERROR            =-34,  // USB BULK�������
        CAMERA_STATUS_SOCKET_INIT_ERROR         =-35,  // ���紫���׼���ʼ��ʧ��
        CAMERA_STATUS_GIGE_FILTER_INIT_ERROR    =-36,  // ��������ں˹���������ʼ��ʧ�ܣ������Ƿ���ȷ��װ���������������°�װ��
        CAMERA_STATUS_NET_SEND_ERROR            =-37,  // �������ݷ��ʹ���
        CAMERA_STATUS_DEVICE_LOST               =-38,  // ���������ʧȥ���ӣ�������ⳬʱ
        CAMERA_STATUS_DATA_RECV_LESS            =-39,  // ���յ����ֽ������������ 
        CAMERA_STATUS_FUNCTION_LOAD_FAILED      =-40,  // ���ļ��м��س���ʧ��
        CAMERA_STATUS_CRITICAL_FILE_LOST        =-41,  // ����������������ļ���ʧ��
        CAMERA_STATUS_SENSOR_ID_DISMATCH        =-42,  // �̼��ͳ���ƥ�䣬ԭ���������˴���Ĺ̼���
        CAMERA_STATUS_OUT_OF_RANGE              =-43,  // ����������Ч��Χ��   
        CAMERA_STATUS_REGISTRY_ERROR            =-44,  // ��װ����ע����������°�װ���򣬻������а�װĿ¼Setup/Installer.exe
        CAMERA_STATUS_ACCESS_DENY               =-45,  // ��ֹ���ʡ�ָ������Ѿ�����������ռ��ʱ����������ʸ�������᷵�ظ�״̬��(һ��������ܱ��������ͬʱ����) 
        //AIA�ı�׼���ݵĴ�����
        CAMERA_AIA_PACKET_RESEND                          =0x0100, //��֡��Ҫ�ش�
        CAMERA_AIA_NOT_IMPLEMENTED                        =0x8001, //�豸��֧�ֵ�����
        CAMERA_AIA_INVALID_PARAMETER                      =0x8002, //��������Ƿ�
        CAMERA_AIA_INVALID_ADDRESS                        =0x8003, //���ɷ��ʵĵ�ַ
        CAMERA_AIA_WRITE_PROTECT                          =0x8004, //���ʵĶ��󲻿�д
        CAMERA_AIA_BAD_ALIGNMENT                          =0x8005, //���ʵĵ�ַû�а���Ҫ�����
        CAMERA_AIA_ACCESS_DENIED                          =0x8006, //û�з���Ȩ��
        CAMERA_AIA_BUSY                                   =0x8007, //�������ڴ�����
        CAMERA_AIA_DEPRECATED                             =0x8008, //0x8008-0x0800B  0x800F  ��ָ���Ѿ�����
        CAMERA_AIA_PACKET_UNAVAILABLE                     =0x800C, //����Ч
        CAMERA_AIA_DATA_OVERRUN                           =0x800D, //���������ͨ�����յ������ݱ���Ҫ�Ķ�
        CAMERA_AIA_INVALID_HEADER                         =0x800E, //���ݰ�ͷ����ĳЩ������Э�鲻ƥ��
        CAMERA_AIA_PACKET_NOT_YET_AVAILABLE               =0x8010, //ͼ��ְ����ݻ�δ׼���ã������ڴ���ģʽ��Ӧ�ó�����ʳ�ʱ
        CAMERA_AIA_PACKET_AND_PREV_REMOVED_FROM_MEMORY    =0x8011, //��Ҫ���ʵķְ��Ѿ������ڡ��������ش�ʱ�����Ѿ����ڻ�������
        CAMERA_AIA_PACKET_REMOVED_FROM_MEMORY             =0x8012, //CAMERA_AIA_PACKET_AND_PREV_REMOVED_FROM_MEMORY
        CAMERA_AIA_NO_REF_TIME                            =0x0813, //û�вο�ʱ��Դ��������ʱ��ͬ��������ִ��ʱ
        CAMERA_AIA_PACKET_TEMPORARILY_UNAVAILABLE         =0x0814, //�����ŵ��������⣬��ǰ�ְ���ʱ�����ã����Ժ���з���
        CAMERA_AIA_OVERFLOW                               =0x0815, //�豸�����������ͨ���Ƕ�������
        CAMERA_AIA_ACTION_LATE                            =0x0816, //����ִ���Ѿ�������Ч��ָ��ʱ��
        CAMERA_AIA_ERROR                                  =0x8FFF   //����
    }

    /*
       
    //tSdkResolutionRange�ṹ����SKIP�� BIN��RESAMPLEģʽ������ֵ
    #define MASK_2X2_HD   (1<<0)    //Ӳ��SKIP��BIN���ز��� 2X2
    #define MASK_3X3_HD   (1<<1)
    #define MASK_4X4_HD   (1<<2)
    #define MASK_5X5_HD   (1<<3)
    #define MASK_6X6_HD   (1<<4)
    #define MASK_7X7_HD   (1<<5)
    #define MASK_8X8_HD   (1<<6)
    #define MASK_9X9_HD   (1<<7)      
    #define MASK_10X10_HD   (1<<8)
    #define MASK_11X11_HD   (1<<9)
    #define MASK_12X12_HD   (1<<10)
    #define MASK_13X13_HD   (1<<11)
    #define MASK_14X14_HD   (1<<12)
    #define MASK_15X15_HD   (1<<13)
    #define MASK_16X16_HD   (1<<14)
    #define MASK_17X17_HD   (1<<15)
    #define MASK_2X2_SW   (1<<16) //Ӳ��SKIP��BIN���ز��� 2X2
    #define MASK_3X3_SW   (1<<17)
    #define MASK_4X4_SW   (1<<18)
    #define MASK_5X5_SW   (1<<19)
    #define MASK_6X6_SW   (1<<20)
    #define MASK_7X7_SW   (1<<21)
    #define MASK_8X8_SW   (1<<22)
    #define MASK_9X9_SW   (1<<23)     
    #define MASK_10X10_SW   (1<<24)
    #define MASK_11X11_SW   (1<<25)
    #define MASK_12X12_SW   (1<<26)
    #define MASK_13X13_SW   (1<<27)
    #define MASK_14X14_SW   (1<<28)
    #define MASK_15X15_SW   (1<<29)
    #define MASK_16X16_SW   (1<<30)
    #define MASK_17X17_SW   (1<<31) 
     */



    //ͼ����任�ķ�ʽ
    public enum emSdkLutMode
    {
        LUTMODE_PARAM_GEN=0,//ͨ�����ڲ�����̬����LUT��
        LUTMODE_PRESET,     //ʹ��Ԥ���LUT��
        LUTMODE_USER_DEF    //ʹ���û��Զ����LUT��
    }

    //�������Ƶ������
    public enum emSdkRunMode
    {
        RUNMODE_PLAY=0,    //����Ԥ��������ͼ�����ʾ�������������ڴ���ģʽ�����ȴ�����֡�ĵ�����
        RUNMODE_PAUSE,     //��ͣ������ͣ�����ͼ�������ͬʱҲ����ȥ����ͼ��
        RUNMODE_STOP       //ֹͣ�������������ʼ��������ʹ���ֹͣģʽ
    }

    //SDK�ڲ���ʾ�ӿڵ���ʾ��ʽ
    public enum emSdkDisplayMode
    {
        DISPLAYMODE_SCALE=0,  //������ʾģʽ�����ŵ���ʾ�ؼ��ĳߴ�
        DISPLAYMODE_REAL      //1:1��ʾģʽ����ͼ��ߴ������ʾ�ؼ��ĳߴ�ʱ��ֻ��ʾ�ֲ�  
    }

    //¼��״̬
    public enum emSdkRecordMode 
    {
      RECORD_STOP = 0,  //ֹͣ
      RECORD_START,     //¼����
      RECORD_PAUSE      //��ͣ
    }

    //ͼ��ľ������
    public enum emSdkMirrorDirection
    {
        MIRROR_DIRECTION_HORIZONTAL = 0,//ˮƽ����
        MIRROR_DIRECTION_VERTICAL       //��ֱ����
    }

    //�����Ƶ��֡��
    public enum emSdkFrameSpeed
    {
        FRAME_SPEED_LOW = 0,  //����ģʽ
        FRAME_SPEED_NORMAL,   //��ͨģʽ
        FRAME_SPEED_HIGH,     //����ģʽ(��Ҫ�ϸߵĴ������,���豸���������ʱ���֡�ʵ��ȶ�����Ӱ��)
        FRAME_SPEED_SUPER     //������ģʽ(��Ҫ�ϸߵĴ������,���豸���������ʱ���֡�ʵ��ȶ�����Ӱ��)
    }

    //�����ļ��ĸ�ʽ����
    public enum emSdkFileType
    {
        FILE_JPG = 1,//JPG
        FILE_BMP = 2,//BMP
        FILE_RAW = 4,//��������bayer��ʽ�ļ�,���ڲ�֧��bayer��ʽ���������޷�����Ϊ�ø�ʽ
        FILE_PNG = 8 //PNG
    }

    //����е�ͼ�񴫸����Ĺ���ģʽ
    public enum emSdkSnapMode 
    { 
        CONTINUATION = 0,//�����ɼ�ģʽ
        SOFT_TRIGGER,    //�������ģʽ�����������ָ��󣬴�������ʼ�ɼ�ָ��֡����ͼ�񣬲ɼ���ɺ�ֹͣ���
        EXTERNAL_TRIGGER //Ӳ������ģʽ�������յ��ⲿ�źţ���������ʼ�ɼ�ָ��֡����ͼ�񣬲ɼ���ɺ�ֹͣ���
    }

    //�Զ��ع�ʱ��Ƶ����Ƶ��
    public enum emSdkLightFrequency
    {
        LIGHT_FREQUENCY_50HZ = 0,//50HZ,һ��ĵƹⶼ��50HZ
        LIGHT_FREQUENCY_60HZ     //60HZ,��Ҫ��ָ��ʾ����
    }

    //��������ò�������ΪA,B,C,D 4����б��档
    public enum emSdkParameterTeam
    {
        PARAMETER_TEAM_DEFAULT = 0xff,
        PARAMETER_TEAM_A = 0,
        PARAMETER_TEAM_B = 1,
        PARAMETER_TEAM_C = 2,
        PARAMETER_TEAM_D = 3
    }
  
  //�����������ģʽ���������ط�Ϊ���ļ��ʹ��豸�������ַ�ʽ
  public enum emSdkParameterMode
  {
    PARAM_MODE_BY_MODEL = 0,  //��������ͺ������ļ��м��ز���������MV-U300
    PARAM_MODE_BY_NAME,       //�����豸�ǳ�(tSdkCameraDevInfo.acFriendlyName)���ļ��м��ز���������MV-U300,���ǳƿ��Զ���
    PARAM_MODE_BY_SN,         //�����豸��Ψһ���кŴ��ļ��м��ز��������к��ڳ���ʱ�Ѿ�д���豸��ÿ̨���ӵ�в�ͬ�����кš�
    PARAM_MODE_IN_DEVICE      //���豸�Ĺ�̬�洢���м��ز������������е��ͺŶ�֧�ִ�����ж�д�����飬��tSdkCameraCapbility.bParamInDevice����
  }
  
    //SDK���ɵ��������ҳ������ֵ
    public enum emSdkPropSheetMask  
    {
      PROP_SHEET_INDEX_EXPOSURE = 0,
      PROP_SHEET_INDEX_ISP_COLOR,
      PROP_SHEET_INDEX_ISP_LUT,
      PROP_SHEET_INDEX_ISP_SHAPE,
      PROP_SHEET_INDEX_VIDEO_FORMAT,
      PROP_SHEET_INDEX_RESOLUTION,
      PROP_SHEET_INDEX_IO_CTRL,
      PROP_SHEET_INDEX_TRIGGER_SET,
      PROP_SHEET_INDEX_OVERLAY
    }

    //SDK���ɵ��������ҳ��Ļص���Ϣ����
    public enum emSdkPropSheetMsg 
    {
      SHEET_MSG_LOAD_PARAM_DEFAULT = 0, //�������ָ���Ĭ�Ϻ󣬴�������Ϣ
      SHEET_MSG_LOAD_PARAM_GROUP,       //����ָ�������飬��������Ϣ
      SHEET_MSG_LOAD_PARAM_FROMFILE,    //��ָ���ļ����ز����󣬴�������Ϣ
      SHEET_MSG_SAVE_PARAM_GROUP        //��ǰ�����鱻����ʱ����������Ϣ
    }

    //���ӻ�ѡ��ο����ڵ�����
    public enum emSdkRefWintype 
    {
      REF_WIN_AUTO_EXPOSURE = 0,
      REF_WIN_WHITE_BALANCE,
    }

    //������豸��Ϣ��ֻ����Ϣ�������޸�
    public struct tSdkCameraDevInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public byte[] acProductSeries; // ��Ʒϵ��
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public byte[] acProductName;    // ��Ʒ����
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public byte[] acFriendlyName;   // �ǳƣ����#��������������
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public byte[] acLinkName;       // �豸�������������ڲ�ʹ��
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public byte[] acDriverVersion;  // �����汾
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public byte[] acSensorType;     // sensor����
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public byte[] acPortType;       // �ӿ�����  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public byte[] acSn;             // ��ƷΨһ���к�
        public uint   uInstance;        // ���ͺ�����ڸõ����ϵ�ʵ�������ţ���������ͬ�ͺŶ����
        
    } 

    //����ķֱ����趨��Χ
    public struct tSdkResolutionRange
    {
        public int iHeightMax;            //ͼ�����߶�
        public int iHeightMin;            //ͼ����С�߶�
        public int iWidthMax;             //ͼ�������
        public int iWidthMin;             //ͼ����С���
        public int uSkipModeMask;         //SKIPģʽ���룬Ϊ0����ʾ��֧��SKIP ��bit0Ϊ1,��ʾ֧��SKIP 2x2 ;bit1Ϊ1����ʾ֧��SKIP 3x3....
        public int uBinSumModeMask;       //BIN(���)ģʽ���룬Ϊ0����ʾ��֧��BIN ��bit0Ϊ1,��ʾ֧��BIN 2x2 ;bit1Ϊ1����ʾ֧��BIN 3x3....
        public int uBinAverageModeMask;   //BIN(���ֵ)ģʽ���룬Ϊ0����ʾ��֧��BIN ��bit0Ϊ1,��ʾ֧��BIN 2x2 ;bit1Ϊ1����ʾ֧��BIN 3x3....
        public int uResampleMask;         //Ӳ���ز���������
    } 

    //����ķֱ�������
    public struct tSdkImageResolution
    {
        public int     iIndex;               // �����ţ�[0,N]��ʾԤ��ķֱ���(N ΪԤ��ֱ��ʵ���������һ�㲻����20),OXFF ��ʾ�Զ���ֱ���(ROI)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[]  acDescription;        // �÷ֱ��ʵ�������Ϣ����Ԥ��ֱ���ʱ����Ϣ��Ч���Զ���ֱ��ʿɺ��Ը���Ϣ
        public uint    uBinMode;             // �Ƿ�BIN�ĳߴ�,16bitΪ1��ʾsum��Ϊ0��ʾaverage����16λ��ʾBIN�����С��Ϊ0��ʾ��ֹBINģʽ,��Χ���ܳ���tSdkResolutionRange��uBinModeMask
        public uint    uSkipMode;            // �Ƿ�SKIP�ĳߴ磬Ϊ0��ʾ��ֹSKIPģʽ��1��ʾSKIP2X2��2��ʾSKIP3X3���Դ�����,��Χ���ܳ���tSdkResolutionRange��uSkipModeMask
        public float   fZoomScale;           // ����֧�����Ź��ܵ������fZoomScale��ʾ����Բ��񵽵�ͼ������ű��������iWidthΪ100��fZoomScaleΪ2.0����ôʵ��Sensor���ԭʼ��ͼ����Ϊ50
        public int     iWidth;               // ����Bin��Skip��Zoom�Ժ�õ���ͼ��Ŀ��
        public int     iHeight;              // ����Bin��Skip��Zoom�Ժ�õ���ͼ��ĸ߶�
        public int     iHOffset;             // �����Sensor�ӳ����ϽǵĴ�ֱƫ��
        public int     iVOffset;             // �����Sensor�ӳ����Ͻǵ�ˮƽƫ��
    } 

    //�����ƽ��ģʽ������Ϣ
    public struct tSdkColorTemperatureDes
    {
        public int iIndex;              // ģʽ������
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[] acDescription;    // ������Ϣ
    } 

    //���֡��������Ϣ
    public struct tSdkFrameSpeed
    {
        public int iIndex;            // ֡�������ţ�һ��0��Ӧ�ڵ���ģʽ��1��Ӧ����ͨģʽ��2��Ӧ�ڸ���ģʽ      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[] acDescription;  // ������Ϣ      
    } 

    //����ع⹦�ܷ�Χ����
    public struct tSdkExpose
    {           
        public uint  uiTargetMin;       //�Զ��ع�����Ŀ����Сֵ     
        public uint  uiTargetMax;       //�Զ��ع�����Ŀ�����ֵ         
        public uint  uiAnalogGainMin;   //ģ���������Сֵ����λΪfAnalogGainStep�ж���      
        public uint  uiAnalogGainMax;   //ģ����������ֵ����λΪfAnalogGainStep�ж���        
        public float fAnalogGainStep;   //ģ������ÿ����1����Ӧ�����ӵķŴ��������磬uiAnalogGainMinһ��Ϊ16��fAnalogGainStepһ��Ϊ0.125����ô��С�Ŵ�������16*0.125 = 2��
        public uint  uiExposeTimeMin;   //�ֶ�ģʽ�£��ع�ʱ�����Сֵ����λ:�С�����CameraGetExposureLineTime���Ի��һ�ж�Ӧ��ʱ��(΢��),�Ӷ��õ���֡���ع�ʱ��    
        public uint  uiExposeTimeMax;   //�ֶ�ģʽ�£��ع�ʱ������ֵ����λ:��        
    } 

    //����ģʽ����
    public struct tSdkTrigger
    {
        public int      iIndex;             //ģʽ������      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[]   acDescription;      //��ģʽ��������Ϣ    
    } 

    //����ְ���С����(��Ҫ��������������Ч)
    public struct tSdkPackLength
    {
        public int    iIndex;        //�ְ���С������      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[] acDescription; //��Ӧ��������Ϣ     
        public uint   iPackSize;
    } 

    //Ԥ���LUT������
    public struct tSdkPresetLut
    {
        public int  iIndex;             //���     
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[] acDescription;    //������Ϣ
    } 
  
  //AE�㷨����
    public struct tSdkAeAlgorithm 
    {
        public int  iIndex;                 //���   
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[] acDescription;        //������Ϣ
    } 
  
    //RAWתRGB�㷨����
    public struct tSdkBayerDecodeAlgorithm
    {
        public int  iIndex;                 //���  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[] acDescription;        //������Ϣ
    } 

    //֡��ͳ����Ϣ
    public struct tSdkFrameStatistic
    {
        public int iTotal;        //��ǰ�ɼ�����֡������������֡��
        public int iCapture;      //��ǰ�ɼ�����Ч֡������    
        public int iLost;         //��ǰ��֡������    
    } 

    //��������ͼ�����ݸ�ʽ
    public struct tSdkMediaType
    {
        public int    iIndex;                 //��ʽ������
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
        public byte[] acDescription;          //������Ϣ
        public uint   iMediaType;             //��Ӧ��ͼ���ʽ���룬��CAMERA_MEDIA_TYPE_BAYGR8���ڱ��ļ����ж��塣
    } 

    //٤����趨��Χ
    public struct tGammaRange
    {
        public int iMin;       //��Сֵ
        public int iMax;       //���ֵ
    } 

    //�Աȶȵ��趨��Χ
    public struct tContrastRange
    {
        public int iMin;    //��Сֵ
        public int iMax;    //���ֵ
    } 

    //RGB��ͨ������������趨��Χ
    public struct tRgbGainRange
    { 
        public int iRGainMin;   //��ɫ�������Сֵ
        public int iRGainMax;   //��ɫ��������ֵ
        public int iGGainMin;   //��ɫ�������Сֵ
        public int iGGainMax;   //��ɫ��������ֵ
        public int iBGainMin;   //��ɫ�������Сֵ
        public int iBGainMax;   //��ɫ��������ֵ
    } 

    //���Ͷ��趨�ķ�Χ
    public struct tSaturationRange
    {  
        public int iMin;    //��Сֵ
        public int iMax;    //���ֵ
    } 

    //�񻯵��趨��Χ
    public struct tSharpnessRange
    {  
      public int iMin;    //��Сֵ
      public int iMax;    //���ֵ
    } 

    //ISPģ���ʹ����Ϣ
    public struct tSdkIspCapacity
    {
        public uint bMonoSensor;        //��ʾ���ͺ�����Ƿ�Ϊ�ڰ����,����Ǻڰ����������ɫ��صĹ��ܶ��޷�����
        public uint bWbOnce;            //��ʾ���ͺ�����Ƿ�֧���ֶ���ƽ�⹦��  
        public uint bAutoWb;            //��ʾ���ͺ�����Ƿ�֧���Զ���ƽ�⹦��
        public uint bAutoExposure;      //��ʾ���ͺ�����Ƿ�֧���Զ��ع⹦��
        public uint bManualExposure;    //��ʾ���ͺ�����Ƿ�֧���ֶ��ع⹦��
        public uint bAntiFlick;         //��ʾ���ͺ�����Ƿ�֧�ֿ�Ƶ������
        public uint bDeviceIsp;         //��ʾ���ͺ�����Ƿ�֧��Ӳ��ISP����
        public uint bForceUseDeviceIsp; //bDeviceIsp��bForceUseDeviceIspͬʱΪTRUEʱ����ʾǿ��ֻ��Ӳ��ISP������ȡ����
        public uint bZoomHD;            //���Ӳ���Ƿ�֧��ͼ���������(ֻ������С)��
    }

    /* �������ϵ��豸������Ϣ����Щ��Ϣ�������ڶ�̬����UI */
    public struct tSdkCameraCapbility
    {
        public IntPtr         pTriggerDesc;
        public int            iTriggerDesc;           //����ģʽ�ĸ�������pTriggerDesc����Ĵ�С

        public IntPtr         pImageSizeDesc;         //Ԥ��ֱ���ѡ��
        public int            iImageSizeDesc;         //Ԥ��ֱ��ʵĸ�������pImageSizeDesc����Ĵ�С 

        public IntPtr         pClrTempDesc;           //Ԥ��ɫ��ģʽ�����ڰ�ƽ��
        public int            iClrTempDesc;

        public IntPtr         pMediaTypeDesc;         //������ͼ���ʽ
        public int            iMediaTypdeDesc;        //������ͼ���ʽ�������������pMediaTypeDesc����Ĵ�С��

        public IntPtr         pFrameSpeedDesc;        //�ɵ���֡�����ͣ���Ӧ��������ͨ ���� �ͳ��������ٶ�����
        public int            iFrameSpeedDesc;        //�ɵ���֡�����͵ĸ�������pFrameSpeedDesc����Ĵ�С��

        public IntPtr         pPackLenDesc;           //��������ȣ�һ�����������豸
        public int            iPackLenDesc;           //�ɹ�ѡ��Ĵ���ְ����ȵĸ�������pPackLenDesc����Ĵ�С�� 

        public int            iOutputIoCounts;        //�ɱ�����IO�ĸ���
        public int            iInputIoCounts;         //�ɱ������IO�ĸ���

        public IntPtr         pPresetLutDesc;         //���Ԥ���LUT��
        public int            iPresetLut;             //���Ԥ���LUT��ĸ�������pPresetLutDesc����Ĵ�С

        public int            iUserDataMaxLen;        //ָʾ����������ڱ����û�����������󳤶ȡ�Ϊ0��ʾ�ޡ�
        public uint           bParamInDevice;         //ָʾ���豸�Ƿ�֧�ִ��豸�ж�д�����顣1Ϊ֧�֣�0��֧�֡�

        public IntPtr         pAeAlmSwDesc;           //����Զ��ع��㷨����
        public int            iAeAlmSwDesc;           //����Զ��ع��㷨����

        public IntPtr         pAeAlmHdDesc;           //Ӳ���Զ��ع��㷨������ΪNULL��ʾ��֧��Ӳ���Զ��ع�
        public int            iAeAlmHdDesc;           //Ӳ���Զ��ع��㷨������Ϊ0��ʾ��֧��Ӳ���Զ��ع�

        public IntPtr         pBayerDecAlmSwDesc;     //���Bayerת��ΪRGB���ݵ��㷨����
        public int            iBayerDecAlmSwDesc;     //���Bayerת��ΪRGB���ݵ��㷨����

        public IntPtr         pBayerDecAlmHdDesc;     //Ӳ��Bayerת��ΪRGB���ݵ��㷨������ΪNULL��ʾ��֧��
        public int            iBayerDecAlmHdDesc;     //Ӳ��Bayerת��ΪRGB���ݵ��㷨������Ϊ0��ʾ��֧��

      /* ͼ������ĵ��ڷ�Χ����,���ڶ�̬����UI*/
        public tSdkExpose             sExposeDesc;      //�ع�ķ�Χֵ
        public tSdkResolutionRange    sResolutionRange; //�ֱ��ʷ�Χ����  
        public tRgbGainRange          sRgbGainRange;    //ͼ���������淶Χ����  
        public tSaturationRange       sSaturationRange; //���Ͷȷ�Χ����  
        public tGammaRange            sGammaRange;      //٤��Χ����  
        public tContrastRange         sContrastRange;   //�Աȶȷ�Χ����  
        public tSharpnessRange        sSharpnessRange;  //�񻯷�Χ����  
        public tSdkIspCapacity        sIspCapacity;     //ISP��������
        
    } 

    //ͼ��֡ͷ��Ϣ
    public struct tSdkFrameHead  
    {
      public uint    uiMediaType;       // ͼ���ʽ,Image Format
      public uint    uBytes;            // ͼ�������ֽ���,Total bytes
      public int     iWidth;            // ��� Image height
      public int     iHeight;           // �߶� Image width
      public int     iWidthZoomSw;      // ������ŵĿ��,����Ҫ��������ü���ͼ�񣬴˱�������Ϊ0.
      public int     iHeightZoomSw;     // ������ŵĸ߶�,����Ҫ��������ü���ͼ�񣬴˱�������Ϊ0.
      public uint    bIsTrigger;        // ָʾ�Ƿ�Ϊ����֡ is trigger 
      public uint    uiTimeStamp;       // ��֡�Ĳɼ�ʱ�䣬��λ0.1���� 
      public uint    uiExpTime;         // ��ǰͼ����ع�ֵ����λΪ΢��us
      public float   fAnalogGain;       // ��ǰͼ���ģ�����汶��
      public int     iGamma;            // ��֡ͼ���٤���趨ֵ������LUTģʽΪ��̬��������ʱ��Ч������ģʽ��Ϊ-1
      public int     iContrast;         // ��֡ͼ��ĶԱȶ��趨ֵ������LUTģʽΪ��̬��������ʱ��Ч������ģʽ��Ϊ-1
      public int     iSaturation;       // ��֡ͼ��ı��Ͷ��趨ֵ�����ںڰ���������壬Ϊ0
      public float   fRgain;            // ��֡ͼ����ĺ�ɫ�������汶�������ںڰ���������壬Ϊ1
      public float   fGgain;            // ��֡ͼ�������ɫ�������汶�������ںڰ���������壬Ϊ1
      public float   fBgain;            // ��֡ͼ�������ɫ�������汶�������ںڰ���������壬Ϊ1
    }

    //ͼ��֡����
    public struct tSdkFrame
    {
      public tSdkFrameHead   head;        //֡ͷ
      public uint            pBuffer;     //������
    }

    //ͼ�񲶻�Ļص���������
    public delegate void CAMERA_SNAP_PROC(CameraHandle hCamera, uint pFrameBuffer, ref tSdkFrameHead pFrameHead, uint pContext);

    //SDK���ɵ��������ҳ�����Ϣ�ص���������
    public delegate void CAMERA_PAGE_MSG_PROC(CameraHandle hCamera, uint MSG, uint uParam, uint pContext);

    static public class MvApi
    {
  
    /******************************************************/
    // ������   : CameraSdkInit
    // �������� : ���SDK��ʼ�����ڵ����κ�SDK�����ӿ�ǰ������
    //        �ȵ��øýӿڽ��г�ʼ�����ú�����������������
    //        �ڼ�ֻ��Ҫ����һ�Ρ�   
    // ����     : iLanguageSel ����ѡ��SDK�ڲ���ʾ��Ϣ�ͽ��������,
    //               0:��ʾӢ��,1:��ʾ���ġ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]  
    public static extern CameraSdkStatus CameraSdkInit(
        int     iLanguageSel
    );

    /******************************************************/
    // ������   : CameraEnumerateDevice
    // �������� : ö���豸���������豸�б��ڵ���CameraInit
    //        ֮ǰ��������øú���������豸����Ϣ��    
    // ����     : pDSCameraList �豸�б�����ָ�롣
    //             piNums        �豸�ĸ���ָ�룬����ʱ����pDSCameraList
    //                            �����Ԫ�ظ�������������ʱ������ʵ���ҵ����豸������
    //              ע�⣬piNumsָ���ֵ�����ʼ�����Ҳ�����pDSCameraList����Ԫ�ظ�����
    //              �����п�������ڴ������
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]  
    public static extern CameraSdkStatus CameraEnumerateDevice(
        IntPtr                  pDSCameraList,
    ref int                     piNums
    );

    /******************************************************/
    // ������   : CameraInit
    // �������� : �����ʼ������ʼ���ɹ��󣬲��ܵ����κ�����
    //        �����صĲ����ӿڡ�    
    // ����     : pCameraInfo    ��������豸������Ϣ����CameraEnumerateDevice
    //               ������á� 
    //            iParamLoadMode  �����ʼ��ʱʹ�õĲ������ط�ʽ��-1��ʾʹ���ϴ��˳�ʱ�Ĳ������ط�ʽ��
    //            emTeam         ��ʼ��ʱʹ�õĲ����顣-1��ʾ�����ϴ��˳�ʱ�Ĳ����顣
    //            pCameraHandle  ����ľ��ָ�룬��ʼ���ɹ��󣬸�ָ��
    //               ���ظ��������Ч������ڵ����������
    //               ��صĲ����ӿ�ʱ������Ҫ����þ������Ҫ
    //               ���ڶ����֮������֡�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")] 
    public static extern CameraSdkStatus CameraInit(
    ref tSdkCameraDevInfo pCameraInfo,
        int               emParamLoadMode,
        int               emTeam,
    ref CameraHandle      pCameraHandle
    );

    /******************************************************/
    // ������   : CameraSetCallbackFunction
    // �������� : ����ͼ�񲶻�Ļص��������������µ�ͼ������֡ʱ��
    //        pCallBack��ָ��Ļص������ͻᱻ���á� 
    // ����     : hCamera ����ľ������CameraInit������á�
    //            pCallBack �ص�����ָ�롣
    //            pContext  �ص������ĸ��Ӳ������ڻص�����������ʱ
    //            �ø��Ӳ����ᱻ���룬����ΪNULL��������
    //            ������ʱЯ��������Ϣ��
    //            pCallbackOld  ���ڱ��浱ǰ�Ļص�����������ΪNULL��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")] 
    public static extern CameraSdkStatus CameraSetCallbackFunction(
        CameraHandle        hCamera,
        CAMERA_SNAP_PROC    pCallBack,
        IntPtr              pContext,
    ref CAMERA_SNAP_PROC    pCallbackOld
    );

    /******************************************************/
    // ������   : CameraUnInit
    // �������� : �������ʼ�����ͷ���Դ��
    // ����     : hCamera ����ľ������CameraInit������á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")] 
    public static extern CameraSdkStatus CameraUnInit(
        CameraHandle    hCamera
    );

    /******************************************************/
    // ������   : CameraGetInformation
    // �������� : ��������������Ϣ
    // ����     : hCamera ����ľ������CameraInit������á�
    //            pbuffer ָ�����������Ϣָ���ָ�롣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetInformation(
        CameraHandle    hCamera, 
        ref uint        pbuffer
    );

    /******************************************************/
    // ������   : CameraImageProcess
    // �������� : ����õ����ԭʼ���ͼ�����ݽ��д������ӱ��Ͷȡ�
    //        ��ɫ�����У��������ȴ���Ч�������õ�RGB888
    //        ��ʽ��ͼ�����ݡ�  
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbyIn    ����ͼ�����ݵĻ�������ַ������ΪNULL�� 
    //            pbyOut   �����ͼ������Ļ�������ַ������ΪNULL��
    //            pFrInfo  ����ͼ���֡ͷ��Ϣ��������ɺ�֡ͷ��Ϣ
    //             �е�ͼ���ʽuiMediaType����֮�ı䡣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraImageProcess(
        CameraHandle        hCamera,
        uint                pbyIn,
        IntPtr              pbyOut,
    ref tSdkFrameHead       pFrInfo
    );

    /******************************************************/
    // ������   : CameraDisplayInit
    // �������� : ��ʼ��SDK�ڲ�����ʾģ�顣�ڵ���CameraDisplayRGB24
    //        ǰ�����ȵ��øú�����ʼ����������ڶ��ο����У�
    //        ʹ���Լ��ķ�ʽ����ͼ����ʾ(������CameraDisplayRGB24)��
    //        ����Ҫ���ñ�������  
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            IntPtrDisplay ��ʾ���ڵľ����һ��Ϊ���ڵ�m_IntPtr��Ա��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraDisplayInit(
        CameraHandle    hCamera,
        IntPtr IntPtrDisplay
    );

    /******************************************************/
    // ������   : CameraDisplayRGB24
    // �������� : ��ʾͼ�񡣱�����ù�CameraDisplayInit����
    //        ��ʼ�����ܵ��ñ�������  
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbyRGB24 ͼ������ݻ�������RGB888��ʽ��
    //            pFrInfo  ͼ���֡ͷ��Ϣ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraDisplayRGB24(
        CameraHandle        hCamera,
        IntPtr              pbyRGB24, 
    ref tSdkFrameHead       pFrInfo
    );

    /******************************************************/
    // ������   : CameraSetDisplayMode
    // �������� : ������ʾ��ģʽ��������ù�CameraDisplayInit
    //        ���г�ʼ�����ܵ��ñ�������
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iMode    ��ʾģʽ��DISPLAYMODE_SCALE����
    //             DISPLAYMODE_REAL,����μ�CameraDefine.h
    //             ��emSdkDisplayMode�Ķ��塣    
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetDisplayMode(
        CameraHandle    hCamera,
        int             iMode
    );

    /******************************************************/
    // ������   : CameraSetDisplayOffset
    // �������� : ������ʾ����ʼƫ��ֵ��������ʾģʽΪDISPLAYMODE_REAL
    //        ʱ��Ч��������ʾ�ؼ��Ĵ�СΪ320X240����ͼ���
    //        �ĳߴ�Ϊ640X480����ô��iOffsetX = 160,iOffsetY = 120ʱ
    //        ��ʾ���������ͼ��ľ���320X240��λ�á�������ù�
    //        CameraDisplayInit���г�ʼ�����ܵ��ñ�������
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            iOffsetX  ƫ�Ƶ�X���ꡣ
    //            iOffsetY  ƫ�Ƶ�Y���ꡣ
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetDisplayOffset(
        CameraHandle    hCamera,
        int             iOffsetX, 
        int             iOffsetY
    );

    /******************************************************/
    // ������   : CameraSetDisplaySize
    // �������� : ������ʾ�ؼ��ĳߴ硣������ù�
    //        CameraDisplayInit���г�ʼ�����ܵ��ñ�������
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            iWidth    ���
    //            iHeight   �߶�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetDisplaySize(
        CameraHandle    hCamera, 
        int             iWidth, 
        int             iHeight
    );

    /******************************************************/
    // ������   : CameraGetImageBuffer
    // �������� : ���һ֡ͼ�����ݡ�Ϊ�����Ч�ʣ�SDK��ͼ��ץȡʱ�������㿽�����ƣ�
    //        CameraGetImageBufferʵ�ʻ�����ں��е�һ����������ַ��
    //        �ú����ɹ����ú󣬱������CameraReleaseImageBuffer�ͷ���
    //        CameraGetImageBuffer�õ��Ļ�����,�Ա����ں˼���ʹ��
    //        �û�������  
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            pFrameInfo  ͼ���֡ͷ��Ϣָ�롣
    //            pbyBuffer   ָ��ͼ������ݵĻ�����ָ�롣����
    //              �������㿽�����������Ч�ʣ����
    //              ����ʹ����һ��ָ��ָ���ָ�롣
    //            uint wTimes ץȡͼ��ĳ�ʱʱ�䡣��λ���롣��
    //              wTimesʱ���ڻ�δ���ͼ����ú���
    //              �᷵�س�ʱ��Ϣ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetImageBuffer(
        CameraHandle        hCamera, 
    out tSdkFrameHead       pFrameInfo,
    out uint pbyBuffer,
        uint                wTimes
    );

    /******************************************************/
    // ������   : CameraSnapToBuffer
    // �������� : ץ��һ��ͼ�񵽻������С���������ץ��ģʽ������
    //        �Զ��л���ץ��ģʽ�ķֱ��ʽ���ͼ�񲶻�Ȼ��
    //        ���񵽵����ݱ��浽�������С�
    //        �ú����ɹ����ú󣬱������CameraReleaseImageBuffer
    //        �ͷ���CameraSnapToBuffer�õ��Ļ�������������ο�
    //        CameraGetImageBuffer�����Ĺ����������֡�  
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            pFrameInfo  ָ�룬����ͼ���֡ͷ��Ϣ��
    //            pbyBuffer   ָ��ָ���ָ�룬��������ͼ�񻺳����ĵ�ַ��
    //            uWaitTimeMs ��ʱʱ�䣬��λ���롣�ڸ�ʱ���ڣ������Ȼû��
    //              �ɹ���������ݣ��򷵻س�ʱ��Ϣ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSnapToBuffer(
        CameraHandle        hCamera,
    out tSdkFrameHead       pFrameInfo,
    out uint                pbyBuffer,
        uint                uWaitTimeMs
    );

    /******************************************************/
    // ������   : CameraReleaseImageBuffer
    // �������� : �ͷ���CameraGetImageBuffer��õĻ�������
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            pbyBuffer   ��CameraGetImageBuffer��õĻ�������ַ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraReleaseImageBuffer(
        CameraHandle    hCamera, 
        uint            pbyBuffer
    );

    /******************************************************/
    // ������   : CameraPlay
    // �������� : ��SDK���빤��ģʽ����ʼ��������������͵�ͼ��
    //        ���ݡ������ǰ����Ǵ���ģʽ������Ҫ���յ�
    //        ����֡�Ժ�Ż����ͼ��
    // ����     : hCamera   ����ľ������CameraInit������á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraPlay(
        CameraHandle hCamera
    );

    /******************************************************/
    // ������   : CameraPause
    // �������� : ��SDK������ͣģʽ�����������������ͼ�����ݣ�
    //        ͬʱҲ�ᷢ�������������ͣ������ͷŴ������
    //        ��ͣģʽ�£����Զ�����Ĳ����������ã���������Ч��  
    // ����     : hCamera   ����ľ������CameraInit������á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraPause(
        CameraHandle hCamera
    );

    /******************************************************/
    // ������   : CameraStop
    // �������� : ��SDK����ֹͣ״̬��һ���Ƿ���ʼ��ʱ���øú�����
    //        �ú��������ã������ٶ�����Ĳ����������á�
    // ����     : hCamera   ����ľ������CameraInit������á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraStop(
        CameraHandle hCamera
    );

    /******************************************************/
    // ������   : CameraInitRecord
    // �������� : ��ʼ��һ��¼��
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            iFormat   ¼��ĸ�ʽ����ǰֻ֧�ֲ�ѹ����MSCV���ַ�ʽ��  
    //              0:��ѹ����1:MSCV��ʽѹ����
    //            pcSavePath  ¼���ļ������·����
    //            b2GLimit    ���ΪTRUE,���ļ�����2Gʱ�Զ��ָ
    //            dwQuality   ¼����������ӣ�Խ��������Խ�á���Χ1��100.
    //            iFrameRate  ¼���֡�ʡ������趨�ı�ʵ�ʲɼ�֡�ʴ�
    //              �����Ͳ���©֡��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraInitRecord(
        CameraHandle    hCamera,
        int             iFormat,
        byte[]          pcSavePath,
        uint            b2GLimit,
        uint            dwQuality,
        int             iFrameRate
    );

    /******************************************************/
    // ������   : CameraStopRecord
    // �������� : ��������¼�񡣵�CameraInitRecord�󣬿���ͨ���ú���
    //        ������һ��¼�񣬲�����ļ����������
    // ����     : hCamera   ����ľ������CameraInit������á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraStopRecord(
        CameraHandle    hCamera
    );

    /******************************************************/
    // ������   : CameraPushFrame
    // �������� : ��һ֡���ݴ���¼�����С��������CameraInitRecord
    //        ���ܵ��øú�����CameraStopRecord���ú󣬲����ٵ���
    //        �ú������������ǵ�֡ͷ��Ϣ��Я����ͼ��ɼ���ʱ���
    //        ��Ϣ�����¼����Ծ�׼��ʱ��ͬ����������֡�ʲ��ȶ�
    //        ��Ӱ�졣
    // ����     : hCamera     ����ľ������CameraInit������á�
    //            pbyImageBuffer    ͼ������ݻ�������������RGB��ʽ��
    //            pFrInfo           ͼ���֡ͷ��Ϣ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraPushFrame(
        CameraHandle    hCamera,
        uint            pbyImageBuffer,
    ref tSdkFrameHead[] pFrInfo
    );

    /******************************************************/
    // ������   : CameraSaveImage
    // �������� : ��ͼ�񻺳��������ݱ����ͼƬ�ļ���
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            lpszFileName   ͼƬ�����ļ�����·����
    //            pbyImageBuffer ͼ������ݻ�������
    //            pFrInfo        ͼ���֡ͷ��Ϣ��
    //            byFileType     ͼ�񱣴�ĸ�ʽ��ȡֵ��Χ�μ�CameraDefine.h
    //               ��emSdkFileType�����Ͷ��塣Ŀǰ֧��  
    //               BMP��JPG��PNG��RAW���ָ�ʽ������RAW��ʾ
    //               ��������ԭʼ���ݣ�����RAW��ʽ�ļ�Ҫ��
    //               pbyImageBuffer��pFrInfo����CameraGetImageBuffer
    //               ��õ����ݣ�����δ��CameraImageProcessת��
    //               ��BMP��ʽ����֮�����Ҫ�����BMP��JPG����
    //               PNG��ʽ����pbyImageBuffer��pFrInfo����
    //               CameraImageProcess������RGB��ʽ���ݡ�
    //                 �����÷����Բο�Advanced�����̡�   
    //            byQuality      ͼ�񱣴���������ӣ���������ΪJPG��ʽ
    //                 ʱ�ò�����Ч����Χ1��100�������ʽ
    //                           ����д��0��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSaveImage(
        CameraHandle    hCamera,
        byte[]           lpszFileName,
        Byte[]          pbyImageBuffer,
    ref tSdkFrameHead   pFrInfo,
        Byte            byFileType,
        Byte            byQuality
    );

    /******************************************************/
    // ������   : CameraGetImageResolution
    // �������� : ��õ�ǰԤ���ķֱ��ʡ�
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            psCurVideoSize �ṹ��ָ�룬���ڷ��ص�ǰ�ķֱ��ʡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetImageResolution(
        CameraHandle            hCamera, 
    ref tSdkImageResolution     psCurVideoSize
    );

    /******************************************************/
    // ������   : CameraSetImageResolution
    // �������� : ����Ԥ���ķֱ��ʡ�
    // ����     : hCamera      ����ľ������CameraInit������á�
    //            pImageResolution �ṹ��ָ�룬���ڷ��ص�ǰ�ķֱ��ʡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetImageResolution(
        CameraHandle            hCamera, 
    ref tSdkImageResolution     pImageResolution
    );

    /******************************************************/
    // ������   : CameraGetMediaType
    // �������� : ��������ǰ���ԭʼ���ݵĸ�ʽ�����š�
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            piMediaType   ָ�룬���ڷ��ص�ǰ��ʽ���͵������š�
    //              ��CameraGetCapability�����������ԣ�
    //              ��tSdkCameraCapbility�ṹ���е�pMediaTypeDesc
    //              ��Ա�У����������ʽ���������֧�ֵĸ�ʽ��
    //              piMediaType��ָ��������ţ����Ǹ�����������š�
    //              pMediaTypeDesc[*piMediaType].iMediaType���ʾ��ǰ��ʽ�� 
    //              ���롣�ñ�����μ�CameraDefine.h��[ͼ���ʽ����]���֡�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetMediaType(
        CameraHandle    hCamera, 
    ref int             piMediaType
    );

    /******************************************************/
    // ������   : CameraSetMediaType
    // �������� : ������������ԭʼ���ݸ�ʽ��
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            iMediaType  ��CameraGetCapability�����������ԣ�
    //              ��tSdkCameraCapbility�ṹ���е�pMediaTypeDesc
    //              ��Ա�У����������ʽ���������֧�ֵĸ�ʽ��
    //              iMediaType���Ǹ�����������š�
    //              pMediaTypeDesc[iMediaType].iMediaType���ʾ��ǰ��ʽ�� 
    //              ���롣�ñ�����μ�CameraDefine.h��[ͼ���ʽ����]���֡�   
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetMediaType(
        CameraHandle    hCamera, 
        int             iMediaType
    );

    /******************************************************/
    // ������   : CameraSetAeState
    // �������� : ��������ع��ģʽ���Զ������ֶ���
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            bAeState    TRUE��ʹ���Զ��ع⣻FALSE��ֹͣ�Զ��ع⡣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetAeState(
        CameraHandle    hCamera, 
        uint            bAeState
    );

    /******************************************************/
    // ������   : CameraGetAeState
    // �������� : ��������ǰ���ع�ģʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pAeState   ָ�룬���ڷ����Զ��ع��ʹ��״̬��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetAeState(
        CameraHandle    hCamera, 
    ref uint            pAeState
    );

    /******************************************************/
    // ������   : CameraSetSharpness
    // �������� : ����ͼ��Ĵ�����񻯲�����
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iSharpness �񻯲�������Χ��CameraGetCapability
    //               ��ã�һ����[0,100]��0��ʾ�ر��񻯴���
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetSharpness(
        CameraHandle    hCamera, 
        int             iSharpness
    );

    /******************************************************/
    // ������   : CameraGetSharpness
    // �������� : ��ȡ��ǰ���趨ֵ��
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            piSharpness ָ�룬���ص�ǰ�趨���񻯵��趨ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetSharpness(
        CameraHandle    hCamera, 
    ref int             piSharpness
    );

    /******************************************************/
    // ������   : CameraSetLutMode
    // �������� : ��������Ĳ��任ģʽLUTģʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            emLutMode  LUTMODE_PARAM_GEN ��ʾ��٤��ͶԱȶȲ�����̬����LUT��
    //             LUTMODE_PRESET    ��ʾʹ��Ԥ���LUT��
    //             LUTMODE_USER_DEF  ��ʾʹ���û��Զ���LUT��
    //             LUTMODE_PARAM_GEN�Ķ���ο�CameraDefine.h��emSdkLutMode���͡�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetLutMode(
        CameraHandle    hCamera,
    ref int             emLutMode
    );

    /******************************************************/
    // ������   : CameraGetLutMode
    // �������� : �������Ĳ��任ģʽLUTģʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pemLutMode ָ�룬���ص�ǰLUTģʽ��������CameraSetLutMode
    //             ��emLutMode������ͬ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetLutMode(
        CameraHandle    hCamera,
    ref int             pemLutMode
    );

    /******************************************************/
    // ������   : CameraSelectLutPreset
    // �������� : ѡ��Ԥ��LUTģʽ�µ�LUT��������ʹ��CameraSetLutMode
    //        ��LUTģʽ����ΪԤ��ģʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iSel     ��������š���ĸ�����CameraGetCapability
    //             ��á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSelectLutPreset(
        CameraHandle    hCamera,
        int             iSel
    );

    /******************************************************/
    // ������   : CameraGetLutPresetSel
    // �������� : ���Ԥ��LUTģʽ�µ�LUT�������š�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            piSel      ָ�룬���ر�������š�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetLutPresetSel(
        CameraHandle    hCamera,
    ref int             piSel
    );

    /******************************************************/
    // ������   : CameraSetCustomLut
    // �������� : �����Զ����LUT��������ʹ��CameraSetLutMode
    //        ��LUTģʽ����Ϊ�Զ���ģʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //             iChannel ָ��Ҫ�趨��LUT��ɫͨ������ΪLUT_CHANNEL_ALLʱ��
    //                      ����ͨ����LUT����ͬʱ�滻��
    //                      �ο�CameraDefine.h��emSdkLutChannel���塣
    //            pLut     ָ�룬ָ��LUT��ĵ�ַ��LUT��Ϊ�޷��Ŷ��������飬�����СΪ
    //           4096���ֱ������ɫͨ����0��4096(12bit��ɫ����)��Ӧ��ӳ��ֵ�� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetCustomLut(
        CameraHandle    hCamera,
        int             iChannel,
    ref ushort[]        pLut
    );

    /******************************************************/
    // ������   : CameraGetCustomLut
    // �������� : ��õ�ǰʹ�õ��Զ���LUT��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //             iChannel ָ��Ҫ��õ�LUT��ɫͨ������ΪLUT_CHANNEL_ALLʱ��
    //                      ���غ�ɫͨ����LUT��
    //                      �ο�CameraDefine.h��emSdkLutChannel���塣
    //            pLut     ָ�룬ָ��LUT��ĵ�ַ��LUT��Ϊ�޷��Ŷ��������飬�����СΪ
    //           4096���ֱ������ɫͨ����0��4096(12bit��ɫ����)��Ӧ��ӳ��ֵ�� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetCustomLut(
        CameraHandle    hCamera,
        int             iChannel,
    ref ushort[] pLut
    );

    /******************************************************/
    // ������   : CameraGetCurrentLut
    // �������� : ��������ǰ��LUT�����κ�LUTģʽ�¶����Ե���,
    //        ����ֱ�۵Ĺ۲�LUT���ߵı仯��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //             iChannel ָ��Ҫ��õ�LUT��ɫͨ������ΪLUT_CHANNEL_ALLʱ��
    //                      ���غ�ɫͨ����LUT��
    //                      �ο�CameraDefine.h��emSdkLutChannel���塣
    //            pLut     ָ�룬ָ��LUT��ĵ�ַ��LUT��Ϊ�޷��Ŷ��������飬�����СΪ
    //           4096���ֱ������ɫͨ����0��4096(12bit��ɫ����)��Ӧ��ӳ��ֵ�� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetCurrentLut(
        CameraHandle    hCamera,
        int             iChannel,
    ref ushort[] pLut
    );

    /******************************************************/
    // ������   : CameraSetWbMode
    // �������� : ���������ƽ��ģʽ����Ϊ�ֶ����Զ����ַ�ʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            bAuto      TRUE�����ʾʹ���Զ�ģʽ��
    //             FALSE�����ʾʹ���ֶ�ģʽ��ͨ������
    //                 CameraSetOnceWB������һ�ΰ�ƽ�⡣        
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetWbMode(
        CameraHandle    hCamera,
        uint            bAuto
    );

    /******************************************************/
    // ������   : CameraGetWbMode
    // �������� : ��õ��ڵİ�ƽ��ģʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbAuto   ָ�룬����TRUE��ʾ�Զ�ģʽ��FALSE
    //             Ϊ�ֶ�ģʽ�� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetWbMode(
        CameraHandle    hCamera,
    ref uint            pbAuto
    );

    /******************************************************/
    // ������   : CameraSetPresetClrTemp
    // �������� : ����ɫ��ģʽ
    // ����     : hCamera  ����ľ������CameraInit������á�
    //             iSel     �����š�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetPresetClrTemp(
        CameraHandle    hCamera,
        int             iSel
    );

    /******************************************************/
    // ������   : CameraGetPresetClrTemp
    // �������� : 
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            int* piSel
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetPresetClrTemp(
        CameraHandle    hCamera,
    ref int             piSel
    );

    /******************************************************/
    // ������   : CameraSetOnceWB
    // �������� : ���ֶ���ƽ��ģʽ�£����øú��������һ�ΰ�ƽ�⡣
    //        ��Ч��ʱ��Ϊ���յ���һ֡ͼ������ʱ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetOnceWB(
        CameraHandle    hCamera
    );

    /******************************************************/
    // ������   : CameraSetOnceBB
    // �������� : ִ��һ�κ�ƽ�������
    // ����     : hCamera  ����ľ������CameraInit������á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetOnceBB(
        CameraHandle    hCamera
    );


    /******************************************************/
    // ������   : CameraSetAeTarget
    // �������� : �趨�Զ��ع������Ŀ��ֵ���趨��Χ��CameraGetCapability
    //        ������á�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iAeTarget  ����Ŀ��ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetAeTarget(
        CameraHandle    hCamera, 
        int             iAeTarget
    );

    /******************************************************/
    // ������   : CameraGetAeTarget
    // �������� : ����Զ��ع������Ŀ��ֵ��
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            *piAeTarget ָ�룬����Ŀ��ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetAeTarget(
        CameraHandle    hCamera, 
    ref int             piAeTarget
    );

    /******************************************************/
    // ������   : CameraSetExposureTime
    // �������� : �����ع�ʱ�䡣��λΪ΢�롣����CMOS�����������ع�
    //        �ĵ�λ�ǰ�����������ģ���ˣ��ع�ʱ�䲢������΢��
    //        ���������ɵ������ǻᰴ��������ȡ�ᡣ�ڵ���
    //        �������趨�ع�ʱ��󣬽����ٵ���CameraGetExposureTime
    //        �����ʵ���趨��ֵ��
    // ����     : hCamera      ����ľ������CameraInit������á�
    //            fExposureTime �ع�ʱ�䣬��λ΢�롣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetExposureTime(
        CameraHandle    hCamera, 
        double          fExposureTime
    );

    /******************************************************/
    // ������   : CameraGetExposureLineTime
    // �������� : ���һ�е��ع�ʱ�䡣����CMOS�����������ع�
    //        �ĵ�λ�ǰ�����������ģ���ˣ��ع�ʱ�䲢������΢��
    //        ���������ɵ������ǻᰴ��������ȡ�ᡣ���������
    //          ���þ��Ƿ���CMOS����ع�һ�ж�Ӧ��ʱ�䡣
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pfLineTime ָ�룬����һ�е��ع�ʱ�䣬��λΪ΢�롣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetExposureLineTime(
        CameraHandle    hCamera, 
    ref double          pfLineTime
    );

    /******************************************************/
    // ������   : CameraGetExposureTime
    // �������� : ���������ع�ʱ�䡣��μ�CameraSetExposureTime
    //        �Ĺ���������
    // ����     : hCamera        ����ľ������CameraInit������á�
    //            pfExposureTime   ָ�룬���ص�ǰ���ع�ʱ�䣬��λ΢�롣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetExposureTime(
        CameraHandle    hCamera, 
    ref double          pfExposureTime
    );

    /******************************************************/
    // ������   : CameraSetAnalogGain
    // �������� : ���������ͼ��ģ������ֵ����ֵ����CameraGetCapability���
    //        ��������Խṹ����sExposeDesc.fAnalogGainStep����
    //        �õ�ʵ�ʵ�ͼ���źŷŴ�����
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            iAnalogGain �趨��ģ������ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetAnalogGain(
        CameraHandle    hCamera,
        int             iAnalogGain
    );

    /******************************************************/
    // ������   : CameraGetAnalogGain
    // �������� : ���ͼ���źŵ�ģ������ֵ���μ�CameraSetAnalogGain
    //        ��ϸ˵����
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            piAnalogGain ָ�룬���ص�ǰ��ģ������ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetAnalogGain(
        CameraHandle    hCamera, 
    ref int             piAnalogGain
    );

    /******************************************************/
    // ������   : CameraSetGain
    // �������� : ����ͼ����������档�趨��Χ��CameraGetCapability
    //        ��õ�������Խṹ����sRgbGainRange��Ա������
    //        ʵ�ʵķŴ������趨ֵ/100��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iRGain   ��ɫͨ��������ֵ�� 
    //            iGGain   ��ɫͨ��������ֵ��
    //            iBGain   ��ɫͨ��������ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetGain(
        CameraHandle    hCamera, 
        int             iRGain, 
        int             iGGain, 
        int             iBGain
    );


    /******************************************************/
    // ������   : CameraGetGain
    // �������� : ���ͼ������������档������μ�CameraSetGain
    //        �Ĺ����������֡�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            piRGain  ָ�룬���غ�ɫͨ������������ֵ��
    //            piGGain    ָ�룬������ɫͨ������������ֵ��
    //            piBGain    ָ�룬������ɫͨ������������ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetGain(
        CameraHandle    hCamera, 
    ref int             piRGain, 
    ref int             piGGain, 
    ref int             piBGain
    );


    /******************************************************/
    // ������   : CameraSetGamma
    // �������� : �趨LUT��̬����ģʽ�µ�Gammaֵ���趨��ֵ��
    //        ���ϱ�����SDK�ڲ�������ֻ�е�������ڶ�̬
    //        �������ɵ�LUTģʽʱ���Ż���Ч����ο�CameraSetLutMode
    //        �ĺ���˵�����֡�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iGamma     Ҫ�趨��Gammaֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetGamma(
        CameraHandle    hCamera, 
        int             iGamma
    );

    /******************************************************/
    // ������   : CameraGetGamma
    // �������� : ���LUT��̬����ģʽ�µ�Gammaֵ����ο�CameraSetGamma
    //        �����Ĺ���������
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            piGamma    ָ�룬���ص�ǰ��Gammaֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetGamma(
        CameraHandle    hCamera, 
    ref int             piGamma
    );

    /******************************************************/
    // ������   : CameraSetContrast
    // �������� : �趨LUT��̬����ģʽ�µĶԱȶ�ֵ���趨��ֵ��
    //        ���ϱ�����SDK�ڲ�������ֻ�е�������ڶ�̬
    //        �������ɵ�LUTģʽʱ���Ż���Ч����ο�CameraSetLutMode
    //        �ĺ���˵�����֡�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iContrast  �趨�ĶԱȶ�ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetContrast(
        CameraHandle    hCamera, 
        int             iContrast
    );

    /******************************************************/
    // ������   : CameraGetContrast
    // �������� : ���LUT��̬����ģʽ�µĶԱȶ�ֵ����ο�
    //        CameraSetContrast�����Ĺ���������
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            piContrast ָ�룬���ص�ǰ�ĶԱȶ�ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetContrast(
        CameraHandle    hCamera, 
    ref int             piContrast
    );

    /******************************************************/
    // ������   : CameraSetSaturation
    // �������� : �趨ͼ����ı��Ͷȡ��Ժڰ������Ч��
    //        �趨��Χ��CameraGetCapability��á�100��ʾ
    //        ��ʾԭʼɫ�ȣ�����ǿ��
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            iSaturation  �趨�ı��Ͷ�ֵ�� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetSaturation(
        CameraHandle    hCamera, 
        int             iSaturation
    );

    /******************************************************/
    // ������   : CameraGetSaturation
    // �������� : ���ͼ����ı��Ͷȡ�
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            piSaturation ָ�룬���ص�ǰͼ����ı��Ͷ�ֵ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetSaturation(
        CameraHandle    hCamera, 
    ref int             piSaturation
    );

    /******************************************************/
    // ������   : CameraSetMonochrome
    // �������� : ���ò�ɫתΪ�ڰ׹��ܵ�ʹ�ܡ�
    // ����     : hCamera ����ľ������CameraInit������á�
    //            bEnable   TRUE����ʾ����ɫͼ��תΪ�ڰס�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetMonochrome(
        CameraHandle    hCamera, 
        uint            bEnable
    );

    /******************************************************/
    // ������   : CameraGetMonochrome
    // �������� : ��ò�ɫת���ڰ׹��ܵ�ʹ��״����
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbEnable   ָ�롣����TRUE��ʾ�����˲�ɫͼ��
    //             ת��Ϊ�ڰ�ͼ��Ĺ��ܡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetMonochrome(
        CameraHandle    hCamera, 
    ref uint            pbEnable
    );

    /******************************************************/
    // ������   : CameraSetInverse
    // �������� : ���ò�ͼ����ɫ��ת���ܵ�ʹ�ܡ�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            bEnable    TRUE����ʾ����ͼ����ɫ��ת���ܣ�
    //             ���Ի�����ƽ����Ƭ��Ч����
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetInverse(
        CameraHandle    hCamera, 
        uint            bEnable
    );

    /******************************************************/
    // ������   : CameraGetInverse
    // �������� : ���ͼ����ɫ��ת���ܵ�ʹ��״̬��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbEnable   ָ�룬���ظù���ʹ��״̬�� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetInverse(
        CameraHandle    hCamera, 
    ref uint            pbEnable
    );

    /******************************************************/
    // ������   : CameraSetAntiFlick
    // �������� : �����Զ��ع�ʱ��Ƶ�����ܵ�ʹ��״̬�������ֶ�
    //        �ع�ģʽ����Ч��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            bEnable    TRUE��������Ƶ������;FALSE���رոù��ܡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetAntiFlick(
        CameraHandle    hCamera,
        uint            bEnable
    );

    /******************************************************/
    // ������   : CameraGetAntiFlick
    // �������� : ����Զ��ع�ʱ��Ƶ�����ܵ�ʹ��״̬��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbEnable   ָ�룬���ظù��ܵ�ʹ��״̬��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetAntiFlick(
        CameraHandle    hCamera, 
    ref uint            pbEnable
    );

    /******************************************************/
    // ������   : CameraGetLightFrequency
    // �������� : ����Զ��ع�ʱ����Ƶ����Ƶ��ѡ��
    // ����     : hCamera      ����ľ������CameraInit������á�
    //            piFrequencySel ָ�룬����ѡ��������š�0:50HZ 1:60HZ
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetLightFrequency(
        CameraHandle    hCamera, 
    ref int             piFrequencySel
    );

    /******************************************************/
    // ������   : CameraSetLightFrequency
    // �������� : �����Զ��ع�ʱ��Ƶ����Ƶ�ʡ�
    // ����     : hCamera     ����ľ������CameraInit������á�
    //            iFrequencySel 0:50HZ , 1:60HZ 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetLightFrequency(
        CameraHandle    hCamera,
        int             iFrequencySel
    );

    /******************************************************/
    // ������   : CameraSetFrameSpeed
    // �������� : �趨������ͼ���֡�ʡ�����ɹ�ѡ���֡��ģʽ��
    //        CameraGetCapability��õ���Ϣ�ṹ����iFrameSpeedDesc
    //        ��ʾ���֡��ѡ��ģʽ������
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            iFrameSpeed ѡ���֡��ģʽ�����ţ���Χ��0��
    //              CameraGetCapability��õ���Ϣ�ṹ����iFrameSpeedDesc - 1   
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetFrameSpeed(
        CameraHandle    hCamera, 
        int             iFrameSpeed
    );

    /******************************************************/
    // ������   : CameraGetFrameSpeed
    // �������� : ���������ͼ���֡��ѡ�������š������÷��ο�
    //        CameraSetFrameSpeed�����Ĺ����������֡�
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            piFrameSpeed ָ�룬����ѡ���֡��ģʽ�����š� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetFrameSpeed(
        CameraHandle    hCamera, 
    ref int             piFrameSpeed
    );

   
  /******************************************************/
  // ������   : CameraSetParameterMode
  // �������� : �趨������ȡ��Ŀ�����
  // ����     : hCamera  ����ľ������CameraInit������á�
  //            iMode  ������ȡ�Ķ��󡣲ο�
  //          emSdkParameterMode�����Ͷ��塣
  // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus
  //            �д�����Ķ��塣
  /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetParameterMode(
        CameraHandle    hCamera, 
        int             iTarget
    );

    /******************************************************/
    // ������   : CameraGetParameterMode
    // �������� : 
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            int* piTarget
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetParameterMode(
        CameraHandle    hCamera, 
    ref int             piTarget
    );

    /******************************************************/
    // ������   : CameraSetParameterMask
    // �������� : ���ò�����ȡ�����롣�������غͱ���ʱ����ݸ�
    //        ��������������ģ��������Ƿ���ػ��߱��档
    // ����     : hCamera ����ľ������CameraInit������á�
    //            uMask     ���롣�ο�CameraDefine.h��PROP_SHEET_INDEX
    //            ���Ͷ��塣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetParameterMask(
        CameraHandle    hCamera, 
        uint            uMask
    );

    /******************************************************/
    // ������   : CameraSaveParameter
    // �������� : ���浱ǰ���������ָ���Ĳ������С�����ṩ��A,B,C,D
    //        A,B,C,D����ռ������в����ı��档 
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iTeam      PARAMETER_TEAM_A ���浽A����,
    //             PARAMETER_TEAM_B ���浽B����,
    //             PARAMETER_TEAM_C ���浽C����,
    //             PARAMETER_TEAM_D ���浽D����
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSaveParameter(
        CameraHandle    hCamera, 
        int             iTeam
    );

    /******************************************************/
    // ������   : CameraReadParameterFromFile
    // �������� : ��PC��ָ���Ĳ����ļ��м��ز������ҹ�˾�������
    //        ������PC��Ϊ.config��׺���ļ���λ�ڰ�װ�µ�
    //        Camera\Configs�ļ����С�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            *sFileName �����ļ�������·����
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraReadParameterFromFile(
        CameraHandle    hCamera,
        byte[]          sFileName
    );

    /******************************************************/
    // ������   : CameraLoadParameter
    // �������� : ����ָ����Ĳ���������С�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iTeam    PARAMETER_TEAM_A ����A�����,
    //             PARAMETER_TEAM_B ����B�����,
    //             PARAMETER_TEAM_C ����C�����,
    //             PARAMETER_TEAM_D ����D�����,
    //             PARAMETER_TEAM_DEFAULT ����Ĭ�ϲ�����    
    //             ���Ͷ���ο�CameraDefine.h��emSdkParameterTeam����
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraLoadParameter(
        CameraHandle    hCamera, 
        int             iTeam
    );

    /******************************************************/
    // ������   : CameraGetCurrentParameterGroup
    // �������� : ��õ�ǰѡ��Ĳ����顣
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            piTeam     ָ�룬���ص�ǰѡ��Ĳ����顣����ֵ
    //             �ο�CameraLoadParameter��iTeam������
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetCurrentParameterGroup(
        CameraHandle    hCamera, 
    ref int             piTeam
    );

    /******************************************************/
    // ������   : CameraSetTransPackLen
    // �������� : �����������ͼ�����ݵķְ���С��
    //        Ŀǰ��SDK�汾�У��ýӿڽ���GIGE�ӿ������Ч��
    //        �����������紫��ķְ���С������֧�־�֡��������
    //        ���ǽ���ѡ��8K�ķְ���С��������Ч�Ľ��ʹ���
    //        ��ռ�õ�CPU����ʱ�䡣
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iPackSel   �ְ�����ѡ��������š��ְ����ȿ���
    //             ���������Խṹ����pPackLenDesc��Ա������
    //             iPackLenDesc��Ա���ʾ����ѡ�ķְ�ģʽ������
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetTransPackLen(
        CameraHandle    hCamera, 
        int             iPackSel
    );

    /******************************************************/
    // ������   : CameraGetTransPackLen
    // �������� : ��������ǰ����ְ���С��ѡ�������š�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            piPackSel  ָ�룬���ص�ǰѡ��ķְ���С�����š�
    //             �μ�CameraSetTransPackLen��iPackSel��
    //             ˵����
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetTransPackLen(
        CameraHandle    hCamera, 
    ref int             piPackSel
    );

    /******************************************************/
    // ������   : CameraIsAeWinVisible
    // �������� : ����Զ��ع�ο����ڵ���ʾ״̬��
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            pbIsVisible  ָ�룬����TRUE�����ʾ��ǰ���ڻ�
    //               ��������ͼ�������ϡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraIsAeWinVisible(
        CameraHandle    hCamera,
    ref uint            pbIsVisible
    );

    /******************************************************/
    // ������   : CameraSetAeWinVisible
    // �������� : �����Զ��ع�ο����ڵ���ʾ״̬�������ô���״̬
    //        Ϊ��ʾ������CameraImageOverlay���ܹ�������λ��
    //        �Ծ��εķ�ʽ������ͼ���ϡ�
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            bIsVisible  TRUE������Ϊ��ʾ��FALSE������ʾ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetAeWinVisible(
        CameraHandle    hCamera,
        uint            bIsVisible
    );

    /******************************************************/
    // ������   : CameraGetAeWindow
    // �������� : ����Զ��ع�ο����ڵ�λ�á�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            piHOff     ָ�룬���ش���λ�����ϽǺ�����ֵ��
    //            piVOff     ָ�룬���ش���λ�����Ͻ�������ֵ��
    //            piWidth    ָ�룬���ش��ڵĿ�ȡ�
    //            piHeight   ָ�룬���ش��ڵĸ߶ȡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetAeWindow(
        CameraHandle    hCamera, 
    ref int            piHOff, 
    ref int            piVOff, 
    ref int            piWidth, 
    ref int            piHeight
    );

    /******************************************************/
    // ������   : CameraSetAeWindow
    // �������� : �����Զ��ع�Ĳο����ڡ�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iHOff    �������Ͻǵĺ�����
    //            iVOff      �������Ͻǵ�������
    //            iWidth     ���ڵĿ�� 
    //            iHeight    ���ڵĸ߶�
    //        ���iHOff��iVOff��iWidth��iHeightȫ��Ϊ0����
    //        ��������Ϊÿ���ֱ����µľ���1/2��С����������
    //        �ֱ��ʵı仯������仯�����iHOff��iVOff��iWidth��iHeight
    //        �������Ĵ���λ�÷�Χ�����˵�ǰ�ֱ��ʷ�Χ�ڣ� 
    //          ���Զ�ʹ�þ���1/2��С���ڡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetAeWindow(
        CameraHandle    hCamera, 
        int             iHOff, 
        int             iVOff, 
        int             iWidth, 
        int             iHeight
    );

    /******************************************************/
    // ������   : CameraSetMirror
    // �������� : ����ͼ������������������Ϊˮƽ�ʹ�ֱ��������
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iDir     ��ʾ����ķ���0����ʾˮƽ����1����ʾ��ֱ����
    //            bEnable  TRUE��ʹ�ܾ���;FALSE����ֹ����
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetMirror(
        CameraHandle    hCamera, 
        int             iDir, 
        uint            bEnable
    );

    /******************************************************/
    // ������   : CameraGetMirror
    // �������� : ���ͼ��ľ���״̬��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iDir     ��ʾҪ��õľ�����
    //             0����ʾˮƽ����1����ʾ��ֱ����
    //            pbEnable   ָ�룬����TRUE�����ʾiDir��ָ�ķ���
    //             ����ʹ�ܡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetMirror(
        CameraHandle    hCamera, 
        int             iDir, 
    ref uint           pbEnable
    );

    /******************************************************/
    // ������   : CameraGetWbWindow
    // �������� : ��ð�ƽ��ο����ڵ�λ�á�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            PiHOff   ָ�룬���زο����ڵ����ϽǺ����� ��
    //            PiVOff     ָ�룬���زο����ڵ����Ͻ������� ��
    //            PiWidth    ָ�룬���زο����ڵĿ�ȡ�
    //            PiHeight   ָ�룬���زο����ڵĸ߶ȡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetWbWindow(
        CameraHandle    hCamera, 
    ref int             PiHOff, 
    ref int             PiVOff, 
    ref int             PiWidth, 
    ref int             PiHeight
    );

    /******************************************************/
    // ������   : CameraSetWbWindow
    // �������� : ���ð�ƽ��ο����ڵ�λ�á�
    // ����     : hCamera ����ľ������CameraInit������á�
    //            iHOff   �ο����ڵ����ϽǺ����ꡣ
    //            iVOff     �ο����ڵ����Ͻ������ꡣ
    //            iWidth    �ο����ڵĿ�ȡ�
    //            iHeight   �ο����ڵĸ߶ȡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetWbWindow(
        CameraHandle    hCamera, 
        int             iHOff, 
        int             iVOff, 
        int             iWidth, 
        int             iHeight
    );

    /******************************************************/
    // ������   : CameraIsWbWinVisible
    // �������� : ��ð�ƽ�ⴰ�ڵ���ʾ״̬��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbShow   ָ�룬����TRUE�����ʾ�����ǿɼ��ġ� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraIsWbWinVisible(
        CameraHandle    hCamera,
    ref uint            pbShow
    );

    /******************************************************/
    // ������   : CameraSetWbWinVisible
    // �������� : ���ð�ƽ�ⴰ�ڵ���ʾ״̬��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            bShow      TRUE�����ʾ����Ϊ�ɼ����ڵ���
    //             CameraImageOverlay��ͼ�������Ͻ��Ծ���
    //             �ķ�ʽ���Ӱ�ƽ��ο����ڵ�λ�á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetWbWinVisible(
        CameraHandle    hCamera, 
        uint            bShow
    );

    /******************************************************/
    // ������   : CameraImageOverlay
    // �������� : �������ͼ�������ϵ���ʮ���ߡ���ƽ��ο����ڡ�
    //        �Զ��ع�ο����ڵ�ͼ�Ρ�ֻ������Ϊ�ɼ�״̬��
    //        ʮ���ߺͲο����ڲ��ܱ������ϡ�
    //        ע�⣬�ú���������ͼ�������RGB��ʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pRgbBuffer ͼ�����ݻ�������
    //            pFrInfo    ͼ���֡ͷ��Ϣ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraImageOverlay(
        CameraHandle    hCamera,
        IntPtr         pRgbBuffer,
    ref tSdkFrameHead   pFrInfo
    );

    /******************************************************/
    // ������   : CameraSetCrossLine
    // �������� : ����ָ��ʮ���ߵĲ�����
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iLine    ��ʾҪ���õڼ���ʮ���ߵ�״̬����ΧΪ[0,8]����9����    
    //            x          ʮ��������λ�õĺ�����ֵ��
    //            y      ʮ��������λ�õ�������ֵ��
    //            uColor     ʮ���ߵ���ɫ����ʽΪ(R|(G<<8)|(B<<16))
    //            bVisible   ʮ���ߵ���ʾ״̬��TRUE����ʾ��ʾ��
    //             ֻ������Ϊ��ʾ״̬��ʮ���ߣ��ڵ���
    //             CameraImageOverlay��Żᱻ���ӵ�ͼ���ϡ�     
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetCrossLine(
        CameraHandle    hCamera, 
        int             iLine, 
        int             x,
        int             y,
        uint            uColor,
        uint            bVisible
    );

    /******************************************************/
    // ������   : CameraGetCrossLine
    // �������� : ���ָ��ʮ���ߵ�״̬��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iLine    ��ʾҪ��ȡ�ĵڼ���ʮ���ߵ�״̬����ΧΪ[0,8]����9����  
    //            px     ָ�룬���ظ�ʮ��������λ�õĺ����ꡣ
    //            py     ָ�룬���ظ�ʮ��������λ�õĺ����ꡣ
    //            pcolor     ָ�룬���ظ�ʮ���ߵ���ɫ����ʽΪ(R|(G<<8)|(B<<16))��
    //            pbVisible  ָ�룬����TRUE�����ʾ��ʮ���߿ɼ���
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetCrossLine(
        CameraHandle    hCamera, 
        int             iLine,
    ref int             px,
    ref int             py,
    ref uint            pcolor,
    ref uint            pbVisible
    );

    /******************************************************/
    // ������   : CameraGetCapability
    // �������� : �����������������ṹ�塣�ýṹ���а��������
    //        �����õĸ��ֲ����ķ�Χ��Ϣ����������غ����Ĳ���
    //        ���أ�Ҳ�����ڶ�̬������������ý��档
    // ����     : hCamera   ����ľ������CameraInit������á�
    //            pCameraInfo ָ�룬���ظ�������������Ľṹ�塣
    //                        tSdkCameraCapbility��CameraDefine.h�ж��塣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetCapability(
        CameraHandle            hCamera, 
        IntPtr                  pCameraInfo
    );

    /******************************************************/
    // ������   : CameraWriteSN
    // �������� : ������������кš��ҹ�˾������кŷ�Ϊ3����
    //        0�������ҹ�˾�Զ����������кţ�����ʱ�Ѿ�
    //        �趨�ã�1����2���������ο���ʹ�á�ÿ������
    //        �ų��ȶ���32���ֽڡ�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbySN    ���кŵĻ������� 
    //            iLevel   Ҫ�趨�����кż���ֻ����1����2��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraWriteSN(
        CameraHandle    hCamera, 
    ref Byte[]          pbySN, 
        int             iLevel
    );

    /******************************************************/
    // ������   : CameraReadSN
    // �������� : ��ȡ���ָ����������кš����кŵĶ�����ο�
    //          CameraWriteSN�����Ĺ����������֡�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            pbySN    ���кŵĻ�������
    //            iLevel     Ҫ��ȡ�����кż���ֻ����1��2��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraReadSN(
        CameraHandle        hCamera,
    ref Byte[]              pbySN, 
        int                 iLevel
    );
    /******************************************************/
    // ������   : CameraSetTriggerDelayTime
    // �������� : ����Ӳ������ģʽ�µĴ�����ʱʱ�䣬��λ΢�롣
    //        ��Ӳ�����ź����ٺ󣬾���ָ������ʱ���ٿ�ʼ�ɼ�
    //        ͼ�񡣽������ͺŵ����֧�ָù��ܡ�������鿴
    //        ��Ʒ˵���顣
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            uDelayTimeUs Ӳ������ʱ����λ΢�롣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetTriggerDelayTime(
        CameraHandle    hCamera, 
        uint            uDelayTimeUs
    );

    /******************************************************/
    // ������   : CameraGetTriggerDelayTime
    // �������� : ��õ�ǰ�趨��Ӳ������ʱʱ�䡣
    // ����     : hCamera     ����ľ������CameraInit������á�
    //            puDelayTimeUs ָ�룬������ʱʱ�䣬��λ΢�롣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetTriggerDelayTime(
        CameraHandle    hCamera, 
    ref uint            puDelayTimeUs
    );

    /******************************************************/
    // ������   : CameraSetTriggerCount
    // �������� : ���ô���ģʽ�µĴ���֡���������������Ӳ������
    //        ģʽ����Ч��Ĭ��Ϊ1֡����һ�δ����źŲɼ�һ֡ͼ��
    // ����     : hCamera ����ľ������CameraInit������á�
    //            iCount    һ�δ����ɼ���֡����
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetTriggerCount(
        CameraHandle    hCamera, 
        int             iCount
    );

    /******************************************************/
    // ������   : CameraGetTriggerCount
    // �������� : ���һ�δ�����֡����
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            int* piCount
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetTriggerCount(
        CameraHandle    hCamera, 
    ref int             piCount
    );

    /******************************************************/
    // ������   : CameraSoftTrigger
    // �������� : ִ��һ��������ִ�к󣬻ᴥ����CameraSetTriggerCount
    //          ָ����֡����
    // ����     : hCamera  ����ľ������CameraInit������á�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSoftTrigger(
        CameraHandle    hCamera
    );

    /******************************************************/
    // ������   : CameraSetTriggerMode
    // �������� : ��������Ĵ���ģʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iModeSel   ģʽѡ�������š����趨��ģʽ��
    //             CameraGetCapability������ȡ����ο�
    //               CameraDefine.h��tSdkCameraCapbility�Ķ��塣
    //             һ�������0��ʾ�����ɼ�ģʽ��1��ʾ
    //             �������ģʽ��2��ʾӲ������ģʽ��  
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetTriggerMode(
        CameraHandle    hCamera, 
        int             iModeSel
    );

    /******************************************************/
    // ������   : CameraGetTriggerMode
    // �������� : �������Ĵ���ģʽ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            piModeSel  ָ�룬���ص�ǰѡ����������ģʽ�������š�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetTriggerMode(
        CameraHandle    hCamera,
    ref int             piModeSel
    );


    /******************************************************/
    // ������   : CameraGetResolutionForSnap
    // �������� : ���ץ��ģʽ�µķֱ���ѡ�������š�
    // ����     : hCamera        ����ľ������CameraInit������á�
    //            pImageResolution ָ�룬����ץ��ģʽ�ķֱ��ʡ� 
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetResolutionForSnap(
        CameraHandle            hCamera,
    ref tSdkImageResolution     pImageResolution
    );

    /******************************************************/
    // ������   : CameraSetResolutionForSnap
    // �������� : ����ץ��ģʽ��������ͼ��ķֱ��ʡ�
    // ����     : hCamera       ����ľ������CameraInit������á�
    //            pImageResolution ���pImageResolution->iWidth 
    //                 �� pImageResolution->iHeight��Ϊ0��
    //                         ���ʾ�趨Ϊ���浱ǰԤ���ֱ��ʡ�ץ
    //                         �µ���ͼ��ķֱ��ʻ�͵�ǰ�趨�� 
    //                 Ԥ���ֱ���һ����
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetResolutionForSnap(
        CameraHandle            hCamera, 
    ref tSdkImageResolution     pImageResolution
    );

    /******************************************************/
    // ������   : CameraCustomizeResolution
    // �������� : �򿪷ֱ����Զ�����壬��ͨ�����ӻ��ķ�ʽ
    //        ������һ���Զ���ֱ��ʡ�
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            pImageCustom ָ�룬�����Զ���ķֱ��ʡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraCustomizeResolution(
        CameraHandle            hCamera,
    ref tSdkImageResolution     pImageCustom
    );

    /******************************************************/
    // ������   : CameraCustomizeReferWin
    // �������� : �򿪲ο������Զ�����塣��ͨ�����ӻ��ķ�ʽ��
    //        ���һ���Զ��崰�ڵ�λ�á�һ�������Զ����ƽ��
    //        ���Զ��ع�Ĳο����ڡ�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            iWintype   Ҫ���ɵĲο����ڵ���;��0,�Զ��ع�ο����ڣ�
    //             1,��ƽ��ο����ڡ�
    //            hParent    ���øú����Ĵ��ڵľ��������ΪNULL��
    //            piHOff     ָ�룬�����Զ��崰�ڵ����ϽǺ����ꡣ
    //            piVOff     ָ�룬�����Զ��崰�ڵ����Ͻ������ꡣ
    //            piWidth    ָ�룬�����Զ��崰�ڵĿ�ȡ� 
    //            piHeight   ָ�룬�����Զ��崰�ڵĸ߶ȡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraCustomizeReferWin(
        CameraHandle    hCamera,
        int             iWintype,
        IntPtr            hParent, 
    ref int             piHOff,
    ref int             piVOff,
    ref int             piWidth,
    ref int             piHeight
    );

    /******************************************************/
    // ������   : CameraShowSettingPage
    // �������� : ��������������ô�����ʾ״̬�������ȵ���CameraCreateSettingPage
    //        �ɹ���������������ô��ں󣬲��ܵ��ñ���������
    //        ��ʾ��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            bShow    TRUE����ʾ;FALSE�����ء�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraShowSettingPage(
        CameraHandle    hCamera,
        uint            bShow
    );

    /******************************************************/
    // ������   : CameraCreateSettingPage
    // �������� : ������������������ô��ڡ����øú�����SDK�ڲ���
    //        ������������������ô��ڣ�ʡȥ�������¿������
    //        ���ý����ʱ�䡣ǿ�ҽ���ʹ����ʹ�øú�����
    //        SDKΪ�����������ô��ڡ�
    // ����     : hCamera     ����ľ������CameraInit������á�
    //            hParent       Ӧ�ó��������ڵľ��������ΪNULL��
    //            pWintext      �ַ���ָ�룬������ʾ�ı�������
    //            pCallbackFunc ������Ϣ�Ļص�����������Ӧ���¼�����ʱ��
    //              pCallbackFunc��ָ��ĺ����ᱻ���ã�
    //              �����л��˲���֮��Ĳ���ʱ��pCallbackFunc
    //              ���ص�ʱ������ڲ�����ָ������Ϣ���͡�
    //              �������Է������Լ������Ľ�����������ɵ�UI
    //              ֮�����ͬ�����ò�������ΪNULL��    
    //            pCallbackCtx  �ص������ĸ��Ӳ���������ΪNULL��pCallbackCtx
    //              ����pCallbackFunc���ص�ʱ����Ϊ����֮һ���롣
    //              ������ʹ�øò�������һЩ�����жϡ�
    //            uReserved     Ԥ������������Ϊ0��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraCreateSettingPage(
        CameraHandle            hCamera,
        IntPtr                  hParent,
        byte[]                  pWintext,
        CAMERA_PAGE_MSG_PROC    pCallbackFunc,
        IntPtr                  pCallbackCtx,
        uint                    uReserved
    );

    /******************************************************/
    // ������   : CameraSetActiveSettingSubPage
    // �������� : ����������ô��ڵļ���ҳ�档������ô����ж��
    //        ��ҳ�湹�ɣ��ú��������趨��ǰ��һ����ҳ��
    //        Ϊ����״̬����ʾ����ǰ�ˡ�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            index      ��ҳ��������š��ο�CameraDefine.h��
    //             PROP_SHEET_INDEX�Ķ��塣
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetActiveSettingSubPage(
        CameraHandle    hCamera,
        int             index
    );

    /******************************************************/
    // ������   : CameraSpecialControl
    // �������� : ���һЩ�������������õĽӿڣ����ο���ʱһ�㲻��Ҫ
    //        ���á�
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            dwCtrlCode �����롣
    //            dwParam    �������룬��ͬ��dwCtrlCodeʱ�����岻ͬ��
    //            lpData     ���Ӳ�������ͬ��dwCtrlCodeʱ�����岻ͬ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSpecialControl(
        CameraHandle    hCamera, 
        uint            dwCtrlCode,
        uint            dwParam,
        IntPtr          lpData
    );

    /******************************************************/
    // ������   : CameraGetFrameStatistic
    // �������� : ����������֡�ʵ�ͳ����Ϣ����������֡�Ͷ�֡�������
    // ����     : hCamera        ����ľ������CameraInit������á�
    //            psFrameStatistic ָ�룬����ͳ����Ϣ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetFrameStatistic(
        CameraHandle            hCamera, 
    out tSdkFrameStatistic      psFrameStatistic
    );

    /******************************************************/
    // ������   : CameraSetNoiseFilter
    // �������� : ����ͼ����ģ���ʹ��״̬��
    // ����     : hCamera ����ľ������CameraInit������á�
    //            bEnable   TRUE��ʹ�ܣ�FALSE����ֹ��
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSetNoiseFilter(
        CameraHandle    hCamera,
        uint            bEnable
    );

    /******************************************************/
    // ������   : CameraGetNoiseFilterState
    // �������� : ���ͼ����ģ���ʹ��״̬��
    // ����     : hCamera  ����ľ������CameraInit������á�
    //            *pEnable   ָ�룬����״̬��TRUE��Ϊʹ�ܡ�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraGetNoiseFilterState(
        CameraHandle    hCamera,
    ref uint            pEnable
    );

    /******************************************************/
    // ������   : CameraRstTimeStamp
    // �������� : ��λͼ��ɼ���ʱ�������0��ʼ��
    // ����     : CameraHandle hCamera
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraRstTimeStamp(
        CameraHandle    hCamera
    );

    /******************************************************/
    // ������   : CameraSaveUserData
    // �������� : ���û��Զ�������ݱ��浽����ķ����Դ洢���С�
    //              ÿ���ͺŵ��������֧�ֵ��û���������󳤶Ȳ�һ����
    //              ���Դ��豸�����������л�ȡ�ó�����Ϣ��
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            uStartAddr  ��ʼ��ַ����0��ʼ��
    //            pbData      ���ݻ�����ָ��
    //            ilen        д�����ݵĳ��ȣ�ilen + uStartAddr����
    //                        С���û�����󳤶�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraSaveUserData(
        CameraHandle    hCamera,
        uint            uStartAddr,
    ref Byte[]          pbData,
        int             ilen
    );

    /******************************************************/
    // ������   : CameraLoadUserData
    // �������� : ������ķ����Դ洢���ж�ȡ�û��Զ�������ݡ�
    //              ÿ���ͺŵ��������֧�ֵ��û���������󳤶Ȳ�һ����
    //              ���Դ��豸�����������л�ȡ�ó�����Ϣ��
    // ����     : hCamera    ����ľ������CameraInit������á�
    //            uStartAddr  ��ʼ��ַ����0��ʼ��
    //            pbData      ���ݻ�����ָ�룬���ض��������ݡ�
    //            ilen        ��ȡ���ݵĳ��ȣ�ilen + uStartAddr����
    //                        С���û�����󳤶�
    // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
    //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
    //            �д�����Ķ��塣
    /******************************************************/
    [DllImport("MVCAMSDK.dll")]
    public static extern CameraSdkStatus CameraLoadUserData(
        CameraHandle    hCamera,
        uint            uStartAddr,
    ref Byte[]          pbData,
        int             ilen
    );

  /******************************************************/
  // ������ : CameraGetFriendlyName
  // �������� : ��ȡ�û��Զ�����豸�ǳơ�
  // ����   : hCamera  ����ľ������CameraInit������á�
  //        pName    ָ�룬����ָ��0��β���ַ�����
  //             �豸�ǳƲ�����32���ֽڣ���˸�ָ��
  //             ָ��Ļ�����������ڵ���32���ֽڿռ䡣
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetFriendlyName(
    CameraHandle  hCamera,
  ref Byte[]      pName
  );
  
  /******************************************************/
  // ������ : CameraSetFriendlyName
  // �������� : �����û��Զ�����豸�ǳơ�
  // ����   : hCamera  ����ľ������CameraInit������á�
  //        pName    ָ�룬ָ��0��β���ַ�����
  //             �豸�ǳƲ�����32���ֽڣ���˸�ָ��
  //             ָ���ַ�������С�ڵ���32���ֽڿռ䡣
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraSetFriendlyName(
    CameraHandle  hCamera,
  ref Byte[]      pName
  );
  
  /******************************************************/
  // ������ : __stdcall CameraSdkGetVersionString
  // �������� : 
  // ����   : pVersionString ָ�룬����SDK�汾�ַ�����
  //                ��ָ��ָ��Ļ�������С�������
  //                32���ֽ�
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraSdkGetVersionString(
  ref Byte[]      pVersionString
  );
  
  /******************************************************/
  // ������ : CameraCheckFwUpdate
  // �������� : ���̼��汾���Ƿ���Ҫ������
  // ����   : hCamera ����ľ������CameraInit������á�
  //        pNeedUpdate ָ�룬���ع̼����״̬��TRUE��ʾ��Ҫ����
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraCheckFwUpdate(
    CameraHandle  hCamera,
  ref uint      pNeedUpdate
  );
  
  /******************************************************/
  // ������ : CameraGetFirmwareVision
  // �������� : ��ù̼��汾���ַ���
  // ����   : hCamera ����ľ������CameraInit������á�
  //        pVersion ����ָ��һ������32�ֽڵĻ�������
  //            ���ع̼��İ汾�ַ�����
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetFirmwareVision(
    CameraHandle  hCamera,
  ref Byte[]      pVersion
  );
  
  /******************************************************/
  // ������ : CameraGetEnumInfo
  // �������� : ���ָ���豸��ö����Ϣ
  // ����   : hCamera ����ľ������CameraInit������á�
  //        pCameraInfo ָ�룬�����豸��ö����Ϣ��
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetEnumInfo(
    CameraHandle    hCamera,
  ref tSdkCameraDevInfo pCameraInfo
  );
  
  /******************************************************/
  // ������ : CameraGetInerfaceVersion
  // �������� : ���ָ���豸�ӿڵİ汾
  // ����   : hCamera ����ľ������CameraInit������á�
  //        pVersion ָ��һ������32�ֽڵĻ����������ؽӿڰ汾�ַ�����
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetInerfaceVersion(
    CameraHandle    hCamera,
  ref Byte[]        pVersion
  );
  
  /******************************************************/
  // ������ : CameraSetIOState
  // �������� : ����ָ��IO�ĵ�ƽ״̬��IOΪ�����IO�����
  //        Ԥ���ɱ�����IO�ĸ�����tSdkCameraCapbility��
  //        iOutputIoCounts������
  // ����   : hCamera ����ľ������CameraInit������á�
  //        iOutputIOIndex IO�������ţ���0��ʼ��
  //        uState Ҫ�趨��״̬��1Ϊ�ߣ�0Ϊ��
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraSetIOState(
    CameraHandle    hCamera,
    int         iOutputIOIndex,
    uint        uState
  );
  
  /******************************************************/
  // ������ : CameraGetIOState
  // �������� : ����ָ��IO�ĵ�ƽ״̬��IOΪ������IO�����
  //        Ԥ���ɱ�����IO�ĸ�����tSdkCameraCapbility��
  //        iInputIoCounts������
  // ����   : hCamera ����ľ������CameraInit������á�      
  //        iInputIOIndex IO�������ţ���0��ʼ��
  //        puState ָ�룬����IO״̬,1Ϊ�ߣ�0Ϊ��
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetIOState(
    CameraHandle    hCamera,
    int         iInputIOIndex,
  ref uint        puState
  );
  
  /******************************************************/
  // ������ : CameraSetAeAlgorithm
  // �������� : �����Զ��ع�ʱѡ����㷨����ͬ���㷨������
  //        ��ͬ�ĳ�����
  // ����   : hCamera     ����ľ������CameraInit������á� 
  //        iIspProcessor   ѡ��ִ�и��㷨�Ķ��󣬲ο�CameraDefine.h
  //                emSdkIspProcessor�Ķ���
  //        iAeAlgorithmSel Ҫѡ����㷨��š���0��ʼ�����ֵ��tSdkCameraCapbility
  //                ��iAeAlmSwDesc��iAeAlmHdDesc������  
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraSetAeAlgorithm(
    CameraHandle  hCamera,
    int       iIspProcessor,
    int       iAeAlgorithmSel
  );
  
  /******************************************************/
  // ������ : CameraGetAeAlgorithm
  // �������� : ��õ�ǰ�Զ��ع���ѡ����㷨
  // ����   : hCamera     ����ľ������CameraInit������á� 
  //        iIspProcessor   ѡ��ִ�и��㷨�Ķ��󣬲ο�CameraDefine.h
  //                emSdkIspProcessor�Ķ���
  //        piAeAlgorithmSel  ���ص�ǰѡ����㷨��š���0��ʼ�����ֵ��tSdkCameraCapbility
  //                ��iAeAlmSwDesc��iAeAlmHdDesc������  
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetAeAlgorithm(
    CameraHandle  hCamera,
    int       iIspProcessor,
  ref int       piAlgorithmSel
  );
  
  /******************************************************/
  // ������ : CameraSetBayerDecAlgorithm
  // �������� : ����Bayer����ת��ɫ���㷨��
  // ����   : hCamera     ����ľ������CameraInit������á� 
  //        iIspProcessor   ѡ��ִ�и��㷨�Ķ��󣬲ο�CameraDefine.h
  //                emSdkIspProcessor�Ķ���
  //        iAlgorithmSel   Ҫѡ����㷨��š���0��ʼ�����ֵ��tSdkCameraCapbility
  //                ��iBayerDecAlmSwDesc��iBayerDecAlmHdDesc������    
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraSetBayerDecAlgorithm(
    CameraHandle  hCamera,
    int       iIspProcessor,
    int       iAlgorithmSel
  );
  
  /******************************************************/
  // ������ : CameraGetBayerDecAlgorithm
  // �������� : ���Bayer����ת��ɫ��ѡ����㷨��
  // ����   : hCamera     ����ľ������CameraInit������á� 
  //        iIspProcessor   ѡ��ִ�и��㷨�Ķ��󣬲ο�CameraDefine.h
  //                emSdkIspProcessor�Ķ���
  //        piAlgorithmSel  ���ص�ǰѡ����㷨��š���0��ʼ�����ֵ��tSdkCameraCapbility
  //                ��iBayerDecAlmSwDesc��iBayerDecAlmHdDesc������  
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetBayerDecAlgorithm(
    CameraHandle  hCamera,
    int       iIspProcessor,
  ref int       piAlgorithmSel
  );
  
  /******************************************************/
  // ������ : CameraSetIspProcessor
  // �������� : ����ͼ����Ԫ���㷨ִ�ж�����PC�˻��������
  //        ��ִ���㷨�����������ִ��ʱ���ή��PC�˵�CPUռ���ʡ�
  // ����   : hCamera   ����ľ������CameraInit������á� 
  //        iIspProcessor �ο�CameraDefine.h��
  //              emSdkIspProcessor�Ķ��塣
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraSetIspProcessor(
    CameraHandle  hCamera,
    int       iIspProcessor
  );
  
  /******************************************************/
  // ������ : CameraGetIspProcessor
  // �������� : ���ͼ����Ԫ���㷨ִ�ж���
  // ����   : hCamera    ����ľ������CameraInit������á� 
  //        piIspProcessor ����ѡ��Ķ��󣬷���ֵ�ο�CameraDefine.h��
  //               emSdkIspProcessor�Ķ��塣
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetIspProcessor(
    CameraHandle  hCamera,
  ref int       piIspProcessor
  );
  
  /******************************************************/
  // ������ : CameraSetBlackLevel
  // �������� : ����ͼ��ĺڵ�ƽ��׼��Ĭ��ֵΪ0
  // ����   : hCamera   ����ľ������CameraInit������á� 
  //        iBlackLevel Ҫ�趨�ĵ�ƽֵ����ΧΪ0��255��  
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraSetBlackLevel(
    CameraHandle    hCamera,
    int         iBlackLevel
  );
  
  /******************************************************/
  // ������ : CameraGetBlackLevel
  // �������� : ���ͼ��ĺڵ�ƽ��׼��Ĭ��ֵΪ0
  // ����   : hCamera    ����ľ������CameraInit������á� 
  //        piBlackLevel ���ص�ǰ�ĺڵ�ƽֵ����ΧΪ0��255��
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetBlackLevel(
    CameraHandle    hCamera,
  ref int         piBlackLevel
  );
  
  /******************************************************/
  // ������ : CameraSetWhiteLevel
  // �������� : ����ͼ��İ׵�ƽ��׼��Ĭ��ֵΪ255
  // ����   : hCamera   ����ľ������CameraInit������á� 
  //        iWhiteLevel Ҫ�趨�ĵ�ƽֵ����ΧΪ0��255��  
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraSetWhiteLevel(
    CameraHandle    hCamera,
    int         iWhiteLevel
  );
  
  /******************************************************/
  // ������ : CameraGetWhiteLevel
  // �������� : ���ͼ��İ׵�ƽ��׼��Ĭ��ֵΪ255
  // ����   : hCamera    ����ľ������CameraInit������á� 
  //        piWhiteLevel ���ص�ǰ�İ׵�ƽֵ����ΧΪ0��255��
  // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
  //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
  //        �д�����Ķ��塣
  /******************************************************/
  [DllImport("MVCAMSDK.dll")]
  public static extern CameraSdkStatus CameraGetWhiteLevel(
    CameraHandle    hCamera,
  ref int         piWhiteLevel
  );
         
    }
}
