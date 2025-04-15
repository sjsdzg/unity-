using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 总体介绍中的流程图
    /// </summary>
	public class ProcessImage : MonoBehaviour, 
        IPointerEnterHandler, 
        IPointerExitHandler
    {

        private RectTransform pictureTras;
        private Vector2 originalScalse;
        private bool isOn = false;

        private Vector2 currentPivot;
        private Vector2 PIDLocalPoint;
        private Vector2 viewportLocalPoint;
        public RectTransform viewPort;

        void Awake()
        {
            pictureTras = transform.GetComponent<RectTransform>();
            originalScalse = pictureTras.localScale;
        }

        void LateUpdate()
        {
            if (isOn)
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(viewPort, new Vector2(Input.mousePosition.x, Input.mousePosition.y), null, out viewportLocalPoint);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(pictureTras, new Vector2(Input.mousePosition.x, Input.mousePosition.y), null, out PIDLocalPoint);
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        currentPivot = new Vector2(pictureTras.pivot.x + PIDLocalPoint.x / pictureTras.sizeDelta.x, pictureTras.pivot.y + PIDLocalPoint.y / pictureTras.sizeDelta.y);
                        pictureTras.pivot = currentPivot;
                        pictureTras.localPosition = new Vector3(viewportLocalPoint.x, viewportLocalPoint.y, 0);

                        originalScalse.x += 0.2f;
                        originalScalse.y += 0.2f;
                    }
                    else
                    {
                        if (Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            originalScalse.x -= 0.2f;
                            originalScalse.y -= 0.2f;
                        }
                    }

                    if (originalScalse.x >= 1.3f && originalScalse.y >= 1.3f)
                    {
                        if (originalScalse.x <= 20f && originalScalse.y <= 20f)
                        {
                            pictureTras.localScale = originalScalse;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        originalScalse = new Vector2(1.3f, 1.3f);
                        return;
                    }
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isOn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOn = false;
        }


    }
}