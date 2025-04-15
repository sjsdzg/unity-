using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DateTimePickerTest : MonoBehaviour
{
    public Button button;

    public RectTransform canvas;

    public DateTimePicker picker;

    private void Start()
    {
        button.onClick.AddListener(button_onClick);
    }

    private void button_onClick()
    {
        picker.Show();
    }

}
