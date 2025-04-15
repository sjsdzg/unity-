using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 右键菜单项
    /// </summary>
    public class ContextMenuItem : Selectable, IEventSystemHandler, IPointerClickHandler, ISubmitHandler
    {
        public const string path = "ContextMenu/";

        /// <summary>
        /// 文本
        /// </summary>
        public Text text;

        /// <summary>
        /// 图标
        /// </summary>
        public Image icon;

        /// <summary>
        /// 右键菜单
        /// </summary>
        public ContextMenuEx Parent { get; private set; }

        /// <summary>
        /// 右键菜单项序号
        /// </summary>
        public int MenuItemId { get; private set; }

        private bool m_Enabled = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled
        {
            get { return m_Enabled; }
            set
            {
                if (m_Enabled == value)
                    return;

                interactable = m_Enabled;
            }
        }

        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private UnityEvent m_OnClick = new UnityEvent();

        public UnityEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            m_OnClick.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Press();

            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="parameter"></param>
        public void SetValue(ContextMenuEx menu, ContextMenuParameter parameter)
        {
            Parent = menu;
            MenuItemId = parameter.ID;
            //设置图标
            Sprite sprite = Resources.Load<Sprite>(path + parameter.Icon);
            if (sprite == null)
            {
                icon.gameObject.SetActive(false);
            }
            else
            {
                icon.sprite = sprite;
            }
            //设置文本
            text.text = parameter.Content;
            //underline.gameObject.SetActive(parameter.isUnderline);
            interactable = parameter.Enabled;
            if (!parameter.Enabled)
            {
                text.color = Color.gray;
            }
            //点击时触发回调
            m_OnClick.AddListener(() => { parameter.call(parameter); });
        }

    }
}
