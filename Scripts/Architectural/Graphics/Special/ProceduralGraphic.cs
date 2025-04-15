using UnityEngine;
using System.Collections;
using XFramework.Math;

namespace XFramework.Architectural
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class ProceduralGraphic : SelectableGraphic
    {
        static protected Material s_DefaultMaterial = null;
        static protected Texture2D s_WhiteTexture = null;
        static public Material DefaultMaterial
        {
            get
            {
                if (s_DefaultMaterial == null)
                {
                    s_DefaultMaterial = new Material(Shader.Find("Architect/Geometry"));
                }

                s_DefaultMaterial.enableInstancing = true;
                return s_DefaultMaterial;
            }
        }

        protected Material m_Material;
        /// <summary>
        /// 材质
        /// </summary>
        public virtual Material Material
        {
            get
            {
                Debug.LogError("111233334");
                return (m_Material != null) ? m_Material : DefaultMaterial;
            }
            set
            {
                if (m_Material == value)
                    return;

                m_Material = value;
                SetMaterialDirty();
            }
        }


        private Texture2D mainTexture;
        /// <summary>
        /// 贴图
        /// </summary>
        public virtual Texture2D MainTexture
        {
            get { return (mainTexture != null) ? mainTexture : s_WhiteTexture; }
            set
            {
                if (mainTexture == value)
                    return;

                mainTexture = value;
                SetMaterialDirty();
            }
        }


        [SerializeField]
        private Color m_Color = Color.white;
        /// <summary>
        /// 顶点颜色
        /// </summary>
        public virtual Color Color
        {
            get
            {
                Debug.LogError("sadsadsa");

                return m_Color;
            }
            set
            {
                if (m_Color == value)
                    return;

                m_Color = value;
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// mesh
        /// </summary>
        private Mesh workerMesh;

        private MeshFilter meshFilter;
        /// <summary>
        /// 网格过滤器
        /// </summary>
        public MeshFilter MeshFilter
        {
            get
            {
                if (meshFilter == null)
                    meshFilter = GetComponent<MeshFilter>();

                return meshFilter;
            }
        }

        private MeshRenderer meshRenderer;
        /// <summary>
        /// 网格渲染器
        /// </summary>
        public MeshRenderer MeshRenderer
        {
            get
            {
                if (meshRenderer == null)
                    meshRenderer = GetComponent<MeshRenderer>();

                return meshRenderer;
            }
        }

        /// <summary>
        /// s_VertexHelper
        /// </summary>
        private static readonly MeshBuilder s_VertexHelper = new MeshBuilder();

        protected override void OnEnable()
        {
            if (s_WhiteTexture == null)
                s_WhiteTexture = Texture2D.whiteTexture;

            if (workerMesh == null)
                workerMesh = new Mesh();

            SetAllDirty();
        }

        protected override void UpdateTransform()
        {

        }

        protected override void UpdateGeometry()
        {
            OnPopulateMesh(s_VertexHelper);
            s_VertexHelper.FillMesh(workerMesh);
            MeshFilter.mesh = workerMesh;
        }

        protected override void UpdateMaterial()
        {
            MeshRenderer.sharedMaterial = Material;
            MeshRenderer.sharedMaterial.mainTexture = MainTexture;
        }

        protected virtual void OnPopulateMesh(MeshBuilder vh)
        {
            vh.Clear();
            //Color32 color32 = Color;
            //vh.MeshTopology = MeshTopology.Lines;

            //vh.AddVert(new Vector3(0, 0, 0), color32);
            //vh.AddVert(new Vector3(0, 1, 0), color32);
            //vh.AddVert(new Vector3(1, 1, 0), color32);
            //vh.AddVert(new Vector3(1, 0, 0), color32);

            //vh.AddIndices(0, 1);
            //vh.AddIndices(1, 2);
            //vh.AddIndices(2, 3);
            //vh.AddIndices(3, 0);
        }
    }
}

