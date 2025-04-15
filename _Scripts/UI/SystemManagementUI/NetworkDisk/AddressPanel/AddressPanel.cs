using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class AddressPanel : MonoBehaviour
{
    public class OnClickedEvent : UnityEvent<object> { }

    private OnClickedEvent m_OnClicked = new OnClickedEvent();
    /// <summary>
    /// 鼠标点击时，触发
    /// </summary>
    public OnClickedEvent OnClicked
    {
        get { return m_OnClicked; }
        set { m_OnClicked = value; }
    }

    /// <summary>
    /// 内容
    /// </summary>
    public RectTransform Content;
    /// <summary>
    /// 默认链接
    /// </summary>
    public GameObject defaultAddressButton;
    /// <summary>
    /// 默认分隔符
    /// </summary>
    public GameObject defaultSeparator;
    /// <summary>
    /// 超按钮列表
    /// </summary>
    public List<AddressButton> AddressButtons = new List<AddressButton>();
    /// <summary>
    /// 分隔符列表
    /// </summary>
    public List<GameObject> Separators = new List<GameObject>();

    void Start()
    {
        if (defaultAddressButton == null)
        {
            Debug.Log("defaultUrl is null");
            return;
        }

        if (defaultSeparator == null)
        {
            Debug.Log("defaultSplit is null");
            return;
        }

        defaultAddressButton.SetActive(false);
        defaultSeparator.SetActive(false);
    }

    /// <summary>
    /// 增加超按钮
    /// </summary>
    /// <param name="herf"></param>
    /// <param name="text"></param>
    public void AddAddressButton(object content, string text)
    {
        GameObject Instant = Instantiate(defaultAddressButton);
        Instant.SetActive(true);

        AddressButton component = Instant.GetComponent<AddressButton>();

        if (component != null && component != null)
        {
            if (AddressButtons.Count >= 1)
            {
                GameObject separator = Instantiate(defaultSeparator);
                separator.SetActive(true);
                separator.transform.SetParent(Content, false);
                separator.transform.SetParent(Content, false);
                separator.layer = Content.gameObject.layer;
                Separators.Add(separator);
            }

            component.transform.SetParent(Content, false);
            Instant.layer = Content.gameObject.layer;

            component.Interactable = false;
            component.SetValue(content, text);
            AddressButtons.Add(component);
            component.OnClicked.AddListener(addressButton_OnClicked);
            //设置序号
            component.Index = AddressButtons.Count - 1;
            if (AddressButtons.Count >= 2)
            {
                AddressButtons[AddressButtons.Count - 2].Interactable = true;
            }
        }
    }

    public void RemoveAddressButton(int index)
    {
        if (index <= AddressButtons.Count - 1)
        {
            AddressButton addressButton = AddressButtons[index];
            AddressButtons.Remove(addressButton);
            Destroy(addressButton.gameObject);
        }
    }

    /// <summary>
    /// 当超按钮点击时，触发
    /// </summary>
    /// <param name="arg0"></param>
    private void addressButton_OnClicked(AddressButton addressButton)
    {
        OnClicked.Invoke(addressButton.Content);
        for (int i = addressButton.Index + 1; i < AddressButtons.Count;)
        {
            RemoveAddressButton(i);
        }

        for (int i = addressButton.Index; i < Separators.Count;)
        {
            GameObject separator = Separators[i];
            Separators.Remove(separator);
            Destroy(separator.gameObject);
        }
    }
}
