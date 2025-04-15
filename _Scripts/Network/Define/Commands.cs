using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Network
{
    public class Commands
    {
        /// <summary>
        /// 试题
        /// </summary>
        public class Question
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0001;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0002;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0003;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0004;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0005;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0006;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0007;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0008;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0009;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x000A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x000B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x000C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x000D;
        }

        /// <summary>
        /// 试题库
        /// </summary>
        public class QuestionBank
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0011;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0012;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0013;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0014;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0015;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0016;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0017;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0018;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0019;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x001A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x001B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x001C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x001D;
        }

        /// <summary>
        /// 试卷
        /// </summary>
        public class Paper
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0021;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0022;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0023;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0024;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0025;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0026;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0027;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0028;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0029;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x002A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x002B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x002C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x002D;
        }

        /// <summary>
        /// 试卷分类
        /// </summary>
        public class PaperCategory
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0031;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0032;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0033;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0034;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0035;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0036;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0037;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0038;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0039;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x003A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x003B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x003C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x003D;
        }

        /// <summary>
        /// 考试
        /// </summary>
        public class Exam
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0041;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0042;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0043;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0044;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0045;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0046;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0047;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0048;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0049;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x004A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x004B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x004C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x004D;
        }

        /// <summary>
        /// 考试分类
        /// </summary>
        public class ExamCategory
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0051;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0052;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0053;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0054;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0055;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0056;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0057;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0058;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0059;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x005A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x005B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x005C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x005D;
        }

        /// <summary>
        /// 考试数据
        /// </summary>
        public class ExamData
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0061;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0062;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0063;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0064;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0065;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0066;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0067;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0068;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0069;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x006A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x006B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x006C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x006D;
        }

        /// <summary>
        /// 专业
        /// </summary>
        public class Branch
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0071;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0072;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0073;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0074;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0075;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0076;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0077;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0078;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0079;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x007A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x007B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x007C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x007D;
        }

        /// <summary>
        /// 班级
        /// </summary>
        public class Position
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0081;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0082;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0083;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0084;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0085;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0086;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0087;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0088;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0089;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x008A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x008B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x008C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x008D;
        }

        /// <summary>
        /// 用户
        /// </summary>
        public class User
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x0091;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x0092;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x0093;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x0094;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0095;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x0096;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0097;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x0098;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x0099;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x009A;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x009B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x009C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x009D;
            /// <summary>
            /// 根据用户名，获取用户
            /// </summary>
            public static short GET_BY_NAME = 0x009E;
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        public class Role
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x00A1;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x00A2;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x00A3;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x00A4;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x00A5;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x00A6;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x00A7;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x00A8;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x00A9;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x00AA;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x00AB;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x00AC;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x00AD;
        }


        /// <summary>
        /// 考试系统基本请求
        /// </summary>
        public class ExamBasic
        {
            /// <summary>
            /// 获取考试统计信息
            /// </summary>
            public const short GET_STATS_INFO = 0x00B1;
            /// <summary>
            /// 获取考试试卷
            /// </summary>
            public static short GET_EXAM_PAPER = 0x00B2;
            /// <summary>
            /// 获取最近几条考试记录
            /// </summary>
            public static short LIST_LATELY_EXAM = 0x00B3;
            /// <summary>
            /// 考试分析
            /// </summary>
            public const short EXAM_ANALYSIS = 0x00B4;
            /// <summary>
            /// 成绩分析
            /// </summary>
            public const short SCORE_ANALYSIS = 0x00B5;
        }

        /// <summary>
        /// 用户关卡信息请求
        /// </summary>
        public class LevelInfo
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x00C1;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x00C2;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x00C3;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x00C4;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x00C5;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x00C6;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x00C7;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x00C8;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x00C9;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x00CA;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x00CB;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x00CC;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x00CD;
        }

        /// <summary>
        ///  用户登录日志
        /// </summary>
        public class UserLoginLog
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x00D1;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x00D2;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x00D3;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x00D4;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x00D5;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x00D6;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x00D7;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x00D8;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x00D9;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x00DA;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x00DB;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x00DC;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x00DD;
        }

        /// <summary>
        ///  用户操作日志
        /// </summary>
        public class UserOperationLog
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x00E1;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x00E2;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x00E3;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x00E4;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x00E5;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x00E6;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x00E7;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x00E8;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x00E9;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x00EA;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x00EB;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x00EC;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x00ED;

        }

        /// <summary>
        ///  服务器状态日志
        /// </summary>
        public class ServerStatusLog
        {
            /// <summary>
            /// 增加
            /// </summary>
            public static short INSERT = 0x00F1;
            /// <summary>
            /// 删除
            /// </summary>
            public static short DELETE = 0x00F2;
            /// <summary>
            /// 更新
            /// </summary>
            public static short UPDATE = 0x00F3;
            /// <summary>
            /// 查询
            /// </summary>
            public static short GET = 0x00F4;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x00F5;
            /// <summary>
            /// 查询所有
            /// </summary>
            public static short LIST_ALL = 0x00F6;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x00F7;
            /// <summary>
            /// 获取总数
            /// </summary>
            public static short COUNT = 0x00F8;
            /// <summary>
            /// 分页查询
            /// </summary>
            public static short PAGE = 0x00F9;
            /// <summary>
            /// 根据条件获取总数
            /// </summary>
            public static short COUNT_BY_CONDITION = 0x00FA;
            /// <summary>
            /// 根据条件分页查询
            /// </summary>
            public static short PAGE_BY_CONDITION = 0x00FB;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x00FC;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x00FD;

        }



        /// <summary>
        /// 网关层
        /// </summary>
        public class Gateway
        {
            /// <summary>
            /// 登陆命令
            /// <summary>
            public static short USER_LOGIN = 0x0101;
            /// <summary>
            /// 订阅系统用户状态
            /// <summary>
            public static short SUB_USER_STATUS = 0x0102;
            /// <summary>
            /// 用户状态更改
            /// <summary>
            public static short USER_STATUS_CHANGE = 0x0103;
            /// <summary>
            /// 用户注销
            /// <summary>
            public static short USER_EXIT = 0x0104;
            /// <summary>
            /// 用户重连
            /// <summary>
            public static short RECONNECT = 0x0105;
            /// <summary>
            /// 获取在线用户
            /// </summary>
            public static short GET_ONLINE_USER = 0x0106;
            /// <summary>
            /// 强制下线用户
            /// </summary>
            public static short FORCED_OFFLINE_USER = 0x0107;
            /// <summary>
            /// 订阅用户下线
            /// </summary>
            public static short USER_OFFLINE = 0x0108;
            /// <summary>
            /// 获取服务器简介
            /// </summary>
            public static short GET_SERVER_PROFILE = 0x0109;
            /// <summary>
            /// 获取服务软件简介
            /// </summary>
            public static short GET_SOFTWARE_PROFILE = 0x010A;
        }

        /// <summary>
        /// 实验数据记录
        /// <summary>
        public class EmpiricalDataRecord
        {
            /// <summary>
            /// 增加
            /// <summary>
            public static short INSERT = 0x0201;
            /// <summary>
            /// 删除
            /// <summary>
            public static short DELETE = 0x0202;
            /// <summary>
            /// 更新
            /// <summary>
            public static short UPDATE = 0x0203;
            /// <summary>
            /// 查询
            /// <summary>
            public static short GET = 0x0204;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0205;
            /// <summary>
            /// 查询所有
            /// <summary>
            public static short LIST_ALL = 0x0206;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0207;
            /// <summary>
            /// 获取总数
            /// <summary>
            public static short COUNT = 0x0208;
            /// <summary>
            /// 分页查询
            /// <summary>
            public static short PAGE = 0x0209;
            /// <summary>
            /// 根据条件获取总数
            /// <summary>
            public static short COUNT_BY_CONDITION = 0x020A;
            /// <summary>
            /// 根据条件分页查询
            /// <summary>
            public static short PAGE_BY_CONDITION = 0x020B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x020C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x020D;

        }

        /// <summary>
        /// 第三方用户
        /// <summary>
        public class OpenUser
        {
            /// <summary>
            /// 增加
            /// <summary>
            public static short INSERT = 0x0301;
            /// <summary>
            /// 删除
            /// <summary>
            public static short DELETE = 0x0302;
            /// <summary>
            /// 更新
            /// <summary>
            public static short UPDATE = 0x0303;
            /// <summary>
            /// 查询
            /// <summary>
            public static short GET = 0x0304;
            /// <summary>
            /// 根据条件，查询
            /// </summary>
            public static short GET_BY_CONDITION = 0x0305;
            /// <summary>
            /// 查询所有
            /// <summary>
            public static short LIST_ALL = 0x0306;
            /// <summary>
            /// 根据条件，查询列表
            /// </summary>
            public static short LIST_BY_CONDITION = 0x0307;
            /// <summary>
            /// 获取总数
            /// <summary>
            public static short COUNT = 0x0308;
            /// <summary>
            /// 分页查询
            /// <summary>
            public static short PAGE = 0x0309;
            /// <summary>
            /// 根据条件获取总数
            /// <summary>
            public static short COUNT_BY_CONDITION = 0x030A;
            /// <summary>
            /// 根据条件分页查询
            /// <summary>
            public static short PAGE_BY_CONDITION = 0x030B;
            /// <summary>
            /// 批量添加
            /// </summary>
            public static short BATCH_INSERT = 0x030C;
            /// <summary>
            /// 批量删除
            /// </summary>
            public static short BATCH_DELETE = 0x030D;


        }
    }
}
