using System.Collections.Generic;
using UnityEngine;
public partial class ArchiteFixedIntroduceScene
{
    private List<Transform> hightRooms = new List<Transform>();
    private Transform firstMan;
    private Transform secondMan;
    private Transform car01;
    private Transform man01;
    private Transform man02;
    private Transform secondFloow;
    private Transform escapeRoom;
    void InitComponent()
    {
        Transform hightRoomParent = transform.Find("发光房间");
        foreach (Transform item in hightRoomParent)
        {
            hightRooms.Add(item);
        }
        firstMan = transform.Find("扩展/一楼小人");
        firstMan.gameObject.SetActive(false);
        secondMan = transform.Find("扩展/二楼小人");
        secondMan.gameObject.SetActive(false);
        car01 = transform.Find("扩展/二楼小货车");
        car01.gameObject.SetActive(false);
        man01 = transform.Find("扩展/man01");
        man01.gameObject.SetActive(false);
        man02 = transform.Find("扩展/man02");
        man02.gameObject.SetActive(false);
        secondFloow = GameObject.Find("固定条件讲解2楼").transform;
        secondFloow.gameObject.SetActive(false);
        escapeRoom = transform.Find("扩展/逃生间");
        escapeRoom.gameObject.SetActive(false);
    }
}

