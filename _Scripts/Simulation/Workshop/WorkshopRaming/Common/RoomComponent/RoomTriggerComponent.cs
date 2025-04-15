using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    public enum SpaceType
    {
        Inter,
        Exter,
    }
    /// <summary>
    /// 触发
    /// </summary>
	public class RoomTriggerComponent : MonoBehaviour
    {

        public SpaceType spaceType;

        private BeforeEvent m_TouchEvent = new BeforeEvent();

        public BeforeEvent onTouchEvent
        {
            get { return m_TouchEvent; }
            set { m_TouchEvent = value; }
        }
        private void OnTriggerExit(Collider other)
        {
            BeforeEventArgs arg = new BeforeEventArgs("", CallBack);
            onTouchEvent.Invoke(gameObject, arg);
        }

        private void CallBack(BeforeEventArgs arg)
        {

        }
    }
} 