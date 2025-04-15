using UnityEngine;
namespace XFramework.Editor
{
    using Component;
    using UnityEditor;
    /// <summary>
    /// 
    /// </summary>
	public class AssembleTheDoor : Editor
    {
        [MenuItem("Tools/AssembleTheDoor #&D")]
        static void Assemble()
        {


            Transform[] door = Selection.GetTransforms(SelectionMode.TopLevel|SelectionMode.ExcludePrefab);
            if (door.Length > 2 || door.Length == 0)
            {
                Debug.LogError("只能选择两个物体");
                return;
            }
            GameObject _parent = door[0].parent.gameObject;
            int endValue = door[0].name.IndexOf("门");
            string _namePart = door[0].name.Substring(0,endValue-1);

            GameObject Parent;

            if (!_parent.name.Contains(_namePart))
            {
                MonoBehaviour.print("parent:"+_parent.name+"  Part:"+_namePart+" end:"+endValue);
                Parent = new GameObject();
                Parent.transform.parent = _parent.transform;
                Parent.name = _namePart+"门";
            }
            else
            {
                Parent = _parent;
            }
            
            ///添加脚本
            for (int i = 0; i < door.Length; i++)
            {
                door[i].transform.parent = Parent.transform;
                if(door[i].GetComponent<OnPointerComponent>()==null)
                {
                    door[i].gameObject.AddComponent<OnPointerComponent>();
                }
                if (door[i].GetComponent<BoxCollider>() == null)
                {
                    door[i].gameObject.AddComponent<BoxCollider>();
                }
            }

            if (Parent.GetComponent<DoorComponent>() == null)
            {
                DoorComponent doorCom = Parent.gameObject.AddComponent<DoorComponent>();
                ///双扇门
                if (Parent.transform.childCount==2)
                {
                    doorCom.m_Left = Parent.gameObject.transform.GetChild(0);
                    doorCom.m_Right = Parent.gameObject.transform.GetChild(1);
                    doorCom.CatchToolTip = "门";
                    doorCom.m_PointerComponents = new OnPointerComponent[2] { door[0].GetComponent<OnPointerComponent>(), door[1].GetComponent<OnPointerComponent>() };
                    doorCom.Disable = false;
                }
                ////单扇门
                else if(Parent.transform.childCount == 1)
                {
          
                    ///默认为右扇门
                    doorCom.m_Right = Parent.gameObject.transform.GetChild(0);
                    doorCom.CatchToolTip = "门";
                    doorCom.m_PointerComponents = new OnPointerComponent[1] { door[0].GetComponent<OnPointerComponent>()};
                    doorCom.Disable = false;
                }
                
            }
           

        }

        [MenuItem("Tools/AddGoodPart")]
        static void AddGoodPart()
        {
            //Transform[] goods = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
            Transform[] goods = Selection.transforms;
            Transform first = Selection.activeTransform;
            if (goods.Length == 2)
            {
                //goods[1].parent = goods[0];
                //Debug.Log("1"+goods[0]+ "  id:"+goods[0].GetInstanceID()+ " 2:"+goods[1]+"  "+ goods[1].GetInstanceID());
            }
        }
     
    }
}
