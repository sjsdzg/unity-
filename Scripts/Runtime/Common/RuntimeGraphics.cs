using UnityEngine;
using XFramework.Core;

namespace XFramework.Runtime
{

    public class RuntimeGraphics
    {
        private static readonly Matrix4x4 s_Matrix = Matrix4x4.identity;
        private static readonly Material s_LinesMaterial;
        private static readonly int s_WireArcPointsCount = 60;
        private static Vector3[] s_RectanglePointsCache = new Vector3[4];
        private static Vector3[] s_CubePointsCache = new Vector3[8];

        static RuntimeGraphics()
        {
            s_LinesMaterial = new Material(Shader.Find("Internal/Color"));

        }

        /// <summary>
        /// 绘制线段
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="color"></param>
        public static void DrawLineGL(Matrix4x4 matrix, Vector3 from, Vector3 to, Color color)
        {
            // set pass
            s_LinesMaterial.SetPass(0);
            // push matrix
            GL.PushMatrix();
            GL.MultMatrix(matrix);
            // begin drawing
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(from);
            GL.Vertex(to);
            GL.End();
            GL.PopMatrix();
        }
       
        public static void DrawDottedLineGL(Matrix4x4 matrix, Vector3 from, Vector3 to, Color color)
        {
            //Handles
        }

        /// <summary>
        /// 绘制线框矩形
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="size"></param>
        public static void DrawWireRectGL(Matrix4x4 matrix, Alignment alignment, Vector2 size, Color color)
        {
            switch (alignment)
            {
                case Alignment.UpperLeft:
                    s_RectanglePointsCache[0] = new Vector3(0, -size.y, 0);
                    s_RectanglePointsCache[1] = new Vector3(0, 0, 0);
                    s_RectanglePointsCache[2] = new Vector3(size.x, 0, 0);
                    s_RectanglePointsCache[3] = new Vector3(size.x, -size.y, 0);
                    break;
                case Alignment.UpperCenter:
                    s_RectanglePointsCache[0] = new Vector3(-size.x * 0.5f, -size.y, 0);
                    s_RectanglePointsCache[1] = new Vector3(-size.x * 0.5f, 0, 0);
                    s_RectanglePointsCache[2] = new Vector3(size.x * 0.5f, 0, 0);
                    s_RectanglePointsCache[3] = new Vector3(size.x * 0.5f, -size.y, 0);
                    break;
                case Alignment.UpperRight:
                    s_RectanglePointsCache[0] = new Vector3(-size.x, -size.y, 0);
                    s_RectanglePointsCache[1] = new Vector3(-size.x, 0, 0);
                    s_RectanglePointsCache[2] = new Vector3(0, 0, 0);
                    s_RectanglePointsCache[3] = new Vector3(0, -size.y, 0);
                    break;
                case Alignment.MiddleLeft:
                    s_RectanglePointsCache[0] = new Vector3(0, -size.y * 0.5f, 0);
                    s_RectanglePointsCache[1] = new Vector3(0, size.y * 0.5f, 0);
                    s_RectanglePointsCache[2] = new Vector3(size.x, size.y * 0.5f, 0);
                    s_RectanglePointsCache[3] = new Vector3(size.x, -size.y * 0.5f, 0);
                    break;
                case Alignment.MiddleCenter:
                    s_RectanglePointsCache[0] = new Vector3(-size.x * 0.5f, -size.y * 0.5f, 0);
                    s_RectanglePointsCache[1] = new Vector3(-size.x * 0.5f, size.y * 0.5f, 0);
                    s_RectanglePointsCache[2] = new Vector3(size.x * 0.5f, size.y * 0.5f, 0);
                    s_RectanglePointsCache[3] = new Vector3(size.x * 0.5f, -size.y * 0.5f, 0);
                    break;
                case Alignment.MiddleRight:
                    s_RectanglePointsCache[0] = new Vector3(-size.x, -size.y * 0.5f, 0);
                    s_RectanglePointsCache[1] = new Vector3(-size.x, size.y * 0.5f, 0);
                    s_RectanglePointsCache[2] = new Vector3(0, size.y * 0.5f, 0);
                    s_RectanglePointsCache[3] = new Vector3(0, -size.y * 0.5f, 0);
                    break;
                case Alignment.LowerLeft:
                    s_RectanglePointsCache[0] = new Vector3(0, 0, 0);
                    s_RectanglePointsCache[1] = new Vector3(0, size.y, 0);
                    s_RectanglePointsCache[2] = new Vector3(size.x, size.y, 0);
                    s_RectanglePointsCache[3] = new Vector3(size.x, 0, 0);
                    break;
                case Alignment.LowerCenter:
                    s_RectanglePointsCache[0] = new Vector3(-size.x * 0.5f, 0, 0);
                    s_RectanglePointsCache[1] = new Vector3(-size.x * 0.5f, size.y, 0);
                    s_RectanglePointsCache[2] = new Vector3(size.x * 0.5f, size.y, 0);
                    s_RectanglePointsCache[3] = new Vector3(size.x * 0.5f, 0, 0);
                    break;
                case Alignment.LowerRight:
                    s_RectanglePointsCache[0] = new Vector3(-size.x, 0, 0);
                    s_RectanglePointsCache[1] = new Vector3(-size.x, size.y, 0);
                    s_RectanglePointsCache[2] = new Vector3(0, size.y, 0);
                    s_RectanglePointsCache[3] = new Vector3(0, 0, 0);
                    break;
                default:
                    break;
            }

            // set pass
            s_LinesMaterial.SetPass(0);
            // push matrix
            GL.PushMatrix();
            GL.MultMatrix(matrix);
            // begin drawing
            GL.Begin(GL.LINES);
            GL.Color(color);

            GL.Vertex(s_RectanglePointsCache[0]);
            GL.Vertex(s_RectanglePointsCache[1]);

            GL.Vertex(s_RectanglePointsCache[1]);
            GL.Vertex(s_RectanglePointsCache[2]);

            GL.Vertex(s_RectanglePointsCache[2]);
            GL.Vertex(s_RectanglePointsCache[3]);

            GL.Vertex(s_RectanglePointsCache[3]);
            GL.Vertex(s_RectanglePointsCache[0]);
            
            GL.End();
            GL.PopMatrix();
        }

        /// <summary>
        /// 绘制弧线
        /// </summary>
        public static void DrawWireDiscGL(Matrix4x4 matrix, float radius, Color color)
        {
            DrawWireArcGL(matrix, 0, Mathf.PI * 2, radius, color);
        }

        /// <summary>
        /// 绘制线框圆
        /// </summary>
        public static void DrawWireArcGL(Matrix4x4 matrix, float startAngle, float sweepAngle, float radius, Color color)
        {
            // set pass
            s_LinesMaterial.SetPass(0);
            // push matrix
            GL.PushMatrix();
            GL.MultMatrix(matrix);
            // begin drawing
            GL.Begin(GL.LINES);
            GL.Color(color);
            float angle = startAngle;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            float z = 0.0f;
            Vector3 prevPoint = new Vector3(x, y, z);
            for (int i = 0; i < s_WireArcPointsCount; i++)
            {
                GL.Vertex(prevPoint);
                angle += sweepAngle / s_WireArcPointsCount;
                x = radius * Mathf.Cos(angle);
                y = radius * Mathf.Sin(angle);
                Vector3 point = new Vector3(x, y, z);
                GL.Vertex(point);
                prevPoint = point;
            }

            GL.End();
            GL.PopMatrix();
        }

        /// <summary>
        /// 绘制线框立方体
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public static void DrawWireCubeGL(Matrix4x4 matrix, Vector3 size, Color color)
        {
            Vector3 extents = size * 0.5f;
            s_CubePointsCache[0] = new Vector3(-extents.x, -extents.y, -extents.z);
            s_CubePointsCache[1] = new Vector3(-extents.x, -extents.y, extents.z);
            s_CubePointsCache[2] = new Vector3(-extents.x, extents.y, -extents.z);
            s_CubePointsCache[3] = new Vector3(-extents.x, extents.y, extents.z);
            s_CubePointsCache[4] = new Vector3(extents.x, -extents.y, -extents.z);
            s_CubePointsCache[5] = new Vector3(extents.x, -extents.y, extents.z);
            s_CubePointsCache[6] = new Vector3(extents.x, extents.y, -extents.z);
            s_CubePointsCache[7] = new Vector3(extents.x, extents.y, extents.z);
            // set pass
            s_LinesMaterial.SetPass(0);
            // push matrix
            GL.PushMatrix();
            GL.MultMatrix(matrix);
            // begin drawing
            GL.Begin(GL.LINES);
            GL.Color(color);

            GL.Vertex(s_CubePointsCache[0]);
            GL.Vertex(s_CubePointsCache[1]);

            GL.Vertex(s_CubePointsCache[2]);
            GL.Vertex(s_CubePointsCache[3]);

            GL.Vertex(s_CubePointsCache[4]);
            GL.Vertex(s_CubePointsCache[5]);

            GL.Vertex(s_CubePointsCache[6]);
            GL.Vertex(s_CubePointsCache[7]);

            GL.Vertex(s_CubePointsCache[0]);
            GL.Vertex(s_CubePointsCache[2]);

            GL.Vertex(s_CubePointsCache[1]);
            GL.Vertex(s_CubePointsCache[3]);

            GL.Vertex(s_CubePointsCache[4]);
            GL.Vertex(s_CubePointsCache[6]);

            GL.Vertex(s_CubePointsCache[5]);
            GL.Vertex(s_CubePointsCache[7]);

            GL.Vertex(s_CubePointsCache[0]);
            GL.Vertex(s_CubePointsCache[4]);

            GL.Vertex(s_CubePointsCache[1]);
            GL.Vertex(s_CubePointsCache[5]);

            GL.Vertex(s_CubePointsCache[2]);
            GL.Vertex(s_CubePointsCache[6]);

            GL.Vertex(s_CubePointsCache[3]);
            GL.Vertex(s_CubePointsCache[7]);

            GL.End();
            GL.PopMatrix();
        }

        //Handles
        //public static Mesh CreateGridMesh(Matrix4x4 matrix, int gridSize, float spacing, Color color)
        //{

        //}

        public static void FillWireRectMesh(ref Mesh mesh, Alignment alignment, Vector2 size, Color color)
        {
            mesh.Clear();

            Vector3[] vertices = new Vector3[4];
            int[] indices = new int[8];
            Color[] colors = new Color[4];

            // vertices
            switch (alignment)
            {
                case Alignment.UpperLeft:
                    vertices[0] = new Vector3(0, -size.y, 0);
                    vertices[1] = new Vector3(0, 0, 0);
                    vertices[2] = new Vector3(size.x, 0, 0);
                    vertices[3] = new Vector3(size.x, -size.y, 0);
                    break;
                case Alignment.UpperCenter:
                    vertices[0] = new Vector3(-size.x * 0.5f, -size.y, 0);
                    vertices[1] = new Vector3(-size.x * 0.5f, 0, 0);
                    vertices[2] = new Vector3(size.x * 0.5f, 0, 0);
                    vertices[3] = new Vector3(size.x * 0.5f, -size.y, 0);
                    break;
                case Alignment.UpperRight:
                    vertices[0] = new Vector3(-size.x, -size.y, 0);
                    vertices[1] = new Vector3(-size.x, 0, 0);
                    vertices[2] = new Vector3(0, 0, 0);
                    vertices[3] = new Vector3(0, -size.y, 0);
                    break;
                case Alignment.MiddleLeft:
                    vertices[0] = new Vector3(0, -size.y * 0.5f, 0);
                    vertices[1] = new Vector3(0, size.y * 0.5f, 0);
                    vertices[2] = new Vector3(size.x, size.y * 0.5f, 0);
                    vertices[3] = new Vector3(size.x, -size.y * 0.5f, 0);
                    break;
                case Alignment.MiddleCenter:
                    vertices[0] = new Vector3(-size.x * 0.5f, -size.y * 0.5f, 0);
                    vertices[1] = new Vector3(-size.x * 0.5f, size.y * 0.5f, 0);
                    vertices[2] = new Vector3(size.x * 0.5f, size.y * 0.5f, 0);
                    vertices[3] = new Vector3(size.x * 0.5f, -size.y * 0.5f, 0);
                    break;
                case Alignment.MiddleRight:
                    vertices[0] = new Vector3(-size.x, -size.y * 0.5f, 0);
                    vertices[1] = new Vector3(-size.x, size.y * 0.5f, 0);
                    vertices[2] = new Vector3(0, size.y * 0.5f, 0);
                    vertices[3] = new Vector3(0, -size.y * 0.5f, 0);
                    break;
                case Alignment.LowerLeft:
                    vertices[0] = new Vector3(0, 0, 0);
                    vertices[1] = new Vector3(0, size.y, 0);
                    vertices[2] = new Vector3(size.x, size.y, 0);
                    vertices[3] = new Vector3(size.x, 0, 0);
                    break;
                case Alignment.LowerCenter:
                    vertices[0] = new Vector3(-size.x * 0.5f, 0, 0);
                    vertices[1] = new Vector3(-size.x * 0.5f, size.y, 0);
                    vertices[2] = new Vector3(size.x * 0.5f, size.y, 0);
                    vertices[3] = new Vector3(size.x * 0.5f, 0, 0);
                    break;
                case Alignment.LowerRight:
                    vertices[0] = new Vector3(-size.x, 0, 0);
                    vertices[1] = new Vector3(-size.x, size.y, 0);
                    vertices[2] = new Vector3(0, size.y, 0);
                    vertices[3] = new Vector3(0, 0, 0);
                    break;
                default:
                    break;
            }

            // indices
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 1;
            indices[3] = 2;
            indices[4] = 2;
            indices[5] = 3;
            indices[6] = 3;
            indices[7] = 0;

            //colors
            colors[0] = color;
            colors[1] = color;
            colors[2] = color;
            colors[3] = color;

            // mesh
            mesh.vertices = vertices;
            mesh.SetIndices(indices, MeshTopology.Lines, 0);
            mesh.colors = colors;
        }
    }
}