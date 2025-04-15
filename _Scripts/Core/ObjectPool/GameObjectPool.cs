using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public class GameObjectPool : UnityObjectPool<GameObject>
    {
        /// <summary>
        /// 对象池中的对象放置在此节点之下
        /// </summary>
        private Transform m_Unused;

        /// <summary>
        /// 模板
        /// </summary>
        private GameObject m_Template;

        public GameObjectPool(Transform unused) 
        {
            m_Unused = unused;
            m_Template = null;

            m_AllocFunc = OnAlloc;
            m_ActionOnSpawn = OnSpawn;
            m_ActionOnDespawn = OnDespawn;
        }

        public GameObjectPool(Transform unused, string path)
        {
            m_Unused = unused;
            m_Template = Resources.Load<GameObject>(path);

            m_AllocFunc = OnAlloc;
            m_ActionOnSpawn = OnSpawn;
            m_ActionOnDespawn = OnDespawn;
        }

        public GameObjectPool(Transform unused, GameObject template)
        {
            m_Unused = unused;
            m_Template = template;

            m_AllocFunc = OnAlloc;
            m_ActionOnSpawn = OnSpawn;
            m_ActionOnDespawn = OnDespawn;
        }

        public GameObjectPool(Func<GameObject> allocFunc, Action<GameObject> actionOnSpawn, Action<GameObject> actionOnDespawn) 
            : base(allocFunc, actionOnSpawn, actionOnDespawn)
        {
            
        }

        public GameObject OnAlloc()
        {
            GameObject go = null;

            if (m_Template == null)
                go = new GameObject();
            else
                go = UnityEngine.Object.Instantiate(m_Template);

            return go;
        }

        public void OnSpawn(GameObject go)
        {
            go.SetActive(true);
        }

        public void OnDespawn(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(m_Unused.transform, false);
        }
    }
}
