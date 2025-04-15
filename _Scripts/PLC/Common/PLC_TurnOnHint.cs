using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class PLC_TurnOnHint : MonoBehaviour
    {
        private Button btn_Close;

        void Awake()
        {
            btn_Close = transform.Find("Title/Button_Close").GetComponent<Button>();

            btn_Close.onClick.AddListener(Btn_Close);
        }

        public void Btn_Close()
        {
            gameObject.SetActive(false);
        }
    }
}
