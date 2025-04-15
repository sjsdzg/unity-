using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;
using DG.Tweening;
using System.Collections;

namespace XFramework.UI
{
    /// <summary>
    /// 更衣路径栏
    /// </summary>
    public class DressingPathBar : MonoBehaviour
    {
        /// <summary>
        /// 更衣流程
        /// </summary>
        private Text DressingProcess;

        /// <summary>
        /// 着装
        /// </summary>
        private Text Dressing;

        /// <summary>
        /// 更衣资源
        /// </summary>
        public DressingAsset dressingAsset;

        /// <summary>
        /// panel
        /// </summary>
        private RectTransform panel;

        private bool fading = false;
        /// <summary>
        /// 正在更衣流程中
        /// </summary>
        public bool Fading
        {
            get
            {
                return fading;
            }
            set
            {
                fading = value;

                if (fading)
                {
                    StopAllCoroutines();
                    StartCoroutine(FadeOn());
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(FadeOff());
                }
            }
        }

        /// <summary>
        /// MiniMap列表
        /// </summary>
        private MiniMap[] miniMaps;

        /// <summary>
        /// CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup = null;

        void Awake()
        {
            miniMaps = MiniMapUtils.GetAllMiniMap();
            panel = transform.Find("Panel").GetComponent<RectTransform>();
            DressingProcess = transform.Find("Panel/DressingProcess").GetComponent<Text>();
            Dressing = transform.Find("Panel/Dressing").GetComponent<Text>();
            m_CanvasGroup = transform.GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// 进入更衣流程
        /// </summary>
        /// <param name="asset"></param>
        public void EnterDressingProcess(DressingAsset asset)
        {
            gameObject.SetActive(true);
            Fading = true;

            dressingAsset = asset;

            if (dressingAsset.ExistCurve)
            {
                panel.gameObject.SetActive(true);
            }
            else
            {
                panel.gameObject.SetActive(false);
            }

            DressingProcess.text = dressingAsset.DressingProcess.text;
            Dressing.text = dressingAsset.Dressing.text;
            dressingAsset.Display();
            dressingAsset.OnCompleted.AddListener(DressingAsset_OnCompleted);
        }

        /// <summary>
        /// 改变Player
        /// </summary>
        private void ChangePlayer(Transform PlayerLocation, Cleanliness cleanliness)
        {
            //销毁旧Player
            Transform lastPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            Destroy(lastPlayer.gameObject);

            //加载新Player
            //string playerPath = CleanlinessDefine.GetPlayerPrefabPath(cleanliness);
            string playerPath = CharacterDefine.GetCharacterPrefabPath(cleanliness, Gender.Male);
            GameObject playerObj = Resources.Load<GameObject>(playerPath);
            GameObject player = Instantiate(playerObj, PlayerLocation.position, PlayerLocation.rotation) as GameObject;
            Camera.main.GetComponent<CameraController>().Target = player.transform;
            Camera.main.transform.rotation = PlayerLocation.rotation;
            //更改小地图相机Target
            for (int i = 0; i < miniMaps.Length; i++)
            {
                MiniMap map = miniMaps[i];
                map.Target = player.transform;
            }
        }

        /// <summary>
        /// 过渡显示
        /// </summary>
        /// <returns></returns>
        IEnumerator FadeOn()
        {
            m_CanvasGroup.alpha = 0;
            while (m_CanvasGroup.alpha < 1)
            {
                m_CanvasGroup.alpha += Time.deltaTime * 2;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// 过渡关闭
        /// </summary>
        /// <returns></returns>
        IEnumerator FadeOff()
        {
            m_CanvasGroup.alpha = 1;
            while (m_CanvasGroup.alpha > 0)
            {
                m_CanvasGroup.alpha -= Time.deltaTime * 2;
                yield return new WaitForEndOfFrame();
            }
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 更衣流程完成时，触发
        /// </summary>
        private void DressingAsset_OnCompleted()
        {
            dressingAsset.OnCompleted.RemoveListener(DressingAsset_OnCompleted);
            //加载Player
            ChangePlayer(dressingAsset.playerLocation, dressingAsset.cleanliness);
            dressingAsset.Close();
            Fading = false;
        }

        void Update()
        {
            if (Fading)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DressingAsset_OnCompleted();
                }
            }
        }
    }
}
