using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.UI;

namespace XFramework.PLC
{
    public class PLC_LoginPanel : MonoBehaviour
    {
        //public class EventListener : UnityEvent { }

        //private EventListener eventListener = new EventListener(); 

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button btn_Close;

        /// <summary>
        /// 重置按钮
        /// </summary>
        private Button btn_Reset;
        /// <summary>
        /// 登录按钮
        /// </summary>
        private Button btn_Login;
        /// <summary>
        /// 退出按钮
        /// </summary>
        private Button btn_Quit;

        /// <summary>
        /// 用户名称
        /// </summary>
        private InputField userName;
        /// <summary>
        /// 用户密码
        /// </summary>
        private InputField password;

        /// <summary>
        /// 退出警告面板
        /// </summary>
        private PLC_QuitWarn panel_QuitWarn;

        /// <summary>
        /// 登陆失提示
        /// </summary>
        private Transform wrongLoginHint;

        /// <summary>
        /// 操作指南
        /// </summary>
        private Panel_OperateInstruction panel_OperateInstruction;
        /// <summary>
        /// 选择信息界面
        /// </summary>
        private Panel_SelectInformation panel_SelectInformation;

        /// <summary>
        /// 事件声明
        /// </summary>
        private UnityEvent m_OnLoginSuccessed = new UnityEvent();
        /// <summary>
        /// 登陆成功事件
        /// </summary>
        public UnityEvent OnLoginSuccessed
        {
            //get { return m_OnLoginSuccessed; }
            //set { m_OnLoginSuccessed = value; }

            get
            {
                return m_OnLoginSuccessed;
            }
        }

        /// <summary>
        /// 声明事件
        /// </summary>
        public UnityEvent closeMainPanel = new UnityEvent();

        /// <summary>
        /// 关闭主界面
        /// </summary>
        public UnityEvent m_CloseMainPanel
        {
            get
            {
                return closeMainPanel;
            }
        }

        void Awake()
        {
            //关闭按钮
            btn_Close = transform.Find("Bg/Component/TitleBg/Button_Close").GetComponent<Button>();
            //重置按钮
            btn_Reset = transform.Find("Bg/Component/Content/Button_Reset").GetComponent<Button>();
            //登录按钮
            btn_Login = transform.Find("Bg/Component/Content/Button_Login").GetComponent<Button>();
            //退出按钮
            btn_Quit = transform.Find("Bg/Component/Content/Button_Quit").GetComponent<Button>();
            //退出警告
            panel_QuitWarn = transform.Find("Bg/Panel_QuitWarn").GetComponent<PLC_QuitWarn>();

            //操作指南界面
            panel_OperateInstruction = transform.Find("Bg/Panel_OperateInstruction").GetComponent<Panel_OperateInstruction>();
            //选择信息界面
            panel_SelectInformation = transform.Find("Bg/Panel_SelectInformation").GetComponent<Panel_SelectInformation>();

            //用户名
            userName = transform.Find("Bg/Component/Content/UserName").GetComponent<InputField>();
            userName.text = "admin";
            //密码
            password = transform.Find("Bg/Component/Content/Password").GetComponent<InputField>();
            password.text = "123456";

            //登陆失败提示
            wrongLoginHint = transform.Find("Bg/Component/Content/WrongLoginHint");

            //注册事件
            btn_Close.onClick.AddListener(Btn_Close);
            btn_Reset.onClick.AddListener(Btn_Reset);
            btn_Login.onClick.AddListener(Btn_Login);
            btn_Quit.onClick.AddListener(Btn_Quit);

            //退出提示
            panel_QuitWarn.gameObject.SetActive(false);
            //登陆失败提示
            wrongLoginHint.gameObject.SetActive(false);
            //操作指南
            panel_OperateInstruction.gameObject.SetActive(false);

            //事件的注册
            InitEvent();
        }

        public void InitEvent()
        {
            //打开选择信息界面的事件
            panel_OperateInstruction.SelectInformaction.AddListener(OpenSelectInformation_OpenSuccessed);

            //选择信息脚本中的操作指南事件
            panel_SelectInformation.OperateInstruction.AddListener(OpennOperateInstruction_OpenSuccessed);
            //选择信息脚本中的登录事件
            panel_SelectInformation.LoginPanel.AddListener(AccessMainPanel_OpenSuccessed);

            //注册事件
            panel_QuitWarn.m_Canvas.AddListener(Btn_Close);
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        public void Btn_Close()
        {
            //canvas.enabled = false;
            m_CloseMainPanel.Invoke();
        }

        /// <summary>
        /// 重置按钮
        /// </summary>
        public void Btn_Reset()
        {
            userName.text = "";
            password.text = "";
        }

        /// <summary>
        /// 登陆按钮
        /// </summary>
        public void Btn_Login()
        {
            if (userName.text == "admin" && password.text == "123456")
            {
                if (PLC_ControlPanel.isLockScreen)
                {
                    PLC_ControlPanel.isLockScreen = false;

                    //事件的执行，打开主界面
                    OnLoginSuccessed.Invoke();
                }
                else
                {
                    //OnOperateInstruction.Invoke();
                    panel_OperateInstruction.gameObject.SetActive(true);
                }
            }
            else
            {
                userName.text = "";
                password.text = "";
                //登陆失败提示
                StartCoroutine(LoginFail());
            }
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        public void Btn_Quit()
        {
            panel_QuitWarn.gameObject.SetActive(true);
        }

        /// <summary>
        /// 打开选择信息界面
        /// </summary>
        public void OpenSelectInformation_OpenSuccessed()
        {
            //打开选择信息界面，关闭操作指南界面
            panel_SelectInformation.gameObject.SetActive(true);
            panel_OperateInstruction.gameObject.SetActive(false);
        }

        /// <summary>
        /// 打开操作指南界面
        /// </summary>
        public void OpennOperateInstruction_OpenSuccessed()
        {
            panel_OperateInstruction.gameObject.SetActive(true);
            panel_SelectInformation.gameObject.SetActive(false);
        }

        /// <summary>
        /// 接收选择信息中的进入系统按钮的信号,进入到主界面
        /// </summary>
        public void AccessMainPanel_OpenSuccessed()
        {
            //事件的执行，打开主界面
            OnLoginSuccessed.Invoke();
        }
        /// <summary>
        /// 返回登陆界面
        /// </summary>
        public void ReturnLoginPanel()
        {
            panel_OperateInstruction.gameObject.SetActive(false);
        }

        /// <summary>
        /// 登陆失败提示
        /// </summary>
        /// <returns></returns>
        IEnumerator LoginFail()
        {
            wrongLoginHint.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            wrongLoginHint.gameObject.SetActive(false);
        }
    }
}
