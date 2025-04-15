using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.UI;

namespace XFramework.Common
{
    /// <summary>
    /// HUD管理器
    /// </summary>
    public class HUDManager : MonoSingleton<HUDManager>
    {
        /// <summary>
        /// The Canvas Root of scene.
        /// </summary>
        [Tooltip("Canvas Root of scene.")]
        public Transform CanvasParent;

        /// <summary>
        /// UI Prefab to instatantiate
        /// </summary>
        public GameObject IndicatorPrefab;

        /// <summary>
        /// 边界
        /// </summary>
        public float clampBorder = 30;

        /// <summary>
        /// uiScaleMode
        /// </summary>
        private CanvasScaler.ScaleMode uiScaleMode;

        /// <summary>
        /// 分辨率
        /// </summary>
        private Vector2 referenceResolution;

        /// <summary>
        /// 屏幕分辨率
        /// </summary>
        private Vector2 screenResolution;

        /// <summary>
        /// HUD内置prafab
        /// </summary>
        public HUDPrafabInfoList HUDPrafabInfoList = new HUDPrafabInfoList();

        /// <summary>
        /// 指示器信息
        /// </summary>
        public List<HUDView> HUDViewList = new List<HUDView>();

        void Start()
        {
            if (CanvasParent == null)
            {
                Debug.LogWarning("CanvasParent field requieres a Canvas GameObject");
            }

            if (IndicatorPrefab == null)
            {
                Debug.LogWarning("IndicatorPrefab is null.");
            }

            //获取Canvas的uiScaleMode
            uiScaleMode = CanvasParent.GetComponent<CanvasScaler>().uiScaleMode;
        }

        void LateUpdate()
        {
            if (HUDUtil.mCamera == null)
            {
                return;
            }

            for (int i = 0; i < HUDViewList.Count; i++)
            {
                //cache the current indicator
                HUDView temporal = HUDViewList[i];
                if (temporal == null)
                {
                    HUDViewList.RemoveAt(i);
                    return;
                }

                //when target is destroyed then remove it from list.
                if (temporal.HUDInfo.m_Target == null)
                {
                    //When player / Object death, destroy all last text.
                    Destroy(temporal.gameObject);
                    HUDViewList.Remove(temporal);
                    return;
                }
                //更新指示器位置
                UpdateIndicatorPostion(temporal);
                temporal.UpdateView();
            }
        }

        /// <summary>
        /// 更新指示器位置
        /// </summary>
        /// <param name="hudView"></param>
        private void UpdateIndicatorPostion(HUDView hudView)
        {
            //if transform destroy, then remove form list
            if (hudView.HUDInfo.m_Target == null)
            {
                HUDViewList.Remove(hudView);
                return;
            }
            //angle
            float angle = 0;
            //Calculate Position of target
            Vector3 relativePosition = hudView.HUDInfo.m_Target.position + hudView.HUDInfo.offset;
            //Caculate distance from player to waypoint
            float distance = Vector3.Distance(HUDUtil.mCamera.transform.position, relativePosition);
            //设置
            hudView.HUDInfo.distance = distance;
            //Get the position in screen
            Vector3 screenPos = HUDUtil.ScreenPosition(relativePosition);
            //绘制位置
            Vector2 drawPosition = Vector2.zero;

            if (HUDUtil.OnScreen(relativePosition, clampBorder)) //INSIDE OF SCREEN
            {
                hudView.OnScreen = true;
                drawPosition.x = screenPos.x - (Screen.width / 2);
                drawPosition.y = screenPos.y - (Screen.height / 2);
                hudView.transform.localEulerAngles = Vector3.zero;
            }
            else // OUT OF SCREEN
            {
                hudView.OnScreen = false;

                angle = Mathf.Atan2(screenPos.y - (Screen.height / 2), screenPos.x - (Screen.width / 2));

                if (screenPos.x - Screen.width / 2 > 0)// right side
                {
                    drawPosition.x = Screen.width / 2 - clampBorder;
                    drawPosition.y = drawPosition.x * Mathf.Tan(angle);
                }
                else //left side
                {
                    drawPosition.x = -Screen.width / 2 + clampBorder;
                    drawPosition.y = drawPosition.x * Mathf.Tan(angle);
                }
                //up side
                if (drawPosition.y > Screen.height / 2 - clampBorder)
                {
                    drawPosition.y = Screen.height / 2 - clampBorder;
                    drawPosition.x = drawPosition.y / Mathf.Tan(angle);
                }
                //down side
                if (drawPosition.y < -Screen.height / 2 + clampBorder)
                {
                    drawPosition.y = -Screen.height / 2 + clampBorder;
                    drawPosition.x = drawPosition.y / Mathf.Tan(angle);
                }
                //判断是否在相机后面
                if (HUDUtil.BehindCamera(relativePosition))
                {
                    drawPosition.x = -drawPosition.x;
                    drawPosition.y = -drawPosition.y;
                } 
            }
            //set position
            if (uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                referenceResolution = CanvasParent.GetComponent<CanvasScaler>().referenceResolution;
                drawPosition.x = drawPosition.x * referenceResolution.x / Screen.width;
                drawPosition.y = drawPosition.y * referenceResolution.y / Screen.height;
            }
            hudView.transform.localPosition = new Vector3(drawPosition.x, drawPosition.y, 0);
            
            //set rotation
            if (!hudView.OnScreen)
            {
                if (HUDUtil.BehindCamera(relativePosition))
                {
                    angle = Mathf.Atan2(-(screenPos.y - (Screen.height / 2)), -(screenPos.x - (Screen.width / 2)));
                }
                else
                {
                    angle = Mathf.Atan2(screenPos.y - (Screen.height / 2), screenPos.x - (Screen.width / 2));
                }
            }
            else
            {
                angle = 90 * Mathf.Deg2Rad;
            }
            hudView.transform.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);
        }

        /// <summary>
        /// 返回实体Id
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public HUDView CreateHUD(HUDInfo info)
        {
            if (info.m_Target == null)
                return null;

            GameObject prefab = (info.hudPrefab == null) ? HUDPrafabInfoList[info.HUDType] : info.hudPrefab;
            GameObject go = Instantiate(prefab) as GameObject;
            go.SetActive(true);
            HUDView item = go.GetComponent<HUDView>();
            item.Initialize(info);

            go.transform.SetParent(CanvasParent, false);
            go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            HUDViewList.Add(item);

            return item;
        }

        /// <summary>
        /// 移除target上的HUD
        /// </summary>
        /// <param name="info"></param>
        public void RemoveHUD(Transform target)
        {
            if (ExistsHUD(target))
            {
                for (int i = 0; i < HUDViewList.Count; i++)
                {
                    HUDView item = HUDViewList[i];
                    if (item.m_Target == target)
                    {
                        HUDViewList.Remove(item);
                    }
                }
            }
        }
        
        /// <summary>
        /// 根据ID，移除HUD
        /// </summary>
        /// <param name="id"></param>
        public void RemoveHUD(int id)
        {
            for (int i = 0; i < HUDViewList.Count; i++)
            {
                HUDView item = HUDViewList[i];
                if (item.Id == id)
                {
                    HUDViewList.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取物体上的HUD
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<HUDView> GetHUD(Transform target)
        {
            List<HUDView> array = null;

            if (ExistsHUD(target))
            {
                array = new List<HUDView>();
                foreach (HUDView item in HUDViewList)
                {
                    if (item.m_Target == target)
                    {
                        array.Add(item);
                    }
                }
            }

            return array;
        }

        /// <summary>
        /// 根据Id获取HUD
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public HUDView GetHUD(int id)
        {
            HUDView hudView = null;
            for (int i = 0; i < HUDViewList.Count; i++)
            {
                HUDView item = HUDViewList[i];
                if (item.Id == id)
                {
                    hudView = item;
                    break;
                }
            }
            return hudView;
        }

        /// <summary>
        /// 物体是否存在指示器
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool ExistsHUD(Transform target)
        {
            bool exists = false;
            foreach (HUDView hudView in HUDViewList)
            {
                if (hudView.m_Target == target)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        /// <summary>
        /// 根据深度排序
        /// </summary>
        public void SortByDepth()
        {
            HUDViewList.Sort((a, b) =>
            {
                return a.m_Target.position.z.CompareTo(b.m_Target.position.z);
            });
            //按深度排序
            for (int i = 0; i < HUDViewList.Count; i++)
            {
                HUDViewList[i].transform.SetSiblingIndex(i);
            }
        }
    }
}
