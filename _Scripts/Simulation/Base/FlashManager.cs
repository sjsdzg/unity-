using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Core;
using XFramework.Module;
using XFramework.Common;
using XFramework.Simulation.Component;
using System.Linq;

namespace XFramework.Simulation
{

    /// <summary>
    /// 高亮管理（引导物体）
    /// </summary>
    public class FlashManager : Singleton<FlashManager>
    {
        List<HighlighterComponent> currentHighlighters = new List<HighlighterComponent>();

        [HideInInspector]
        public bool isStudy=true;

        public void ShowFlash(params GameObject[] gameObjects)
        {
            if (!isStudy)
            {
                return;
            }
            if (currentHighlighters != null)
            {
                currentHighlighters.ForEach(x => x.FlashingOff());
            }
            gameObjects.ForEach(x => 
            {
                HighlighterComponent component =  x.GetOrAddComponent<HighlighterComponent>();
                component.FlashingOn();
                if (!currentHighlighters.Contains(component))
                {
                    currentHighlighters.Add(component);
                }
            });
        }

        public void CloseFlash(params GameObject[] gameObjects)
        {
            if (!isStudy)
            {
                return;
            }
            gameObjects.ForEach(x =>
            {
                HighlighterComponent component = x.GetComponent<HighlighterComponent>();
                if (component != null)
                {
                    component.FlashingOff();
                    if (currentHighlighters.Contains(component))
                    {
                        currentHighlighters.Remove(component);
                    }
                }
            });
        }

        public void ClearAllFlash()
        {
            currentHighlighters.ForEach(x =>
            {
                x.FlashingOff();
            });
            currentHighlighters.Clear();
        }

        //public void ShowHighlight(params HighlighterComponent[] highlighters)
        //{
        //    if (currentHighlighters!=null)
        //    {
        //        currentHighlighters.ForEach(x => x.FlashingOff());
        //    }
        //    highlighters.ForEach(x => x.FlashingOn());
        //    currentHighlighters = highlighters.ToList();
        //}

        //public void CloseHighlight(params HighlighterComponent[] highlighters)
        //{
        //    highlighters.ForEach(x => 
        //    {
        //        x.FlashingOff();
        //        if (currentHighlighters.Contains(x))
        //        {
        //            currentHighlighters.Remove(x);
        //        }
        //    });
        //}

        
    }
}
