using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TimePicker : MonoBehaviour
{
    private int hour;
    /// <summary>
    /// 时
    /// </summary>
    public int Hour
    {
        get
        {
            hour = int.Parse(inputFieldHour.text);
            return hour;
        }
        set
        {
            hour = value;
            inputFieldHour.text = hour.ToString().PadLeft(2, '0');
        }
    }

    private int minute;
    /// <summary>
    /// 分
    /// </summary>
    public int Minute
    {
        get
        {
            minute = int.Parse(inputFieldMinute.text);
            return minute;
        }
        set
        {
            minute = value;
            inputFieldMinute.text = minute.ToString().PadLeft(2, '0');
        }
    }

    private int second;
    /// <summary>
    /// 秒
    /// </summary>
    public int Second
    {
        get
        {
            second = int.Parse(inputFieldSecond.text);
            return second;
        }
        set
        {
            second = value;
            inputFieldSecond.text = second.ToString().PadLeft(2, '0');
        }
    }

    /// <summary>
    /// 输入框Hour
    /// </summary>
    private InputField inputFieldHour;
    /// <summary>
    /// 输入框Minute
    /// </summary>
    private InputField inputFieldMinute;
    /// <summary>
    /// 输入框Second
    /// </summary>
    private InputField inputFieldSecond;

    void Awake()
    {
        inputFieldHour = transform.Find("InputFieldHour").GetComponent<InputField>();
        inputFieldMinute = transform.Find("InputFieldMinute").GetComponent<InputField>();
        inputFieldSecond = transform.Find("InputFieldSecond").GetComponent<InputField>();

        inputFieldHour.onEndEdit.AddListener(inputFieldHour_onEndEdit);
        inputFieldMinute.onEndEdit.AddListener(inputFieldMinute_onEndEdit);
        inputFieldSecond.onEndEdit.AddListener(inputFieldSecond_onEndEdit);
    }

    private void inputFieldHour_onEndEdit(string value)
    {
        int _hour = int.Parse(value);
        if (_hour > 23)
        {
            _hour = 23;
        }
        else if (_hour < 0)
        {
            _hour = 0;
        }
        inputFieldHour.text = _hour.ToString().PadLeft(2, '0');
    }

    private void inputFieldMinute_onEndEdit(string value)
    {
        int _minute = int.Parse(value);
        if (_minute > 59)
        {
            _minute = 59;
        }
        else if (_minute < 0)
        {
            _minute = 0;
        }
        inputFieldMinute.text = _minute.ToString().PadLeft(2, '0');
    }

    private void inputFieldSecond_onEndEdit(string value)
    {
        int _second = int.Parse(value);
        if (_second > 59)
        {
            _second = 59;
        }
        else if (_second < 0)
        {
            _second = 0;
        }
        inputFieldSecond.text = _second.ToString().PadLeft(2, '0');
    }

}