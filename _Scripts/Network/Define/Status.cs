using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Network
{
    /**
    * Status Code
    * 1XX 信息性状态码（Informational）      服务器正在处理请求
    *
    * 2XX 成功状态码（Success）              请求已正常处理完毕
    * 200 OK                               表示请求被服务器正常处理
    *
    * 3XX 重定向状态码（Redirection）        需要进行额外操作以完成请求
    *
    * 4XX 客户端错误状态码（Client Error）   客户端原因导致服务器无法处理请求
    * 400 Bad Request                      表示请求报文存在语法错误或参数错误，服务器不理解
    * 404 Not Found                        表示服务器找不到你请求的资源
    *
    * 5XX 服务器错误状态码（Server Error）   服务器原因导致处理请求出错
    * 500 Internal Server Error            表示服务器执行请求的时候出错了
    */
    public class Status
    {
        //成功
        public const byte OK                            = 0x20;    // 成功
        public const byte NO_CONTENT                    = 0x24;    // 无内容。服务器成功处理，但未返回内容。该条记录不存在或已被删除。

        //客户端错误
        public const byte BAD_REQUEST                   = 0x40;    // 请求报文存在语法错误或参数错误，服务器不理解
        public const byte NOT_FOUND                     = 0x44;    // 服务器找不到你请求的资源

        //服务器错误
        public const byte SERVER_ERROR                  = 0x50;    // 服务器错误
        public const byte SERVER_BUSY                   = 0x51;    // 服务器繁忙
        public const byte DATA_ACCESS_EXCEPTION         = 0x52;    // 数据库访问异常
    }
}
