using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Common;

namespace UnityEngine.UI.Extensions
{
    /// <summary>
    /// 文本框
    /// </summary>
    public class TextBox : InputField
    {
        private int finalCaretPosition;
        /// <summary>
        /// 插入字符的最终位置
        /// </summary>
        public int FinalCaretPosition
        {
            get { return finalCaretPosition; }
            set { finalCaretPosition = value; }
        }

        protected override void OnDisable()
        {
            finalCaretPosition = caretPosition;
            base.OnDisable();
        }

        /// <summary>
        /// Handle the specified event.
        /// </summary>
        private Event processingEvent = new Event();

        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (!isFocused)
                return;

            bool consumedEvent = false;
            while (Event.PopEvent(processingEvent))
            {
                if (processingEvent.rawType == EventType.KeyDown)
                {
                    consumedEvent = true;
                    var shouldContinue = KeyPressed(processingEvent);
                    if (shouldContinue == EditState.Finish)
                    {
                        finalCaretPosition = caretPosition;
                        DeactivateInputField();
                        break;
                    }
                }

                switch (processingEvent.type)
                {
                    case EventType.ValidateCommand:
                    case EventType.ExecuteCommand:
                        switch (processingEvent.commandName)
                        {
                            case "SelectAll":
                                SelectAll();
                                consumedEvent = true;
                                break;
                        }
                        break;
                }
            }

            if (consumedEvent)
                UpdateLabel();

            eventData.Use();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            finalCaretPosition = caretPosition;
            base.OnDeselect(eventData);
        }

        /// <summary>
        /// 在相应位置插入值
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="value"></param>
        public void Insert(int startIndex, string value)
        {
            StartCoroutine(InsertValue(startIndex, value));
        }

        IEnumerator InsertValue(int startIndex, string value)
        {
            //Select();
            ActivateInputField();
            yield return new WaitForEndOfFrame();
            caretPosition = startIndex;
            caretPositionInternal = startIndex;
            caretSelectPositionInternal = startIndex;

            string replaceString = value;
            // Can't go past the character limit
            if (characterLimit > 0 && text.Length >= characterLimit)
                yield break;

            m_Text = text.Insert(m_CaretPosition, replaceString);
            caretSelectPositionInternal = caretPositionInternal += replaceString.Length;

            if (onValueChanged != null)
                onValueChanged.Invoke(text);

            UpdateLabel();
        }
    }
}
