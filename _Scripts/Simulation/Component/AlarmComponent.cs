using UnityEngine;
using System.Collections;

namespace XFramework.Component
{
    /// <summary>
    /// 报警器
    /// </summary>
    public class AlarmComponent : MonoBehaviour
    {
        /// <summary>
        /// 报警灯
        /// </summary>
        private GameObject m_FoggyLight;

        private bool isFlashing;
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsFlashing
        {
            get { return isFlashing; }
            set
            {
                if (isFlashing == value)
                    return;

                isFlashing = value;
                if (isFlashing)
                {
                    StartCoroutine(Flashing());
                }
            }
        }

        /// <summary>
        /// 频率
        /// </summary>
        private float flashingFreq = 0.2f;

        private void Awake()
        {
            m_FoggyLight = transform.Find("警报灯/FoggyLight").gameObject;
        }

        /// <summary>
        /// 报警
        /// </summary>
        /// <returns></returns>
        IEnumerator Flashing()
        {
            while (IsFlashing)
            {
                m_FoggyLight.SetActive(true);
                yield return new WaitForSeconds(flashingFreq);
                m_FoggyLight.SetActive(false);
                yield return new WaitForSeconds(flashingFreq);
            }
        }
    }
}

