using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class DoorCreateToolArgs : ToolArgs
    {
        /// <summary>
        /// 门类型
        /// </summary>
        public DoorType DoorType { get; set; }

        public DoorCreateToolArgs()
        {

        }

        public DoorCreateToolArgs(DoorType doorType)
        {
            DoorType = doorType;
        }
    }

    public class DoorCreateTool : ToolBase
    {
        private Vector3 point;
        private Wall wall;
        private Door door;
        public DoorType doorType;
        private Matrix4x4 matrix = Matrix4x4.identity;
        private bool hasSumbit = false;

        public override void Init(ToolArgs t)
        {
            base.Init(t);
            TopMost = true;
            // args
            DoorCreateToolArgs args = (DoorCreateToolArgs)t;
            hasSumbit = false;
            Color = ArchitectSettings.Door.color2d;
            // 初始化门
            door = new Door();
            door.Height = ArchitectSettings.Door.height;
            door.Thickness = ArchitectSettings.Door.thickness;
            door.Bottom = ArchitectSettings.Door.bottom;
            door.DoorType = args.DoorType;
            switch (door.DoorType)
            {
                case DoorType.Single:
                    door.Length = ArchitectSettings.Door.length;
                    break;
                case DoorType.Double:
                    door.Length = ArchitectSettings.Door.double_length;
                    break;
                case DoorType.Revolving:
                    door.Length = ArchitectSettings.Door.revolving_length;
                    break;
                default:
                    break;
            }
        }

        public override void Update()
        {
            base.Update();
            point = Architect.PointerToWorldPoint;
            // 捕捉墙体
            if (Architect.SnapWall(point, out wall, door.Length * 0.5f))
            {
                point = wall.ToLine2(wall.Corner0).ClosestPointOnLine(point.XZ()).XOZ();
                door.Position = new Vector3(point.x, wall.Corner0.Position.y + door.Bottom + door.Height * 0.5f, point.z);
                if (wall.CanAddHole(door))
                {
                    door.Wall = wall;
                    Color = ArchitectSettings.Door.color2d;
                }
                else
                {
                    door.Wall = null;
                    Color = ArchitectSettings.Assist.warningColor;
                }
            }
            else
            {
                door.Wall = null;
                Color = ArchitectSettings.Door.color2d;
            }

            DrawDoor();

            if (InputUtils.GetMouseButtonDown(InputUtils.left_button))
            {
                Submit();
            }
        }

        public void DrawDoor()
        {
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            Mesh mesh = s_MeshPool.Spawn();

            float length = door.Length;
            float thickness = door.Thickness;
            Vector2 size = new Vector2(thickness, length);
            switch (door.DoorType)
            {
                case DoorType.Single:
                    vh.AddWireRect(new Vector3(length * -0.5f + thickness * 0.5f, 0, length * 0.5f), Quaternion.identity, size, Color);
                    vh.AddWireArc(new Vector3(length * -0.5f, 0, 0), Vector3.up, 0, Mathf.PI * 0.5f, length, Color);
                    vh.FillMesh(mesh, MeshTopology.Lines);
                    break;
                case DoorType.Double:
                    float half = length / 2;
                    size = new Vector2(thickness, half);
                    vh.AddWireRect(new Vector3(length * -0.5f + thickness * 0.5f, 0, half * 0.5f), Quaternion.identity, size, Color);
                    vh.AddWireRect(new Vector3(length * 0.5f - thickness * 0.5f, 0, half * 0.5f), Quaternion.identity, size, Color);
                    vh.AddWireArc(new Vector3(length * -0.5f, 0, 0), Vector3.up, 0, Mathf.PI * 0.5f, half, Color);
                    vh.AddWireArc(new Vector3(length * 0.5f, 0, 0), Vector3.up, Mathf.PI * 0.5f, Mathf.PI * 0.5f, half, Color);
                    vh.FillMesh(mesh, MeshTopology.Lines);
                    break;
                case DoorType.Revolving:
                    half = length / 2;
                    vh.AddWireCircle(new Vector3(0, 0, 0), Vector3.up, 0.1f, Color);
                    vh.AddWireArc(new Vector3(0, 0, 0), Vector3.up, 2 * Mathf.PI / 3, 2 * Mathf.PI / 3, half, Color);
                    vh.AddWireArc(new Vector3(0, 0, 0), Vector3.up, -Mathf.PI / 3, 2 * Mathf.PI / 3, half, Color);
                    vh.AddWireArc(new Vector3(0, 0, 0), Vector3.up, 2 * Mathf.PI / 3, 2 * Mathf.PI / 3, half - 0.15f, Color);
                    vh.AddWireArc(new Vector3(0, 0, 0), Vector3.up, -Mathf.PI / 3, 2 * Mathf.PI / 3, half - 0.15f, Color);
                    
                    // 直线两点
                    Vector3 p1, p2;

                    // 十字线
                    float theta = Mathf.PI / 4;
                    float radius = 0.1f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half - 0.15f;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    vh.AddLine(p1, p2, Color);

                    theta = 3 * Mathf.PI / 4;
                    radius = 0.1f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half - 0.15f;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    vh.AddLine(p1, p2, Color);

                    theta = -3 * Mathf.PI / 4;
                    radius = 0.1f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half - 0.15f;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    vh.AddLine(p1, p2, Color);

                    theta = -Mathf.PI / 4;
                    radius = 0.1f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half - 0.15f;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    vh.AddLine(p1, p2, Color);

                    // 圆弧连接线
                    theta = Mathf.PI / 3;
                    radius = half - 0.15f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    vh.AddLine(p1, p2, Color);

                    theta = 2 * Mathf.PI / 3;
                    radius = half - 0.15f; ;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    vh.AddLine(p1, p2, Color);

                    theta = -2 * Mathf.PI / 3;
                    radius = half - 0.15f; ;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    vh.AddLine(p1, p2, Color);

                    theta = -Mathf.PI / 3;
                    radius = half - 0.15f; ;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    vh.AddLine(p1, p2, Color);

                    vh.FillMesh(mesh, MeshTopology.Lines);
                    break;
                default:
                    break;
            }


            // 绘制网格
            Vector3 position = point;
            if (wall == null)
            {
                matrix.SetTRS(position, Quaternion.identity, Vector3.one);
            }
            else
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                matrix.SetTRS(position, rotation, Vector3.one);
            }
            
            Graphics.DrawMesh(mesh, matrix, ArchitectSettings.Door.material2d, 0);

            s_VertexHelperPool.Despawn(vh);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public override void Submit()
        {
            base.Submit();

            hasSumbit = true;
            if (door.Wall != null)
            {
                door.Flip = 0;
                door.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                Architect.AddEntity(door);
                GraphicManager.CreateGraphic(door);
            }
            else
            {
                Debug.LogWarning("不能在此处添加门。");
            }
            
            Architect.Instance.ActiveTool = null;
        }

        public override void Cancel()
        {
            base.Cancel();

            Architect.Instance.ActiveTool = null;
        }

        public override void Release()
        {
            base.Release();

            if (!hasSumbit && door.Wall != null)
            {
                door.Wall = null;
            }
            
            wall = null;
            door = null;
        }
    }
}
