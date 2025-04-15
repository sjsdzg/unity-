using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Toggle))]
public class DatePickerItem : MonoBehaviour
{
    public class SelectedEvent : UnityEvent<string> { }

    private SelectedEvent m_OnSelected = new SelectedEvent();
    /// <summary>
    /// 日期Item点击时，触发
    /// </summary>
    public SelectedEvent OnSelected
    {
        get { return m_OnSelected; }
        set { m_OnSelected = value; }
    }

    private bool selected = false;
    /// <summary>
    /// 是否选中
    /// </summary>
    public bool Selected
    {
        get
        {
            selected = toggle.isOn;
            return selected;
        }
        set
        {
            selected = value;
            toggle.isOn = selected;
        }
    }

    /// <summary>
    /// toggle
    /// </summary>
    private Toggle toggle;

    /// <summary>
    /// text
    /// </summary>
    private Text text;

    void Awake()
    {
        toggle = transform.GetComponent<Toggle>();
        text = transform.GetComponentInChildren<Text>();
        toggle.onValueChanged.AddListener(toggle_onValueChanged);
    }

    /// <summary>
    /// Toggle点击时，触发
    /// </summary>
    /// <param name="arg0"></param>
    private void toggle_onValueChanged(bool value)
    {
        if (value)
        {
            text.color = new Color32(255, 255, 255, 255);
            string day = text.text;
            OnSelected.Invoke(day);
        }
        else
        {
            text.color = new Color32(50, 50, 50, 255);
        }
    }
}
