using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 房间人物路径管理
    /// </summary>
	public class WorkShopPathManager : MonoBehaviour {

        /// <summary>
        /// 路径字典
        /// </summary>
        public Dictionary<string, Waypoints> waypointDic = new Dictionary<string, Waypoints>();
        public Transform [] Points;

		// Use this for initialization
		void Start () {

            //for (int i = 0; i < transform.childCount; i++)
            //{
            //    Waypoints w = transform.FindChild("路径 ("+i+")").GetComponent<Waypoints>();
            //    Register(w.name,w);
            //}
            foreach (Transform item in transform)
            {
                    Waypoints w = item.GetComponent<Waypoints>();
                    Register(w.name, w);
            }
		}
		
		public void Register(string name,Waypoints point)
        {
            if (!waypointDic.ContainsKey(name))
            {
                waypointDic.Add(name,point);
            }
        }
        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="name"></param>
        public Waypoints GetWaypoints(string name)
        {
            if(waypointDic.ContainsKey(name))
            {
                return waypointDic[name];
            }
            return null;
        }
	}
}