using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace XFramework.Core
{
    public class WebRequestOperation : AsyncLoadOperation
    {
        UnityWebRequest request;
        AsyncOperation async;

        public string GetText()
        {
            return request.downloadHandler.text;
        }

        public WebRequestOperation(UnityWebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("UnityWebRequest");

            this.request = request;
            async = this.request.Send();
        }

        public override bool Update()
        {
            if (/*request.isNetworkError ||*/ !string.IsNullOrEmpty(request.error))
            {
                string msg = string.Format("Failed request from {0}: {1}", request.url, request.error);
                Error = msg;
                IsDone = true;
            }

            if (async != null )
            {
                Progress = async.progress;
                IsDone = async.isDone;
            }

            Notify();

            return !IsDone;
        }
    }
}
