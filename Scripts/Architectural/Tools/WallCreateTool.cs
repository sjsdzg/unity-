using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XFramework.Math;
using XFramework.Runtime;

namespace XFramework.Architectural
{
    public class WallCreateTool : ToolBase
    {
        private Vector3 point;

        private Vector3 point0;
        private Vector3 point1;

        private Corner corner0;
        private Corner corner1;

        /// <summary>
        /// 是否创建
        /// </summary>
        private bool canCreate = false;

        /// <summary>
        /// 是否可以添加
        /// </summary>
        private bool canAdd = false;

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
                    vh.AddLine(new Vector3(-10, 0, 0), new Vector3( 10, 0, 0), Color.white);
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
                    vh.AddLine(new Vector3(param, 0, param),new Vector3(-param, 0, -param), ArchitectSettings.Assist.color);
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

            Vector3 axis;
            Corner corner;
            point = Architect.PointerToWorldPoint;
            // 绘制十字光标
            DrawCrossCursor();

            if (Architect.SnapCorner(point, out corner))
            {
                point = corner.Position;
                DrawSnapPoint();
            }
            else if (ArchitectSettings.Wall.ortho && canCreate) // 开启正交模式
            {
                Vector3 vector = point - point0;
                if (Mathf.Abs(vector.x) >= Mathf.Abs(vector.z))
                {
                    point.z = point0.z;
                }
                else
                {
                    point.x = point0.x;
                }
                
                // 捕捉轴
                if (Architect.SnapAxis(point, out axis))
                {
                    if (!float.IsInfinity(axis.x))
                    {
                        point.x = axis.x;
                    }

                    if (!float.IsInfinity(axis.z))
                    {
                        point.z = axis.z;
                    }
                    
                }

                //DrawSnapPoint();
                DrawSnapAxis(axis);
            }
            else if (Architect.SnapAxis(point, out axis))
            {
                if (!float.IsInfinity(axis.x))
                {
                    point.x = axis.x;
                }

                if (!float.IsInfinity(axis.z))
                {
                    point.z = axis.z;
                }

                DrawSnapPoint();
                DrawSnapAxis(axis);
            }

            if (InputUtils.GetMouseButtonDown(InputUtils.left_button))
            {
                if (!canCreate)
                {
                    point0 = point;
                    if (!Architect.SnapCorner(point, out corner0))
                    {
                        corner0 = new Corner();
                        corner0.Position = point0;
                        Architect.AddEntity(corner0);
                    }
                    canCreate = true;
                }
                else
                {
                    point1 = point;
                    if (!Architect.SnapCorner(point, out corner1))
                    {
                        corner1 = new Corner();
                        corner1.Position = point1;
                        Architect.AddEntity(corner1);
                    }
                    Submit();
                }
            }

            if (canCreate)
            {
                point1 = point;
                CheckCanAdd();
                DrawWall();
                // 绘制捕捉点
                DrawSnapPoint();
            }
        }

        /// <summary>
        /// 绘制捕捉点
        /// </summary>
        public void DrawSnapPoint()
        {
            //VertexHelper vh = s_VertexHelperPool.Spawn();
            //Mesh mesh = s_MeshPool.Spawn();

            float handleSize = RuntimeHandles.GetHandleSize(point);

            //vh.AddWireCircle(point, Vector3.up, size, ArchitectSettings.Assist.color);
            //float param = size * Mathf.Sin(Mathf.PI * 0.25f);
            //vh.AddLine(point + new Vector3(param, 0, param), point + new Vector3(-param, 0, -param), ArchitectSettings.Assist.color);
            //vh.AddLine(point + new Vector3(param, 0, -param), point + new Vector3(-param, 0, param), ArchitectSettings.Assist.color);
            //vh.FillMesh(mesh, MeshTopology.Lines);
            Matrix4x4 matrix = Matrix4x4.TRS(point, Quaternion.identity, Vector3.one * handleSize * 1.5f);
            Graphics.DrawMesh(SnapPointMesh, matrix, ArchitectSettings.Assist.material, 0);

            //s_VertexHelperPool.Despawn(vh);
            //s_MeshPool.DespawnEndOfFrame(mesh);
        }

        /// <summary>
        /// 绘制十字光标
        /// </summary>
        public void DrawCrossCursor()
        {
            //VertexHelper vh = s_VertexHelperPool.Spawn();
            //Mesh mesh = s_MeshPool.Spawn();

            float handleSize = RuntimeHandles.GetHandleSize(point);
            //vh.AddLine(point + new Vector3(-handleSize * 10, 0, 0), point + new Vector3(handleSize * 10, 0, 0), Color.white);
            //vh.AddLine(point + new Vector3(0, 0, -handleSize * 10), point + new Vector3(0, 0, handleSize * 10), Color.white);
            //vh.AddWireRect(point, Quaternion.identity, Vector2.one * handleSize * 2, Color.white);
            //vh.FillMesh(mesh, MeshTopology.Lines);
            //vh.AddCrossShape(point, Vector3.up, size, size * 0.05f, Color.white);
            //vh.FillMesh(mesh, MeshTopology.Triangles);
            Matrix4x4 matrix = Matrix4x4.TRS(point, Quaternion.identity, Vector3.one * handleSize);
            Graphics.DrawMesh(CrossCursorMesh, matrix, ArchitectSettings.Assist.material, 0);

            //s_VertexHelperPool.Despawn(vh);
            //s_MeshPool.DespawnEndOfFrame(mesh);
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

            Vector3 pos = Architect.WorldToScreenPoint(point);
            if (!float.IsInfinity(axis.x))
            {
                p0 = Architect.ScreenToWorldPoint(new Vector3(pos.x, Screen.height));
                p1 = Architect.ScreenToWorldPoint(new Vector3(pos.x, 0));
                float size = RuntimeHandles.GetHandleSize(p0);
                vh.AddDashLine(p0, p1, new float[]{ size, size }, ArchitectSettings.Assist.color);
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

        public void DrawWall()
        {
            if (canAdd)
                Color = ArchitectSettings.Wall.color2d;
            else
                Color = ArchitectSettings.Assist.warningColor;

            // 生成矩形网格
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            Mesh mesh = s_MeshPool.Spawn();

            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, point1 - point0);
            Vector2 size = new Vector3(Vector3.Distance(point0, point1), ArchitectSettings.Wall.thickness);
            vh.AddWireRect(Vector3.zero, rotation, size, Color);
            
            vh.FillMesh(mesh, MeshTopology.Lines);
            s_MeshPool.DespawnEndOfFrame(mesh);

            // 绘制网格
            Matrix4x4 matrix = Matrix4x4.TRS((point0 + point1) / 2, Quaternion.identity, Vector3.one);
            Graphics.DrawMesh(mesh, matrix, ArchitectSettings.Assist.material, 0);
        }

        public void UpdateGUI()
        {
            
        }

        public void CheckCanAdd()
        {
            canAdd = true;

            if (point0.Equals(point1))
            {
                canAdd = false;
                return;
            }

            Segment2 origin = new Segment2(point0.XZ(), point1.XZ());
            List<SegmentCastWallHit> hits = Architect.SegmentCastWalls(origin);
            foreach (var hit in hits)
            {
                if (!hit.Wall.CanSplit(hit.Point))
                {
                    canAdd = false;
                    break;
                }
            }
        }

        public override void Submit()
        {
            base.Submit();

            Wall wall = new Wall(corner0, corner1);
            wall.Thickness = ArchitectSettings.Wall.thickness;

            //Architect.AddComponentData(wall);
            ArchitectUtility.AddWallHandler(Architect.Instance.CurrentFloor, wall);
            ArchitectUtility.SearchAndMergeRoom(Architect.Instance.CurrentFloor);

            point0 = point1;
            corner0 = corner1;
        }

        public override void Cancel()
        {
            base.Cancel();

            if (canCreate && corner0.Walls.Count == 0)
                Architect.RemoveEntity(corner0);

            corner0 = null;
            canCreate = false;
        }

        public override void Release()
        {
            base.Release();

            if (canCreate && corner0.Walls.Count == 0)
                Architect.RemoveEntity(corner0);

            corner0 = null;
            canCreate = false;
            Cursor.visible = true;
        }
    }
}                                                  