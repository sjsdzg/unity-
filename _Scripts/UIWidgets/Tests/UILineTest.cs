using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.UIWidgets;

public class UILineTest : MaskableGraphic
{
    [SerializeField]
    private Vector3 p1;
    /// <summary>
    /// p1
    /// </summary>
    public Vector3 P1
    {
        get { return p1; }
        set 
        {
            if (SetPropertyUtils.SetStruct(ref p1, value))
            {
                SetVerticesDirty();
            }
        }
    }

    [SerializeField]
    private Vector3 p2;
    /// <summary>
    /// p2
    /// </summary>
    public Vector3 P2
    {
        get { return p2; }
        set
        {
            if (SetPropertyUtils.SetStruct(ref p2, value))
            {
                SetVerticesDirty();
            }
        }
    }

    [SerializeField]
    private float thickness;
    /// <summary>
    /// 厚度
    /// </summary>
    public float Thickness
    {
        get { return thickness; }
        set
        {
            if (SetPropertyUtils.SetStruct(ref thickness, value))
            {
                SetVerticesDirty();
            }
        }
    }


    protected override void OnPopulateMesh(VertexHelper vh)
    {
        //rectTransform.sizeDelta = new Vector2((p2 - p1).magnitude, thickness);
        //var r = GetPixelAdjustedRect();
        //base.OnPopulateMesh(vh);
        vh.Clear();
        vh.AddLine(p1, p2, thickness, color);
    }
}
