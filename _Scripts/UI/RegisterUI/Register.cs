using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework
{
    class Register
    {


        //步骤二: 收集硬件信息生成机器码, 代码如下: 

        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <returns></returns>
        //public string CreateCode()
        //{
        //    string temp = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
        //    string[] strid = new string[24];//
        //    for (int i = 0; i < 24; i++)//把字符赋给数组
        //    {
        //        strid[i] = temp.Substring(i, 1);
        //    }
        //    temp = "";
        //    //Random rdid = new Random();
        //    for (int i = 0; i < 24; i++)//从数组随机抽取24个字符组成新的字符生成机器三
        //    {
        //        //temp += strid[rdid.Next(0, 24)];
        //        temp += strid[i + 2 >= 24 ? 0 : i + 2];
        //    }
        //    return temp;
        //}

        //步骤三: 使用机器码生成软件注册码, 代码如下:
        //使用机器码生成注册码
        public int[] intCode = new int[127];//用于存密钥

        public void setIntCode()//给数组赋值个小于10的随机数
        {
            //Random ra = new Random();
            //for (int i = 1; i < intCode.Length;i++ )
            //{
            //    intCode[i] = ra.Next(0, 9);
            //}
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = (i + 3) * (i + 5) % 9;
            }
        }

        public int[] intNumber = new int[17];//用于存机器码的Ascii值
        public char[] Charcode = new char[17];//存储机器码字

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <param name="code">机器码</param>
        /// <returns></returns>
        public string GetLicense(string code)
        {
            if (code != "")
            {
                //把机器码存入数组中
                setIntCode();//初始化127位数组
                for (int i = 1; i < Charcode.Length; i++)//把机器码存入数组中
                {
                    Charcode[i] = Convert.ToChar(code.Substring(i + 7, 1));
                }//
                for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。
                {
                    intNumber[j] =
                       intCode[Convert.ToInt32(Charcode[j])] +
                       Convert.ToInt32(Charcode[j]);

                }
                string strAsciiName = null;//用于存储机器码
                for (int j = 1; j < intNumber.Length; j++)
                {
                    //MessageBox.Show((Convert.ToChar(intNumber[j])).ToString());
                    //判断字符ASCII值是否0－9之间

                    if (intNumber[j] >= 48 && intNumber[j] <= 57)
                    {
                        strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                    }
                    //判断字符ASCII值是否A－Z之间

                    else if (intNumber[j] >= 65 && intNumber[j] <= 90)
                    {
                        strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                    }
                    //判断字符ASCII值是否a－z之间


                    else if (intNumber[j] >= 97 && intNumber[j] <= 122)
                    {
                        strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                    }
                    else//判断字符ASCII值不在以上范围内
                    {
                        if (intNumber[j] > 122)//判断字符ASCII值是否大于z
                        {
                            strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
                        }
                        else
                        {
                            strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
                        }

                    }
                    //label3.Text = strAsciiName;//得到注册码
                }
                return strAsciiName;
            }
            else
            {
                return "";
            }
        }
    }
}
