using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;
using XFramework.Core;
using UnityEngine.Events;
namespace XFramework.UI
{
    public class ProcessDropItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private UniEvent<ProcessDropItem> m_OnSelected = new UniEvent<ProcessDropItem>();
        /// <summary>
        /// 参数触发事件
        /// </summary>
        public UniEvent<ProcessDropItem> OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }
        private UniEvent<ProcessDropItem> m_OnEnqueued = new UniEvent<ProcessDropItem>();
        /// <summary>
        /// 入列事件
        /// </summary>
        public UniEvent<ProcessDropItem> OnEnqueued
        {
            get { return m_OnEnqueued; }
            set { m_OnEnqueued = value; }
        }
        private UniEvent<ProcessDropItem> m_OnDeleted = new UniEvent<ProcessDropItem>();
        /// <summary>
        /// 删除事件
        /// </summary>
        public UniEvent<ProcessDropItem> OnDeleted
        {
            get { return m_OnDeleted; }
            set { m_OnDeleted = value; }
        }
        private UnityEvent updateDropItem = new UnityEvent() { };
        public UnityEvent UpdateDropItem
        {
            get { return updateDropItem; }
            set { updateDropItem = value; }
        }
        /// <summary>
        /// Icon
        /// </summary>
        private Image m_Icon;
        public Sprite Sprite
        {
            get { return m_Icon.sprite; }
            set
            {
                m_Icon.sprite = value;
            }
        }

        /// <summary>
        /// Text
        /// </summary>
        private Text m_Text;
        public string Text
        {
            get { return m_Text.text; }
            set { m_Text.text = value; }
        }

        /// <summary>
        /// 清除按钮
        /// </summary>
        private Button buttonDelete;
        /// <summary>
        /// 参数设置按钮
        /// </summary>
        private Button buttonParamSet;

        /// <summary>
        /// 序号文本
        /// </summary>
        private Text m_TextNumber;
        public string TextNumber
        {
            get { return m_TextNumber.text; }
            set { m_TextNumber.text = value;
                currentNumber = int.Parse(m_TextNumber.text);
            }

        }
        public int currentNumber;
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
        [SerializeField]
        private RectTransform m_Container;
        /// <summary>
        /// 占位
        /// </summary>
        private GameObject placeHolder = null;
        private GameObject nextArrow;
        public GameObject NextArrow
        {
            get { return nextArrow; }
        }
        /// <summary>
        /// Data
        /// </summary>
        public ProcessLibraryItemData Data { get; set; }

        private void Awake()
        {
            // gui
            m_Icon = transform.Find("Frame/Icon").GetComponent<Image>();
            m_Text = transform.Find("Text").GetComponent<Text>();
            buttonDelete = transform.Find("ButtonDelete").GetComponent<Button>();
            buttonParamSet = transform.Find("ButtonParam").GetComponent<Button>();
            m_TextNumber = transform.Find("Number/Text").GetComponent<Text>();
            // event
            buttonDelete.onClick.AddListener(buttonDelete_onClick);
            buttonParamSet.onClick.AddListener(buttonParamSet_onClick);
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        private void buttonParamSet_onClick()
        {
            OnSelected.Invoke(this);
        }


        /// <summary>
        /// 设置序号
        /// </summary>
        /// <param name="number"></param>
        public void SetData(int number)
        {
            m_TextNumber.text = number.ToString();
        }
        /// <summary>
        /// 设置箭头
        /// </summary>
        /// <param name="number"></param>
        public void SetData(GameObject _nextArrow)
        {
            nextArrow = _nextArrow;
        }
        public void SetData(ProcessLibraryItemData data)
        {
            Data = data;
            m_Icon.sprite = data.Sprite;
            m_Text.text = data.Name;
            OnEnqueued.Invoke(this);
        }
        public void SetData(int number, ProcessLibraryItemData data)
        {
            m_TextNumber.text = number.ToString();

            Data = data;
            m_Icon.sprite = data.Sprite;
            m_Text.text = data.Name;
          

            OnEnqueued.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
           // transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
            if (eventData.pointerDrag!=null)
            {
                ProcessDropItem item = eventData.pointerDrag.GetComponent<ProcessDropItem>();
                if (item != null && item.isDrag)
                {
                    item.transform.SetSiblingIndex(transform.GetSiblingIndex());
                    item.nextArrow.transform.SetSiblingIndex(item.transform.GetSiblingIndex() + 1);

                    nextArrow.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
                    nextArrow.transform.gameObject.SetActive(true);
                }
            }
           
        }

        public void OnPointerExit(PointerEventData eventData)
        {
           // transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
           // OnSelected.Invoke(this);
        }
        /// <summary>
        /// 放置
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrop(PointerEventData eventData)
        {
            ProcessSampleItem d = eventData.pointerDrag.GetComponent<ProcessSampleItem>();

            if (d != null)
            {
                SetData(d.Data);
            }
        }
        private void ChangeSelf(ProcessDropItem tempItem,ProcessDropItem item)
        {
            Data = item.Data;
            m_Icon.sprite = item.Sprite;
            m_Text.text = item.Text;

            item.Data = tempItem.Data;
            item.Sprite = tempItem.Sprite;
            item.Text = tempItem.Text;
        }
        /// <summary>
        /// 清除
        /// </summary>
        private void buttonDelete_onClick()
        {
           
            //m_Icon.sprite = null;
            //m_Text.text = "";
            //Data = null;
            if (nextArrow != null)
            {
                Destroy(nextArrow);
            }
            transform.SetParent(m_Container);
            OnDeleted.Invoke(this);
            transform.DOLocalMove(new Vector3(484f, -235f,0), 1f);
            transform.DOScale(Vector3.zero, 1f).OnComplete(()=> {
                Destroy(gameObject);
            });
        }

        public void DoSelect()
        {
           // GetComponent<Image>().sprite = selectSprite;
        }

        public void DoUnselect()
        {
            //GetComponent<Image>().sprite = normalSprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            if (nextArrow != null)
            {
                nextArrow.gameObject.SetActive(false);
            }
        }
        public bool isDrag = false;
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            isDrag = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
            transform.SetSiblingIndex(transform.GetSiblingIndex());
            if (nextArrow != null)
            {
                nextArrow.gameObject.SetActive(true);
                nextArrow.transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
            }
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            UpdateDropItem.Invoke();
        }
    }
}

