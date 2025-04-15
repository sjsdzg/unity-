using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Core;
using System;

public class Panel_IOSet : MonoBehaviour
{
    private Button buttonSave;
    public UniEvent<int,string,string> onClick = new UniEvent<int,string,string>();
    public UniEvent<int,string,string> OnClick
    {
        get { return onClick; }
        set { onClick = value; }
    }

    public int ClickButtonSaveSum { get => clickButtonSaveSum; set => clickButtonSaveSum = value; }

    private int clickButtonSaveSum = 0;
    private Text dataText;
    private InputField dataInputField;
    private string result;
    private void Awake()
    {
        InitGUI();
        InitEvent();
    }
    private void Start()
    {
        
    }
    void InitGUI()
    {
        buttonSave = transform.Find("Content/Button_Save").GetComponent<Button>();
        dataText = transform.Find("Content/ContentData/data/Text").GetComponent<Text>();
        dataInputField = transform.Find("Content/ContentData/data/InputField_Num").GetComponent<InputField>();
    }
    void InitEvent()
    {
        buttonSave.onClick.AddListener(buttonSave_onClick);
    }

    public void SetData(string data,string _result)
    {
        dataText.text = data;
        result = _result;
    }
    private void buttonSave_onClick()
    {
        ClickButtonSaveSum++;     
        OnClick.Invoke(ClickButtonSaveSum, dataInputField.text, result);
        dataInputField.text = "";
        if (ClickButtonSaveSum == 2 || ClickButtonSaveSum == 7)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

