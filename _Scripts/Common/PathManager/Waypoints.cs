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
    /// 路径  （新）
    /// </summary>
	public class Waypoints : MonoBehaviour {

        public List<Transform> points = new List<Transform>();


        // Use this for initialization
        void Awake()
        {
            foreach (Transform item in transform)
            {
                points.Add(item);
            }
        }

    }
}