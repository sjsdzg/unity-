using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.UIWidgets;

public class UIGridTest : MaskableGraphic
{
    [SerializeField]
    private float m_CellSize;
    /// <summary>
    /// 单元尺寸
    /// </summary>
    public float CellSize
    {
        get { return m_CellSize; }
        set
        {
            if (SetPropertyUtils.SetStruct(ref m_CellSize, value))
            {
                SetVerticesDirty();
            }
        }
    }


    [SerializeField]
    private float m_LineWidth = 1;
    /// <summary>
    /// 线条宽度
    /// </summary>
    public float LineWidth
    {
        get { return m_LineWidth; }
        set
        {
            if (SetPropertyUtils.SetStruct(ref m_LineWidth, value))
            {
                SetVerticesDirty();
            }
        }
    }

    [SerializeField]
    private Color m_GridColor;
    /// <summary>
    /// 网格颜色
    /// </summary>
    public Color GridColor
    {
        get { return m_GridColor; }
        set 
        { 
            m_GridColor = value;
            if (SetPropertyUtils.SetColor(ref m_GridColor, value))
            {
                SetVerticesDirty();
            }
        }
    }

    [SerializeField]
    private float scale = 1;
    /// <summary>
    /// 缩放值
    /// </summary>
    public float Scale
    {
        get { return scale; }
        set 
        {
            if (SetPropertyUtils.SetStruct(ref scale, value))
            {
                SetVerticesDirty();
            }
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);
        vh.AddGrid(rectTransform.rect, CellSize, LineWidth / Scale, m_GridColor);
    }

    private void LateUpdate()
    {
        Scale = rectTransform.lossyScale.x / canvas.transform.lossyScale.x;
    }
}
