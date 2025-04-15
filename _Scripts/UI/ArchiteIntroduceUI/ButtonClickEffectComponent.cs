using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace XFramework.UI
{
  public  class ButtonClickEffectComponent:MonoBehaviour
    {
        private Image image;
        private void Awake()
        {
            image =transform.Find("ImageEffect").GetComponent<Image>();
            image.DOFade(0, 0.25f).SetLoops(-1, LoopType.Yoyo);
        }


    }
}
