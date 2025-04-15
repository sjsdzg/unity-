using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class PassCreateTool : ToolBase
    {
        private Vector3 point;
        private Wall wall;
        private Pass pass;
        private Matrix4x4 matrix = Matrix4x4.identity;
        private bool hasSumbit = false;

        public override void Init(ToolArgs t)
        {
            base.Init(t);
            TopMost = true;
            hasSumbit = false;
            Color = ArchitectSettings.Pass.color2d;
            // 初始化窗
            pass = new Pass();
            pass.Length = ArchitectSettings.Pass.length;
            pass.Height = ArchitectSettings.Pass.height;
            pass.Thickness = ArchitectSettings.Pass.thickness;
            pass.Bottom = ArchitectSettings.Pass.bottom;
        }

        public override void Update()
        {
            base.Update();
            point = Architect.PointerToWorldPoint;
            if (Architect.SnapWall(point, out wall, pass.Length * 0.5f))
            {
                point = wall.ToLine2(wall.Corner0).ClosestPointOnLine(point.XZ()).XOZ();
                pass.Position = new Vector3(point.x, wall.Corner0.Position.y + pass.Bottom + pass.Height * 0.5f, point.z);
                if (wall.CanAddHole(pass))
                {
                    pass.Wall = wall;
                    Color = ArchitectSettings.Pass.color2d;
                }
                else
                {
                    pass.Wall = null;
                    Color = ArchitectSettings.Assist.warningColor;
                }
            }
            else
            {
                pass.Wall = null;
                Color = ArchitectSettings.Pass.color2d;
            }

            DrawPass();

            if (InputUtils.GetMouseButtonDown(InputUtils.left_button))
            {
                Submit();
            }
        }

        public void DrawPass()
        {
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            Mesh mesh = s_MeshPool.Spawn();

            float length = ArchitectSettings.Pass.length;
            float thickness = ArchitectSettings.Pass.thickness;

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

            //vh.AddDashLine(new Vector3(length * -0.5f, 0, -thickness / 2), new Vector3(length * 0.5f, 0, -thickness / 2), new float[2] { 0.1f, 0.1f }, Color);
            //vh.AddDashLine(new Vector3(length * -0.5f, 0, thickness / 2), new Vector3(length * 0.5f, 0, thickness / 2), new float[2] { 0.1f, 0.1f }, Color);
            vh.AddLine(new Vector3(length * -0.5f, 0, -thickness / 2), new Vector3(length * 0.5f, 0, -thickness / 2), Color);
            vh.AddLine(new Vector3(length * -0.5f, 0, thickness / 2), new Vector3(length * 0.5f, 0, thickness / 2), Color);
            vh.FillMesh(mesh, MeshTopology.Lines);
            Debug.LogError("当前房间无法添加，可能是因为房间重叠或超出边界。");
            Graphics.DrawMesh(mesh, matrix, ArchitectSettings.Pass.material2d, 0);

            s_VertexHelperPool.Despawn(vh);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public override void Submit()
        {
            base.Submit();

            hasSumbit = true;
            if (pass.Wall != null)
            {
                pass.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                Architect.AddEntity(pass);
                GraphicManager.CreateGraphic(pass);
            }
            else
            {
                Debug.LogWarning("不能在此处添加垭口。");
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

            if (!hasSumbit && pass.Wall != null)
            {
                pass.Wall = null;
            }

            wall = null;
            pass = null;
        }
    }
}
