using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Archite_SlideRun : MonoBehaviour
{
    private Action OnCompleted;
    /// <summary>
    /// 获取当前Slider
    /// </summary>
    private Slider sliderCheck;

    /// <summary>
    /// 滚动条增加速度
    /// </summary>
    public float addSpeed = 1.0f;

    /// <summary>
    /// 文本框提示
    /// </summary>
    [HideInInspector] public Text text_SliderHint;

    /// <summary>
    /// 滚动条的值
    /// </summary>
    private Text text_ProgressBarNum;

    private string startValue;
    private string endValue;


    void Start()
    {
        //滚动条的位置
        sliderCheck = transform.Find("Panel/Slider").GetComponent<Slider>();
        //文本框
        text_SliderHint = transform.Find("Panel/Slider/TextHit").GetComponent<Text>();

        //滚动条的值
        text_ProgressBarNum = transform.Find("Panel/Slider/ProgressText").GetComponent<Text>();
        gameObject.SetActive(false);
    }
    public void SetData(string _startValue,string _endValue,Action _action=null)
    {
        gameObject.SetActive(true);
        startValue = _startValue;
        endValue = _endValue;
        OnCompleted = _action;
        StartCoroutine(SliderAdd());
    }
    IEnumerator SliderAdd()
    {
        //从0开始
        sliderCheck.value = 0;
        text_SliderHint.text = startValue;
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            sliderCheck.value += addSpeed;
            //滚动条的值
            text_ProgressBarNum.text = (sliderCheck.value).ToString()+"%";
            if (sliderCheck.value >= 100)
            {
                sliderCheck.value = 100;
                text_ProgressBarNum.text = (sliderCheck.value).ToString() + "%";
                text_SliderHint.text = endValue;            
                yield return new WaitForSeconds(1f);
                OnCompleted?.Invoke();
                break;
            }
        }
        gameObject.SetActive(false);
    }
}
    

