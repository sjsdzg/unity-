using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.IO
{
    public class ArchCode
    {
        #region string 0 - 29
        /// <summary>
        /// 标签
        /// </summary>
        public const short tag = 0;

        /// <summary>
        /// 字符串类型
        /// </summary>
        public const short @string = 1;

        /// <summary>
        /// 头部
        /// </summary>
        public const short head = 2;

        /// <summary>
        /// id
        /// </summary>
        public const short id = 3;

        /// <summary>
        /// 专业
        /// </summary>
        public const short special = 4;

        /// <summary>
        /// 墙角0
        /// </summary>
        public const short corner0_id = 5;

        /// <summary>
        /// 墙角1
        /// </summary>
        public const short corner1_id = 6;

        /// <summary>
        /// 墙体
        /// </summary>
        public const short wall_id = 7;

        /// <summary>
        /// 门类型
        /// </summary>
        public const short doorType = 8;

        /// <summary>
        /// 名称
        /// </summary>
        public const short name = 9;

        /// <summary>
        /// 链接房间
        /// </summary>
        public const short relatedRooms = 10;

        /// <summary>
        /// 链接墙体
        /// </summary>
        public const short linkedWalls = 11;

        /// <summary>
        /// 地址
        /// </summary>
        public const short address = 12;
        #endregion

        #region int 30 - 39
        /// <summary>
        /// 整数类型
        /// </summary>
        public const short @int = 30;

        /// <summary>
        /// 序号 1 - 
        /// </summary>
        public const short number = 31;

        /// <summary>
        /// 索引 0 - 
        /// </summary>
        public const short index = 32;
        #endregion

        #region Vector3 40 - 49
        /// <summary>
        /// Vector3 类型
        /// </summary>
        public const short @Vector3 = 40;

        /// <summary>
        /// 位置
        /// </summary>
        public const short position = 41;

        /// <summary>
        /// 缩放
        /// </summary>
        public const short scale = 42;
        #endregion

        #region Quaternion 50 - 59
        /// <summary>
        /// Quaternion 类型
        /// </summary>
        public const short @Quaternion = 50;

        /// <summary>
        /// 旋转
        /// </summary>
        public const short rotation = 51;
        #endregion

        #region float 60 - 89
        /// <summary>
        /// 浮点类型
        /// </summary>
        public const short @float = 60;

        /// <summary>
        /// 长度
        /// </summary>
        public const short length = 61;

        /// <summary>
        /// 高度
        /// </summary>
        public const short height = 62;

        /// <summary>
        /// 厚度
        /// </summary>
        public const short thickness = 63;

        /// <summary>
        /// 离地距离（地板）
        /// </summary>
        public const short bottom = 64;

        /// <summary>
        /// 离地高度（水平面）
        /// </summary>
        public const short altitude = 65;

        /// <summary>
        /// 面积
        /// </summary>
        public const short area = 66;
        #endregion

        #region long 90 - 99
        /// <summary>
        /// 长整型类型
        /// </summary>
        public const short @long = 90;
        #endregion

        #region bool 100 - 109
        public const short @bool = 91;

        /// <summary>
        /// 是否激活
        /// </summary>
        public const short active = 91;
        #endregion
    }
}
