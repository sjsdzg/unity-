using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    class PLC_StartUpAnimation : MonoBehaviour
    {
        //公司标志
        private Transform CompanyLogo;
        //公司名字
        private Transform companyName;
        //加载滚动条
        private Transform slider;

        void Start()
        {
            //公司标志
            CompanyLogo = transform.Find("CompanyLogo");
            //公司名字
            companyName = transform.Find("CompanyName");
            //加载滚动条
            slider = transform.Find("Slider");

            //动画开始运行
            StartCoroutine(StartSequence());
        }

        /// <summary>
        /// 动画开始运行
        /// </summary>
        /// <returns></returns>
        IEnumerator StartSequence()
        {
            //公司Logo动画播放
            CompanyLogo.gameObject.SetActive(true);
            yield return new WaitForSeconds(2.0f);

            //公司名字动画播放
            companyName.gameObject.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            
            //滚动条加载
            slider.gameObject.SetActive(true);
        }
    }
}
