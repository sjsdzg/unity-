using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HighlightingSystem;
using UnityEngine;
using XFramework.Simulation;
using XFramework.Simulation.Component;

namespace XFramework.Common
{
    public class InvokeFlashAction : ActionBase
    {
        /// <summary>
        /// gameObject
        /// </summary>
        public GameObject[] gameObjects;

        //public HighlighterComponent[] highlighterComponents;
        /// <summary>
        /// 是否高亮
        /// </summary>
        public bool isFlash;

        public InvokeFlashAction(bool _isFlash, params GameObject[] _gameObjects)
        {
            gameObjects = _gameObjects;
            isFlash = _isFlash;
            //int length = gameObjects.Length;
            //highlighterComponents = new HighlighterComponent[length];
            //for (int i = 0; i < length; i++)
            //{
            //    highlighterComponents[i] = gameObjects[i].GetOrAddComponent<HighlighterComponent>();
            //}
        }

        public override void Execute()
        {
            ProductionMode mode = ProductionMode.None;
            CustomWorkshop workshop = GameObject.FindObjectOfType<CustomWorkshop>();
            if (workshop!=null)
            {
                mode = workshop.customStage.ProductMode;
            }
            if (mode!=ProductionMode.Study)
            {
                Completed();
            }
            else
            {
                if (isFlash)
                {
                    FlashManager.Instance.ShowFlash(gameObjects);
                }
                else
                {
                    FlashManager.Instance.CloseFlash(gameObjects);
                }
                //完成
                Completed();
            }
        }
    }
}
