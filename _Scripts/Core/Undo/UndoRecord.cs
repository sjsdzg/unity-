using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public delegate void UndoAction<T>(T data);

    public class UndoRecord<T> : IUndoRecord
    {
        /// <summary>
        /// 撤销/重做行为
        /// </summary>
        private UndoAction<T> action;

        /// <summary>
        /// 撤销/重做数据
        /// </summary>
        private T data;

        /// <summary>
        /// 描述
        /// </summary>
        private string description;

        public string Name
        {
            get { return description; }
        }

        public UndoRecord()
        {

        }

        public UndoRecord(UndoAction<T> action, T data, string description)
        {
            this.action = action;
            this.data = data;
            this.description = description;
        }

        public void Execute()
        {
            action(data);
        }
    }
}
