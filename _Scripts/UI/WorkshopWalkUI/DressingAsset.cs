using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 行走曲线
    /// </summary>
    public class DressingAsset : MonoBehaviour
    {
        /// <summary>
        /// 开始点
        /// </summary>
        private MiniMapItem startPoint;

        /// <summary>
        /// 结束点
        /// </summary>
        private MiniMapItem endPoint;

        /// <summary>
        /// 路径点控制器
        /// </summary>
        private PathPointController pathPointControl;

        /// <summary>
        /// 小地图
        /// </summary>
        private MiniMap m_MiniMap;

        /// <summary>
        /// 默认矩阵相机尺寸
        /// </summary>
        public float orthographicSize = 7;

        /// <summary>
        /// 小地图相机的Trans
        /// </summary>
        private Transform miniCameraLocation;

        /// <summary>
        /// 更衣流程碰撞器
        /// </summary>
        private DressingProcessCollision conllision;

        /// <summary>
        /// 玩家初始点
        /// </summary>
        public Transform playerLocation;

        /// <summary>
        /// 小地图相机名称
        /// </summary>
        public string miniMapName;

        /// <summary>
        /// 更衣流程
        /// </summary>
        public TextAsset DressingProcess;

        /// <summary>
        /// 着装
        /// </summary>
        public TextAsset Dressing;

        /// <summary>
        /// 地图索引
        /// </summary>
        public int mapIndex;

        /// <summary>
        /// 洁净等级
        /// </summary>
        public Cleanliness cleanliness = Cleanliness.N;

        /// <summary>
        /// 有切换更衣流程曲线
        /// </summary>
        public bool ExistCurve = true;

        public class OnCollisionEvent : UnityEvent<DressingAsset> { }

        private OnCollisionEvent m_OnCollision = new OnCollisionEvent();
        /// <summary>
        /// 进入更衣流程事件
        /// </summary>
        public OnCollisionEvent OnCollision
        {
            get { return m_OnCollision; }
            set { m_OnCollision = value; }
        }

        private UnityEvent m_OnCompleted = new UnityEvent();
        /// <summary>
        /// 展示完成事件
        /// </summary>
        public UnityEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }

        void Awake()
        {

            playerLocation = transform.Find("playerLocation");
            conllision = transform.Find("DressingProcessCollision").GetComponent<DressingProcessCollision>();
            if (ExistCurve)
            {
                m_MiniMap = MiniMapUtils.GetWorldMap(miniMapName);
                startPoint = transform.Find("StartPoint").GetComponent<MiniMapItem>();
                endPoint = transform.Find("EndPoint").GetComponent<MiniMapItem>();
                pathPointControl = transform.Find("PathPoints").GetComponent<PathPointController>();
                miniCameraLocation = transform.Find("miniCameraLocation");
                pathPointControl.OnCompleted.AddListener(pathPointControl_OnCompleted);
            }

            conllision.OnCollision.AddListener(conllision_OnCollision);
        }

        /// <summary>
        /// 展示行走曲线和相机设置
        /// </summary>
        public void Display()
        {
            StartCoroutine(Displaying());
        }

        /// <summary>
        /// 展示过程
        /// </summary>
        /// <returns></returns>
        IEnumerator Displaying()
        {
            if (ExistCurve)
            {
                m_MiniMap.MiniMapCamera.transform.position = miniCameraLocation.position;
                m_MiniMap.DefaultSize = orthographicSize;
                yield return new WaitForSeconds(0.5f);
                startPoint.ShowMark();
                yield return new WaitForSeconds(0.5f);
                pathPointControl.Play();
            }
            else
            {
                yield return new WaitForSeconds(0.6f);
                OnCompleted.Invoke();
            }
        }

        /// <summary>
        /// 关闭路径曲线
        /// </summary>
        public void Close()
        {
            if (!ExistCurve)
                return;

            StopAllCoroutines();
            startPoint.HideMark();
            endPoint.HideMark();
            pathPointControl.Stop();
        }

        /// <summary>
        /// 触发更衣流程
        /// </summary>
        private void conllision_OnCollision()
        {
            OnCollision.Invoke(this);
        }

        /// <summary>
        /// 播放完成时，触发
        /// </summary>
        private void pathPointControl_OnCompleted()
        {
            endPoint.ShowMark();
            this.Invoke(0.5f, () => { OnCompleted.Invoke(); });
        }
    }
}
