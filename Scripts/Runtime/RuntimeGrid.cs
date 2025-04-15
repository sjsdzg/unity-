using System;
using UnityEngine;
using System.Collections;

namespace XFramework.Runtime
{
    [RequireComponent(typeof(Camera))]
    public class RuntimeGrid : MonoBehaviour
    {
        private Material m_GridMaterial;

        private Camera m_Camera;

        [SerializeField]
        public Color32 xColor = new Color32(98, 37, 39, 255);

        [SerializeField]
        public Color32 yColor = new Color32(34, 99, 39, 255);

        [SerializeField]
        private Color m_GridColor = new Color32(57, 63, 82, 255);

        private float m_Spacing = 0.5f;

        private int m_GridSize = 1000;

        private Mesh m_GridMesh;

        private void Awake()
        {
            m_Camera = GetComponent<Camera>();

            m_GridMaterial = new Material(Shader.Find("Internal/Grid"));
            m_GridMaterial.color = Color.white;
            m_GridMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
            //m_GridMaterial.enableInstancing = true;
            //m_GridMaterial.SetFloat("_FadeDistance", 100);
        }

        void Start()
        {
            m_GridMesh = CreateGridMesh();
        }

        //private void OnPostRender()
        //{
        //    float h = m_Camera.transform.position.y;
        //    if (m_Camera.orthographic)
        //    {
        //        h = m_Camera.orthographicSize;
        //    }
            
        //    h = Mathf.Abs(h);
        //    h = Mathf.Max(0, h);

        //    float d = Mathf.Ceil(Mathf.Log10(h));

        //    float spacing = Mathf.Pow(10, d - 1);
        //    float nextSpacing = Mathf.Pow(10, d);
        //    float alpha = 1.0f - (h - spacing) / (nextSpacing - spacing);
        //    // 绘制网格
        //    DrawGrid(m_Camera.transform.position, spacing, alpha, h * 20);
        //}

        private void DrawGrid(Vector3 position, float spacing, float alpha, float fadeDisance)
        {
            m_GridMaterial.SetFloat("_FadeDistance", fadeDisance);
            m_GridMaterial.SetPass(0);

            Vector3 offset = Vector3.zero;
            offset.x = Mathf.Floor(position.x / spacing);
            offset.y = 0;
            offset.z = Mathf.Floor(position.z / spacing);

            GL.PushMatrix();
            GL.Begin(GL.LINES);
            for (int i = -150; i < 150; ++i)
            {
                Color color = m_GridColor;
                if ((i + offset.x) % 10 != 0)
                {
                    color.a *= alpha;
                }

                GL.Color(color);
                GL.Vertex((offset + new Vector3(i, 0, -150)) * spacing);
                GL.Vertex((offset + new Vector3(i, 0, 150)) * spacing);

                color = m_GridColor;
                if ((i + offset.z) % 10 != 0)
                {
                    color.a *= alpha;
                }

                GL.Color(color);
                GL.Vertex((offset + new Vector3(-150, 0, i)) * spacing);
                GL.Vertex((offset + new Vector3(150, 0, i)) * spacing);
            }
            GL.End();
            GL.PopMatrix();
        }

        private float h;
        void Update()
        {
            float h = m_Camera.transform.position.y;
            if (m_Camera.orthographic)
            {
                h = m_Camera.orthographicSize;
            }
            h = Mathf.Abs(h);
            h = Mathf.Max(0, h);

            m_GridMaterial.SetFloat("_FadeDistance", 20 * h);

            UnityEngine.Graphics.DrawMesh(m_GridMesh, Matrix4x4.identity, m_GridMaterial, 0);
        }

        private Mesh CreateGridMesh()
        {
            float half = m_GridSize / 2;
            m_GridSize++;
            Vector3[] vextices = new Vector3[m_GridSize * 4];
            int[] indices = new int[m_GridSize * 4];
            Color[] colors = new Color[m_GridSize * 4];

            int index = 0;
            Color color = m_GridColor;
            for (int i = 0; i < m_GridSize; i++)
            {
                if (i == half)
                    continue;

                if (i % 10 == 0)
                {
                    color = m_GridColor;
                }
                else
                {
                    color = m_GridColor;
                    color.a *= 0.25f;
                }

                indices[index] = index;
                colors[index] = i == half ? Color.blue : color;
                vextices[index++] = new Vector3(i - half, 0, -half) * m_Spacing;

                indices[index] = index;
                colors[index] = i == half ? Color.blue : color;
                vextices[index++] = new Vector3(i - half, 0, half) * m_Spacing;

                indices[index] = index;
                colors[index] = i == half ? Color.red : color;
                vextices[index++] = new Vector3(-half, 0, i - half) * m_Spacing;

                indices[index] = index;
                colors[index] = i == half ? Color.red : color;
                vextices[index++] = new Vector3(half, 0, i - half) * m_Spacing;
            }

            // 绘制y轴
            indices[index] = index;
            colors[index] = yColor;
            vextices[index++] = new Vector3(0, 0, -half) * m_Spacing;

            indices[index] = index;
            colors[index] = yColor;
            vextices[index++] = new Vector3(0, 0, half) * m_Spacing;

            // 绘制x轴
            indices[index] = index;
            colors[index] = xColor;
            vextices[index++] = new Vector3(-half, 0, 0) * m_Spacing;

            indices[index] = index;
            colors[index] = xColor;
            vextices[index++] = new Vector3(half, 0, 0) * m_Spacing;

            // 网格
            Mesh mesh = new Mesh();
            mesh.vertices = vextices;
            mesh.colors = colors;
            mesh.SetIndices(indices, MeshTopology.Lines, 0);

            return mesh;
        }
    }
}

