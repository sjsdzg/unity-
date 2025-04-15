using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Core;

public class PotSetPanel : MonoBehaviour
{
    private Button saveButton;

    /// <summary>
    /// 时间
    /// </summary>
    private InputField timeInputField;
    /// <summary>
    /// 温度
    /// </summary>
    private InputField temInputField;
    private Transform hit;

    public UniEvent<string, string> SaveEvent = new UniEvent<string, string>();

    private void Start()
    {
        saveButton = transform.Find("FirstBg/SecondBg/Content/Button_Save").GetComponent<Button>();
        timeInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/time/InputField_Num").GetComponent<InputField>();
        temInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/tem/InputField_Num").GetComponent<InputField>();
        hit = transform.Find("FirstBg/SecondBg/Hit");
        saveButton.onClick.AddListener(saveButton_onClick);
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    private void saveButton_onClick()
    {
        Hide();
        SaveEvent.Invoke(temInputField.text,timeInputField.text);
      //  EventDispatcher.ExecuteEvent<string>(Events.PLC.PLCToWorkshop, "水浴锅参数设置");
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


