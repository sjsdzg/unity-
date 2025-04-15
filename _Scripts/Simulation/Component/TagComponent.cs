using System.Collections.Generic;
using UnityEngine;
using XFramework.Simulation;

namespace XFramework.Component
{
    /// <summary>
    /// 标签组件
    /// </summary>    
    public class TagComponent : ComponentBase
    {
        private readonly string path = "Icons/Tag";

        /// <summary>
        /// 标签列表
        /// </summary>
        private Dictionary<string, Texture> m_Textures = new Dictionary<string, Texture>();

        [SerializeField]
        private TagState state = TagState.None;
        /// <summary>
        /// 标签状态
        /// </summary>
        public TagState State
        {
            get { return state; }
            set
            {
                if (state == value)
                    return;

                state = value;
                OnChangeState();
            }
        }

        private void Awake()
        {
            Texture[] array = Resources.LoadAll<Texture>(path);
            for (int i = 0; i < array.Length; i++)
            {
                Texture texture = array[i];
                m_Textures.Add(texture.name, texture);
            }
            OnChangeState();
        }

        /// <summary>
        /// 改变标签状态
        /// </summary>
        /// <param name="_state"></param>
        public virtual void OnChangeState()
        {
            Texture texture = m_Textures[state.ToString()];
            transform.GetComponent<Renderer>().material.mainTexture = texture;
        }
    }

    public enum TagState
    {
        None,
        /// <summary>
        /// 备用完好
        /// </summary>
        Inactive,
        /// <summary>
        /// 待用
        /// </summary>
        Standby,
        /// <summary>
        /// 已清洁
        /// </summary>
        Cleaned,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
    }
}
