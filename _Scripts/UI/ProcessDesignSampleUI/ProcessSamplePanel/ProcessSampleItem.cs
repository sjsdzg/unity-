using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace XFramework.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ProcessSampleItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// Icon
        /// </summary>
        private Image m_Icon;
        /// <summary>
        /// image
        /// </summary>
        private Image m_Image;
        /// <summary>
        /// Text
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 占位
        /// </summary>
        private GameObject placeHolder = null;
        /// <summary>
        /// 正常 sprite
        /// </summary>
        [SerializeField]
        private Sprite normalSprite;

        /// <summary>
        /// 选中 sprite
        /// </summary>
        [SerializeField]
        private Sprite selectSprite;
        /// <summary>
        /// 
        /// </summary>
        public Canvas Canvas { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public ProcessLibraryItemData Data { get; set; }

        private void Awake()
        {
            m_Image = GetComponent<Image>();
            m_Icon = transform.Find("Icon").GetComponent<Image>();
            m_Text = transform.Find("Text").GetComponent<Text>();
        }

        public void SetData(ProcessLibraryItemData data)
        {
            Data = data;
            m_Icon.sprite = data.Sprite;
            m_Text.text = data.Name;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            placeHolder = Instantiate(gameObject);
           // placeHolder.AddComponent<RectTransform>();
            placeHolder.transform.SetParent(transform.parent);
            placeHolder.transform.SetSiblingIndex(transform.GetSiblingIndex());

            transform.SetParent(Canvas.transform);
            GetComponent<CanvasGroup>().blocksRaycasts = false;


        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(placeHolder.transform.parent);
            transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy(placeHolder);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_Image.sprite = selectSprite;
           // transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_Image.sprite = normalSprite;
            // transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }
    }
}

