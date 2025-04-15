using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    public class PartItemComponent : MonoBehaviour
    {
        /// <summary>
        /// 物品图标路径
        /// </summary>
        public const string PATH = "EquipmentSimulation";

        /// <summary>
        /// 图标
        /// </summary>
        private Image icon;

        /// <summary>
        /// 文本
        /// </summary>
        private Text text;

        /// <summary>
        /// 部件信息
        /// </summary>
        public EquipmentPart data;

        /// <summary>
        /// 点击事件类
        /// </summary>
        public class ClickEvent : UnityEvent<PartItemComponent> { }

        /// <summary>
        /// Toggle
        /// </summary>
        private Toggle m_Toggle;

        private ClickEvent m_OnClick = new ClickEvent();
        /// <summary>
        /// 工具点击事件
        /// </summary>
        public ClickEvent OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        void Awake()
        {
            icon = transform.Find("Background/Icon").GetComponent<Image>();
            text = transform.Find("Label").GetComponent<Text>();
            m_Toggle = transform.GetComponent<Toggle>();

            m_Toggle.onValueChanged.AddListener(m_Toggle_onValueChanged);
        }


        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="part"></param>
        public void SetValue(EquipmentPart part)
        {
            data = part;
            string assetBundleName = "Assets/Textures/Equipments/" + part.EquipmentName;
            string assetName = part.Sprite;
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<Sprite>(assetBundleName, assetName);
            //string path = string.Format("{0}/{1}/Icons/{2}", PATH, equipment.EquipmentName, equipment.Name);
            //Sprite sprite = Resources.Load<Sprite>(path);
            if (async != null)
            {
                async.OnCompleted(x => {
                    AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                    Sprite sprite = loader.GetAsset<Sprite>();
                    if (sprite != null)
                    {
                        icon.sprite = sprite;
                    }
                });
            }

            text.text = part.Name;
        }


        private void m_Toggle_onValueChanged(bool flag)
        {
            if (flag)
            {
                OnClick.Invoke(this);
            }
        }
    }
}
