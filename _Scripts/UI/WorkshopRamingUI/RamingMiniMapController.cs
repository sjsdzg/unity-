using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.UI
{
    public class RamingMiniMapController : MonoBehaviour
    {
        /// <summary>
        /// 地图数组
        /// </summary>
        private Transform[] maps;

        /// <summary>
        /// 洁净分区字典
        /// </summary>
        private CleanLevelPartition[] parts;

        /// <summary>
        /// 地面触发器
        /// </summary>
        private GroundConllision[] grounds;

        public class OnGroundCollisionEvent : UnityEvent<int> { }

        private OnGroundCollisionEvent m_OnGroundCollision = new OnGroundCollisionEvent();
        /// <summary>
        /// 地面触发事件
        /// </summary>
        public OnGroundCollisionEvent OnGroundCollision
        {
            get { return m_OnGroundCollision; }
            set { m_OnGroundCollision = value; }
        }

        void Awake()
        {
            //maps = new Transform[2];
            //maps[0] = transform.Find("一层地图");
            //maps[1] = transform.Find("二层地图");

            //parts = new CleanLevelPartition[2];
            //parts[0] = transform.Find("一层地图/洁净等级分区").GetComponent<CleanLevelPartition>();
            //parts[1] = transform.Find("二层地图/洁净等级分区").GetComponent<CleanLevelPartition>();

            //grounds = new GroundConllision[2];
            //grounds[0] = transform.Find("一层地面碰撞器").GetComponent<GroundConllision>();
            //grounds[1] = transform.Find("二层地面碰撞器").GetComponent<GroundConllision>();

            //grounds[0].OnGroundCollision.AddListener(ground_OnGroundCollision);
            //grounds[1].OnGroundCollision.AddListener(ground_OnGroundCollision);
        }

        /// <summary>
        /// 人物和
        /// </summary>
        /// <param name="index"></param>
        private void ground_OnGroundCollision(int index)
        {
            OnGroundCollision.Invoke(index);
        }

        void Start()
        {
            //maps[1].gameObject.SetActive(true);
            //maps[1].gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示地图
        /// </summary>
        /// <param name="index"></param>
        public void DisplayMap(int index)
        {
            if (index > maps.Length || index < 0)
                return;

            for (int i = 0; i < maps.Length; i++)
            {
                if (i == index)
                {
                    maps[i].gameObject.SetActive(true);
                }
                else
                {
                    maps[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 显示洁净分区
        /// </summary>
        /// <param name="mapIndex"></param>
        /// <param name="partName"></param>
        public void DisplayPartition(int mapIndex, string partName, bool b = true)
        {
            parts[mapIndex].Display(partName, b);
        }
    }
}
