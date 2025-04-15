using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.PLC
{
    class PLC_InputFieldWrite : MonoBehaviour
    {
        public bool isWrite = false;
        private Transform text_Num;
        private Transform img_Shelf;

        void Start()
        {

            text_Num = transform.Find("Text_Num");
            img_Shelf = transform.Find("Image_Shelf");

            if (isWrite)
            {
                text_Num.gameObject.SetActive(false);
                img_Shelf.gameObject.SetActive(false);
            }
            else
            {
                text_Num.gameObject.SetActive(true);
                img_Shelf.gameObject.SetActive(true);
            }
        }
    }
}
