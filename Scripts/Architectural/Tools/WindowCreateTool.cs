using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class WindowCreateTool : ToolBase
    {
        private Vector3 point;
        private Wall wall;
        private Window window;
        private Matrix4x4 matrix = Matrix4x4.identity;
        private bool hasSumbit = false;

        public override void Init(ToolArgs t)
        {
            base.Init(t);
            TopMost = true;
            hasSumbit = false;
            Color = ArchitectSettings.Window.color2d;
            // 初始化窗
            window = new Window();
            window.Length = ArchitectSettings.Window.length;
            window.Height = ArchitectSettings.Window.height;
            window.Thickness = ArchitectSettings.Window.thickness;
            window.Bottom = ArchitectSettings.Window.bottom;
        }

        public override void Update()
        {
            base.Update();
            point = Architect.PointerToWorldPoint;
            if (Architect.SnapWall(point, out wall, window.Length * 0.5f))
            {
                point = wall.ToLine2(wall.Corner0).ClosestPointOnLine(point.XZ()).XOZ();
                window.Position = new Vector3(point.x, wall.Corner0.Position.y + window.Bottom + window.Height * 0.5f, point.z);
                if (wall.CanAddHole(window))
                {
                    window.Wall = wall;
                    Color = ArchitectSettings.Window.color2d;
                }
                else
                {
                    window.Wall = null;
                    Color = ArchitectSettings.Assist.warningColor;
                }
            }
            else
            {
                window.Wall = null;
                Color = ArchitectSettings.Window.color2d;
            }

            DrawWindow();

            if (InputUtils.GetMouseButtonDown(InputUtils.left_button))
            {
                Submit();
            }
        }

        public void DrawWindow()
        {
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            Mesh mesh = s_MeshPool.Spawn();

            float length = ArchitectSettings.Window.length;
            float thickness = ArchitectSettings.Window.thickness;

            // 绘制网格
            Vector3 position = point;
            if (wall == null)
            {
                matrix.SetTRS(position, Quaternion.identity, Vector3.one);
            }
            else
            {
                thickness = wall.Thickness;
                Quaternion rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                matrix.SetTRS(position, rotation, Vector3.one);
            }
            
            Vector2 size = new Vector2(length, thickness);
            vh.AddWireRect(Vector3.zero, Quaternion.identity, size, Color);
            vh.AddLine(new Vector3(length * -0.5f, 0, -thickness / 6), new Vector3(length * 0.5f, 0, -thickness / 6), Color);
            vh.AddLine(new Vector3(length * -0.5f, 0, thickness / 6), new Vector3(length * 0.5f, 0, thickness / 6), Color);
            vh.FillMesh(mesh, MeshTopology.Lines);

            Graphics.DrawMesh(mesh, matrix, ArchitectSettings.Window.material2d, 0);

            s_VertexHelperPool.Despawn(vh);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public override void Submit()
        {
            base.Submit();

            hasSumbit = true;
            if (window.Wall != null)
            {
                window.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                Architect.AddEntity(window);
                GraphicManager.CreateGraphic(window);
            }
            else
            {
                Debug.LogWarning("不能在此处添加窗。");
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

            if (!hasSumbit && window.Wall != null)
            {
                window.Wall = null;
            }

            wall = null;
            window = null;
        }
    }
}
