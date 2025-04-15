using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.PLC
{
    public class GoOnHint : MonoBehaviour
    {

        private Button btnSure;
        
        void Awake()
        {
            btnSure = transform.Find("Btn_Sure").GetComponent<Button>();
           
            btnSure.onClick.AddListener(BtnSure);
        }

        public void BtnSure()
        {
            gameObject.SetActive(false);
            
        }
    }
}

