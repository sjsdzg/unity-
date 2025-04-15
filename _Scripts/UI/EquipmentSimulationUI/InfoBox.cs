using UnityEngine;
using System.Collections;
using XFramework.Core;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Common;
using DG.Tweening;

public class InfoBox : MonoSingleton<InfoBox>
{
    /// <summary>
    /// m_RectTransform
    /// </summary>
    private RectTransform m_RectTransform = null;

    /// <summary>
    /// 图标
    /// </summary>
    private Image m_Icon = null;

    /// <summary>
    /// 名称
    /// </summary>
    private Text m_Name = null;

    /// <summary>
    /// 文本
    /// </summary>
    private Text m_Text = null;

    /// <summary>
    /// 偏移
    /// </summary>
    [SerializeField]
    public Vector3 offset = new Vector3(0, -24, 0);

    /// <summary>
    /// 物品图标路径
    /// </summary>
    public const string PATH = "EquipmentSimulation";

    protected override void Init()
    {
        base.Init();
        m_RectTransform = transform as RectTransform;
        m_Icon = transform.Find("Image/Icon").GetComponent<Image>();
        m_Name = transform.Find("Image/Name").GetComponent<Text>();
        m_Text = transform.Find("Text").GetComponent<Text>();
        m_RectTransform.anchorMin = Vector2.zero;
        m_RectTransform.anchorMax = Vector2.zero;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="mousePosition"></param>
    protected virtual void SetPosition(Vector3 mousePosition)
    {
        Vector3 pos = mousePosition + offset;
        var width = m_RectTransform.rect.width;
        var height = m_RectTransform.rect.height;

        float x = 0;
        float y = 0;
        //x轴位置
        if (pos.x > Screen.width - width * (1 - m_RectTransform.pivot.x))
        {
            x = Screen.width - width * (1 - m_RectTransform.pivot.x);
        }
        else if (pos.x < width * m_RectTransform.pivot.x)
        {
            x = width * m_RectTransform.pivot.x;
        }
        else
        {
            x = pos.x;
        }
        //y轴位置
        if (pos.y > Screen.height - height * (1 - m_RectTransform.pivot.x))
        {
            y = Screen.height - height * (1 - m_RectTransform.pivot.x);
        }
        else if (pos.y < height * m_RectTransform.pivot.y)
        {
            y = height * m_RectTransform.pivot.y;
        }
        else
        {
            y = pos.y;
        }
        //设置
        m_RectTransform.anchoredPosition = new Vector2(x, y);
    }

    /// <summary>
    /// 显示提示内容
    /// </summary>
    /// <param name="b">show?</param>
    public void Show(string name, Sprite sprte, string text = "")
    {
        if (Instance == null)
            return;
        //动态显示
        m_RectTransform.localScale = new Vector3(0, 0);
        m_RectTransform.DOScale(1, 0.2f);
        gameObject.SetActive(true);
        m_Name.text = name;
        m_Icon.sprite = sprte;
        m_Text.text = text;

        SetPosition(Input.mousePosition);
    }

    /// <summary>
    /// 显示提示内容
    /// </summary>
    /// <param name="info"></param>
    public void Show(EquipmentPart part)
    {
        if (part == null)
            return;

        //string path = string.Format("{0}/{1}/Icons/{2}", PATH, info.EquipmentName, info.Name);
        //Sprite sprite = Resources.Load<Sprite>(path);
        //Show(info.Name, sprite, info.Description);
        string assetBundleName = "Assets/Textures/Equipments/" + part.EquipmentName;
        string assetName = part.Sprite;
        AsyncLoadAssetOperation async = Assets.LoadAssetAsync<Sprite>(assetBundleName, assetName);
        if (async == null)
            return;

        async.OnCompleted(x =>
        {
            AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
            Sprite sprite = loader.GetAsset<Sprite>();
            if (sprite != null)
            {
                Show(part.Name, sprite, part.Description);
            }
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            GameObject go = Utils.CurrentPointerEnterGameObject();
            if (go==null || ((go.name != name) && !go.transform.IsChildOf(transform)))
            {
                Hide();
            }
        }
    }
}
