using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    /// <summary>
    /// HUD组件
    /// </summary>
    public abstract class HUDComponent<T> : MonoBehaviour where T : HUDInfo
    {
        /// <summary>
        /// 指示器信息
        /// </summary>
        [SerializeField]
        public T hudInfo;

        /// <summary>
        /// 初始化时
        /// </summary>
        public bool OnStartCreated = false;

        /// <summary>
        /// 是否创建
        /// </summary>
        public bool HasCreated { get; set; }

        public HUDView View { get; set; }

        void Start()
        {
            if (OnStartCreated)
            {
                CreateHUD();
            }
        }

        public virtual void CreateHUD()
        {
            if (HUDManager.Instance != null)
            {
                if (hudInfo.m_Target == null)
                {
                    hudInfo.m_Target = transform;
                }
                View = HUDManager.Instance.CreateHUD(hudInfo);
                HasCreated = true;
            }
            else
            {
                Debug.LogError("Need have a HUD Manager in scene");
            }
        }

        public virtual void show()
        {
            
        }

        public virtual void hide()
        {

        }
    }
}
