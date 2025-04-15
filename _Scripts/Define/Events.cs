using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    public static class Events
    {
        public static readonly string Unkown = "Unkown";

        public static class Entity
        {
            /// <summary>
            /// NPC说话事件
            /// </summary>
            public static readonly string Speak = "Entity.Speak";
            /// <summary>
            /// NPC放置事件
            /// </summary>
            public static readonly string Drop = "Entity.Drop";
        }
        public static class ArchiteIntroduce
        {
            /// <summary>
            /// 
            /// </summary>
            public static readonly string SceneToUI = "ArchiteIntroduce.SceneToUI";
            /// <summary>
            /// 
            /// </summary>
            public static readonly string UIToScene = "ArchiteIntroduce.UIToScene";
        }
        public static class PLC
        {
            /// <summary>
            /// 
            /// </summary>
            public static readonly string PLCToWorkshop = "PLC.PLCToWorkshop";
        }
        public static class Process
        {
            public static class ProcessSortItem
            {
                /// <summary>
                /// 
                /// </summary>
                public static readonly string AddSortItem = "Process.ProcessSortItem.AddSortItem";
                /// <summary>
                /// 
                /// </summary>
                public static readonly string UpdateSortItem = "Process.ProcessSortItem.UpdateSortItem";
            }
           
        }
        /// <summary>
        /// Selector Event
        /// </summary>
        public static class Selector
        {
            /// <summary>
            /// Selector选中事件
            /// </summary>
            public static readonly string Select = "Selector.Select";
            /// <summary>
            /// Selector未选中事件
            /// </summary>
            public static readonly string Deselect = "Selector.Deselect";
            /// <summary>
            /// Useable
            /// </summary>
            public static readonly string Useable = "Selector.Useable";
        }

        public static class Prompt
        {
            /// <summary>
            /// 显示
            /// </summary>
            public static readonly string Show = "Prompt.Show";
            /// <summary>
            /// 隐藏
            /// </summary>
            public static readonly string Hide = "Prompt.Hide";
        }

        /// <summary>
        /// 对话
        /// </summary>
        public static class Dialogue
        {
            /// <summary>
            /// 显示
            /// </summary>
            public static readonly string Show = "Dialogue.Show";
            /// <summary>
            /// 隐藏
            /// </summary>
            public static readonly string Hide = "Dialogue.Hide";
            /// <summary>
            /// 提交
            /// </summary>
            public static readonly string Submit = "Dialogue.Submit";
        }

        /// <summary>
        /// Item Event
        /// </summary>
        public static class Item
        {
            /// <summary>
            /// 物品
            /// </summary>
            public static class Goods
            {
                public static readonly string Add = "Item.Goods.Add";
                public static readonly string Remove = "Item.Goods.Remove";
                public static readonly string Click = "Item.Goods.Click";
                public static readonly string Clear = "Item.Goods.Clear";
            }

            /// <summary>
            /// 文件
            /// </summary>
            public static class Document
            {
                public static readonly string Add = "Item.Document.Add";
                public static readonly string Remove = "Item.Document.Remove";
                public static readonly string Click = "Item.Document.Click";
                public static readonly string Open = "Item.Document.Open";
                public static readonly string Close = "Item.Document.Close";
                public static readonly string Result = "Item.Document.Result";
            }

            /// <summary>
            /// 清洁
            /// </summary>
            public static class Clean
            {
                public static readonly string Add = "Item.Clean.Add";
                public static readonly string Remove = "Item.Clean.Remove";
            }
        }

        public static class LogInfo
        {
            /// <summary>
            /// 显示日志
            /// </summary>
            public static readonly string Show = "LogInfo.Show";
        }

        public static class HUDText
        {
            /// <summary>
            /// 显示HUDText
            /// </summary>
            public static readonly string Show = "HUDText.Show";
        }

        public static class KnowledgePoint
        {
            /// <summary>
            /// 知识点通知
            /// </summary>
            public static readonly string Notify = "KnowledgePoint.Notify";
        }

        public static class Procedure
        {
            /// <summary>
            /// 当前步骤
            /// </summary>
            public static readonly string Current = "Procedure.Current";

            /// <summary>
            /// 完成当前步骤
            /// </summary>
            public static readonly string Completed = "Procedure.Completed";
            /// <summary>
            ///初始化当前步骤
            /// </summary>
            public static readonly string Initialize = "Procedure.Initialize";
            /// <summary>
            ///初始化ActionState
            /// </summary>
            public static readonly string InitActionState = "Procedure.InitActionState";
            /// <summary>
            /// 当前大步骤弹出框
            /// </summary>
            public static readonly string StepPrompt = "Procedure.StepPrompt";
        }

        public static class Interactable
        {
            /// <summary>
            /// 误操作
            /// </summary>
            public static readonly string Misoperation = "Interactable.Misoperation";
        }

        /// <summary>
        /// 状态
        /// </summary>
        public static class Status
        {
            /// <summary>
            /// 初始化
            /// </summary>
            public static readonly string Init = "Status.Init";

            /// <summary>
            /// 更新
            /// </summary>
            public static readonly string Update = "Status.Update";
        }

        /// <summary>
        /// 故障
        /// </summary>
        public static class Fault
        {
            public static readonly string AddPhenomena = "Fault.AddPhenomena";
            /// <summary>
            /// 完成名称
            /// </summary>
            public static readonly string NameCompleted = "Fault.CompletedName";
            /// <summary>
            /// 完成故障原因
            /// </summary>
            public static readonly string CauseCompleted = "Fault.CompletedCause";
        }

        public static class Guide
        {
            /// <summary>
            /// 显示引导
            /// </summary>
            public static readonly string Show = "Guide.Show";
        }
        /// <summary>
        /// 任务监控
        /// </summary>
        public static class TaskMonitor
        {
            public static readonly string Add = "TaskMonitor.Add";
            public static readonly string Remove = "TaskMonitor.Remove";
        }
    }
}
