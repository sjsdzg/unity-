using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class CurrentProcedureBar : MonoBehaviour
{
    /// <summary>
    /// 文本
    /// </summary>
    private Text m_Text;

    /// <summary>
    /// 最小化
    /// </summary>
    private Button buttonMin;

    /// <summary>
    /// 最大化
    /// </summary>
    private Button buttonMax;

    /// <summary>
    /// background
    /// </summary>
    private RectTransform background;

    /// <summary>
    /// 标题
    /// </summary>
    private Text m_Title;

    private void Awake()
    {
        background = transform.Find("Background").GetComponent<RectTransform>();
        m_Text = transform.Find("Background/Scroll View/Viewport/Content/Text").GetComponent<Text>();
        m_Title = transform.Find("Background/TitleBar/Text").GetComponent<Text>();
        buttonMin = transform.Find("Background/TitleBar/ButtonMin").GetComponent<Button>();
        buttonMax = transform.Find("ButtonMax").GetComponent<Button>();
        //State
        buttonMax.gameObject.SetActive(false);
        //Event
        buttonMin.onClick.AddListener(buttonMin_onClick);
        buttonMax.onClick.AddListener(buttonMax_onClick);
    }

    private void buttonMin_onClick()
    {
        background.DOScale(0, 0.2f).OnComplete(() => buttonMax.gameObject.SetActive(true));
    }

    private void buttonMax_onClick()
    {
        buttonMax.gameObject.SetActive(false);
        background.DOScale(1, 0.2f);
    }

    public void SetValue(string title, string content)
    {
        m_Title.text = title;
        m_Text.text = "";
        m_Text.DOText(content, 0.5f);
    }
}
