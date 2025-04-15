using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class ButtonUploadCell : ButtonCell, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private RectTransform uploadOptions;

        /// <summary>
        /// 上传文件
        /// </summary>
        private Button buttonFile;

        /// <summary>
        /// 上传文件
        /// </summary>
        private Button buttonDir;

        private void Start()
        {
            if (uploadOptions == null)
            {
                Debug.LogError("uploadOptions is null");
            }
            uploadOptions.gameObject.SetActive(false);

            buttonFile = uploadOptions.Find("ButtonFile").GetComponent<Button>();
            buttonDir = uploadOptions.Find("ButtonDir").GetComponent<Button>();

            buttonFile.onClick.AddListener(buttonFile_onClick);
            buttonDir.onClick.AddListener(buttonDir_onClick);
        }

        private void buttonFile_onClick()
        {
            OnClicked.Invoke(ButtonCellType.UploadFile);
        }

        private void buttonDir_onClick()
        {
            OnClicked.Invoke(ButtonCellType.UploadDirectory);
        }


        public override ButtonCellType GetButtonCellType()
        {
            return ButtonCellType.UploadFile;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            uploadOptions.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uploadOptions.gameObject.SetActive(false);
        }
    }
}
