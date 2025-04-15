using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 标签
    /// </summary>
	public class LabelItem : MonoBehaviour {
        private Transform m_Cam = null;

        /// <summary>
        /// 距离
        /// </summary>
        private float Distance;

        /// <summary>
        /// 正常距离
        /// </summary>
        private float nomoalDis = 28f;

        #region //缩放
        private float rate;

        private Vector3 _scale;

        private Vector3 currentScale;

        private Vector3 desiredScale;

        private float speedScale= 5;
        #endregion

        #region 旋转
        private Vector3 _rotation;



        #endregion
        void Awake()
        {
            m_Cam = Camera.main.transform;
            currentScale = transform.localScale;

        }

        void LateUpdate()
        {

            _rotation = new Vector3(m_Cam.eulerAngles.x, m_Cam.eulerAngles.y, m_Cam.eulerAngles.z);
            transform.eulerAngles = _rotation;

            Distance = Vector3.Distance(m_Cam.position, transform.position);

            rate = Distance / nomoalDis;

            desiredScale = currentScale * rate;
            //_scale = Vector3.Lerp(currentScale, desiredScale, Time.deltaTime*speed);
            transform.localScale = desiredScale;
        }

    }
}