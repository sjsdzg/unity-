using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace XFramework.Common
{
    [RequireComponent(typeof(Camera))]
    public class MouseLook : MonoBehaviour
    {
        private bool m_Interactable = true;
        /// <summary>
        /// 是否交互
        /// </summary>
        public bool Interactable
        {
            get { return m_Interactable; }
            set { m_Interactable = value; }
        }

        public float xSensitivity = 2f;
        public float ySensitivity = 2f;
        public float yMinLimit = -90;
        public float yMaxLimit = 90;
        private float x;
        private float y;

        private void Start()
        {
            SyncEulerAngles();
        }

        public void SyncEulerAngles()
        {
            x = transform.eulerAngles.y;
            y = transform.eulerAngles.x;
        }

        private void Update()
        {
            if (!m_Interactable)
                     
                return;

            //if (IsPointerOverUI())
            //    return;

            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSensitivity;
                y -= Input.GetAxis("Mouse Y") * ySensitivity;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
                transform.eulerAngles = new Vector3(y, x, 0);
            }
        }

        public float ClampAngle(float angle, float min, float max)
        {
            angle %= 360;
            if (angle < -360f)
            {
                angle += 360f;
            }
            if (angle > 360f)
            {
                angle -= 360f;
            }
            return Mathf.Clamp(angle, min, max);
        }

        /// <summary>
        /// 传送
        /// </summary>
        /// <param name="target"></param>
        /// <param name="callback"></param>
        /// <param name="instant"></param>
        /// <param name="duration"></param>
        public void Teleport(Transform target, UnityAction callback = null, bool instant = true, float duration = 1f)
        {
            Teleport(target.position, target.rotation, callback, instant, duration);
        }

        /// <summary>
        /// 传送
        /// </summary>
        /// <param name="position"></param>
        /// <param name="callback"></param>
        /// <param name="immediate"></param>
        public void Teleport(Vector3 position, UnityAction callback = null, bool instant = true, float duration = 1f)
        {
            Teleport(position, transform.rotation, callback, instant, duration);
        }

        /// <summary>
        /// 传送
        /// </summary>
        /// <param name="position"></param>
        /// <param name="callback"></param>
        /// <param name="immediate"></param>
        public void Teleport(Quaternion rotation, UnityAction callback = null, bool instant = true, float duration = 1f)
        {
            Teleport(transform.position, rotation, callback, instant, duration);
        }

        /// <summary>
        /// 传送
        /// </summary>
        /// <param name="position"></param>
        /// <param name="callback"></param>
        /// <param name="immediate"></param>
        public void Teleport(Vector3 position, Quaternion rotation, UnityAction callback = null, bool instant = true, float duration = 1f)
        {
            if (instant)
            {
                transform.position = position;
                transform.rotation = rotation;
                SyncEulerAngles();
                callback?.Invoke();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(TeleportEnumerator(position, rotation, callback, duration));
            }
        }

        /// <summary>
        /// 传送协程
        /// </summary>
        /// <param name="target"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator TeleportEnumerator(Vector3 position, Quaternion rotation, UnityAction callback = null, float duration = 1f)
        {
            float time = 0;
            while (time <= duration)
            {
                transform.position = Vector3.Lerp(transform.position, position, time/ duration);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, time/ duration);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
               
            }

            SyncEulerAngles();
            callback?.Invoke();
        }

        /// <summary>
        /// 判断是否在UI上
        /// </summary>
        /// <returns></returns>
        public bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == 5)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

