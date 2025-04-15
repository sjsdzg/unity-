using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public class UndoGroup : IUndoRecord, IDisposable
    {
        private string name;
        /// <summary>
        /// 撤销/重做 组名称
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// 撤销/重做 记录列表
        /// </summary>
        private List<IUndoRecord> records = new List<IUndoRecord>();

        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordCount
        {
            get { return records.Count; }
        }

        public UndoGroup(string name = "")
        {
            this.name = name;

            UndoManager.Instance.BeginGroup(this);
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="record"></param>
        public void AddRecord(IUndoRecord record)
        {
            records.Insert(0, record);
        }

        public void Execute()
        {
            records.ForEach(x => x.Execute());
        }

        public void Dispose()
        {
            UndoManager.Instance.EndGroup(this);
        }
    }
}
