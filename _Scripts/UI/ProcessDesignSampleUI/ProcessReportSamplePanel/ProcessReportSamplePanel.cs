using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;
using System;
using XFramework.Core;
using XFramework.UI;
using UnityEngine.Events;

public class ProcessReportSamplePanel : MultiListViewCustom<ProcessSampleReport, ProcessParamGroup, ProcessParamGroupData>
{
    private UnityEvent m_OnBack = new UnityEvent();
    /// <summary>
    /// 返回按钮事件
    /// </summary>
    public UnityEvent OnBack
    {
        get { return m_OnBack; }
        set { m_OnBack = value; }
    }

    /// <summary>
    /// 表格节点
    /// </summary>
    [SerializeField]
    private RectTransform m_Table;

    /// <summary>
    /// 标题头
    /// </summary>
    [SerializeField]
    private Text m_TextHeader;

    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button buttonClose;

    /// <summary>
    /// 返回按钮
    /// </summary>
    private Button buttonBack;

    /// <summary>
    /// 标准流程文本
    /// </summary>
    private Text m_TextStandardProcess;

    /// <summary>
    /// 用户流程文本
    /// </summary>
    private Text m_TextUserProcess;

    /// <summary>
    /// 流程得分文本
    /// </summary>
    private Text m_TextScoreProcess;

    /// <summary>
    /// 总分文本
    /// </summary>
    private Text m_TextTotalScore;

    protected override void Awake()
    {
        base.Awake();
        buttonClose = transform.Find("Background/Header/ButtonClose").GetComponent<Button>();
        buttonBack = transform.Find("Background/ButtonBack").GetComponent<Button>();
        m_TextTotalScore = m_Table.Find("TotalScore/Score/Text").GetComponent<Text>();
        m_TextStandardProcess = m_Table.Find("ProcessPreview/Standard/Text").GetComponent<Text>();
        m_TextUserProcess = m_Table.Find("ProcessPreview/User/Text").GetComponent<Text>();
        m_TextScoreProcess = m_Table.Find("ProcessPreview/Score/Text").GetComponent<Text>();
        buttonClose.onClick.AddListener(buttonClose_onClick);
        buttonBack.onClick.AddListener(buttonBack_onClick);
    }



    /// <summary>
    /// 设置标题 / 按钮状态
    /// </summary>
    /// <param name="title"></param>
    /// <param name="closeActive"></param>
    /// <param name="backActive"></param>
    public void SetState(string title, bool closeActive, bool backActive)
    {
        m_TextHeader.text = title;
        buttonClose.gameObject.SetActive(closeActive);
        buttonBack.gameObject.SetActive(backActive);
    }

    public override void SetData(ProcessSampleReport data)
    {
        base.SetData(data);
        DataSource.Clear();
        if (data.GroupDataList != null)
        {
            DataSource.AddRange(data.GroupDataList);
        }
    }

    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="data"></param>
    public void Show(ProcessSampleReport data)
    {
        gameObject.SetActive(true);
        m_TextStandardProcess.text = data.StandardProcess;
        m_TextUserProcess.text = data.UserProcess;
        m_TextScoreProcess.text = data.GraphScore.ToString();
        m_TextTotalScore.text = data.TotalScore.ToString();
        SetData(data);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void buttonClose_onClick()
    {
        Hide();
    }

    private void buttonBack_onClick()
    {
        OnBack.Invoke();
    }

}