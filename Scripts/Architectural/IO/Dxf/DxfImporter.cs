using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Architectural
{
    public class DxfImporter : IUpdate, IDisposable
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isDone { get; private set; }

        /// <summary>
        /// 进度
        /// </summary>
        public float progress { get; private set; }

        /// <summary>
        /// 文档
        /// </summary>
        public netDxf.DxfDocument document { get; private set; }

        /// <summary>
        /// 进度事件事件
        /// </summary>
        public event Action<DxfImporter> onUpdate;

        /// <summary>
        /// 完成事件
        /// </summary>
        public event Action<DxfImporter> onCompleted;

        /// <summary>
        /// 流
        /// </summary>
        private Stream stream;

        /// <summary>
        /// 流长度
        /// </summary>
        private long totalLength;

        /// <summary>
        /// 开始时间
        /// </summary>
        private float startTime;

        public DxfImporter(Stream stream)
        {
            MonoDriver.Attach(this);
            this.stream = stream;
            totalLength = stream.Length;
        }

        public void Import()
        { 
            Thread thread = new Thread(LoadDxf);
            thread.IsBackground = true;
            thread.Start();
            // 启动时间
            startTime = Time.time;
        }

        private void LoadDxf()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            document = netDxf.DxfDocument.Load(stream);
            isDone = true;

            stopwatch.Stop();
            Debug.LogFormat("--- 导入 dxf，耗时：{0}【秒】---", stopwatch.ElapsedMilliseconds * 0.001f);
        }

        public void Update()
        {
            if (stream != null  && totalLength > 0)
            {
                if (Time.time - startTime < 1f) // 2秒内为0
                    progress = 0;
                else
                    progress = stream.Position * 1.0f / totalLength;

                onUpdate?.Invoke(this);
                LoadingBar.Instance.Show(progress, "正在加载图纸中。。。");
            }

            if (isDone)
            {
                LoadingBar.Instance.Hide();
                Dispose();
                onCompleted?.Invoke(this);
            }
        }
        public void Dispose()
        {
            MonoDriver.Detach(this);
            this.stream.Dispose();
        }
    }
}
