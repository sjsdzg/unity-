using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace XFramework.UI
{
  public  class ProcessSortItem:MonoBehaviour
    {
        private Text TextSort;

        public void SetData(string value)
        {
            TextSort.text = value;
        }
        private void Awake()
        {
            TextSort = GetComponent<Text>();
        }
    }
}
