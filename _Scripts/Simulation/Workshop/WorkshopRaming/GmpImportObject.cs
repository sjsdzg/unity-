using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Component
{
    /// <summary>
    /// Gmp 重要物体
    /// </summary>
	public class GmpImportObject : MonoBehaviour {


        public Dictionary<string, UnityEngine.GameObject> GmpDic = new Dictionary<string, UnityEngine.GameObject>();

        /// <summary>
        /// Lable组件
        /// </summary>
        private List<HUDLabelComponent> lableList = new List<HUDLabelComponent>();

        /// <summary>
        /// 高亮组件
        /// </summary>
        private List<TwinklingComponent> twinklingList = new List<TwinklingComponent>();
         
        private void Start()
        {
            foreach(Transform item in transform)
            {
                foreach (Transform element in item)
                {
                    string _name = item.name + "_" + element.name;
                    Register(_name, element.gameObject);
                }
            }
               
        }

        /// <summary>
        /// 注册物体
        /// </summary>
        /// <param name="obj"></param>
        public void  Register(string name, UnityEngine.GameObject obj)
        {
            name = name.Trim();
            if (!GmpDic.ContainsKey(name))
            {
                GmpDic.Add(name, obj);
                HUDLabelComponent lableCom = obj.GetComponent<HUDLabelComponent>();
                if(lableCom!=null)
                    lableList.Add(lableCom);
                TwinklingComponent twinkCom = obj.GetComponent<TwinklingComponent>();
                if (twinkCom != null)
                    twinklingList.Add(twinkCom);

            }
        }
        public GameObject GetGameObject(string name)
        {
            GameObject obj = null;
            if (GmpDic.TryGetValue(name.Trim(),out obj))
            {
                return obj;
            }
            return null;
        }
        /// <summary>
        /// 获取物体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetGameobject(string name)
        {

            UnityEngine.GameObject obj = null;
            if(!GmpDic.TryGetValue(name.Trim(),out obj))
            {
                throw new ApplicationException("GMPDic TryGetValue Failure! name : " + name.ToString());
            }
            return obj;
        }

        /// <summary>
        /// 高闪物体
        /// </summary>
        /// <param name="name"></param>
        public void TwinklingObject(string name)
        {

            GameObject obj = GetGameObject(name);

            if (obj == null)
                return;
            for (int i = 0; i < twinklingList.Count; i++)
            {
                TwinklingComponent tCom = obj.GetComponent<TwinklingComponent>();
                print("遍历高闪组件:"+ twinklingList[i].name+"  "+obj.name); 
                if (twinklingList[i].gameObject.Equals(obj))
                {
                    tCom.isTwinkling = true;
                    tCom.ShowTwinkling(true);

                }
                else
                {
                    twinklingList[i].isTwinkling = false;
                }
            }
        }

        public void HUDLableShow(string name)
        {
            GameObject obj = GetGameObject(name);

            if (obj == null)
                return;
            for (int i = 0; i < lableList.Count; i++)
            {
                HUDLabelComponent hCom = obj.GetComponent<HUDLabelComponent>();
                print("遍历HUD组件:" + twinklingList[i].name + "  " + obj.name);

                if ( lableList[i].gameObject.Equals(obj))
                {
                    hCom.show();
                }
                else
                {
                    hCom.hide();
                }
            }
          
            
        }
        /// <summary>
        /// 关闭标签显示所有
        /// </summary>
        public void HUDLableHideAll()
        {
            for (int i = 0; i < lableList.Count; i++)
            {
                lableList[i].hide();
            }
        }
        /// <summary>
        /// 关闭所有高亮
        /// </summary>
        public void TwinklingObjectCLoseAll()
        {
            for (int i = 0; i < twinklingList.Count; i++)
            {
                twinklingList[i].isTwinkling = false;
            }
        }

    }
}