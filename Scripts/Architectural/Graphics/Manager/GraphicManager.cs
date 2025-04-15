using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.UI;

namespace XFramework.Architectural
{
    public partial class GraphicManager : Singleton<GraphicManager>, IUpdate
    {
        private GameObject m_RootNode;

        private GameObject m_2DNode;

        private GameObject m_3DNode;

        private GameObject m_BothNode;

        private Dictionary<string, GameObject> m_GraphicIndexSet = new Dictionary<string, GameObject>();

        /// <summary>
        /// 对象池释放后，放置的节点
        /// </summary>
        private GameObject m_Unused;

        private PrefabList m_PrefabList;
        /// <summary>
        /// 预制体列表
        /// </summary>
        public PrefabList PrefabList
        {
            get 
            {
                if (m_PrefabList == null)
                {
                    m_PrefabList = GameObject.FindObjectOfType<PrefabList>();
                }
                return m_PrefabList; 
            }
        }


        protected override void Init()
        {
            base.Init();
            MonoDriver.Attach(this);

            m_RootNode = new GameObject("Architect");
            m_Unused = new GameObject("unused base");
            m_Unused.transform.SetParent(m_RootNode.transform);

            m_2DNode = new GameObject("2D");
            m_2DNode.transform.SetParent(m_RootNode.transform);

            m_3DNode = new GameObject("3D");
            m_3DNode.transform.SetParent(m_RootNode.transform);

            m_BothNode = new GameObject("Both");
            m_BothNode.transform.SetParent(m_RootNode.transform);


        }

        public override void Release()
        {
            base.Release();
            MonoDriver.Detach(this);
        }

        public void Update()
        {
            PreformUpdate();
        }

        private void PreformUpdate()
        {
            // clean
            CleanInvalidGraphics();
            // update
            for (int i = 0; i < m_GraphicRebuildQueue.Count; i++)
            {
                try
                {
                    var component = m_GraphicRebuildQueue[i];
                    component.Rebuild();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                
            }
            m_GraphicRebuildQueue.Clear();
        }

        public Transform GetSpecialNode(GameObject node, string special)
        {
            if (string.IsNullOrEmpty(special))
            {
                special = "辅助";
            }

            Transform trans = node.transform.Find(special);
            if (trans == null)
            {
                GameObject go = new GameObject(special);
                trans = go.transform;
                trans.transform.SetParent(node.transform);
            }
            return trans;
        }

        private void PlaceGraphic2D(GameObject go, string special)
        {
            Transform specialNode = GetSpecialNode(m_2DNode, special);
            PlaceGraphic(go, specialNode.gameObject);
        }

        private void PlaceGraphic3D(GameObject go, string special)
        {
            Transform specialNode = GetSpecialNode(m_3DNode, special);
            PlaceGraphic(go, specialNode.gameObject);
        }

        private void PlaceGraphicBoth(GameObject go, string special)
        {
            Transform specialNode = GetSpecialNode(m_BothNode, special);
            PlaceGraphic(go, specialNode.gameObject);
        }

        private void PlaceGraphic(GameObject go, GameObject node)
        {
            SetParent(go, node);
            m_GraphicIndexSet.Add(go.name, go);
        }

        private bool TryGetGameobject(string key, out GameObject go)
        {
            return m_GraphicIndexSet.TryGetValue(key, out go);
        }

        /// <summary>
        /// 根据实体类型，获取名称。
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public string GetName(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Corner:
                    return "Corner";
                case EntityType.Wall:
                    return "Wall";
                case EntityType.Room:
                    return "Room";
                case EntityType.Door:
                    return "Door";
                case EntityType.Window:
                    return "Window";
                case EntityType.Pass:
                    return "Pass";
                case EntityType.Group:
                    return "Group";
                case EntityType.AText:
                    return "Text";
                case EntityType.Area:
                    return "Area";
                case EntityType.Equipment:
                    return "Equipment";
                default:
                    return "";
            }
        }

        public List<GameObject> FindGameObjects(EntityObject entity)
        {
            if (entity == null)
                return null;

            List<GameObject> results = new List<GameObject>();
            // 构建名称
            string name = GetName(entity.Type);
            //  判断 3 种情况
            string key = name + "2D#" + entity.Id;
            if (TryGetGameobject(key, out GameObject go2))
            {
                results.Add(go2);
            }

            key = name + "3D#" + entity.Id;
            if (TryGetGameobject(key, out GameObject go3))
            {
                results.Add(go3);
            }

            key = name + "#" + entity.Id;
            if (TryGetGameobject(key, out GameObject go1))
            {
                results.Add(go1);
            }

            return results;
        }

        public bool TryGetGraphic2D(EntityObject entity, out GraphicObject graphicObject)
        {
            graphicObject = null;

            if (entity == null)
                return false;

            // 构建名称
            string name = GetName(entity.Type);
            //  判断 2 种情况
            string key = name + "2D#" + entity.Id;
            GameObject go;
            if (TryGetGameobject(key, out go))
            {
                graphicObject = go.GetComponent<GraphicObject>();
                if (graphicObject != null)
                {
                    return true;
                }
            }

            key = name + "#" + entity.Id;
            if (TryGetGameobject(key, out go))
            {
                graphicObject = go.GetComponent<GraphicObject>();
                if (graphicObject != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetGraphic3D(EntityObject entity, out GraphicObject graphicObject)
        {
            graphicObject = null;

            if (entity == null)
                return false;

            // 构建名称
            string name = GetName(entity.Type);
            //  判断 2 种情况
            string key = name + "3D#" + entity.Id;
            GameObject go;
            if (TryGetGameobject(key, out go))
            {
                graphicObject = go.GetComponent<GraphicObject>();
                if (graphicObject != null)
                {
                    return true;
                }
            }

            key = name + "#" + entity.Id;
            if (TryGetGameobject(key, out go))
            {
                graphicObject = go.GetComponent<GraphicObject>();
                if (graphicObject != null)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetParent(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform);
        }

        public void GetRaycasts(List<IGraphicRaycast> results)
        {
            if (results == null)
                return;
            m_RootNode.GetComponentsInChildren<IGraphicRaycast>(results);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show(ViewMode mode)
        {
            switch (mode)
            {
                case ViewMode.None:
                    break;
                case ViewMode.Drawing:
                    m_2DNode.gameObject.SetActive(true);
                    m_3DNode.gameObject.SetActive(false);
                    break;
                case ViewMode.Facade:
                    m_2DNode.gameObject.SetActive(false);
                    m_3DNode.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
