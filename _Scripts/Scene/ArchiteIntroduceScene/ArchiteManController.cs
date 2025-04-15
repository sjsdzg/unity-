using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Actions;
using XFramework.Simulation;


public  class ArchiteManController:MonoBehaviour
    {
    private Transform man01;
    private Transform man02;
    private Transform man03;
    private Transform man04;
    private Transform man05;
    private Transform man06;
    private Transform man07;
    private Transform man08;
    private Transform man09;
    private Transform man10;
    private Transform man11;
    private Transform man12;

    private void Awake()
    {
        man01 = transform.Find("man01");
        man02 = transform.Find("man02");
        man03 = transform.Find("man03");
        man04 = transform.Find("man04");
        man05 = transform.Find("man05");
        man06 = transform.Find("man06");
        man07 = transform.Find("man07");
        man08 = transform.Find("man08");
        man09 = transform.Find("man09");
        man10 = transform.Find("man10");
        man11 = transform.Find("man11");
        man12 = transform.Find("man12");
    }
    private void Start()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(38.35193f, 20, 28.11922f),19f))
            //.Append(new DOLocalMoveAction(man12, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            //.Append(new DOLocalMoveAction(man12, new Vector3(39.44f, 0.1f, 13.36f), 1f))
            //.Append(new DOLocalMoveAction(man12, new Vector3(39.44f, 0.1f, 8.84f), 0.5f))

            //.Append(new DOLocalMoveAction(man01,new Vector3 (31.55f, 0.1f, 13.36f),0.5f))
            //.Append(new DOLocalMoveAction(man01, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            //.Append(new DOLocalMoveAction(man01, new Vector3(41.76f, 0.1f, 16.3f), 0.5f))
            //.Append(new DOLocalMoveAction(man01, new Vector3(36.16f, 0.1f, 16.3f), 1f))

            //.Append(new DOLocalMoveAction(man02, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            //.Append(new DOLocalMoveAction(man02, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            //.Append(new DOLocalMoveAction(man02, new Vector3(41.76f, 0.1f, 16.3f), 0.5f))
            //.Append(new DOLocalMoveAction(man02, new Vector3(50.45f, 0.1f, 16.3f), 1f))

            //.Append(new DOLocalMoveAction(man03, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            //.Append(new DOLocalMoveAction(man03, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            //.Append(new DOLocalMoveAction(man03, new Vector3(41.76f, 0.1f, 22.84f), 2f))
            //.Append(new DOLocalMoveAction(man03, new Vector3(45.78f, 0.1f, 22.84f), 1f))
            //.Append(new DOLocalMoveAction(man03, new Vector3(45.78f, 0.1f, 23.65f), 0.5f))
            .Append(new DelayedAction(2))
            .Append(new DOLocalMoveAction(man04, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            .Append(new DOLocalMoveAction(man04, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            .Append(new DOLocalMoveAction(man04, new Vector3(41.76f, 0.1f, 30.19f), 3f))
            .Append(new DOLocalMoveAction(man04, new Vector3(39.16f, 0.1f, 30.19f), 0.5f))
            .Append(new DOLocalMoveAction(man04, new Vector3(39.16f, 0.1f, 24f), 1f))
            .Append(new DOLocalMoveAction(man04, new Vector3(32f, 0.1f, 24f), 1.5f))

            .Append(new DOLocalMoveAction(man05, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            .Append(new DOLocalMoveAction(man05, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            .Append(new DOLocalMoveAction(man05, new Vector3(41.76f, 0.1f, 33.71f), 3f))//
            .Append(new DOLocalMoveAction(man05, new Vector3(39.33f, 0.1f, 33.71f), 0.5f))
            .Append(new DOLocalMoveAction(man05, new Vector3(39.33f, 0.1f, 31.68f), 0.5f))
            .Append(new DOLocalMoveAction(man05, new Vector3(31.95f, 0.1f, 31.68f), 1f))

            .Append(new DOLocalMoveAction(man06, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            .Append(new DOLocalMoveAction(man06, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            .Append(new DOLocalMoveAction(man06, new Vector3(41.76f, 0.1f, 36.27f), 3f))//
            .Append(new DOLocalMoveAction(man06, new Vector3(44.54f, 0.1f, 36.27f), 0.5f))
            .Append(new DOLocalMoveAction(man06, new Vector3(44.54f, 0.1f, 32.54f), 0.5f))
            .Append(new DOLocalMoveAction(man06, new Vector3(48.3f, 0.1f, 32.54f), 1f))

            .Append(new DOLocalMoveAction(man07, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            .Append(new DOLocalMoveAction(man07, new Vector3(41.76f, 0.1f, 37.82f), 3f))//
            .Append(new DOLocalMoveAction(man07, new Vector3(39.72f, 0.1f, 37.82f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(39.72f, 0.1f, 35.04f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(29.69f, 0.1f, 35.04f), 1f))

            .Append(new DOLocalMoveAction(man08, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            .Append(new DOLocalMoveAction(man08, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            .Append(new DOLocalMoveAction(man08, new Vector3(41.76f, 0.1f, 39.01f), 3f))//
            .Append(new DOLocalMoveAction(man08, new Vector3(44.01f, 0.1f, 39.01f), 0.5f))
            .Append(new DOLocalMoveAction(man08, new Vector3(44.01f, 0.1f, 37.68f), 0.5f))
            .Append(new DOLocalMoveAction(man08, new Vector3(50.39f, 0.1f, 37.68f), 1f))

            .Append(new DOLocalMoveAction(man09, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            .Append(new DOLocalMoveAction(man09, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            .Append(new DOLocalMoveAction(man09, new Vector3(41.76f, 0.1f, 39.01f), 3f))//
            .Append(new DOLocalMoveAction(man09, new Vector3(39.19f, 0.1f, 39.01f), 0.5f))
            .Append(new DOLocalMoveAction(man09, new Vector3(39.19f, 0.1f, 43.89f), 0.5f))
            .Append(new DOLocalMoveAction(man09, new Vector3(33.14f, 0.1f, 43.89f), 1f))

            .Append(new DOLocalMoveAction(man10, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            .Append(new DOLocalMoveAction(man10, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            .Append(new DOLocalMoveAction(man10, new Vector3(41.76f, 0.1f, 40.76f), 4f))//
            .Append(new DOLocalMoveAction(man10, new Vector3(53.76f, 0.1f, 40.76f), 2f))
            .Append(new DOLocalMoveAction(man10, new Vector3(53.76f, 0.1f, 43.81f), 0.5f))
            .Append(new DOLocalMoveAction(man10, new Vector3(44.18f, 0.1f, 43.81f), 1f))

            .Append(new DOLocalMoveAction(man11, new Vector3(31.55f, 0.1f, 13.36f), 0.5f))
            .Append(new DOLocalMoveAction(man11, new Vector3(41.76f, 0.1f, 13.36f), 1f))
            .Append(new DOLocalMoveAction(man11, new Vector3(41.76f, 0.1f, 40.76f), 4f))//
            .Append(new DOLocalMoveAction(man11, new Vector3(55f, 0.1f, 40.76f), 2f))
            .Append(new DOLocalMoveAction(man11, new Vector3(55f, 0.1f, 43.37f), 0.5f))
            .Append(new DOLocalMoveAction(man11, new Vector3(56.15f, 0.1f, 43.37f), 0.5f))
            .Append(new DOLocalMoveAction(man11, new Vector3(56.15f, 0.1f, 45.12f), 0.5f))
            .Append(new DOLocalMoveAction(man11, new Vector3(54.5f, 0.1f, 45.12f), 0.5f))

            .Append(new DelayedAction(2))
            .Append(new SetCameraPositionAction(new Vector3(28.53829f, 20, 36.7933f), 16f))
            .Append(new DOLocalMoveAction(man11, new Vector3(54.5f, 0.1f, 48.5f), 0.5f))
            .Append(new DOLocalMoveAction(man11, new Vector3(21.72f, 0.1f, 48.5f), 3f))
            .Append(new DOLocalMoveAction(man11, new Vector3(21.72f, 0.1f, 51.05f), 0.5f))//
            .Append(new DOLocalMoveAction(man10, new Vector3(51.19f, 0.1f, 43.81f), 0.5f))
            .Append(new DOLocalMoveAction(man10, new Vector3(51.19f, 0.1f, 48.5f), 0.5f))
            .Append(new DOLocalMoveAction(man10, new Vector3(21.72f, 0.1f, 48.5f), 3f))
            .Append(new DOLocalMoveAction(man10, new Vector3(21.72f, 0.1f, 51.05f), 0.5f))
            .Append(new DOLocalMoveAction(man09, new Vector3(23.7f, 0.1f, 43.89f), 1f))
            .Append(new DOLocalMoveAction(man09, new Vector3(23.7f, 0.1f, 38.68f), 0.5f))
            .Append(new DOLocalMoveAction(man09, new Vector3(19.97f, 0.1f, 38.68f), 0.5f))
            .Append(new DOLocalMoveAction(man09, new Vector3(19.97f, 0.1f, 48.24f), 1f))
            .Append(new DOLocalMoveAction(man09, new Vector3(21.72f, 0.1f, 48.24f), 0.5f))
            .Append(new DOLocalMoveAction(man09, new Vector3(21.72f, 0.1f, 51.05f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(29.69f, 0.1f, 37.5f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(23.2f, 0.1f, 37.5f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(23.2f, 0.1f, 35.09f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(19.97f, 0.1f, 35.09f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(19.97f, 0.1f, 48.24f), 3f))
            .Append(new DOLocalMoveAction(man07, new Vector3(21.72f, 0.1f, 48.24f), 0.5f))
            .Append(new DOLocalMoveAction(man07, new Vector3(21.72f, 0.1f, 51.05f), 0.5f))
            .Append(new DOLocalMoveAction(man05, new Vector3(21.72f, 0.1f, 31.68f), 0.5f))
            .Append(new DOLocalMoveAction(man05, new Vector3(21.72f, 0.1f, 24.58f), 0.5f))
            .Append(new DOLocalMoveAction(man05, new Vector3(19.97f, 0.1f, 24.58f), 0.5f))
            .Append(new DOLocalMoveAction(man05, new Vector3(19.97f, 0.1f, 48.24f), 3f))
            .Append(new DOLocalMoveAction(man05, new Vector3(21.72f, 0.1f, 48.24f), 0.5f))
            .Append(new DOLocalMoveAction(man05, new Vector3(21.72f, 0.1f, 51.05f), 0.5f))
            .Append(new DOLocalMoveAction(man04, new Vector3(32f, 0.1f, 23.39f), 0.5f))
            .Append(new DOLocalMoveAction(man04, new Vector3(22.72f, 0.1f, 23.39f), 1f))
            .Append(new DOLocalMoveAction(man04, new Vector3(22.72f, 0.1f, 21.78f), 0.5f))
            .Append(new DOLocalMoveAction(man04, new Vector3(19.97f, 0.1f, 21.78f), 0.5f))
            .Append(new DOLocalMoveAction(man04, new Vector3(19.97f, 0.1f, 48.24f), 3f))
            .Append(new DOLocalMoveAction(man04, new Vector3(21.72f, 0.1f, 48.24f), 0.5f))
            .Append(new DOLocalMoveAction(man04, new Vector3(21.72f, 0.1f, 51.05f), 0.5f))

            .Append(new SetCameraPositionAction(new Vector3(49.10769f, 20, 20.59664f), 14f))
            .Append(new DOLocalMoveAction(man08, new Vector3(52.82f, 0.1f, 37.68f), 0.5f))
            .Append(new DOLocalMoveAction(man08, new Vector3(52.82f, 0.1f, 31.21f), 0.5f))
            .Append(new DOLocalMoveAction(man08, new Vector3(55.4f, 0.1f, 31.21f), 0.5f))
            .Append(new DOLocalMoveAction(man08, new Vector3(55.4f, 0.1f, 9.19f), 3f))
            .Append(new DOLocalMoveAction(man08, new Vector3(51.98f, 0.1f, 9.19f), 0.5f))
            .Append(new DOLocalMoveAction(man06, new Vector3(48.3f, 0.1f, 29.89f), 0.5f))
            .Append(new DOLocalMoveAction(man06, new Vector3(53.32f, 0.1f, 29.89f), 0.5f))
            .Append(new DOLocalMoveAction(man06, new Vector3(53.32f, 0.1f, 27.38f), 0.5f))
            .Append(new DOLocalMoveAction(man06, new Vector3(55.4f, 0.1f, 27.38f), 0.5f))
            .Append(new DOLocalMoveAction(man06, new Vector3(55.4f, 0.1f, 9.19f), 3f))
            .Append(new DOLocalMoveAction(man06, new Vector3(51.98f, 0.1f, 9.19f), 0.5f))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "3-4");
            })
            .Execute();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

