using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.Common
{
    public enum InputButton : int
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public class InternalTrigger : MonoBehaviour
    {
        /// <summary>
        /// TriggerEvent
        /// </summary>
        [Serializable]
        public class TriggerEvent : UnityEvent<BaseInternalData> { }

        [Serializable]
        public class Entry
        {
            public InternalTriggerType eventID = InternalTriggerType.OnMouseDown;
            public TriggerEvent callback = new TriggerEvent();
        }

        protected InternalTrigger() { }

        [SerializeField]
        private List<Entry> m_Triggers;
        /// <summary>
        /// Triggers
        /// </summary>
        public List<Entry> Triggers
        {
            get
            {
                if (m_Triggers == null)
                {
                    m_Triggers = new List<Entry>();
                }
                return m_Triggers;
            }
            set { m_Triggers = value; }
        }

        private void Execute(InternalTriggerType id, BaseInternalData args)
        {
            for (int i = 0, imax = Triggers.Count; i < imax; i++)
            {
                var ent = Triggers[i];
                if (ent.eventID == id && ent.callback != null)
                    ent.callback.Invoke(args);
            }
        }

        public void Awake()
        {
            Execute(InternalTriggerType.OnAwake, new BaseInternalData(gameObject));
        }

        public void Start()
        {
            Execute(InternalTriggerType.OnStart, new BaseInternalData(gameObject));
        }

        public void Update()
        {
            Execute(InternalTriggerType.OnUpdate, new BaseInternalData(gameObject));

            if (Input.GetMouseButton(0))
                Execute(InternalTriggerType.OnGetMouseButton, new PointerInternalData(gameObject, InputButton.Left));

            if (Input.GetMouseButton(1))
                Execute(InternalTriggerType.OnGetMouseButton, new PointerInternalData(gameObject, InputButton.Right));

            if (Input.GetMouseButton(2))
                Execute(InternalTriggerType.OnGetMouseButton, new PointerInternalData(gameObject, InputButton.Middle));


            if (Input.GetMouseButtonDown(0))
                Execute(InternalTriggerType.OnGetMouseButtonDown, new PointerInternalData(gameObject, InputButton.Left));

            if (Input.GetMouseButtonDown(1))
                Execute(InternalTriggerType.OnGetMouseButtonDown, new PointerInternalData(gameObject, InputButton.Right));

            if (Input.GetMouseButtonDown(2))
                Execute(InternalTriggerType.OnGetMouseButtonDown, new PointerInternalData(gameObject, InputButton.Middle));


            if (Input.GetMouseButtonUp(0))
                Execute(InternalTriggerType.OnGetMouseButtonUp, new PointerInternalData(gameObject, InputButton.Left));

            if (Input.GetMouseButton(1))
                Execute(InternalTriggerType.OnGetMouseButtonUp, new PointerInternalData(gameObject, InputButton.Right));

            if (Input.GetMouseButton(2))
                Execute(InternalTriggerType.OnGetMouseButtonUp, new PointerInternalData(gameObject, InputButton.Middle));
        }

        public void FixedUpdate()
        {
            Execute(InternalTriggerType.OnFixedUpdate, new BaseInternalData(gameObject));
        }

        public void LateUpdate()
        {
            Execute(InternalTriggerType.OnLateUpdate, new BaseInternalData(gameObject));
        }

        private void OnMouseDown()
        {
            Execute(InternalTriggerType.OnMouseDown, new BaseInternalData(gameObject));
        }

        private void OnMouseDrag()
        {
            Execute(InternalTriggerType.OnMouseDrag, new BaseInternalData(gameObject));
        }

        private void OnMouseEnter()
        {
            Execute(InternalTriggerType.OnMouseEnter, new BaseInternalData(gameObject));
        }

        private void OnMouseExit()
        {
            Execute(InternalTriggerType.OnMouseExit, new BaseInternalData(gameObject));
        }

        private void OnMouseOver()
        {
            Execute(InternalTriggerType.OnMouseOver, new BaseInternalData(gameObject));
        }

        private void OnMouseUp()
        {
            Execute(InternalTriggerType.OnMouseUp, new BaseInternalData(gameObject));
        }

        private void OnTriggerEnter(Collider other)
        {
            Execute(InternalTriggerType.OnTriggerEnter, new BaseInternalData(gameObject));
        }

        private void OnTriggerExit(Collider other)
        {
            Execute(InternalTriggerType.OnTriggerExit, new BaseInternalData(gameObject));
        }


        private void OnTriggerStay(Collider other)
        {
            Execute(InternalTriggerType.OnTriggerStay, new BaseInternalData(gameObject));
        }

        public void OnDrawGizmos()
        {
            Execute(InternalTriggerType.OnDrawGizmos, new BaseInternalData(gameObject));
        }
    }
}
