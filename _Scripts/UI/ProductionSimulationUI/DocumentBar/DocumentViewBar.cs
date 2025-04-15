using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class DocumentViewBar : MonoBehaviour
    {
        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        /// <summary>
        /// 按钮文本
        /// </summary>
        public Text buttonText;

        /// <summary>
        /// 内容
        /// </summary>
        private RectTransform m_Content;

        /// <summary>
        /// 文件视图
        /// </summary>
        private RectTransform ScrollView;

        /// <summary>
        /// 底部
        /// </summary>
        private RectTransform Bottom;

        /// <summary>
        /// 基本文件
        /// </summary>
        private List<BaseDocument> m_BaseDocuments;

        /// <summary>
        /// 当前工具
        /// </summary>
        public Document Current { get; private set; }

        /// <summary>
        /// 提交按钮
        /// </summary>
        private Button submitButton;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 主角对象 
        /// </summary>
        private Transform player;

        private bool isActive = false;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;

                if (isActive)
                {
                    submitButton.interactable = true;
                }
                else
                {
                    submitButton.interactable = false;
                }
            }
        }

        void Awake()
        {
            ScrollView = transform.Find("ScrollView").GetComponent<RectTransform>();
            Bottom = transform.Find("Bottom").GetComponent<RectTransform>();
            m_Title = transform.Find("TitleBar/Text").GetComponent<Text>();
            m_Content = transform.Find("ScrollView/Viewport/Content").GetComponent<RectTransform>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            submitButton = transform.Find("Bottom/SubmitButton").GetComponent<Button>();
            //buttonText = transform.Find("Bottom/SubmitButton/Text").GetComponent<Text>();

            submitButton.onClick.AddListener(submitButton_onClick);
            buttonClose.onClick.AddListener(buttonClose_onClick);
            m_BaseDocuments = new List<BaseDocument>();

            IsActive = false;
        }

        /// <summary>
        /// 提交文件操作
        /// </summary>
        private void submitButton_onClick()
        {
            foreach (var item in m_BaseDocuments)
            {
                item.Submit();
            }
            gameObject.SetActive(false);
            ///兼容 之前的角色
            //ThirdPersonUserControlEx m_ThirdEX = player.GetComponent<ThirdPersonUserControlEx>();
            MyselfControl m_MySelfCtrl = player.GetComponent<MyselfControl>();
            if (player != null)
            {
                //if(m_ThirdEX!=null)
                //    m_ThirdEX.IsEnabled = true;
                if (m_MySelfCtrl != null)
                    m_MySelfCtrl.disable = false;
            }
        }

        /// <summary>
        /// 关闭文档栏
        /// </summary>
        public void Close()
        {
            buttonClose_onClick();
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void buttonClose_onClick()
        {
            foreach (var item in m_BaseDocuments)
            {
                item.Cancel();
            }
            gameObject.SetActive(false);
            ///兼容 之前的角色
            //ThirdPersonUserControlEx m_ThirdEX = player.GetComponent<ThirdPersonUserControlEx>();
            MyselfControl m_MySelfCtrl = player.GetComponent<MyselfControl>();
            if (player != null)
            {
                //if (m_ThirdEX != null)
                //    m_ThirdEX.IsEnabled = true;
                if (m_MySelfCtrl != null)
                    m_MySelfCtrl.disable = false;
            }
        }

        /// <summary>
        /// 设置文件内容
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        public void SetDocumentData(Document item, Action<DocumentResult> _action, params object[] _params)
        {
            StartCoroutine(SetDocument(item, _action, false, _params));
        }

        /// <summary>
        /// 设置是否增加的文件内容
        /// </summary>
        /// <param name="item"></param>
        /// <param name="_action"></param>
        /// <param name="_params"></param>
        public void SetDocumentData(Document item, Action<DocumentResult> _action, bool isAdd, params object[] _params)
        {
            StartCoroutine(SetDocument(item, _action, false, _params));
        }

        /// <summary>
        /// 设置文件数据
        /// </summary>
        /// <param name="setting"></param>
        public void SetDocumentData(DocumentSetting setting)
        {
            gameObject.SetActive(true);
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            #region entityNPC
            if (setting.EntityNPCList!=null)
            {
                foreach (EntityNPC item in setting.EntityNPCList)
                {
                    if (item != null)
                    {
                        NPCControl ctrl = item.CacheTransform.GetComponent<NPCControl>();
                        if (ctrl != null)
                        {
                            setting.ButtonInteractable = ctrl.Active;
                            ///只需要找到一个符合条件的NPC
                            if (ctrl.Active)
                            {
                                object[] data = setting.Data as object[];
                                if(data.Length==2)
                                     data[1] = ctrl;
                                if (player != null)
                                {
                                    player.GetComponent<MyselfControl>().disable = true;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("管理员名字错误");
                    }
                }
            }
            #endregion

            //底部是否隐藏
            if (setting.BottomActive)
            {
                Bottom.gameObject.SetActive(true);
                //ScrollView.offsetMin = new Vector2(ScrollView.offsetMin.x, 36);
            }
            else
            {
                Bottom.gameObject.SetActive(false);
                //ScrollView.offsetMin = new Vector2(ScrollView.offsetMin.x, 0);
            }

            //按钮是否交互
            submitButton.interactable = setting.ButtonInteractable;
            //按钮文本
            //buttonText.text = setting.ButtonText;

            //设置文件
            StartCoroutine(SetDocument(setting.Document, setting.Action, setting.Append, setting.Data));
        }

        IEnumerator SetDocument(Document item, Action<DocumentResult> _action, bool isAdd, params object[] _params)
        {
            if (!isAdd)
            {
                if (m_BaseDocuments != null && m_BaseDocuments.Count > 0)
                {
                    for (int i = 0; i < m_BaseDocuments.Count; i++)
                    {
                        Destroy(m_BaseDocuments[i].gameObject);
                    }
                    m_BaseDocuments.Clear();
                }
            }

            yield return new WaitForEndOfFrame();

            m_Title.text = item.Description;
            GameObject prefab = Resources.Load<GameObject>(item.URL);
            GameObject Document = Instantiate(prefab);
            BaseDocument m_Document = Document.GetComponent<BaseDocument>();

            if (m_Document != null)
            {
                Document.transform.SetParent(m_Content);
                Document.layer = m_Content.gameObject.layer;
                Document.transform.localScale = Vector3.one;
                Document.name = item.URL;
                m_Document.SetParams(item, _action, _params);

                Current = item;
                m_BaseDocuments.Add(m_Document);
            }
        }
    }

    /// <summary>
    /// 文档设置
    /// </summary>
    public class DocumentSetting
    {
        private DocumentType type;
        /// <summary>
        /// 文件Item类型
        /// </summary>
        public DocumentType Type
        {
            get { return type; }
            set { type = value; }
        }

        private Document m_Document;
        /// <summary>
        /// 文件Item
        /// </summary>
        public Document Document
        {
            get { return m_Document; }
            set { m_Document = value; }
        }

        private Action<DocumentResult> action = null;
        /// <summary>
        /// 文件处理回调
        /// </summary>
        public Action<DocumentResult> Action
        {
            get { return action; }
            set { action = value; }
        }

        private bool append = false;
        /// <summary>
        /// 附加模式，显示多个文档
        /// </summary>
        public bool Append
        {
            get { return append; }
            set { append = value; }
        }

        private bool bottomActive = true;
        /// <summary>
        /// 底部是否隐藏
        /// </summary>
        public bool BottomActive
        {
            get { return bottomActive; }
            set { bottomActive = value; }
        }

        private List<EntityNPC> m_EntityNPCList = new List<EntityNPC>();
        /// <summary>
        /// 受影响NPC列表
        /// </summary>
        public List<EntityNPC> EntityNPCList
        {
            get { return m_EntityNPCList; }
            set { m_EntityNPCList = value; }
        }

        private bool buttonInteractable = true;
        /// <summary>
        /// 按钮能否交互
        /// </summary>
        public bool ButtonInteractable
        {
            get { return buttonInteractable; }
            set { buttonInteractable = value; }
        }

        private string buttonText = null;
        /// <summary>
        /// 按钮文本内容
        /// </summary>
        public string ButtonText
        {
            get { return buttonText; }
            set { buttonText = value; }
        }

        private object data = null;
        /// <summary>
        /// 传送的数据
        /// </summary>
        public object Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
