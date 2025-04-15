using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Architectural
{
    /// <summary>
    /// 平面布置图特别资源
    /// </summary>
    public static class FloorPlanExtra
    {
        private const string kCornerTexture2DPath = "Texture2D/Corner";

        /// <summary>
        /// 贴图数据结构
        /// </summary>
        public struct Textures
        {
            public Texture2D corner;
        }

        private static Textures s_DefaultTextures;
        /// <summary>
        /// 默认贴图
        /// </summary>
        public static Textures DefaultTextures
        {
            get
            {
                if (s_DefaultTextures.corner == null)
                {
                    s_DefaultTextures.corner = Resources.Load<Texture2D>(kCornerTexture2DPath);
                }
                return s_DefaultTextures;
            }
        }

        /// <summary>
        /// 默认颜色
        /// </summary>
        public static class DefaultColors
        {
            /// <summary>
            /// 背景颜色
            /// </summary>
            public static Color32 background = new Color32(33, 40, 48, 255);
            /// <summary>
            /// 捕捉模式/辅助线颜色
            /// </summary>
            public static Color32 assist = new Color32(1, 161, 1, 255);
            /// <summary>
            /// 墙颜色
            /// </summary>
            public static Color32 wall = new Color32(255, 255, 255, 255);
            /// <summary>
            /// 墙角颜色
            /// </summary>
            public static Color32 corner = new Color32(255, 255, 255, 153);
        }

        /// <summary>
        /// 默认渲染深度
        /// </summary>
        public static class DefaultRenderQueues
        {
            public static int wall = (int)UnityEngine.Rendering.RenderQueue.Geometry;
            public static int corner = (int)UnityEngine.Rendering.RenderQueue.Geometry + 1;
        }

        private const string kGeometryShaderPath = "Internal/Geometry";
        private const string kHandlesShaderPath = "Internal/Color";

        /// <summary>
        /// 材质数据结构
        /// </summary>
        public struct Materials
        {
            /// <summary>
            /// 辅助线材质
            /// </summary>
            public Material assist;
            public Material wall;
            public Material corner;
        }

        private static Materials s_DefaultMaterials;
        /// <summary>
        /// 默认材质
        /// </summary>
        public static Materials DefaultMaterials
        {
            get
            {
                if (s_DefaultMaterials.corner == null)
                {
                    s_DefaultMaterials.assist = new Material(Shader.Find(kHandlesShaderPath));

                    s_DefaultMaterials.wall = new Material(Shader.Find(kGeometryShaderPath));
                    s_DefaultMaterials.wall.renderQueue = DefaultRenderQueues.wall;

                    s_DefaultMaterials.corner = new Material(Shader.Find(kGeometryShaderPath));
                    s_DefaultMaterials.corner.renderQueue = DefaultRenderQueues.corner;
                }
                return s_DefaultMaterials;
            }
        }
    }
}
