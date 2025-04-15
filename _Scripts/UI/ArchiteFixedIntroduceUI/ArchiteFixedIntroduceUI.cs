using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Simulation;
using PathologicalGames;
using UIWidgets;
using System.Text;
using XFramework.Proto;
using Newtonsoft.Json.Linq;
using XFramework;
using XFramework.UI;

public class ArchiteFixedIntroduceUI : BaseUI
{
    private Button buttonSound;
    private Button buttonNext;
    private Button buttonBack;
    private int nextClickSum = 1;
    /// <summary>v
    /// 固定条件讲解模块
    /// </summary>
    private ArchiteFixedIntroduceModule module;
    /// <summary>
    /// 流程
    /// </summary>
    private Procedure m_Procedure;

    /// <summary>
    /// 流程栏目
    /// </summary>
    private ProcedureBar m_ProcedureBar;

    /// <summary>
    /// 当前流程栏目
    /// </summary>
    private CurrentProcedureBar m_CurrentProcedureBar;

    private ArchiteIntroduceCenterBar m_ArchiteIntroduceCenterBar;
    private SoundBar m_SoundBar;
    public override EnumUIType GetUIType()
    {
        return EnumUIType.ArchiteFixedIntroduceUI;
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        ModuleManager.Instance.Register<ArchiteFixedIntroduceModule>();
        module = ModuleManager.Instance.GetModule<ArchiteFixedIntroduceModule>();
        EventDispatcher.RegisterEvent<int, int>(Events.Procedure.Current, OnProcedureCurrent);
        EventDispatcher.RegisterEvent<string>(Events.ArchiteIntroduce.SceneToUI, ReceiveSceneToUI_CallBack);
       
        InitGUI();
        InitEvent();
    }
    /// <summary>
    /// 声音播放结束事件
    /// </summary>
    /// <param name="obj"></param>
    private void Instance_EventPlayEnd(string name)
    {
        EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, name);
        //switch (name)
        //{
        //    case "1-1":
        //        buttonNext.gameObject.SetActive(true);
        //        break;
        //    case "1-2":
        //        m_ArchiteIntroduceCenterBar.Hide();
        //        break;
        //    default:
        //        break;
        //}
    }
    private void ReceiveSceneToUI_CallBack(string name)
    {
        //switch (name)
        //{
        //    case "1-1":
        //        m_ArchiteIntroduceCenterBar.Show();
        //        break;
        //    case "1-2":
        //        m_ArchiteIntroduceCenterBar.Hide();
        //        break;
        //    default:
        //        break;
        //}
        buttonNext.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        EventDispatcher.UnregisterEvent<int, int>(Events.Procedure.Current, OnProcedureCurrent);
        EventDispatcher.UnregisterEvent<string>(Events.ArchiteIntroduce.SceneToUI, ReceiveSceneToUI_CallBack);
    }

    protected override void OnRelease()
    {
        base.OnRelease();
       
    }
    void InitGUI()
    {
        m_ProcedureBar = transform.Find("Background/ProcedureBar").GetComponent<ProcedureBar>();
        m_CurrentProcedureBar = transform.Find("Background/CurrentProcedureBar").GetComponent<CurrentProcedureBar>();
        m_ArchiteIntroduceCenterBar = transform.Find("Background/CenterBar").GetComponent<ArchiteIntroduceCenterBar>();
        m_SoundBar = transform.Find("Background/SoundBar").GetComponent<SoundBar>();
        buttonSound = transform.Find("Background/MenuBar/ButtonSound").GetComponent<Button>();
        buttonNext = transform.Find("Background/MenuBar/ButtonNext").GetComponent<Button>();
        buttonBack = transform.Find("Background/TitleBar/ButtonBack").GetComponent<Button>();
    }
    void InitEvent()
    {
        m_SoundBar.OnSliderMusicValueChanged.AddListener(m_SoundBar_OnPitchValueChanged);
        m_SoundBar.OnSliderEffectValueChanged.AddListener(m_SoundBar_OnVolumeValueChanged);
        buttonSound.onClick.AddListener(buttonSound_onClick);
        buttonNext.onClick.AddListener(buttonNext_onClick);
        buttonBack.onClick.AddListener(buttonBack_onClick);
    }

    private void buttonBack_onClick()
    {
        SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
    }

    private void buttonNext_onClick()
    {
        nextClickSum++;
        switch (nextClickSum)
        {
            case 2:
                OnProcedureCurrent(1, 2);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "1-2-1");
                break;
            case 3:
                OnProcedureCurrent(1, 3);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "1-3-1");
                break;
            case 4:
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "1-4-1");
                OnProcedureCurrent(1, 4);
                break;
            case 5:
                OnProcedureCurrent(1, 5);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "1-5-1");
                break;
            case 6:
                OnProcedureCurrent(1, 6);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "1-6-1");
                break;
            case 7:
                OnProcedureCurrent(1, 7);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "1-7-1");
                break;
            case 8:
                OnProcedureCurrent(2, 1);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "2-1-1");
                break;
            case 9:
                OnProcedureCurrent(2, 2);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "2-2-1");
                break;
            case 10:
                OnProcedureCurrent(2, 3);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "2-3-1");
                break;
            case 11:
                OnProcedureCurrent(3, 1);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "3-1-1");
                break;
            case 12:
                OnProcedureCurrent(3, 2);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "3-2-1");
                break;
            case 13:
                OnProcedureCurrent(3, 3);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "3-3-1");
                break;
            case 14:
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "3-4-1");
                OnProcedureCurrent(3, 4);
                break;
            case 15:
                OnProcedureCurrent(4, 1);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "4-1-1");
                break;
            case 16:
                OnProcedureCurrent(4, 2);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "4-2-1");
                break;
            case 17:
                OnProcedureCurrent(4, 3);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "4-3-1");
                break;
            case 18:
                OnProcedureCurrent(4, 4);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "4-4-1");
                break;
            case 19:
                OnProcedureCurrent(5, 1);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "5-1-1");
                break;
            case 20:
                OnProcedureCurrent(5, 2);
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.UIToScene, "5-2-1");
                break;
            default:
                break;
        }
        buttonNext.gameObject.SetActive(false);
    }

    private void buttonSound_onClick()
    {
        m_SoundBar.Show();
    }

    private void m_SoundBar_OnVolumeValueChanged(float arg0)
    {
        ArchiteIntroduceAudioController.Instance.guideAudioSource.volume = arg0;
    }

    /// <summary>
    /// 音速改变
    /// </summary>
    /// <param name="arg0"></param>
    private void m_SoundBar_OnPitchValueChanged(float arg0)
    {
        ArchiteIntroduceAudioController.Instance.guideAudioSource.pitch = arg0;
    }

    protected override void SetUI(params object[] uiParams)
    {
        base.SetUI(uiParams);
      
    }
    protected override void OnStart()
    {
        base.OnStart();
        m_Procedure = module.GetProcedure();
        m_ProcedureBar.SetValue(m_Procedure.Sequences);
        m_SoundBar.Hide();
        m_ArchiteIntroduceCenterBar.Hide();
        buttonNext.gameObject.SetActive(false);
        ArchiteIntroduceAudioController.Instance.RootPath = "Assets/Audios/ArchiteFixedIntroduce/";
        ArchiteIntroduceAudioController.Instance.EventPlayEnd += Instance_EventPlayEnd;
    }

    /// <summary>
    /// 当前流程内容
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void OnProcedureCurrent(int seqId, int actionId)
    {
        string name = string.Format("{0}-{1}", seqId, actionId);
        ArchiteIntroduceAudioController.Instance.Play(name);
        m_ProcedureBar.SetCurrentItem(seqId, actionId);
        string title = string.Format("当前流程({0}.{1})", seqId, actionId);
        string content = m_Procedure.GetAction(seqId, actionId).Desc;
        m_CurrentProcedureBar.SetValue(title, content);
    }
}
