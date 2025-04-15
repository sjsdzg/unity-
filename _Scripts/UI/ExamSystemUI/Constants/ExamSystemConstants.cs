using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.UI
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public class ExamSystemConstants
    {
        /// <summary>
        /// 试题类型常量
        /// </summary>
        public class QuestionType
        {
            /// <summary>
            /// 单选题
            /// </summary>
            public const int SINGLE_CHOICE = 1;
            /// <summary>
            /// 多选题
            /// </summary>
            public const int MULTIPLE_CHOICE = 2;
            /// <summary>
            /// 判断题
            /// </summary>
            public const int JUDGMENT = 3;
            /// <summary>
            /// 填空题
            /// </summary>
            public const int BLANK_FILL = 4;
            /// <summary>
            /// 名词解释
            /// </summary>
            public const int EXPLAIN = 5;
            /// <summary>
            /// 问答题
            /// </summary>
            public const int ESSAY = 6;
            /// <summary>
            /// 操作题
            /// </summary>
            public const int OPERATION = 7;

            public static string GetComment(int type)
            {
                string comment = string.Empty;
                switch (type)
                {
                    case SINGLE_CHOICE:
                        comment = "单选题";
                        break;
                    case MULTIPLE_CHOICE:
                        comment = "多选题";
                        break;
                    case JUDGMENT:
                        comment = "判断题";
                        break;
                    case BLANK_FILL:
                        comment = "填空题";
                        break;
                    case EXPLAIN:
                        comment = "名词解释";
                        break;
                    case ESSAY:
                        comment = "简答题";
                        break;
                    case OPERATION:
                        comment = "操作题";
                        break;
                    default:
                        break;
                }
                return comment;
            }
        }

        /// <summary>
        /// 试题难度等级
        /// </summary>
        public class QuestionLevel
        {
            /// <summary>
            /// 简单
            /// </summary>
            public const int EASY = 1;
            /// <summary>
            /// 一般
            /// </summary>
            public const int GENERAL = 2;
            /// <summary>
            /// 困难
            /// </summary>
            public const int DIFFICULTY = 3;

            public static string GetComment(int level)
            {
                string comment = string.Empty;
                switch (level)
                {
                    case EASY:
                        comment = "简单";
                        break;
                    case GENERAL:
                        comment = "一般";
                        break;
                    case DIFFICULTY:
                        comment = "困难";
                        break;
                    default:
                        break;
                }
                return comment;
            }
        }
        
        /// <summary>
        /// 状态
        /// </summary>
        public class Status
        {
            /// <summary>
            /// 禁用
            /// </summary>
            public const int DISABLE = 0;
            /// <summary>
            /// 启用
            /// </summary>
            public const int ENABLE = 1;

            public static string GetComment(int status)
            {
                string comment = string.Empty;
                switch (status)
                {
                    case DISABLE:
                        comment = "禁用";
                        break;
                    case ENABLE:
                        comment = "启用";
                        break;
                    default:
                        break;
                }
                return comment;
            }
        }
    }

    /// <summary>
    /// 操作方式
    /// </summary>
    public enum OperationType
    {
        Add,
        Modify,
    }
}
