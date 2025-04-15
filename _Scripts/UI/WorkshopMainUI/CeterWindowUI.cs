using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
using DG.Tweening;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 中央窗口
    /// </summary>
	public class CeterWindowUI : MonoBehaviour {

        private Text Content;

        /// <summary>
        /// 标题
        /// </summary>
        //private Text Title;

        /// <summary>
        /// 关闭
        /// </summary>
        private Button btnClose;

        /// <summary>
        /// 跳转按钮
        /// </summary>
        private Button btnLogin;

        /// <summary>
        /// 初始位置
        /// </summary>
        private Vector3 InitPos;
        /// <summary>
        /// 打印速度
        /// </summary>
        private  float speed =5;

        public bool IsShow { get; set; }
                                           // Use this for initialization
        void Start () {
         
            //Content = transform.FindChild("Content/Text").GetComponent<Text>();
            //Title = transform.FindChild("Title").GetComponentInChildren<Text>();
            btnClose = transform.Find("Close").GetComponent<Button>();
            btnLogin = transform.Find("LoginIn").GetComponent<Button>();

            InitPos = transform.localPosition;

            btnClose.onClick.AddListener(HideWindow);
            btnLogin.onClick.AddListener(btnLogin_onClick);

        }
		private void btnLogin_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.WorkshopRoamingScene);
        }

        public Tween ShowWindow()
        {
            return transform.DOLocalMoveY(0,1f).SetEase( Ease.InOutBack);
        }
		
        public void HideWindow()
        {
            DOTween.Kill(transform);
            transform.DOLocalMoveY(InitPos.y,1f).SetEase( Ease.InOutCirc).OnComplete(()=> {

            IsShow = false;
            });
        }
        //private void Update()
        //{
        //    if(Input.GetKeyDown(KeyCode.Alpha1))
        //    {
        //        ShowWindow();
        //    }
        //    else if(Input.GetKeyDown(KeyCode.Alpha2))
        //    {
        //        HideWindow();
        //    }
        //}

        public void ShowWindow(string title,IntroduceContents info)
        {

            DOTween.Kill(Content);
            info.Desc = info.Desc.Replace("\\n", "\n");
            Content.text = string.Empty;
            //Title.text = title+" 总体介绍";

            if (IsShow)
            {
                Content.DOText(info.Desc, 1);
            }
            else
            {
                ShowWindow().OnComplete(() => {
                    Content.DOText(info.Desc, speed);
                    IsShow = true;
                });
            }
            
        }
    }
}