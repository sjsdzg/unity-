using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Simulation;
using PathologicalGames;
using UIWidgets;
using System.Text;
using XFramework.Proto;
using Newtonsoft.Json.Linq;
using XFramework;
using XFramework.UI;
using UnityEngine;
public class ArchiteIntroduceCenterBar : MonoBehaviour
{
    private Transform Image_Flow;
    private void Awake()
    {
        Image_Flow = transform.Find("Image_Flow");
    }
    public void Show()
    {
        Image_Flow.gameObject.SetActive(true);
    }
    public void Hide()
    {
        Image_Flow.gameObject.SetActive(false);
    }
}


