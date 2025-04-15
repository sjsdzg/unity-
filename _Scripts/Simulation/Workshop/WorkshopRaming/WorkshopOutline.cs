using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 
    /// </summary>
	public class WorkshopOutline : MonoBehaviour ,IPointerClickHandler{

        private UnityEvent m_OnClickWorkShop = new UnityEvent();

        private bool isAffect;

        public bool IsAffect 
        {
            get { return isAffect; }
            set { isAffect = value;
             transform.GetComponent<Collider>().enabled = value;

            }
        }

        /// <summary>
        /// 点击房间事件
        /// </summary>
        public UnityEvent OnClickWorkShop
        {
            get { return m_OnClickWorkShop; }
            set { m_OnClickWorkShop = value; }
        }

        private MeshRenderer m_MeshRender;

        // Use this for initialization
        void Start () {
            m_MeshRender = transform.GetComponent<MeshRenderer>();
            HideOutLine();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickWorkShop.Invoke();
        }

        public void ShowOutLine()
        {
            m_MeshRender.enabled = true;
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void HideOutLine()
        {
            m_MeshRender.enabled = false;
        }
    }
}