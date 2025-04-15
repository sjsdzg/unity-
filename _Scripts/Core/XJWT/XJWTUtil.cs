using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using XFramework.Common;

namespace XFramework.Core
{
    public class XJWTUtil
    {
        public static string secret = "hm7cm1";
        public static string aeskey = "13YoDwEn8KRx7YU9BTKG2TCOVtVDGoKxsrZdYd2Tshg=";
        public static long issueId = 100239;

        /// <summary>
        /// 解密 token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Dencrty(string token)
        {
            try
            {
                // 获取当前时间
                long now = DateTimeUtil.ToEpochMilli(DateTime.Now);
                // 创建JWT实例
                XJWT xjwt = new XJWT(secret, aeskey, issueId);
                token = WWW.UnEscapeURL(token, Encoding.UTF8);
                // 调用解密方法解密token
                string result = xjwt.verifyAndDecrypt(token, now);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string Encrty(string json)
        {
            try
            {
                // 获取当前时间
                long now = DateTimeUtil.ToEpochMilli(DateTime.Now);
                // 创建JWT实例
                XJWT xjwt = new XJWT(secret, aeskey, issueId);
                // 创建payload用来装参数
                ByteBuffer payload = ByteBuffer.Allocate(1024);
                payload.Put(Encoding.UTF8.GetBytes(json));
                payload.Flip();
                // 创建out对象
                ByteBuffer outBuffer = ByteBuffer.Allocate(1024);
                // 调用加密方法，加密参数
                xjwt.encryptAndSign((byte)XJWT.Type.SYS, payload, outBuffer, now + 60 * 60 * 1000);
                string result = Encoding.UTF8.GetString(outBuffer.ToArray(), 0, outBuffer.Remaining);
                result = WWW.EscapeURL(result);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
