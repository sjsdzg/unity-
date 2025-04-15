using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Core;

public class SmallCentrifugeSetPanel : MonoBehaviour
{
    private Button saveButton;

    /// <summary>
    /// 时间
    /// </summary>
    private InputField timeInputField;
    /// <summary>
    /// 转速
    /// </summary>
    private InputField speedInputField;
    private Transform hit;
    /// <summary>
    /// 
    /// </summary>
    public UniEvent<string, string> SaveEvent = new UniEvent<string, string>();

    private void Start()
    {
        saveButton = transform.Find("FirstBg/SecondBg/Content/Button_Save").GetComponent<Button>();
        timeInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/time/InputField_Num").GetComponent<InputField>();
        speedInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/speed/InputField_Num").GetComponent<InputField>();     
        hit = transform.Find("FirstBg/SecondBg/Hit");
        saveButton.onClick.AddListener(saveButton_onClick);
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    private void saveButton_onClick()
    {
        Hide();
        SaveEvent.Invoke(speedInputField.text,timeInputField.text);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}


