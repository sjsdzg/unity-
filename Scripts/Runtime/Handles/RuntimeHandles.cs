using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Runtime
{
    public class RuntimeHandles
    {
        // Color of the X axis handle
        private static PrefColor s_XAxisColor = new PrefColor("Scene/X Axis", new Color(219f / 255, 62f / 255, 29f / 255, .93f));
        public static Color xAxisColor { get { return s_XAxisColor; } }
        // Color of the Y axis handle
        private static PrefColor s_YAxisColor = new PrefColor("Scene/Y Axis", new Color(154f / 255, 243f / 255, 72f / 255, .93f));
        public static Color yAxisColor { get { return s_YAxisColor; } }
        // Color of the Z axis handle
        private static PrefColor s_ZAxisColor = new PrefColor("Scene/Z Axis", new Color(58f / 255, 122f / 255, 248f / 255, .93f));
        public static Color zAxisColor { get { return s_ZAxisColor; } }

        // color for handles the currently hovered handle
        private static PrefColor s_highlightedColor = new PrefColor("Scene/Highlight Axis", new Color(201f / 255, 200f / 255, 144f / 255, 0.89f));
        public static Color highlightedColor { get { return s_highlightedColor; } }

        private static PrefColor s_SelectedColor = new PrefColor("Scene/Selected Axis", new Color(246f / 255, 242f / 255, 50f / 255, .89f));
        public static Color selectedColor { get { return s_SelectedColor; } }

        private static Camera m_Camera = null;
        /// <summary>
        /// 相机
        /// </summary>
        public static Camera Camera
        {
            get
            {
                if (m_Camera == null)
                {
                    m_Camera = Camera.main;
                }
                return m_Camera;
            }
            set { m_Camera = value; }
        }

        /// <summary>
        /// 获取在给定位置世界空间的控制柄大小
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float GetHandleSize(Vector3 position)
        {
            if (Camera)
            {
                Transform tr = Camera.transform;
                Vector3 camPos = tr.position;
                float distance = Vector3.Dot(position - camPos, tr.TransformDirection(new Vector3(0, 0, 1)));
                Vector3 screenPos = Camera.WorldToScreenPoint(camPos + tr.TransformDirection(new Vector3(0, 0, distance)));
                Vector3 screenPos2 = Camera.WorldToScreenPoint(camPos + tr.TransformDirection(new Vector3(1, 0, distance)));
                float screenDist = (screenPos - screenPos2).magnitude;
#if UNITY_WEBGL
                return 1 / Mathf.Max(screenDist, 0.0001f);
#else
                return 1 / Mathf.Max(screenDist, 0.0001f) * (Screen.dpi / 25.4f);
#endif
            }
            return 1.0f;
        }

        private static MeshBuilder s_CubeVertexHelper;
        /// <summary>
        /// cube
        /// </summary>
        public static MeshBuilder cubeVertexHelper
        {
            get
            {
                if (s_CubeVertexHelper == null)
                {
                    s_CubeVertexHelper = s_VertexHelperPool.Spawn();
                    s_CubeVertexHelper.AddCube(Vector3.zero, Quaternion.identity, Vector3.one, Color.white);
                }
                return s_CubeVertexHelper;
            }
        }

        private static MeshBuilder s_ConeVertexHelper;
        /// <summary>
        /// cone
        /// </summary>
        public static MeshBuilder coneVertexHelper
        {
            get
            {
                if (s_ConeVertexHelper == null)
                {
                    s_ConeVertexHelper = s_VertexHelperPool.Spawn();
                    s_ConeVertexHelper.AddCone(Vector3.zero, Quaternion.identity, 0.35f, 1, Color.white);
                }
                return s_ConeVertexHelper;
            }
        }

        private static MeshBuilder s_CylinderVertexHelper;
        /// <summary>
        /// cylinder
        /// </summary>
        public static MeshBuilder cylinderVertexHelper
        {
            get
            {
                if (s_CylinderVertexHelper == null)
                {
                    s_CylinderVertexHelper = s_VertexHelperPool.Spawn();
                    s_CylinderVertexHelper.AddCylinder(Vector3.zero, Quaternion.identity, 0.5f, 1, Color.white);
                }
                return s_CylinderVertexHelper;
            }
        }

        private static MeshBuilder s_SphereVertexHelper;
        /// <summary>
        /// sphere
        /// </summary>
        public static MeshBuilder sphereVertexHelper
        {
            get
            {
                if (s_SphereVertexHelper == null)
                {
                    s_SphereVertexHelper = s_VertexHelperPool.Spawn();
                    s_SphereVertexHelper.AddSphere(Vector3.zero, Quaternion.identity, 1, Color.white);
                }
                return s_SphereVertexHelper;
            }
        }


        /// <summary>
        /// s_VertexHelperPool
        /// </summary>
        private static readonly ObjectPool<MeshBuilder> s_VertexHelperPool = new ObjectPool<MeshBuilder>(null, x => x.Clear());

        /// <summary>
        /// s_MeshPool
        /// </summary>
        private static readonly ObjectPool<Mesh> s_MeshPool = new ObjectPool<Mesh>(null, x => x.Clear());


        /// <summary>
        /// Draw a line from p1 to p2
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="color"></param>
        public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            Debug.LogError("3333");
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            vh.AddLine(p1, p2, color);
            Mesh mesh = s_MeshPool.Spawn();
            vh.FillMesh(mesh, MeshTopology.Lines);
            Graphics.DrawMesh(mesh, Matrix4x4.identity, HandleUtility.handleWireMaterial, 0);
            s_VertexHelperPool.Despawn(vh);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public static void DrawDottedLine(Vector3 p1, Vector3 p2, float screenSpaceSize, Color color)
        {
            Debug.LogError("44444");
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            float size = screenSpaceSize * HandleUtility.GetHandleSize((p1 + p2) / 2);
            vh.AddDashLine(p1, p2, new float[] { size, size }, color);
            Mesh mesh = s_MeshPool.Spawn();
            vh.FillMesh(mesh, MeshTopology.Lines);
            Graphics.DrawMesh(mesh, Matrix4x4.identity, HandleUtility.handleWireMaterial, 0);
            s_VertexHelperPool.Despawn(vh);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        /// <summary>
        /// 绘制Function
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public delegate void CapFunction(Vector3 position, Quaternion rotation, float size, Color color);

        public static void CubeHandleCap(Vector3 position, Quaternion rotation, float size, Color color)
        {
            Debug.LogError("55555");
            Mesh mesh = s_MeshPool.Spawn();
            cubeVertexHelper.SetColors(color);
            cubeVertexHelper.FillMesh(mesh, MeshTopology.Triangles);
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one * size);
            Graphics.DrawMesh(mesh, matrix, HandleUtility.handleMaterial, 0);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public static void SphereHandleCap(Vector3 position, Quaternion rotation, float size, Color color)
        {
            Mesh mesh = s_MeshPool.Spawn();
            sphereVertexHelper.SetColors(color);
            sphereVertexHelper.FillMesh(mesh, MeshTopology.Triangles);
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one * size);
            Graphics.DrawMesh(mesh, matrix, HandleUtility.handleMaterial, 0);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public static void ConeHandleCap(Vector3 position, Quaternion rotation, float size, Color color)
        {
            Mesh mesh = s_MeshPool.Spawn();
            coneVertexHelper.SetColors(color);
            coneVertexHelper.FillMesh(mesh, MeshTopology.Triangles);
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one * size);
            Graphics.DrawMesh(mesh, matrix, HandleUtility.handleMaterial, 0);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public static void CylinderHandleCap(Vector3 position, Quaternion rotation, float size, Color color)
        {
            Mesh mesh = s_MeshPool.Spawn();
            cylinderVertexHelper.SetColors(color);
            cylinderVertexHelper.FillMesh(mesh, MeshTopology.Triangles);
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one * size);
            Graphics.DrawMesh(mesh, matrix, HandleUtility.handleMaterial, 0);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public static void RectangleHandleCap(Vector3 position, Quaternion rotation, float size, Color color)
        {
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            vh.AddWireRect(position, rotation, new Vector2(size, size), color);
            Mesh mesh = s_MeshPool.Spawn();
            vh.FillMesh(mesh, MeshTopology.Lines);
            Graphics.DrawMesh(mesh, Matrix4x4.identity, HandleUtility.handleWireMaterial, 0);
            s_VertexHelperPool.Despawn(vh);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public static void DotHandleCap(Vector3 position, Quaternion rotation, float size, Color color)
        {
            rotation = HandleUtility.Camera.transform.rotation * Quaternion.Euler(-90, 0, 0);
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            vh.AddRect(position, rotation, new Vector2(size, size), color);
            Mesh mesh = s_MeshPool.Spawn();
            vh.FillMesh(mesh, MeshTopology.Triangles);
            Graphics.DrawMesh(mesh, Matrix4x4.identity, HandleUtility.handleWireMaterial, 0);
            s_VertexHelperPool.Despawn(vh);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public static void CircleHandleCap(Vector3 position, Quaternion rotation, float size, Color color)
        {
            Vector3 normal = rotation * Vector3.up;
            MeshBuilder vh = s_VertexHelperPool.Spawn();
            vh.AddWireCircle(position, normal, size, color);
            Mesh mesh = s_MeshPool.Spawn();
            vh.FillMesh(mesh, MeshTopology.Lines);
            Graphics.DrawMesh(mesh, Matrix4x4.identity, HandleUtility.handleWireMaterial, 0);
            s_VertexHelperPool.Despawn(vh);
            s_MeshPool.DespawnEndOfFrame(mesh);
        }

        public static void ArrowHandleCap(Vector3 position, Quaternion rotation, float size, Color color)
        {
            Vector3 direction = rotation * Vector3.up;
            ConeHandleCap(position + direction * size, rotation, size * 0.2f, color);
            DrawLine(position, position + direction * size * 0.9f, color);
        }

        public static Slider1DHandle Slider(PropertyWrapper<Vector3> handlePosWrapper, Vector3 direction, PropertyWrapper<float> handleSizeWrapper, CapFunction capFunction, Color normalColor)
        {
            return Slider(handlePosWrapper, direction, handleSizeWrapper, capFunction, normalColor, -1);
        }

        public static Slider1DHandle Slider(PropertyWrapper<Vector3> handlePosWrapper, Vector3 direction, PropertyWrapper<float> handleSizeWrapper, CapFunction capFunction, Color normalColor, float snap)
        {
            int id = HandleUtility.GetHandleId();
            ColorBlockRef colorBlock = ColorBlockRef.defaultColorBlockRef;
            colorBlock.normalColor = normalColor;
            colorBlock.highlightedColor = highlightedColor;
            colorBlock.pressedColor = selectedColor;
            colorBlock.selectedColor = highlightedColor;
            return Slider(id, handlePosWrapper, direction, direction, handleSizeWrapper, capFunction, colorBlock, snap);
        }

        public static Slider1DHandle Slider(int id, PropertyWrapper<Vector3> handlePosWrapper, Vector3 handleDir, Vector3 slideDir, PropertyWrapper<float> handleSizeWrapper, CapFunction capFunction, ColorBlockRef colorBlock, float snap)
        {
            Slider1DHandle slider1D = new Slider1DHandle(id, handlePosWrapper, handleDir, slideDir, handleSizeWrapper, capFunction, colorBlock, snap);
            slider1D.Initialize();

            if (capFunction != null)
            {
                if (capFunction == ArrowHandleCap)
                {
                    HandleUtility.AddHandleControl(id, () => HandleUtility.DistanceToLineSegment(handlePosWrapper.Value, handlePosWrapper.Value + slideDir * handleSizeWrapper.Value * 0.9f));
                    HandleUtility.AddHandleControl(id, () => HandleUtility.DistanceToCircle(handlePosWrapper.Value + slideDir * handleSizeWrapper.Value, handleSizeWrapper.Value * 0.2f));
                }
                else if (capFunction == RectangleHandleCap)
                {
                    HandleUtility.AddHandleControl(id, () => HandleUtility.DistanceToRectangle(handlePosWrapper.Value, Quaternion.FromToRotation(Vector3.up, handleDir), new Vector2(0.5f, 0.5f) * handleSizeWrapper.Value));
                }
                else
                {
                    HandleUtility.AddHandleControl(id, () => HandleUtility.DistanceToCircle(handlePosWrapper.Value, handleSizeWrapper.Value * 0.5f));
                }
            }

            return slider1D;
        }

        public static Slider2DHandle Slider2D(PropertyWrapper<Vector3> handlePosWrapper, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, PropertyWrapper<float> handleSizeWrapper, CapFunction capFunction, Color normalColor)
        {
            return Slider2D(handlePosWrapper, PropertyWrapper<Vector3>.Default, handleDir, slideDir1, slideDir2, handleSizeWrapper, capFunction, normalColor, -1);
        }

        public static Slider2DHandle Slider2D(PropertyWrapper<Vector3> handlePosWrapper, PropertyWrapper<Vector3> offsetWrapper, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, PropertyWrapper<float> handleSizeWrapper, CapFunction capFunction, Color normalColor)
        {
            return Slider2D(handlePosWrapper, offsetWrapper, handleDir, slideDir1, slideDir2, handleSizeWrapper, capFunction, normalColor, -1);
        }

        public static Slider2DHandle Slider2D(PropertyWrapper<Vector3> handlePosWrapper, PropertyWrapper<Vector3> offsetWrapper, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, PropertyWrapper<float> handleSizeWrapper, CapFunction capFunction, Color normalColor, float snap)
        {
            int id = HandleUtility.GetHandleId();
            ColorBlockRef colorBlock = ColorBlockRef.defaultColorBlockRef;
            colorBlock.normalColor = normalColor;
            colorBlock.highlightedColor = highlightedColor;
            colorBlock.pressedColor = selectedColor;
            colorBlock.selectedColor = highlightedColor;
            return Slider2D(id, handlePosWrapper, offsetWrapper, handleDir, slideDir1, slideDir2, handleSizeWrapper, capFunction, colorBlock, snap);
        }

        public static Slider2DHandle Slider2D(int id, PropertyWrapper<Vector3> handlePosWrapper, PropertyWrapper<Vector3> offsetWrapper, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, PropertyWrapper<float> handleSizeWrapper, CapFunction capFunction, ColorBlockRef colorBlock, float snap)
        {
            Slider2DHandle slider2D = new Slider2DHandle(id, handlePosWrapper, offsetWrapper, handleDir, slideDir1, slideDir2, handleSizeWrapper, capFunction, colorBlock, snap);
            slider2D.Initialize();

            if (capFunction != null)
            {
                if (capFunction == ArrowHandleCap)
                {
                    HandleUtility.AddHandleControl(id, () => HandleUtility.DistanceToLineSegment(handlePosWrapper.Value + offsetWrapper.Value, handlePosWrapper.Value + offsetWrapper.Value + handleDir * handleSizeWrapper.Value * 0.9f));
                    HandleUtility.AddHandleControl(id, () => HandleUtility.DistanceToCircle(handlePosWrapper.Value + offsetWrapper.Value + handleDir * handleSizeWrapper.Value, handleSizeWrapper.Value * 0.2f));
                }
                else if (capFunction == RectangleHandleCap)
                {
                    HandleUtility.AddHandleControl(id, () => HandleUtility.DistanceToRectangle(handlePosWrapper.Value + offsetWrapper.Value, Quaternion.FromToRotation(Vector3.up, handleDir), new Vector2(0.5f, 0.5f) * handleSizeWrapper.Value));
                }
                else
                {
                    HandleUtility.AddHandleControl(id, () => HandleUtility.DistanceToCircle(handlePosWrapper.Value + offsetWrapper.Value, handleSizeWrapper.Value * 0.5f));
                }
            }

            return slider2D;
        }
    }
}