using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DateTimePicker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public class OnSubmitEvent : UnityEvent<DateTime> { }

    private OnSubmitEvent m_OnSubmit = new OnSubmitEvent();
    /// <summary>
    /// 提交事件
    /// </summary>
    public OnSubmitEvent OnSubmit
    {
        get { return m_OnSubmit; }
        set { m_OnSubmit = value; }
    }

    private UnityEvent m_OnClear = new UnityEvent();
    /// <summary>
    /// 清空事件
    /// </summary>
    public UnityEvent OnClear
    {
        get { return m_OnClear; }
        set { m_OnClear = value; }
    }

    /// <summary>
    /// 总数量
    /// </summary>
    public const int TOTAL_ITEM_NUM = 42;

    public GameObject dateItemContainer;
    public GameObject defaultDateItem;
    public List<GameObject> dateItems = new List<GameObject>();

    /// <summary>
    /// year text
    /// </summary>
    public Text yearText;

    /// <summary>
    /// month text
    /// </summary>
    public Text monthText;

    /// <summary>
    /// 时间选择器
    /// </summary>
    public TimePicker timePicker;
    /// <summary>
    /// 日期Item宽度
    /// </summary>
    public int itemWidth = 40;

    /// <summary>
    /// 日期Item高度
    /// </summary>
    public int itemHeight = 30;

    /// <summary>
    /// 更新日期时间
    /// </summary>
    private DateTime updateDate;

    /// <summary>
    /// 选中日期
    /// </summary>
    private DateTime selectedDate = new DateTime();

    /// <summary>
    /// 指针是否进入
    /// </summary>
    private bool isPointerEnter = false;

    /// <summary>
    /// 是否显示中
    /// </summary>
    private bool showing = false;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Vector3 startPos = defaultDateItem.transform.localPosition;
        dateItems.Clear();
        dateItems.Add(defaultDateItem);

        for (int i = 1; i < TOTAL_ITEM_NUM; i++)
        {
            GameObject item = GameObject.Instantiate(defaultDateItem) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(defaultDateItem.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * itemWidth + startPos.x, startPos.y - (i / 7) * itemHeight, startPos.z);
            DatePickerItem datePickerItem = item.GetComponent<DatePickerItem>();
            datePickerItem.OnSelected.AddListener(datePickerItem_OnSelected);

            dateItems.Add(item);
        }

        //隐藏
        Hide();
    }

    void Start()
    {
        //Now();
    }

    /// <summary>
    /// 更新日期选择器
    /// </summary>
    private void UpdateDatePicker()
    {
        yearText.text = updateDate.Year.ToString();
        monthText.text = updateDate.Month.ToString();

        DateTime firstDay = updateDate.AddDays(-(updateDate.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < TOTAL_ITEM_NUM; i++)
        {
            Text label = dateItems[i].GetComponentInChildren<Text>();
            dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);

                if (thatDay.Month == firstDay.Month)
                {
                    dateItems[i].SetActive(true);
                    date++;
                    label.text = date.ToString();

                    //清除所有选中状态
                    DatePickerItem item = dateItems[i].GetComponent<DatePickerItem>();
                    if (item != null)
                    {
                        item.Selected = false;
                    }

                    //设置选中状态
                    if (updateDate.Year == selectedDate.Year && updateDate.Month == selectedDate.Month && selectedDate.Day == date)
                    {
                        DatePickerItem selectItem = dateItems[i].GetComponent<DatePickerItem>();
                        if (selectItem != null)
                        {
                            selectItem.Selected = true;
                        }
                    }
                }
            }
        }
    }

    private void datePickerItem_OnSelected(string day)
    {
        //选中时间
        selectedDate = new DateTime(int.Parse(yearText.text), int.Parse(monthText.text), int.Parse(day));
    }

    /// <summary>
    /// 获取一周的第几天
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    private int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }

    public void YearPrev()
    {
        updateDate = updateDate.AddYears(-1);
        UpdateDatePicker();
    }

    public void YearNext()
    {
        updateDate = updateDate.AddYears(1);
        UpdateDatePicker();
    }

    public void MonthPrev()
    {
        updateDate = updateDate.AddMonths(-1);
        UpdateDatePicker();
    }

    public void MonthNext()
    {
        updateDate = updateDate.AddMonths(1);
        UpdateDatePicker();
    }

    /// <summary>
    /// 此刻
    /// </summary>
    public void Now()
    {
        timePicker.Hour = DateTime.Now.Hour;
        timePicker.Minute = DateTime.Now.Minute;
        timePicker.Second = DateTime.Now.Second;
        updateDate = DateTime.Now;
        selectedDate = updateDate;
        //更新日期选择器
        UpdateDatePicker();
    }

    /// <summary>
    /// 显示
    /// </summary>
    public void Show()
    {
        showing = true;
        gameObject.SetActive(true);
        Now();
    }

    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="dateTime"></param>
    public void Show(DateTime dateTime)
    {
        showing = true;
        gameObject.SetActive(true);
        timePicker.Hour = dateTime.Hour;
        timePicker.Minute = dateTime.Minute;
        timePicker.Second = dateTime.Second;
        updateDate = dateTime;
        selectedDate = updateDate;
        //更新日期选择器
        UpdateDatePicker();
    }

    /// <summary>
    /// 隐藏
    /// </summary>

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 确定
    /// </summary>
    public void Submit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        DateTime submitDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, timePicker.Hour, timePicker.Minute, timePicker.Second);
        OnSubmit.Invoke(submitDateTime);
        Debug.Log(submitDateTime.ToString());
        Hide();
    }

    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        OnClear.Invoke();
    }

    void Update()
    {
        if (!isPointerEnter)
        {
            if (showing)
            {
                showing = false;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    Hide();
                }
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerEnter = false;
    }
}
