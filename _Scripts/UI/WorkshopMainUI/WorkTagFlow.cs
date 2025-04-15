using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 生产车间 流程 标识
    /// </summary>
	public class WorkTagFlow : MonoBehaviour {

        /// <summary>
        ///标识点
        /// </summary>
        public WorkTagPointItem[] points;


        /// <summary>
        /// 时间间隔
        /// </summary>
        public float Interval = 0.1f;

        /// <summary>
        /// 是否在执行
        /// </summary>
        private bool doing = false;

        /// <summary>
        /// 最佳视角
        /// </summary>
        private BestAngle m_BestAngle;

        public class OnClickedEvent : UnityEvent<string> { }

        private OnClickedEvent m_WorkPointClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent WorkPointClicked
        {
            get { return m_WorkPointClicked; }
            set { m_WorkPointClicked = value; }
        }
        private void Awake()
        {
            m_BestAngle = transform.Find("BestAngle").GetComponent<BestAngle>();

            Transform parent = transform.Find("流程");
            points = new WorkTagPointItem[parent.childCount];

            string _name = string.Empty;
            for (int i = 0; i < parent.childCount; i++)
            {

                //_name = string.Format("工艺_{0}",i);
                WorkTagPointItem point = parent.GetChild(i).GetComponent<WorkTagPointItem>() ;
                points[i] = point;
                point.OnClicked.AddListener((x) => {
                    WorkPointClicked.Invoke(x);
                });
                //if(parent.FindChild(_name)!=null)
                //{
                //    point = parent.FindChild(_name).GetComponent<WorkTagPointItem>();

                //    point.OnClicked.AddListener((x)=> {
                //        WorkPointClicked.Invoke(x);
                //    });
                //}
            }
        }

        public void Appear()
        {
            if (doing)
                return;

            StopAllCoroutines();
            m_BestAngle.Enter();
            StartCoroutine(Appearing());
            doing = true;

        }
        /// <summary>
        /// 消失
        /// </summary>
        public void Disappear()
        {
            StopAllCoroutines();
            doing = false;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].Disappear();
            }
        }
        /// <summary>
        /// 出现协程
        /// </summary>
        /// <returns></returns>
        IEnumerator Appearing()
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < points.Length; i++)
            {
                points[i].Disappear();
            }
            yield return new WaitForSeconds(0.5f);
            int index = 0;
            while (index < points.Length)
            {
                points[index].Appear();

                yield return new WaitForSeconds(Interval);
                index++;
            }

            doing = false;
        }

        /// <summary>
        /// 显示高亮
        /// </summary>
        /// <param name="name"></param>
        public void SetHighlightByName(string name)
        {
            for (int i = 0; i < points.Length; i++)
            {
                string PointText = points[i].GetComponentInChildren<Text>().text;
                //if(string.Equals(PointText, name))
                //{
                //    points[i].ShowHighlight();
                //    print("找到了");
                //}
                if(name.Contains(PointText))
                {
                    points[i].ShowHighlight();
                    print("找到了");
                }
                //print("++:"+ PointText + "  target"+name);
            }
        }
    }
}