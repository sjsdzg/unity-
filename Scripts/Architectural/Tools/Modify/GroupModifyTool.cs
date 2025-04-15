using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Math;
using XFramework.Runtime;

namespace XFramework.Architectural
{
    public class GroupModifyToolArgs : ToolArgs
    {
        public Group Group { get; set; }
    }

    public class GroupModifyTool : ToolBase
    {
        private Vector3 worldPoint;
        private Group group;
        private bool hasSumbit = false;
        private Vector3 delta;

        private bool isCanAdd = true;
        private bool isSnapCorner = false;
        private bool isSnapWall = false;

        private List<Corner> corners;
        private List<Wall> walls;
        private List<Room> rooms;

        private Slider2DHandle m_Slider2DHandle;

        public override void Init(ToolArgs t)
        {
            base.Init(t);
            TopMost = true;
            hasSumbit = false;
            GroupModifyToolArgs args = (GroupModifyToolArgs)t;
            group = args.Group;

            group.TryGetCorners(out corners);
            group.TryGetWalls(out walls);
            group.TryGetRooms(out rooms);
        }

        public override void Update()
        {
            base.Update();
            worldPoint = Architect.PointerToWorldPoint;
            group.Position = worldPoint;

            isCanAdd = true;
            isSnapCorner = false;
            isSnapWall = false;
            delta = Vector3.zero;

            foreach (var room in rooms)
            {
                if (!Architect.Instance.CurrentFloor.CanAddRoom(room))
                {
                    isCanAdd = false;
                }
            }

            if (isCanAdd)
            {
                foreach (var corner in corners)
                {
                    if (Architect.SnapCorner(corner.Position, out Corner outCorner))
                    {
                        isSnapCorner = true;
                        delta = outCorner.Position - corner.Position;
                        break;
                    }
                }

                if (!isSnapCorner)
                {
                    foreach (var wall in walls)
                    {
                        Segment2 segment = wall.ToSegment2(wall.Corner0);
                        if (Architect.SnapWall(segment, out Wall outWall))
                        {
                            isSnapWall = true;
                            float distance = ((Line2)segment).GetDistanceToPoint(outWall.Corner0.ToPoint2());

                            float cross = Vector2.Dot(segment.normal, outWall.Corner0.ToPoint2() - wall.Corner0.ToPoint2());
                            if (cross > 0)
                                delta = segment.normal.XOZ() * distance;
                            else
                                delta = -segment.normal.XOZ() * distance;

                            break;
                        }
                    }
                }
            }

            DrawState();

            worldPoint += delta;
            group.Position = worldPoint;

            if (InputUtils.GetMouseButtonDown(InputUtils.left_button) && isCanAdd)
            {
                Submit();
            }
        }

        /// <summary>
        /// 更新能否添加的状态
        /// </summary>
        public void DrawState()
        {
            if (!isCanAdd)
            {
                Color = ArchitectSettings.Assist.warningColor;

                // 生成矩形网格
                MeshBuilder vh = s_VertexHelperPool.Spawn();
                Mesh mesh = s_MeshPool.Spawn();

                foreach (var wall in walls)
                {
                    float y = wall.Corner0.Position.y;
                    foreach (var segment in wall.Segments)
                    {
                        vh.AddLine(segment.p1.XOZ(y), segment.p2.XOZ(y), Color);
                    }
                }

                vh.FillMesh(mesh, MeshTopology.Lines);
                s_MeshPool.DespawnEndOfFrame(mesh);
                Debug.LogError("当前房间无法添加，可能是因为房间重叠或超出边界。");
                // 绘制网格
                Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
                Graphics.DrawMesh(mesh, matrix, ArchitectSettings.Assist.material, 0);
            }
        }

        public override void Submit()
        {
            base.Submit();
            hasSumbit = true;
            ArchitectUtility.AddGroup(Architect.Instance.CurrentFloor, group);
            ArchitectUtility.AddGroupHandler(Architect.Instance.CurrentFloor, group);

            Architect.Instance.ActiveTool = null;
        }

        public override void Cancel()
        {
            base.Cancel();
            //this.group = null;
            //ArchitectUtility.RegisterGroupForDestory(group);

            Architect.Instance.ActiveTool = null;
        }

        public override void Release()
        {
            base.Release();
            if (!hasSumbit && group != null)
            {
                ArchitectUtility.RegisterGroupForDestory(group);
            }
            this.group = null;
        }
    }
}
