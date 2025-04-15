using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 设置Text
    /// </summary>
	public class SetText : MonoBehaviour {

        private Text text;
		// Use this for initialization
		void Start () {
            text = transform.GetComponentInChildren<Text>();
            text.text = name;
		}
		
		
	}
}