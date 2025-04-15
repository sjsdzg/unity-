using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Math
{
    public partial class MeshBuilder : IDisposable
    {
        //public enum FillMode
        //{
        //    /// <summary>
        //    /// 新建
        //    /// </summary>
        //    New,
        //    /// <summary>
        //    /// 附加
        //    /// </summary>
        //    Append,
        //}

        private List<Vector3> m_Positions = ListPool<Vector3>.Get();
        private List<Color32> m_Colors = ListPool<Color32>.Get();
        private List<Vector2> m_Uv0S = ListPool<Vector2>.Get();
        private List<Vector2> m_Uv1S = ListPool<Vector2>.Get();
        private List<Vector2> m_Uv2S = ListPool<Vector2>.Get();
        private List<Vector2> m_Uv3S = ListPool<Vector2>.Get();
        private List<Vector3> m_Normals = ListPool<Vector3>.Get();
        private List<Vector4> m_Tangents = ListPool<Vector4>.Get();
        private List<int> m_Indices = ListPool<int>.Get();

        private static readonly Vector4 s_DefaultTanget = new Vector4(1.0f, 0.0f, 0.0f, -1.0f);
        private static readonly Vector3 s_DefaultNormal = Vector3.back;

        private MeshTopology meshTopology = MeshTopology.Lines;
        /// <summary>
        /// 网格拓扑
        /// </summary>
        public MeshTopology MeshTopology
        {
            get { return meshTopology; }
            set { meshTopology = value; }
        }

        public MeshBuilder()
        {

        }

        public MeshBuilder(Mesh m)
        {
            m_Positions.AddRange(m.vertices);
            m_Colors.AddRange(m.colors32);
            m_Uv0S.AddRange(m.uv);
            m_Uv1S.AddRange(m.uv2);
            m_Uv2S.AddRange(m.uv3);
            m_Uv3S.AddRange(m.uv4);
            m_Normals.AddRange(m.normals);
            m_Tangents.AddRange(m.tangents);
            m_Indices.AddRange(m.GetIndices(0));
        }

        public void Clear()
        {
            m_Positions.Clear();
            m_Colors.Clear();
            m_Uv0S.Clear();
            m_Uv1S.Clear();
            m_Uv2S.Clear();
            m_Uv3S.Clear();
            m_Normals.Clear();
            m_Tangents.Clear();
            m_Indices.Clear();
        }

        public void Dispose()
        {
            ListPool<Vector3>.Release(m_Positions);
            ListPool<Color32>.Release(m_Colors);
            ListPool<Vector2>.Release(m_Uv0S);
            ListPool<Vector2>.Release(m_Uv1S);
            ListPool<Vector2>.Release(m_Uv2S);
            ListPool<Vector2>.Release(m_Uv3S);
            ListPool<Vector3>.Release(m_Normals);
            ListPool<int>.Release(m_Indices);

            m_Positions = null;
            m_Colors = null;
            m_Uv0S = null;
            m_Uv1S = null;
            m_Uv2S = null;
            m_Uv3S = null;
            m_Normals = null;
            m_Tangents = null;
            m_Indices = null;
        }

        /// <summary>
        /// 当前顶点数
        /// </summary>
        public int CurrentVertCount
        {
            get { return m_Positions.Count; }
        }

        /// <summary>
        /// 当前三角索引数
        /// </summary>
        public int CurrentIndexCount
        {
            get { return m_Indices.Count; }
        }

        public void PopulateVertex(ref Vertex vertex, int i)
        {
            vertex.position = m_Positions[i];
            vertex.color = m_Colors[i];
            vertex.uv0 = m_Uv0S[i];
            vertex.uv1 = m_Uv1S[i];
            vertex.uv2 = m_Uv2S[i];
            vertex.uv3 = m_Uv3S[i];
            vertex.normal = m_Normals[i];
            vertex.tangent = m_Tangents[i];
        }

        public void SetVertex(Vertex vertex, int i)
        {
            m_Positions[i] = vertex.position;
            m_Colors[i] = vertex.color;
            m_Uv0S[i] = vertex.uv0;
            m_Uv1S[i] = vertex.uv1;
            m_Uv2S[i] = vertex.uv2;
            m_Uv3S[i] = vertex.uv3;
            m_Normals[i] = vertex.normal;
            m_Tangents[i] = vertex.tangent;
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="color"></param>
        public void SetColors(int startIndex, int count, Color color)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                m_Colors[i] = color;
            }
        }

        public void SetColors(Color color)
        {
            for (int i = 0; i < CurrentVertCount; i++)
            {
                m_Colors[i] = color;
            }
        }

        public void AddVert(Vector3 position, Color32 color)
        {
            AddVert(position, color, Vector2.zero);
        }

        public void AddVert(Vector3 position, Color32 color, Vector2 uv0)
        {
            AddVert(position, color, uv0, s_DefaultNormal);
        }

        public void AddVert(Vector3 position, Color32 color, Vector2 uv0, Vector3 normal)
        {
            AddVert(position, color, uv0, Vector2.zero, normal, s_DefaultTanget);
        }

        public void AddVert(Vector3 position, Color32 color, Vector2 uv0, Vector2 uv1, Vector3 normal, Vector4 tangent)
        {
            m_Positions.Add(position);
            m_Colors.Add(color);
            m_Uv0S.Add(uv0);
            m_Uv1S.Add(uv1);
            m_Uv2S.Add(Vector2.zero);
            m_Uv3S.Add(Vector2.zero);
            m_Normals.Add(normal);
            m_Tangents.Add(tangent);
        }

        public void AddVert(Vertex vertex)
        {
            AddVert(vertex.position, vertex.color, vertex.uv0, vertex.uv1, vertex.normal, vertex.tangent);
        }

        /// <summary>
        /// point
        /// </summary>
        /// <param name="idx0"></param>
        /// <param name="idx1"></param>
        public void AddIndices(int idx)
        {
            m_Indices.Add(idx);
        }

        /// <summary>
        /// Line
        /// </summary>
        /// <param name="idx0"></param>
        /// <param name="idx1"></param>
        public void AddIndices(int idx0, int idx1)
        {
            m_Indices.Add(idx0);
            m_Indices.Add(idx1);
        }

        /// <summary>
        /// Triangle
        /// </summary>
        /// <param name="idx0"></param>
        /// <param name="idx1"></param>
        /// <param name="idx2"></param>
        public void AddIndices(int idx0, int idx1, int idx2)
        {
            m_Indices.Add(idx0);
            m_Indices.Add(idx1);
            m_Indices.Add(idx2);
        }

        /// <summary>
        /// Polygon
        /// </summary>
        /// <param name="idx0"></param>
        /// <param name="idx1"></param>
        /// <param name="idx2"></param>
        public void AddIndices(int[] indices)
        {
            m_Indices.AddRange(indices);
        }

        public void AddVertexQuad(Vertex[] verts)
        {
            int startIndex = CurrentVertCount;
            for (int i = 0; i < 4; i++)
            {
                AddVert(verts[i].position, verts[i].color, verts[i].uv0, verts[i].uv1, verts[i].normal, verts[i].tangent);
            }

            AddIndices(startIndex, startIndex + 1, startIndex + 2);
            AddIndices(startIndex + 2, startIndex + 3, startIndex);
        }

        /// <summary>
        /// 使用此方法，需要提前设置 MeshTopology 属性。
        /// </summary>
        /// <param name="mesh"></param>
        public void FillMesh(Mesh mesh)
        {
            FillMesh(mesh, MeshTopology);
        }

        public void FillMesh(Mesh mesh, MeshTopology topology)
        {
            mesh.Clear();

            if (m_Positions.Count >= 65000)
                throw new ArgumentException("Mesh can not have more than 65000 vertices");

            mesh.SetVertices(m_Positions);
            mesh.SetColors(m_Colors);
            mesh.SetUVs(0, m_Uv0S);
            mesh.SetUVs(1, m_Uv1S);
            mesh.SetUVs(2, m_Uv2S);
            mesh.SetUVs(3, m_Uv3S);
            mesh.SetNormals(m_Normals);
            mesh.SetTangents(m_Tangents);
            mesh.SetIndices(m_Indices.ToArray(), topology, 0);
        }

        //public void FillMesh(Mesh mesh, MeshTopology topology, int submesh, FillMode fillMode)
        //{
        //    switch (fillMode)
        //    {
        //        case FillMode.New:
        //            mesh.Clear();
        //            break;
        //        case FillMode.Append:
        //            m_Positions.InsertRange(0, mesh.vertices);
        //            m_Colors.InsertRange(0, mesh.colors32);
        //            m_Uv0S.InsertRange(0, mesh.uv);
        //            m_Uv1S.InsertRange(0, mesh.uv2);
        //            m_Uv2S.InsertRange(0, mesh.uv3);
        //            m_Uv3S.InsertRange(0, mesh.uv4);
        //            m_Normals.InsertRange(0, mesh.normals);
        //            m_Tangents.InsertRange(0, mesh.tangents);
        //            int start = mesh.vertices.Length;
        //            for (int i = 0; i < m_Indices.Count; i++)
        //            {
        //                m_Indices[i] += start;
        //            }
        //            m_Indices.InsertRange(0, mesh.GetIndices(submesh));
        //            break;
        //        default:
        //            break;
        //    }

        //    if (m_Positions.Count >= 65000)
        //        throw new ArgumentException("Mesh can not have more than 65000 vertices");

        //    mesh.SetVertices(m_Positions);
        //    mesh.SetColors(m_Colors);
        //    mesh.SetUVs(0, m_Uv0S);
        //    mesh.SetUVs(1, m_Uv1S);
        //    mesh.SetUVs(2, m_Uv2S);
        //    mesh.SetUVs(3, m_Uv3S);
        //    mesh.SetNormals(m_Normals);
        //    mesh.SetTangents(m_Tangents);
        //    mesh.SetIndices(m_Indices.ToArray(), topology, submesh);
        //}
    }
}
