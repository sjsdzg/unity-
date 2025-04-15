using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Dongle
{
    public class RockeyUtility
    {
        [DllImport("Dongle_d")]
        public static extern uint Dongle_Enum(ref DONGLE_INFO pDongleInfo, out uint pCount);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_Open(ref UIntPtr phDongle, int nIndex);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_Close(UIntPtr hDongle);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_VerifyPIN(UIntPtr hDongle, uint nFlags, byte[] pPIN, out int pRemainCount);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_CreateFile(UIntPtr hDongle, uint nFileType, ushort wFileID, Int64 pFileAttr);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_WriteFile(UIntPtr hDongle, uint nFileType, ushort wFileID, short wOffset, byte[] buffer, int nDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_ReadFile(UIntPtr hDongle, short wFileID, short wOffset, byte[] buffer, int nDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_ListFile(UIntPtr hDongle, uint nFileType, DATA_FILE_LIST[] pFileList, ref int pDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_DeleteFile(UIntPtr hDongle, uint nFileType, short wFileID);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_DownloadExeFile(UIntPtr hDongle, EXE_FILE_INFO[] pExeFileInfo, int nCount);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_RunExeFile(UIntPtr hDongle, short wFileID, byte[] pInOutData, short wInOutDataLen, ref int nMainRet);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_WriteShareMemory(UIntPtr hDongle, byte[] pData, int nDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_ReadShareMemory(UIntPtr hDongle, byte[] pData);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_WriteData(UIntPtr hDongle, int nOffset, byte[] pData, int nDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_ReadData(UIntPtr hDongle, int nOffset, byte[] pData, int nDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_LEDControl(UIntPtr hDongle, uint nFlag);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_SwitchProtocol(UIntPtr hDongle, uint nFlag);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_GetUTCTime(UIntPtr hDongle, ref uint pdwUTCTime);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_SetDeadline(UIntPtr hDongle, uint dwTime);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_GetDeadline(UIntPtr hDongle, ref uint dwTime);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_GenUniqueKey(UIntPtr hDongle, int nSeedLen, byte[] pSeed, byte[] pPIDstr, byte[] pAdminPINstr);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_ResetState(UIntPtr hDongle);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_ChangePIN(UIntPtr hDongle, uint nFlags, byte[] pOldPIN, byte[] pNewPIN, int nTryCount);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_RFS(UIntPtr hDongle);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_SetUserID(UIntPtr hDongle, uint dwUserID);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_ResetUserPIN(UIntPtr hDongle, byte[] pAdminPIN);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_RsaGenPubPriKey(UIntPtr hDongle, ushort wPriFileID, ref RSA_PUBLIC_KEY pPubBakup, ref RSA_PRIVATE_KEY pPriBakup);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_RsaPri(UIntPtr hDongle, ushort wPriFileID, uint nFlag, byte[] pInData, uint nInDataLen, byte[] pOutData, ref uint pOutDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_RsaPub(UIntPtr hDongle, uint nFlag, ref RSA_PUBLIC_KEY pPubKey, byte[] pInData, uint nInDataLen, byte[] pOutData, ref uint pOutDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_TDES(UIntPtr hDongle, ushort wKeyFileID, uint nFlag, byte[] pInData, byte[] pOutData, uint nDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_SM4(UIntPtr hDongle, ushort wKeyFileID, uint nFlag, byte[] pInData, byte[] pOutData, uint nDataLen);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_DeleteFile(UIntPtr hDongle, uint nFileType, ushort wFileID);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_HASH(UIntPtr hDongle, uint nFlag, byte[] pInData, uint nDataLen, byte[] pHash);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_LimitSeedCount(UIntPtr hDongle, int nCount);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_Seed(UIntPtr hDongle, byte[] pSeed, uint nSeedLen, byte[] pOutData);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_EccGenPubPriKey(UIntPtr hDongle, ushort wPriFileID, ref ECCSM2_PUBLIC_KEY vPubBakup, ref ECCSM2_PRIVATE_KEY vPriBakup);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_EccSign(UIntPtr hDongle, ushort wPriFileID, byte[] pHashData, uint nHashDataLen, byte[] pOutData);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_EccVerify(UIntPtr hDongle, ref ECCSM2_PUBLIC_KEY pPubKey, byte[] pHashData, uint nHashDataLen, byte[] pSign);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_SM2GenPubPriKey(UIntPtr hDongle, ushort wPriFileID, ref ECCSM2_PUBLIC_KEY pPubBakup, ref ECCSM2_PRIVATE_KEY pPriBakup);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_SM2Sign(UIntPtr hDongle, ushort wPriFileID, byte[] pHashData, uint nHashDataLen, byte[] pOutData);
        [DllImport("Dongle_d")]
        public static extern uint Dongle_SM2Verify(UIntPtr hDongle, ref ECCSM2_PUBLIC_KEY pPubKey, byte[] pHashData, uint nHashDataLen, byte[] pSign);
    }

    /************************************************************************/
    /*                              结构                                    */
    /************************************************************************/
    //RSA公钥格式(兼容1024,2048)
    [StructLayout(LayoutKind.Sequential)]
    public struct RSA_PUBLIC_KEY
    {
        public uint bits;                   // length in bits of modulus        	
        public uint modulus;                  // modulus
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] exponent;       // public exponent
    }
    //RSA私钥格式(兼容1024,2048)
    [StructLayout(LayoutKind.Sequential)]
    public struct RSA_PRIVATE_KEY
    {
        public uint bits;                   // length in bits of modulus        	
        public uint modulus;                  // modulus  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] publicExponent;       // public exponent
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] exponent;       // public exponent
    }
    //外部ECCSM2公钥格式 ECC(支持bits为192或256)和SM2的(bits为固定值0x8100)公钥格式
    [StructLayout(LayoutKind.Sequential)]
    public struct ECCSM2_PUBLIC_KEY
    {
        public uint bits;                   // length in bits of modulus        	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] XCoordinate;       // 曲线上点的X坐标
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] YCoordinate;       // 曲线上点的Y坐标
    }
    //外部ECCSM2私钥格式 ECC(支持bits为192或256)和SM2的(bits为固定值0x8100)私钥格式  
    [StructLayout(LayoutKind.Sequential)]
    public struct ECCSM2_PRIVATE_KEY
    {
        public uint bits;                   // length in bits of modulus        	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] PrivateKey;           // 私钥
    }

    //加密锁信息
    [StructLayout(LayoutKind.Sequential)]
    public struct DONGLE_INFO
    {
        public ushort m_Ver;               //COS版本,比如:0x0201,表示2.01版             	
        public ushort m_Type;              //产品类型: 0xFF表示标准版, 0x00为时钟锁,0x01为带时钟的U盘锁,0x02为标准U盘锁  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_BirthDay;       //出厂日期 
        public uint m_Agent;             //代理商编号,比如:默认的0xFFFFFFFF
        public uint m_PID;               //产品ID
        public uint m_UserID;            //用户ID
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_HID;            //8字节的硬件ID
        public uint m_IsMother;          //母锁标志: 0x01表示是母锁, 0x00表示不是母锁
        public uint m_DevType;           //设备类型(PROTOCOL_HID或者PROTOCOL_CCID)
    }
    /*************************文件授权结构***********************************/
    //数据文件授权结构
    [StructLayout(LayoutKind.Sequential)]
    public struct DATA_LIC
    {
        public ushort m_Read_Priv;     //读权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限            	
        public ushort m_Write_Priv;    //写权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限
    }

    //私钥文件授权结构
    [StructLayout(LayoutKind.Sequential)]
    public struct PRIKEY_LIC
    {
        public uint m_Count;        //可调次数: 0xFFFFFFFF表示不限制, 递减到0表示已不可调用
        public byte m_Priv;         //调用权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限
        public byte m_IsDecOnRAM;   //是否是在内存中递减: 1为在内存中递减，0为在FLASH中递减
        public byte m_IsReset;      //用户态调用后是否自动回到匿名态: TRUE为调后回到匿名态 (开发商态不受此限制)
        public byte m_Reserve;      //保留,用于4字节对齐
    }

    //对称加密算法(SM4/TDES)密钥文件授权结构
    [StructLayout(LayoutKind.Sequential)]
    public struct KEY_LIC
    {
        public uint m_Priv_Enc;   //加密时的调用权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限
    }


    //可执行文件授权结构
    [StructLayout(LayoutKind.Sequential)]
    public struct EXE_LIC
    {
        public ushort m_Priv_Exe;   //运行的权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限
    }

    /****************************文件属性结构********************************/
    //数据文件属性数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct DATA_FILE_ATTR
    {
        public uint m_Size;      //数据文件长度，该值最大为4096
        public DATA_LIC m_Lic;       //授权
    }

    //ECCSM2/RSA私钥文件属性数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct PRIKEY_FILE_ATTR
    {
        public ushort m_Type;       //数据类型:ECCSM2私钥 或 RSA私钥
        public ushort m_Size;       //数据长度:RSA该值为1024或2048, ECC该值为192或256, SM2该值为0x8100
        public PRIKEY_LIC m_Lic;        //授权
    }

    //对称加密算法(SM4/TDES)密钥文件属性数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct KEY_FILE_ATTR
    {
        public uint m_Size;       //密钥数据长度=16
        public KEY_LIC m_Lic;        //授权
    }

    //可执行文件属性数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct EXE_FILE_ATTR
    {
        public EXE_LIC m_Lic;        //授权	
        public ushort m_Len;        //文件长度
    }
    /*************************文件列表结构***********************************/
    //获取私钥文件列表时返回的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct PRIKEY_FILE_LIST
    {
        public ushort m_FILEID;  //文件ID
        public ushort m_Reserve; //保留,用于4字节对齐
        public PRIKEY_FILE_ATTR m_attr;    //文件属性
    }

    //获取SM4及TDES密钥文件列表时返回的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct KEY_FILE_LIST
    {
        public ushort m_FILEID;  //文件ID
        public ushort m_Reserve; //保留,用于4字节对齐
        public KEY_FILE_ATTR m_attr;    //文件属性
    }

    //获取数据文件列表时返回的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct DATA_FILE_LIST
    {
        public ushort m_FILEID;  //文件ID
        public ushort m_Reserve; //保留,用于4字节对齐
        public DATA_FILE_ATTR m_attr;    //文件属性
    }

    //获取可执行文件列表时返回的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct EXE_FILE_LIST
    {
        public ushort m_FILEID;    //文件ID
        public EXE_FILE_ATTR m_attr;
        public ushort m_Reserve;  //保留,用于4字节对齐
    }

    //下载和列可执行文件时填充的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct EXE_FILE_INFO
    {
        public ushort m_dwSize;           //可执行文件大小
        public ushort m_wFileID;          //可执行文件ID
        public byte m_Priv;             //调用权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限

        public byte[] m_pData;            //可执行文件数据
    }


    //需要发给空锁的初始化数据
    [StructLayout(LayoutKind.Sequential)]
    public struct SON_DATA
    {
        public int m_SeedLen;                 //种子码长度
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string m_SeedForPID;        //产生产品ID和开发商密码的种子码 (最长250个字节)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string m_UserPIN;         //用户密码(16个字符的0终止字符串)
        public sbyte m_UserTryCount;            //用户密码允许的最大错误重试次数
        public sbyte m_AdminTryCount;           //开发商密码允许的最大错误重试次数
                                                //RSA_PRIVATE_KEY m_UpdatePriKey;   //远程升级私钥
        public int m_UserID_Start;            //起始用户ID
    }

    //母锁数据
    [StructLayout(LayoutKind.Sequential)]
    public struct MOTHER_DATA
    {
        public SON_DATA m_Son;                  //子锁初始化数据
        public int m_Count;                //可产生子锁初始化数据的次数 (-1表示不限制次数, 递减到0时会受限)
    }
}
