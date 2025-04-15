using System.Collections.Generic;
using UnityEngine;


public partial class ArchiteIntroduceScene
{
    private List<Transform> flowRooms = new List<Transform>();
    private List<Transform> hightRooms = new List<Transform>();
    private List<Transform> hightArrows = new List<Transform>();
    private Transform car01;
    private Transform car02;
    private Transform car03;
    private Transform car04;
    private Transform man01;
    private Transform man02;
    /// <summary>
    /// 料斗
    /// </summary>
    private Transform hopper01;
    private Transform hopper01_1;
    private Transform hopper02;
    private Transform hopper02_2;
    private ArchiteManController m_ArchiteManController;
    private Transform dirtyMan;
    private Transform cleanMan01;
    private Transform cleanMan02;
    private Transform cleanMan03;
    private Archite_SlideRun m_Archite_SlideRun;
    void InitComponent()
    {
        Transform flowRoomParent= transform.Find("流程房间");
        foreach (Transform item in flowRoomParent)
        {
            flowRooms.Add(item);
        }
        Transform hightRoomParent = transform.Find("发光房间");
        foreach (Transform item in hightRoomParent)
        {
            hightRooms.Add(item);
        }
        Transform hightArrowParent = transform.Find("箭头");
        foreach (Transform item in hightArrowParent)
        {
            hightArrows.Add(item);
        }
        car01 = transform.Find("扩展/car01");
        car02 = transform.Find("扩展/car02");
        car03 = transform.Find("扩展/car03");
        car04 = transform.Find("扩展/car04");


        man01 = transform.Find("扩展/man01");
        man02 = transform.Find("扩展/man02");
        hopper01 = transform.Find("扩展/hopper01");
        hopper01_1 = transform.Find("扩展/hopper01_1");
        hopper02 = transform.Find("扩展/hopper02");
        hopper02_2 = transform.Find("扩展/hopper02_2");
        m_ArchiteManController = transform.Find("扩展/manControl").GetComponent<ArchiteManController>();
        dirtyMan = transform.Find("扩展/dirtyMan");
        cleanMan01 = transform.Find("扩展/cleanMan01");
        cleanMan02 = transform.Find("扩展/cleanMan02");
        cleanMan03 = transform.Find("扩展/cleanMan03");
        m_Archite_SlideRun = transform.Find("扩展/Archite_SlideRun").GetComponent<Archite_SlideRun>();
    }
}

