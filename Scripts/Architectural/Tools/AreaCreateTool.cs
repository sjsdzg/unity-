using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.Math;
using XFramework.Runtime;

namespace XFramework.Architectural
{
    public class AreaCreateTool : ToolBase
    {
        /// <summary>
        /// 指针在世界的坐标
        /// </summary>
        private Vector3 worldPoint;

        /// <summary>
        /// 是否能增加点
        /// </summary>
        private bool canAddPoint = false;

        /// <summary>
        /// 点集合
        /// </summary>
        private List<Vector3> points = new List<Vector3>();

        private Mesh m_CrossCursorMesh;
        /// <summary>
        /// 光标网格
        /// </summary>
        public Mesh CrossCursorMesh
        {
            get
            {
                if (m_CrossCursorMesh == null)
                {
                    m_CrossCursorMesh = new Mesh();

                    MeshBuilder vh = s_VertexHelperPool.Spawn();
                    vh.AddLine(new Vector3(-10, 0, 0), new Vector3(10, 0, 0), Color.white);
                    vh.AddLine(new Vector3(0, 0, -10), new Vector3(0, 0, 10), Color.white);
                    vh.AddWireRect(Vector3.zero, Quaternion.identity, Vector2.one * 2, Color.white);
                    vh.FillMesh(m_CrossCursorMesh, MeshTopology.Lines);
                    s_VertexHelperPool.Despawn(vh);
                }
                return m_CrossCursorMesh;
            }
        }

        private Mesh m_SnapPointMesh;
        /// <summary>
        /// 捕捉点网格
        /// </summary>
        public Mesh SnapPointMesh
        {
            get
            {
                if (m_SnapPointMesh == null)
                {
                    m_SnapPointMesh = new Mesh();

                    MeshBuilder vh = s_VertexHelperPool.Spawn();
                    vh.AddWireCircle(Vector3.zero, Vector3.up, 1, ArchitectSettings.Assist.color);
                    float param = Mathf.Sin(Mathf.PI * 0.25f);
                    vh.AddLine(new Vector3(param, 0, param), new Vector3(-param, 0, -param), ArchitectSettings.Assist.color);
                    vh.AddLine(new Vector3(param, 0, -param), new Vector3(-param, 0, param), ArchitectSettings.Assist.color);
                    vh.FillMesh(m_SnapPointMesh, MeshTopology.Lines);
                    s_VertexHelperPool.Despawn(vh);
                }
                return m_SnapPointMesh;
            }
        }

        public override void Init(ToolArgs t)
        {
            base.Init(t);
            TopMost = true;
            Cursor.visible = false;
        }

        public override void Update()
        {
            base.Update();

            worldPoint = Architect.PointerToWorldPoint;

            // 绘制十字光标
            DrawCrossCursor();

            if (SnapPoint(worldPoint, out Vector3 point))
            {
                worldPoint = point;
                DrawSnapPoint();
            }
            else
            {
                AxisMode axisMode = SnapAxis(worldPoint, out Vector3 axis);
                if ((axisMode & AxisMode.X) != 0 || (axisMode & AxisMode.Z) != 0)
                {
                    if ((axisMode & AxisMode.X) != 0)
                    {
                        worldPoint.x = axis.x;
                    }

                    if ((axisMode & AxisMode.Z) != 0)
                    {
                        worldPoint.z = axis.z;
                    }

                    DrawSnapPoint();
                    DrawSnapAxis(axis);
                }
            }

            CheckCanAddPoint(worldPoint);
            if (InputUtils.GetMouseButtonDown(InputUtils.left_button))
            {
                if (canAddPoint)
                {
                    points.Add(worldPoint);
                }

                if (CanAdd())
                {
                    Submit();
                }
            }
            Debug.LogError("111");
            DrawOutline();
            DrawArea();
        }

        public bool SnapPoint(Vector3 target, out Vector3 result)
        {
            bool flag = Architect.SnapPoint(worldPoint, out result);
            float k = Architect.Instance.SnapEpsilon;

            if (flag)
            {
                k = Vector3.Distance(target, result);
            }

            foreach (var point in points)
            {
                float temp = Vector3.Distance(target, point);
                if (temp <= k)
                {
                    result = point;
                    flag = true;
                    k = temp;
                }
            }

            return flag;
        }

        public AxisMode SnapAxis(Vector3 target, out Vector3 axis)
        {
            AxisMode axisMode = AxisMode.N;
            axis = Vector3.positiveInfinity;

            float k = Architect.Instance.SnapEpsilon;
            foreach (var point in points)
            {
                float temp = Mathf.Abs(target.x - point.x);
                if (temp <= k)
                {
                    axis.x = point.x;
                    k = temp;
                    axisMode |= AxisMode.X;
                }
            }

            k = Architect.Instance.SnapEpsilon;
            foreach (var point in points)
            {
                float temp = Mathf.Abs(target.z - point.z);
                if (temp <= k)
                {
                    axis.z = point.z;
                    k = temp;
                    axisMode |= AxisMode.Z;
                }
            }

            return axisMode;
        }

        private void CheckCanAddPoint(Vector3 worldPoint)
        {
            Color = ArchitectSettings.Area.outlineColor2d;
            if (points.Count == 0)
            {
                canAddPoint = true;
                return;
            }
            else
            {
                // 和尾点重合
                if (MathUtility.Appr(worldPoint, points[points.Count - 1]))
                {
                    canAddPoint = false;
                    return;
                }

                // 相交
                int length = points.Count;
                Segment2 original = new Segment2(points[length - 1].XZ(), worldPoint.XZ());
                for (int i = 0; i < length - 1; i++)
                {
                    Segment2 segment = new Segment2(points[i].XZ(), points[i + 1].XZ());
                    if (original.Intersect(segment, out Vector2 result))
                    {
                        if (!original.OverlapPoint(result))
                        {
                            Color = ArchitectSettings.Assist.warningColor;
                            canAddPoint = false;
                            return;
                        }
                    }
                }

                canAddPoint = true;
            }
        }

        private bool CanAdd()
        {
            if (points.Count <= 3)
            {
                return false;
            }
            else
            {
                // 闭合
                bool closed = MathUtility.Appr(points[0], points[points.Count - 1]);
                if (closed)
                {
                    float area = MathUtility.CalculateArea(points, Vector3.up);
                    if (MathUtility.Appr(area, 0))
                    {
                        Cancel();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    var lastPoint = points[points.Count - 1];
                    for (int i = 1; i < points.Count - 1; i++)
                    {
                        if (MathUtility.Appr(lastPoint, points[i]))
                        {
                            Cancel();
                            return false;
                        }
                    }

                    return false;
                }
            }
        }


        /// <summary>
        /// 绘制十字光标
        /// </summary>
        private void DrawCrossCursor()
        {
            float handleSize = RuntimeHandles.GetHandleSize(worldPoint);
            Matrix4x4 matrix = Matrix4x4.TRS(worldPoint, Quaternion.identity, Vector3.one * handleSize);
            Graphics.DrawMesh(CrossCursorMesh, matrix, ArchitectSettings.Assist.material, 0);
        }

        /// <summary>
        /// 绘制捕捉点
        /// </summary>
        public void DrawSnapPoint()
        {
            float handleSize = RuntimeHandles.GetHandleSize(worldPoint);
            Matrix4x4 matrix = Matrix4x4.TRS(worldPoint, Quaternion.identity, Vector3.one * handleSize * 1.5f);
            Graphics.DrawMesh(SnapPointMesh, matrix, ArchitectSettings.Assist.material, 0);
        }

        /// <summary>
        /// 绘制捕捉线
        /// </summary>
        /// <param name="axis"></param>
        public void DrawSnapAxis(Vector3 axis)
        {
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            Mesh mesh = s_MeshPool.Spawn();
            Vector3 p0;
            Vector3 p1;

            Vector3 pos = Architect.WorldToScreenPoint(worldPoint);
            if (!float.IsInfinity(axis.x))
            {
                p0 = Architect.ScreenToWorldPoint(new Vector3(pos.x, Screen.height));
                p1 = Architect.ScreenToWorldPoint(new Vector3(pos.x, 0));
                float size = RuntimeHandles.GetHandleSize(p0);
                vh.AddDashLine(p0, p1, new float[] { size, size }, ArchitectSettings.Assist.color);
            }

            if (!float.IsInfinity(axis.z))
            {
                p0 = Architect.ScreenToWorldPoint(new Vector3(0, pos.y));
                p1 = Architect.ScreenToWorldPoint(new Vector3(Screen.width, pos.y));
                float size = RuntimeHandles.GetHandleSize(p0);
                vh.AddDashLine(p0, p1, new float[] { size, size }, ArchitectSettings.Assist.color);
            }

            vh.FillMesh(mesh, MeshTopology.Lines);
            Graphics.DrawMesh(mesh, Matrix4x4.identity, ArchitectSettings.Assist.material, 0);

            s_MeshPool.DespawnEndOfFrame(mesh);
            s_VertexHelperPool.Despawn(vh);
        }

        /// <summary>
        /// 绘制轮廓线
        /// </summary>
        private void DrawOutline()
        {
            if (points.Count >= 1)
            {
                MeshBuilder vh = s_VertexHelperPool.Spawn();
                Mesh mesh = s_MeshPool.Spawn();

                float size = RuntimeHandles.GetHandleSize(worldPoint) * 2f;

                for (int i = 0; i < points.Count - 1; i++)
                {
                    vh.AddDashLine(points[i], points[i + 1], new float[] { size, size }, ArchitectSettings.Area.outlineColor2d);
                }

                vh.AddDashLine(points[points.Count - 1], worldPoint, new float[] { size, size }, Color);

                vh.FillMesh(mesh, MeshTopology.Lines);
                Graphics.DrawMesh(mesh, Matrix4x4.identity, ArchitectSettings.Assist.material, 0);

                s_MeshPool.DespawnEndOfFrame(mesh);
                s_VertexHelperPool.Despawn(vh);
            }
        }

        /// <summary>
        /// 绘制区域
        /// </summary>
        private void DrawArea()
        {
            if (points.Count >= 2)
            {
                if (points.Count == 2 && MathUtility.Collinear(points[0], points[1], worldPoint))
                {
                    return;
                }

                MeshBuilder vh = s_VertexHelperPool.Spawn();
                Mesh mesh = s_MeshPool.Spawn();
                // tessellator
                var tessellator = new Tessellator();
                // 轮廓
                List<Vector3> contour = ListPool<Vector3>.Get();
                foreach (var point in points)
                {
                    contour.Add(point);
                }

                contour.Add(worldPoint);
                contour.Add(points[0]);

                tessellator.AddContour(contour);
                // normal
                Vector3 normal = Vector3.up;
                // Tessellate
                tessellator.Tessellate(normal: normal);
                // uvs
                Vector2[] uvs = MathUtility.CalculateUVs(tessellator.Positions, normal);
                // vertices
                int startIndex = vh.CurrentVertCount;
                for (int i = 0; i < tessellator.VertexCount; i++)
                {
                    vh.AddVert(tessellator.Positions[i], ArchitectSettings.Area.color2d, uvs[i], normal);
                }
                // indices
                var indices = tessellator.Indices;
                for (int i = 0, length = indices.Length; i < length; i++)
                {
                    indices[i] += startIndex;
                }
                vh.AddIndices(indices);

                vh.FillMesh(mesh, MeshTopology.Triangles);
                Graphics.DrawMesh(mesh, Matrix4x4.identity, ArchitectSettings.Area.material2d, 0);

                s_MeshPool.DespawnEndOfFrame(mesh);
                s_VertexHelperPool.Despawn(vh);
            }
        }

        public override void Submit()
        {
            base.Submit();
            Area area = new Area();
            area.SetContour(points);

            Architect.AddEntity(area);
            GraphicManager.CreateGraphic(area);

            points.Clear();
        }

        public override void Cancel()
        {
            base.Cancel();
            points.Clear();
        }

        public override void Release()
        {
            base.Release();
            points.Clear();
            Cursor.visible = true;
        }
    }
}
