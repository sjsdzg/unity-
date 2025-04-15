using UnityEngine;
using System.Collections;
using XFramework.Math;

namespace XFramework.Architectural
{
    public abstract class GraphicObject : MonoBehaviour, IGraphicObject
    {
        private bool m_ActiveDirty;
        private bool m_NameDirty;
        private bool m_TransformDirty;
        private bool m_VertsDirty;
        private bool m_MaterialDirty;

        /// <summary>
        /// 所属
        /// </summary>
        public GraphicObject Owner { get; set; }

        public virtual void SetAllDirty()
        {
            SetActiveDirty();
            SetNameDirty();
            SetTranformDirty();
            SetVerticesDirty();
            SetMaterialDirty();
        }

        public virtual void SetActiveDirty()
        {
            m_ActiveDirty = true;
            GraphicManager.RegisterGraphicForRebuild(this);
        }

        public virtual void SetNameDirty()
        {
            m_NameDirty = true;
            GraphicManager.RegisterGraphicForRebuild(this);
        }

        public virtual void SetTranformDirty()
        {
            m_TransformDirty = true;
            GraphicManager.RegisterGraphicForRebuild(this);
        }

        public virtual void SetVerticesDirty()
        {
            m_VertsDirty = true;
            GraphicManager.RegisterGraphicForRebuild(this);
        }

        public virtual void SetMaterialDirty()
        {
            m_MaterialDirty = true;
            GraphicManager.RegisterGraphicForRebuild(this);
        }


        protected virtual void OnEnable()
        {
            SetAllDirty();
        }

        protected virtual void UpdateActive()
        {

        }

        protected virtual void UpdateName()
        {

        }

        protected virtual void UpdateTransform()
        {

        }

        protected virtual void UpdateGeometry()
        {

        }

        protected virtual void UpdateMaterial()
        {

        }

        public virtual void Rebuild()
        {
            if (m_ActiveDirty)
            {
                UpdateActive();
                m_ActiveDirty = false;
            }

            if (m_NameDirty)
            {
                UpdateName();
                m_NameDirty = true;
            }

            if (m_TransformDirty)
            {
                UpdateTransform();
                m_TransformDirty = false;
            }
            if (m_VertsDirty)
            {
                UpdateGeometry();
                m_VertsDirty = false;
            }
            if (m_MaterialDirty)
            {
                UpdateMaterial();
                m_MaterialDirty = false;
            }
        }

        public virtual void Reset()
        {

        }
    }
}

