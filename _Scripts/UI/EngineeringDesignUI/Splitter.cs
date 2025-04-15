using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 分隔器类
/// </summary>
public class Splitter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler,IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 得到整个画布的ugui坐标
    /// </summary>
    private RectTransform engineeringDesign;

    /// <summary>
    /// 得到segmentLine的ugui坐标
    /// </summary>
    public RectTransform segmentLine;

    /// <summary>
    /// 要改变的鼠标形状
    /// </summary>
    public Texture2D cursorTexture; 

    public CursorMode cursorMode = CursorMode.Auto;

    /// <summary>
    /// 鼠标原来的形状与改变后的偏移量
    /// </summary>
    public Vector2 hotSpot = new Vector2(15,15);

    /// <summary>
    /// 鼠标按下时的位置与鼠标移动的偏移量
    /// </summary>
    Vector2 offset = new Vector2();

    private RectTransform imgRect;
   
    /// <summary>
    /// 分隔器最小界限
    /// </summary>
    public float minOffset = -100;

    /// <summary>
    /// 分隔器最大界限
    /// </summary>
    public float maxOffset = 350;

    /// <summary>
    /// 分界线左侧Panel的面板坐标
    /// </summary>    
    private RectTransform leftPanel;
    
    /// <summary>
    /// 分界线右侧Panel的面板坐标
    /// </summary>    
    private RectTransform rightPanel;

    /// <summary>
    /// 最终偏移量
    /// </summary>
    Vector2 endOffset = new Vector2();

    /// <summary>
    /// 是否拖动中
    /// </summary>
    private bool dragging = false;

    /// <summary>
    /// 初始分界线的X坐标
    /// </summary>
    private float initXPosition;

    void Awake()
    {
        imgRect = GetComponent<RectTransform>();
        engineeringDesign = transform.parent.parent.GetComponent<RectTransform>();
        leftPanel = transform.parent.Find("DesignInfoPanel").GetComponent<RectTransform>();
        rightPanel = transform.parent.Find("DesignDisplayPanel").GetComponent<RectTransform>();
    }

    void Start()
    {        
        initXPosition = imgRect.position.x- engineeringDesign.position.x;
    }

    /// <summary>
    /// 当鼠标结束拖动时调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录鼠标按下时的屏幕坐标
        Vector2 mouseDown = eventData.position;
        Vector2 mouseUguiPos = new Vector2(); //定义一个接收返回的ugui坐标
                                              //RectTransformUtility.ScreenPointToLocalPointInRectangle()：把屏幕坐标转化成ugui坐标
                                              //canvas：坐标要转换到哪一个物体上，这里img父类是Canvas，我们就用Canvas
                                              //eventData.enterEventCamera：这个事件是由哪个摄像机执行的
                                              //out mouseUguiPos：返回转换后的ugui坐标
                                              //isRect：方法返回一个bool值，判断鼠标按下的点是否在要转换的物体上                                              
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(segmentLine, mouseDown, eventData.enterEventCamera, out mouseUguiPos);
        if (isRect) //如果在
        {
            //计算图片中心和鼠标点的差值
            offset.x = imgRect.anchoredPosition.x - mouseUguiPos.x;
        }
    }

    /// <summary>
    /// 当鼠标拖动时调用 对应接口 IDragHandler
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mouseDrag = eventData.position; //当鼠标拖动时的屏幕坐标
        Vector2 uguiPos = new Vector2(); //用来接收转换后的拖动坐标
                                         //和上面类似
        bool isOnRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(segmentLine, mouseDrag, eventData.enterEventCamera, out uguiPos);        
        if (isOnRect)
        {
                        
            //设置偏移量的最大最小值
            if (uguiPos.x <= minOffset)
            {
                uguiPos.x = minOffset;                
            }
            else if (uguiPos.x >= maxOffset)
            {
                uguiPos.x = maxOffset;                
            }
            endOffset.x = uguiPos.x - initXPosition;
            //设置图片的ugui坐标与鼠标的ugui坐标保持不变
            imgRect.anchoredPosition = new Vector2(uguiPos.x, imgRect.anchoredPosition.y);
            //leftPanel.offsetMin = new Vector2(endOffset.x, 0);

            //rightPanel.offsetMax = new Vector2(-endOffset.x, 0);
            leftPanel.offsetMax = new Vector2(endOffset.x, 0);
            rightPanel.offsetMin = new Vector2(endOffset.x, 0);
        }
    }

    /// <summary>
    /// 当鼠标结束拖动时调用 对应接口 IEndDragHandler
    /// </summary>
    /// <param name="eventData"></param>    
    public void OnEndDrag(PointerEventData eventData)
    {
        offset = Vector2.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!dragging)
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}



