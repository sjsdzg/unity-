using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.UIWidgets
{
    public class InspectorManager : Singleton<InspectorManager>
    {
        /// <summary>
        /// 对象池
        /// </summary>
        private Dictionary<string, GameObjectPool> m_Pools = new Dictionary<string, GameObjectPool>();

        /// <summary>
        /// Editor Map
        /// </summary>
        private Dictionary<string, EditorBase> m_EditorMap = new Dictionary<string, EditorBase>();

        /// <summary>
        /// Editor Map
        /// </summary>
        private Dictionary<string, FieldBase> m_FieldMap = new Dictionary<string, FieldBase>();

        /// <summary>
        /// Control Map
        /// </summary>
        private Dictionary<string, ControlBase> m_ControlMap = new Dictionary<string, ControlBase>();

        private InspectorView m_View;
        /// <summary>
        /// 属性窗口
        /// </summary>
        public InspectorView View
        {
            get
            {
                if (m_View == null)
                {
                    m_View = GameObject.FindObjectOfType<InspectorView>();
                }

                return m_View; 
            }
        }

        protected override void Init()
        {
            base.Init();
            var settings = View.Settings;

            foreach (var field in settings.DefaultFields)
            {
                m_FieldMap.Add(field.GetKey(), field);
            }

            foreach (var editor in settings.DefaultEditors)
            {
                m_EditorMap.Add(editor.GetKey(), editor);
            }

            foreach (var control in settings.DefaultControls)
            {
                m_ControlMap.Add(control.GetKey(), control);
            }
        }

        public bool HasPool(string key)
        {
            if (m_Pools.ContainsKey(key))
                return true;
            else
                return false;
        }

        public GameObjectPool AddPool(string key, GameObject template)
        {
            if (!m_Pools.TryGetValue(key, out GameObjectPool pool))
            {
                pool = new GameObjectPool(View.Unused, template);
                m_Pools.Add(key, pool);
            }

            return pool;
        }

        public GameObjectPool GetPool(string key)
        {
            m_Pools.TryGetValue(key, out GameObjectPool pool);
            return pool;
        }

        public EditorBase CreateEditor(Type targetType)
        {
            string key = targetType.ToString();
            //if (m_EditorMap.TryGetValue(key, out EditorBase editor))
            //{
            //    // pool
            //    GameObjectPool pool = AddPool(key, editor.gameObject);
            //    // create
            //    GameObject go = pool.Spawn();
            //    // return
            //    return go.GetComponent<EditorBase>();
            //}
            //else if (m_EditorScriptMap.TryGetValue(key, out Type editorScript))
            //{
            //    // pool
            //    GameObjectPool pool;
            //    if (!HasPool(key))
            //    {
            //        GameObject template = new GameObject(editorScript.Name);
            //        template.AddComponent<RectTransform>();
            //        var layout = template.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
            //        layout.childControlWidth = true;
            //        layout.childControlHeight = true;
            //        layout.childForceExpandWidth = true;
            //        layout.childForceExpandHeight = true;
            //        template.AddComponent(editorScript);
            //        pool = AddPool(key, template);
            //    }
            //    else
            //    {
            //        pool = GetPool(key);
            //    }
            //    // create
            //    GameObject go = pool.Spawn();
            //    return go.GetComponent<EditorBase>();
            //}

            if (!m_EditorMap.TryGetValue(key, out EditorBase editor))
                return null;

            // pool
            GameObjectPool pool = AddPool(key, editor.gameObject);
            // create
            GameObject go = pool.Spawn();
            // return
            return go.GetComponent<EditorBase>();
        }

        //public GameObject CreateEditor

        public FieldBase CreateField<T>(string token = "")
        {
            string key = typeof(T).ToString() + token;
            if (!m_FieldMap.TryGetValue(key, out FieldBase field))
                return null;

            // pool
            GameObjectPool pool = AddPool(key, field.gameObject);
            // create
            GameObject go = pool.Spawn();
            // return
            return go.GetComponent<FieldBase>();
        }

        public T CreateControl<T>() where T : ControlBase
        {
            string key = typeof(T).ToString();
            if (!m_ControlMap.TryGetValue(key, out ControlBase control))
                return null;

            // pool
            GameObjectPool pool = AddPool(key, control.gameObject);
            // create
            GameObject go = pool.Spawn();
            // return
            return go.GetComponent<T>();
        }

        public void Destory(InspectorBehaviour behaviour)
        {
            string key = behaviour.GetKey();
            // pool
            GameObjectPool pool = GetPool(key);
            // destory
            pool.Despawn(behaviour.gameObject);
        }

        public void RegisterEditor<T>() where T : EditorBase
        {
            Type type = typeof(T);

            GameObject go = new GameObject(type.Name);
            go.AddComponent<RectTransform>();
            var layout = go.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;
            EditorBase editor = (EditorBase)go.AddComponent(type);
            RectTransformUtils.SetParentAndAlign(editor.gameObject, View.Unused.gameObject);

            if (!m_EditorMap.ContainsKey(editor.GetKey()))
            {
                m_EditorMap.Add(editor.GetKey(), editor);
            }
        }
    }
}
