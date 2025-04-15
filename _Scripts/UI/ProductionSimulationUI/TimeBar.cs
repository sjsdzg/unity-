using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimeBar : MonoBehaviour
{
    /// <summary>
    /// 秒数
    /// </summary>
    public int TotalSeconds { get; set; }

    /// <summary>
    /// 是否开启
    /// </summary>
    public bool IsOn { get; set; }

    /// <summary>
    /// 文本
    /// </summary>
    private Text m_Text;

    /// <summary>
    /// 提示时间(分钟)
    /// </summary>
    private const int promptTime = 5;

    private UnityEvent m_OnPrompt = new UnityEvent();
    /// <summary>
    /// 倒计时到提示时间时，触发事件
    /// </summary>
    public UnityEvent OnPrompt
    {
        get { return m_OnPrompt; }
        set { m_OnPrompt = value; }
    }

    private UnityEvent m_OnEnd = new UnityEvent();
    /// <summary>
    /// 倒计时结束时，触发事件
    /// </summary>
    public UnityEvent OnEnd
    {
        get { return m_OnEnd; }
        set { m_OnEnd = value; }
    }

    void Awake()
    {
        m_Text = transform.Find("Text").GetComponent<Text>();
        IsOn = true;
        //StartCoroutine(Timing(0));
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    /// <param name="seconds"></param>
    public void StartTiming(int seconds)
    {
        StartCoroutine(Timing(seconds));
    }

    /// <summary>
    /// 开始倒计时
    /// </summary>
    /// <param name="seconds"></param>
    public void StartCutDowning(int seconds)
    {
        StartCoroutine(CutDowning(seconds));
    }

    /// <summary>
    /// 计时
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    IEnumerator Timing(int seconds)
    {
        TotalSeconds = seconds;
        while (IsOn)
        {
            TotalSeconds += 1;
            TimeSpan timeSpan = new TimeSpan(0, 0, TotalSeconds);
            m_Text.text = timeSpan.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// 倒计时协程
    /// </summary>
    /// <param name="seconds">秒</param>
    /// <returns></returns>
    IEnumerator CutDowning(int seconds)
    {
        TimeSpan tmspan = new TimeSpan(0, 0, seconds);
        TotalSeconds = (int)tmspan.TotalSeconds;
        while (tmspan.TotalSeconds > 0)
        {
            TotalSeconds -= 1;
            tmspan = new TimeSpan(0, 0, TotalSeconds);
            m_Text.text = tmspan.ToString();
            yield return new WaitForSeconds(1);
            //考试最后时间，提醒考生注意。
            if (TotalSeconds == promptTime * 60)
            {
                //notify.Template().Show("注意还有5分钟！\n时间结束后，如果您没有交卷，试卷将自动提交。", 15f);
                OnPrompt.Invoke();
            }
        }
        //考试试卷完成，提交试卷
        //ReturnExamSystem();
        OnEnd.Invoke();
    }
}
