using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public class UndoManager : Singleton<UndoManager>
    {
        /// <summary>
        /// 执行撤销操作时，触发事件
        /// </summary>
        public event Action UndoPerformed;
        
        /// <summary>
        /// 执行重做操作时，触发事件
        /// </summary>
        public event Action RedoPerformed;

        /// <summary>
        /// 状态改变时，触发
        /// </summary>
        public event Action OnStatusChanged;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 撤销记录列表
        /// </summary>
        private List<IUndoRecord> m_UndoStack = new List<IUndoRecord>();

        /// <summary>
        /// 重做记录列表
        /// </summary>
        private List<IUndoRecord> m_RedoStack = new List<IUndoRecord>();

        /// <summary>
        /// 正在执行 撤销操作
        /// </summary>
        private bool undoing = false;

        /// <summary>
        ///  正在执行 重做操作
        /// </summary>
        private bool redoing = false;

        private int maxRecords = 10;
        /// <summary>
        /// 存储记录最大值
        /// </summary>
        public int MaxRecords
        {
            get { return maxRecords; }
            set
            {
                if (maxRecords <= 0)
                {
                    throw new ArgumentOutOfRangeException("max records can`t be <= 0");
                }

                maxRecords = value;
            }
        }

        /// <summary>
        /// 是否存在撤销记录
        /// </summary>
        public bool HasUndoRecords
        {
            get { return m_UndoStack.Count != 0; }
        }

        /// <summary>
        /// 是否存在重做记录
        /// </summary>
        public bool HasRedoRecords
        {
            get { return m_RedoStack.Count != 0; }
        }

        /// <summary>
        /// 撤销记录数量
        /// </summary>
        public int UndoRecordCount
        {
            get { return m_UndoStack.Count; }
        }

        /// <summary>
        /// 重做记录数量
        /// </summary>
        public int RedoRecordCount
        {
            get { return m_RedoStack.Count; }
        }

        /// <summary>
        /// 存储当前执行撤销/重做操作的组
        /// </summary>
        private UndoGroup currentGroup;

        /// <summary>
        /// 开始组
        /// </summary>
        /// <param name="group"></param>
        public void BeginGroup(UndoGroup group)
        {
            if (currentGroup == null)
            {
                currentGroup = group;
                m_UndoStack.Push(new UndoGroup(group.Name));
                m_RedoStack.Push(new UndoGroup(group.Name));
            }
        }

        /// <summary>
        /// 结束组
        /// </summary>
        /// <param name="group"></param>
        public void EndGroup(UndoGroup group)
        {
            if (currentGroup == group)
            {
                currentGroup = null;

                if (m_UndoStack.Count > 0)
                {
                    UndoGroup t = m_UndoStack[0] as UndoGroup;
                    if (t != null && t.RecordCount == 0)
                    {
                        m_UndoStack.Pop();
                    }
                }

                if (m_RedoStack.Count > 0)
                {
                    UndoGroup t = m_RedoStack[0] as UndoGroup;
                    if (t != null && t.RecordCount == 0)
                    {
                        m_RedoStack.Pop();
                    }
                }
            }
        }

        /// <summary>
        /// 记录对象 PropertyInfo
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyInfo"></param>
        public void RecordObject(object target, PropertyInfo propertyInfo)
        {
            Push(x => { RecordObject(target, propertyInfo); propertyInfo.SetValue(target, x); }, propertyInfo.GetValue(target), "set " + propertyInfo.Name + " value");
        }

        /// <summary>
        /// 将一个操作推到撤消/重做堆栈上。
        /// 1）如果这是在撤销/重做操作的上下文之外调用的，该项将被添加到撤销堆栈中。
        /// 2）如果在撤销操作的上下文中调用该项，则将该项添加到重做堆栈中。
        /// 3) 如果这是在重做操作的上下文中调用的，项目被添加到撤消堆栈。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <param name="description"></param>
        public void Push<T>(UndoAction<T> action, T data, string description = "")
        {
            List<IUndoRecord> stack = null;

            // 决定将此操作添加到的堆栈
            if (undoing) // 正在撤销
            {
                stack = m_RedoStack;
            }
            else
            {
                stack = m_UndoStack;
            }

            // 如果添加了一个撤销记录，且当前不是撤销/重做状态，清空重做列表
            if (!undoing && !redoing)
            {
                m_RedoStack.Clear();
            }

            // 如果当前组不为空，则将该记录添加到组中。
            if (currentGroup == null)
            {
                stack.Push(new UndoRecord<T>(action, data, description));
            }
            else
            {
                (stack[0] as UndoGroup).AddRecord(new UndoRecord<T>(action, data, description));
            }

            // 如果堆栈数量超过最大值
            if (stack.Count > MaxRecords)
            {
                stack.RemoveRange(MaxRecords - 1, stack.Count - MaxRecords);
            }
            // 状态
            OnStatusChanged?.Invoke();
        }

        /// <summary>
        /// 执行撤销操作
        /// </summary>
        public void Undo()
        {
            try
            {
                undoing = true;

                if (m_UndoStack.Count == 0)
                {
                    throw new InvalidOperationException("nothing in the undo stack!");
                }

                IUndoRecord record = m_UndoStack.Pop();
                // 如此存储的操作是一个组，那么也将撤销操作作为组执行。
                if (record is UndoGroup)
                {
                    BeginGroup(record as UndoGroup);
                }

                record.Execute();
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
                //Debug.LogException(ex);
            }
            finally
            {
                undoing = false;
                EndGroup(currentGroup);
                UndoPerformed?.Invoke();
                OnStatusChanged?.Invoke();
            }
        }

        /// <summary>
        /// 执行重做操作
        /// </summary>
        public void Redo()
        {
            try
            {
                redoing = true;
                if (m_RedoStack.Count == 0)
                {
                    throw new InvalidOperationException("nothing in the redo stack!");
                }

                IUndoRecord record = m_RedoStack.Pop();
                // 如此存储的操作是一个组，那么也将撤销操作作为组执行。
                if (record is UndoGroup)
                {
                    BeginGroup(record as UndoGroup);
                }

                record.Execute();
            }
            catch(Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
            finally
            {
                redoing = false;
                EndGroup(currentGroup);
                RedoPerformed?.Invoke();
                OnStatusChanged?.Invoke();
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            m_UndoStack.Clear();
            m_RedoStack.Clear();
            OnStatusChanged?.Invoke();
        }

        /// <summary>
        /// 返回撤销堆栈的记录列表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetUndoStackInformation()
        {
            return m_UndoStack.ConvertAll(x => x.Name == null ? "" : x.Name);
        }

        /// <summary>
        /// 返回重做堆栈的记录列表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetRedoStackInformation()
        {
            return m_RedoStack.ConvertAll(x => x.Name == null ? "" : x.Name);
        }
    }
}
