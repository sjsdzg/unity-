using UnityEngine;
using System.Collections;
using Dongle;
using System;
using System.Text;
using XFramework.Common;
using System.Diagnostics;
using XFramework;
using XFramework.Core;

namespace Dongle
{
    /// <summary>
    /// 加密狗
    /// </summary>
    public class RockeyARM : Singleton<RockeyARM>
    {
        public delegate void LicenseCallback(LicenseResult result);
        uint ret = 0;//返回值
        uint pCount = 0;//加密狗数量
        DONGLE_INFO pDongleInfo = new DONGLE_INFO();//加密狗信息
        public event LicenseCallback LicenseChecked;//授权检测事件

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public static DateTime Deadline { get; private set; }

        public void Start()
        {
            Enabled = true;
            CoroutineManager.Instance.StartCoroutine(ExistRockeyEnumerator(60));
            CoroutineManager.Instance.StartCoroutine(CheckLicenseEnumerator(60 * 10));
        }

        public void Stop()
        {
            Enabled = false;
        }

        /// <summary>
        /// 是否存在加密狗
        /// </summary>
        public void ExistRockey()
        {
            try
            {
                //枚举加密锁
                ret = RockeyUtility.Dongle_Enum(ref pDongleInfo, out pCount);
                if (ret != 0)
                {
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.RockeyNotFound);
                }
            }
            catch (Exception ex)
            {
                if (LicenseChecked != null)
                    LicenseChecked(LicenseResult.Exception);
            }

        }

        /// <summary>
        /// 检测授权
        /// </summary>
        public void CheckLicense()
        {
            try
            {
                UIntPtr hDongle = (UIntPtr)0;
                byte[] bDataSec = new byte[8192];//数据区总大小8k
                byte[] rsaPubBuffer = new byte[1024];//数据文件
                RSA_PUBLIC_KEY rsaPub;//RSA公钥
                uint nInDataLen = 0;
                uint nOutDataLen = 0;
                byte[] outData;
                int nIndex = -1;//是否有时钟锁
                uint dwTime = 0;//当前时间
                uint dlTime = 0;//截止时间
                                //枚举加密锁
                ret = RockeyUtility.Dongle_Enum(ref pDongleInfo, out pCount);
                if (ret != 0)
                {
                    //Log("Enum Dongle Failed! Return value:" + ret.ToString("X") + "\r\n");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.RockeyNotFound);

                    return;
                }

                //0xFF表示标准版, 0x00为时钟锁,0x01为带时钟的U盘锁,0x02为标准U盘锁
                for (int i = 0; i < pCount; i++)
                {
                    if (pDongleInfo.m_Type == 0 || pDongleInfo.m_Type == 1)
                    {
                        nIndex = i;
                    }
                }
                //没有找到时钟锁
                if (nIndex == -1)
                {
                    //Log("Can't Find Time Dongle ARM.\n" + "\r\n");
                    //RockeyUtility.Dongle_Close(hDongle);
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.Invalid);
                    return;
                }

                //打开加密锁
                ret = RockeyUtility.Dongle_Open(ref hDongle, 0);
                if (ret != 0)
                {
                    //Log("Open Dongle Failed! Return value:" + ret.ToString("X") + "\r\n");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.Invalid);
                    return;
                }

                //读取数据区
                ret = RockeyUtility.Dongle_ReadData(hDongle, 0, bDataSec, 8192);
                if (ret != 0)
                {
                    //Log("Read data section Failed! Return value:" + ret.ToString("X") + "\r\n");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.Invalid);
                    return;
                }

                //读RSA公钥数据文件
                ret = RockeyUtility.Dongle_ReadFile(hDongle, 0x0002, 0, rsaPubBuffer, 1024);
                if (ret != 0)
                {
                    //Log("Read File from Dongle Failed! Return value:" + ret.ToString("X") + "\r\n");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.Invalid);
                    return;
                }
                else
                {
                    //Log("Read File from Dongle Success! \r\n");
                    rsaPub = Converter.ToSturct<RSA_PUBLIC_KEY>(rsaPubBuffer);
                    ////Log("RSA公钥：" + rsaPub.ToString());
                }
                //RSA公钥解密
                nInDataLen = 128;
                nOutDataLen = 128 - 11;
                outData = new byte[117];
                ret = RockeyUtility.Dongle_RsaPub(hDongle, DongleDef.FLAG_DECODE, ref rsaPub, bDataSec, nInDataLen, outData, ref nOutDataLen);
                if (ret != 0)
                {
                    //Log("RSA public key decode Failed! Return value:" + ret.ToString("X") + "\r\n");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.Invalid);
                    return;
                }
                else
                {
                    //Log("RSA public key decode Success! \r\n");
                    bool flag = true;
                    string license = Encoding.Default.GetString(outData);
                    string[] array = license.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < array.Length; i++)
                    {
                        string[] map = array[i].Split('=');
                        if (i == 0 && !map[0].Equals("ID"))
                        {
                            flag = false;
                        }

                        if (i == 1 && !map[0].Equals("NAME"))
                        {
                            flag = false;
                        }

                        if (i == 2 && !map[0].Equals("LICENSETYPE"))
                        {
                            flag = false;
                        }


                        if (i == 3 && !map[0].Equals("CLIENTCOUNT"))
                        {
                            flag = false;
                        }

                        if (i == 4)
                        {
                            if (map[0].Equals("ACTIVE"))
                            {
                                string value = map[1].TrimEnd('\0');
                                if (!value.Equals("1"))
                                {
                                    if (LicenseChecked != null)
                                        LicenseChecked(LicenseResult.NotActive);

                                    return;
                                }
                            }
                            else
                            {
                                flag = false;
                            }
                        }

                    }
                    if (!flag)
                    {
                        if (LicenseChecked != null)
                            LicenseChecked(LicenseResult.Invalid);

                        return;
                    }
                    //Log(license);
                }
                //获取时间
                ret = RockeyUtility.Dongle_GetUTCTime(hDongle, ref dwTime);
                if (ret != 0)
                {
                    Log("Get UTC Time Failed! Return value:" + ret.ToString("X") + "\r\n");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.Invalid);
                    return;
                }

                //时间转换
                Double secs = Convert.ToDouble(dwTime);
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);
                //Log("UTC Time：" + dateTime.ToString("yyyy-MM-dd hh:mm:ss"));
                //获取截止时间
                ret = RockeyUtility.Dongle_GetDeadline(hDongle, ref dlTime);
                if (ret != 0)
                {
                    //Log("Get Deadline Failed! Return value:" + ret.ToString("X") + "\r\n");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.Invalid);
                    return;
                }

                Double secs2 = Convert.ToDouble(dlTime);
                Deadline = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs2);
                //判断是否过期
                if (dwTime <= dlTime)
                {
                    //Log("在授权时间范围内");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.Success);
                }
                else
                {
                    //Log("授权过期");
                    if (LicenseChecked != null)
                        LicenseChecked(LicenseResult.ClockExpire);
                }
                //关闭加密锁
                ret = RockeyUtility.Dongle_Close(hDongle);
                if (ret != 0)
                {
                    //Log("Close Dongle Failed! Return value:" + ret.ToString("X") + "\r\n");
                }

                //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                if (LicenseChecked != null)
                    LicenseChecked(LicenseResult.Exception);
            }
            
        }

        private void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        /// <summary>
        /// 检测加密狗是否存在
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        IEnumerator ExistRockeyEnumerator(float interval)
        {
            while (Enabled)
            {
                ExistRockey();
                //Log("检测加密狗是否存在");
                yield return new WaitForSecondsRealtime(interval);
            }
        }

        /// <summary>
        /// 检测授权是否有效
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        IEnumerator CheckLicenseEnumerator(float interval)
        {
            while (Enabled)
            {
                CheckLicense();
                //Log("检测授权是否有效");
                yield return new WaitForSecondsRealtime(interval);
            }
        }

        /// <summary>
        /// 授权结果
        /// </summary>
        public enum LicenseResult
        {
            /// <summary>
            /// 出现异常
            /// </summary>
            Exception,
            /// <summary>
            /// 没发现加密狗
            /// </summary>
            RockeyNotFound,
            /// <summary>
            /// 无效的
            /// </summary>
            Invalid,
            /// <summary>
            /// 未激活
            /// </summary>
            NotActive,
            /// <summary>
            /// 过期
            /// </summary>
            ClockExpire,
            /// <summary>
            /// 操作成功
            /// </summary>
            Success,
        }
    }
}



