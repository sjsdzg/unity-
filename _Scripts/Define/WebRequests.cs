using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Define
{
    public static class WebRequests
    {
        public static class NetworkDisk
        {
            /// <summary>
            /// 添加磁盘
            /// </summary>
            public const string Add = "disk/add";
            /// <summary>
            /// 根据名称，获取磁盘
            /// </summary>
            public const string GetByName = "disk/getbyname";
            /// <summary>
            /// 获取所有磁盘
            /// </summary>
            public const string ListAll = "disk/listall";
            /// <summary>
            /// 删除磁盘
            /// </summary>
            public const string Remove = "disk/remove";
        }

        public static class NetworkFile
        {
            /// <summary>
            /// 添加文件
            /// </summary>
            public const string Add = "file/add";
            /// <summary>
            /// 根据ID，获取文件
            /// </summary>
            public const string Get = "file/get";
            /// <summary>
            /// 根据ID，获取文件列表
            /// </summary>
            public const string List = "file/list";
            /// <summary>
            /// 添加文件夹
            /// </summary>
            public const string Mkdir = "file/mkdir";
            /// <summary>
            /// 移动文件
            /// </summary>
            public const string Move = "file/move";
            /// <summary>
            /// 删除文件
            /// </summary>
            public const string Remove = "file/remove";
            /// <summary>
            /// 重命名文件
            /// </summary>
            public const string Rename = "file/rename";
            /// <summary>
            /// 上传文件
            /// </summary>
            public const string Upload = "file/upload";
            /// <summary>
            /// 下载文件
            /// </summary>
            public const string Download = "file/download";
            /// <summary>
            /// 下载文件
            /// </summary>
            public const string Preview = "file/preview";
            /// <summary>
            /// 根据条件分页
            /// </summary>
            public const string PageByCondition = "file/pagebycondition";
        }
    }
}
