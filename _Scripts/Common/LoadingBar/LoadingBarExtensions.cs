using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

public static class LoadingBarExtensions
{
    public static void ShowLoadingBar(this MonoBehaviour behaviour, float progress, string info)
    {
        RectTransform rt = behaviour.GetComponent<RectTransform>();
        LoadingBar.Instance.Show(progress, info, rt);
    }
}
