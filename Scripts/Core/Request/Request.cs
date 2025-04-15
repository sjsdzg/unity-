using System;
using System.Collections;
using UnityEngine;

namespace XFramework.Core
{
    public abstract class Request : IEnumerator
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isDone { get; protected set; }

        /// <summary>
        /// 进度
        /// </summary>
        public float progress { get; protected set; }

        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool isError 
        { 
            get { return !string.IsNullOrEmpty(error); }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string error { get; protected set; }

        /// <summary>
        /// 更新事件
        /// </summary>
        public event Action<Request> updateEvent;

        /// <summary>
        /// 完成事件
        /// </summary>
        public event Action<Request> completedEvent;

        public object Current
        { 
            get
            {
                return null;
            }
        }

        public bool MoveNext()
        {
            return !isDone;
        }

        public void Reset()
        {

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            OnUpdate();

            OnNotify();

            return !isDone;
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        public abstract void OnUpdate();

        /// <summary>
        /// 通知操作
        /// </summary>
        protected virtual void OnNotify()
        {
            if (isError)
            {
                isDone = true;
                Debug.LogError(error);
            }

            try
            {
                updateEvent?.Invoke(this);

                if (isDone)
                {
                    completedEvent?.Invoke(this);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                isDone = true;
                Debug.LogException(e);
            }
        }
    }
}
