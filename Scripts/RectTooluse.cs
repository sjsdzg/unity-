using Battlehub.RTHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XFramework.Architectural;

public class RectTooluse : MonoBehaviour
{
    //public GameObject cad;


    public RectTool rectTool;
    /// <summary>
    /// 射线检测
    /// </summary>
    public ArchitectRaycaster ArchitectRaycaster;
    Transform cachedTransform;
    Camera mainCamera;

    public bool IsUsingTool = false;
    private void Awake()
    {
        cachedTransform = transform;
        rectTool.gameObject.SetActive(false);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // 设置全屏
            Screen.fullScreen = !Screen.fullScreen;
        }
        
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    cad.SetActive(!cad.activeSelf);
        //}

        if (!IsUsingTool)
        {
            return;
        }

        //鼠标右键按下
        if (Input.GetMouseButtonDown(1))
        {
            startDragPoint.Clear();

            //RectTool.enabled = true;
            rectTool.gameObject.SetActive(false);
            ArchitectRaycaster.enabled = false;
            StopUsingTool();
        }

        //当前相机模式为透视模式
        if (mainCamera.orthographic == false)
        {
            //RectTool.enabled = false;
            rectTool.gameObject.SetActive(false);
            ArchitectRaycaster.enabled = false;
            StopUsingTool();
        }
    }

    public void StartUsingTool()
    {
        rectTool.BeforeDrag.AddListener(SatrtDrag);
        rectTool.Drop.AddListener(EndDrag);
        IsUsingTool = true;
        rectTool.gameObject.SetActive(true);
        ArchitectRaycaster.enabled = false;
        RecordDragPoint();
    }
    /// <summary>
    /// 拖拽点  -1表示没有拖拽点
    /// </summary>
    int pointIndex = -1;
    /// <summary>
    /// 拖拽边  -1表示没有拖拽边
    /// </summary>
    int edgeIndex = -1;
    public Vector3[] SatrtVectors1;
    public Vector3[] EndVectors2;
    /// <summary>
    /// 单次拖拽结束
    /// </summary>
    public Action<RectTooluse> finish;
    private void EndDrag(BaseHandle arg0)
    {
        EndVectors2 = rectTool.GetVertices();
        finish?.Invoke(this);
        RecordDragPoint();
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="arg0"></param>
    private void SatrtDrag(BaseHandle arg0)
    {
        SatrtVectors1 = rectTool.GetVertices();
    }

    public void StopUsingTool()
    {
        IsUsingTool = false;
        rectTool.gameObject.SetActive(false);
        ArchitectRaycaster.enabled = true;
    }

    /// <summary>
    /// 设置RectTool的位置和缩放
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    public void SetPositionAndScale(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 pos)
    {
        //RectTool.enabled = false;
        rectTool.gameObject.SetActive(false);
        //p1 p2 p3 p4 为矩形四个顶点
        Vector3 center = (p1 + p2 + p3 + p4) / 4;
        cachedTransform.position = center;
        cachedTransform.position = pos;
        Vector3 scale = new Vector3(Vector3.Distance(p1, p2), 1, Vector3.Distance(p1, p4));
        cachedTransform.localScale = scale;

    }

    public List<Vector3> startDragPoint = new List<Vector3>();
    /// <summary>
    /// 记录拖拽点的初始位置
    /// </summary>
    public void RecordDragPoint()
    {
        var points = rectTool.GetVertices();
        startDragPoint.Clear();
        startDragPoint.Add(points[0]);
        startDragPoint.Add(points[1]);
        startDragPoint.Add(points[2]);
        startDragPoint.Add(points[3]);
    }
}