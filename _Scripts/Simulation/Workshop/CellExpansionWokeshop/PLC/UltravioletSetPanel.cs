using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Core;

public class UltravioletSetPanel : MonoBehaviour
{
    private Button saveButton;

    /// <summary>
    /// 紫外灭菌时间
    /// </summary>
    private InputField timeInputField;
    private Transform hit;

    public UniEvent<string> SaveEvent = new UniEvent<string>();

    private void Start()
    {
        saveButton = transform.Find("FirstBg/SecondBg/Content/Button_Save").GetComponent<Button>();
        timeInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/time/InputField_Num").GetComponent<InputField>();
        hit = transform.Find("FirstBg/SecondBg/Hit");
        saveButton.onClick.AddListener(saveButton_onClick);
    }

    /// <summary>
    /// 保存方法
/// </summary>
    private void saveButton_onClick()
    {
        Hide();
        SaveEvent.Invoke(timeInputField.text);
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


