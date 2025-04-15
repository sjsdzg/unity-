using System;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.UI
{
    public class SoundBar : MonoBehaviour
    {
        Button buttonClose;
        [HideInInspector]
        public Slider sliderMusic;
        [HideInInspector]
        public Slider sliderEffect;

        public UniEvent<float> OnSliderMusicValueChanged = new UniEvent<float>();

        public UniEvent<float> OnSliderEffectValueChanged = new UniEvent<float>();

        private void Awake()
        {
            buttonClose = transform.Find("Header/ButtonClose").GetComponent<Button>();
            sliderMusic = transform.Find("Content/Music/Slider").GetComponent<Slider>();
            sliderEffect = transform.Find("Content/Effect/Slider").GetComponent<Slider>();

            buttonClose.onClick.AddListener(buttonClose_onClick);
            sliderMusic.onValueChanged.AddListener(sliderMusic_onValueChanged);
            sliderEffect.onValueChanged.AddListener(sliderEffect_onValueChanged);
        }

        private void sliderEffect_onValueChanged(float arg0)
        {
            OnSliderEffectValueChanged.Invoke(arg0);
        }

        private void sliderMusic_onValueChanged(float arg0)
        {
            OnSliderMusicValueChanged.Invoke(arg0);
        }

        private void buttonClose_onClick()
        {
            Hide();
        }
        
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="message"></param>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
