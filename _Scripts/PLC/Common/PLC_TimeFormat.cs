using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.PLC
{
    class PLC_TimeFormat
    {
        public static PLC_TimeFormat instance = new PLC_TimeFormat();

        /// <summary>
        /// Hour
        /// </summary>
        private int hours;
        /// <summary>
        /// Use to save Hour
        /// </summary>
        private string str_Hour;

        /// <summary>
        /// Minute
        /// </summary>
        private int minutes;
        /// <summary>
        /// Use to save Minute
        /// </summary>
        private string str_Min;

        /// <summary>
        /// Second
        /// </summary>
        private int seconds;
        /// <summary>
        /// Use to save Second
        /// </summary>
        private string str_Sec;

        /// <summary>
        /// Current time format
        /// </summary>
        private string timeFormat;

        /// <summary>
        /// test time parameter
        /// </summary>
        public int time = 0;

        /// <summary>
        /// 修改isNeedSeconds的布尔值来确定是否需要随机出现秒数。 true:不需要随机出现秒数，false:需要随即出现
        /// </summary>
        public bool isNeedSeconds = false;

        /// <summary>
        /// Way
        /// </summary>
        /// <param name="isNeedSecond">是否需要随机出现秒数，true:不需要随机出现秒数，false:需要随即出现</param>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string TimeFormat(bool isNeedSecond, int time, ref string format)
        {
            //Get Hours
            hours = time / 3600;
            //Get Minutes
            minutes = (time % 3600) / 60;
            //Get Seconds
            seconds = (time % 3600) % 60;

            str_Hour = hours.ToString();
            str_Min = minutes.ToString();
            str_Sec = seconds.ToString();

            if (str_Hour == "0")
            {
                str_Hour = "00";
            }
            else
            {
                if(str_Hour.Length <2)
                {
                    str_Hour = "0" + str_Hour;
                }
            }

            if(str_Min == "0")
            {
                str_Min = "00";
            }
            else
            {
                if(str_Min.Length <2)
                {
                    str_Min = "0" + str_Min;
                }
            }

            if(isNeedSecond)
            {
                if (str_Sec == "0")
                {
                    str_Sec = "00";
                }
                else
                {
                    if (str_Sec.Length < 2)
                    {
                        str_Sec = "0" + str_Sec;
                    }
                }
            }
            else
            {
                RandomSecond();
            }

            //Time Fromat
            format = str_Hour + " : " + str_Min + " : " + str_Sec;

            return format;
        }

        /// <summary>
        /// 秒数
        /// </summary>
        public void RandomSecond()
        {
            str_Sec = UnityEngine.Random.Range(0, 60).ToString();
            if (str_Sec == "0")
            {
                str_Sec = "00";
            }
            else
            {
                if (str_Sec.Length < 2)
                {
                    str_Sec = "0" + str_Sec;
                }
            }
        }
    }
}
