using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public static class RequestExtensions
    {
        /// <summary>
        /// 更新事件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Request UpdateEvent(this Request request, Action<Request> action)
        {
            request.updateEvent += action;
            return request;
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Request CompletedEvent(this Request request, Action<Request> action)
        {
            request.completedEvent += action;
            return request;
        }
    }
}
