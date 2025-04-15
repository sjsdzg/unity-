using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 物体状态管理
    /// </summary>
	public class ObjectStateItem : MonoBehaviour {

        public bool IsInitStatic = false;
        private void Awake()
        {
         
        }
        // Use this for initialization
        void Start () {
		}
		
		// Update is called once per frame
		void Update () {
            if (IsInitStatic)
            {
                SetStatic();
            }
            else
            {
                SetNotStatic();
            }
        }

        public void SetStatic()
        {
            gameObject.isStatic = true;
            SetChild(gameObject,true);
        }

        public void SetNotStatic()
        {
            gameObject.isStatic = false;
            SetChild(gameObject,false);
        }

        public void SetChild(GameObject obj,bool bo)
        {
            int length = obj.transform.childCount;
            for (int i = 0; i < length; i++)
            {
                GameObject go = obj.transform.GetChild(i).gameObject;
                go.isStatic = bo;
                SetChild(go,bo);
            }
        }

	}
}