using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class UpdateRenderingModeAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 材质索引
        /// </summary>
        public int matIndex;

        /// <summary>
        /// 渲染模式
        /// </summary>
        public RenderingMode mode = RenderingMode.Opaque;

        public UpdateRenderingModeAction(GameObject _gameObject, RenderingMode _mode = RenderingMode.Opaque, int _matIndex = 0)
        {
            gameObject = _gameObject;
            matIndex = _matIndex;
            mode = _mode;
        }

        public override void Execute()
        {
            Material material = gameObject.GetComponent<Renderer>().materials[matIndex];
            SetupMaterialWithBlendMode(material, mode);
            //Completed
            Completed();
        }

        public void SetupMaterialWithBlendMode(Material material, RenderingMode mode)
        {
            material.SetFloat("_Mode", (int)mode);
            switch (mode)
            {
                case RenderingMode.Opaque:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case RenderingMode.Cutout:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 2450;
                    break;
                case RenderingMode.Fade:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
                case RenderingMode.Transparent:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
            }
        }
    }

    /// <summary>
    /// 渲染模式
    /// </summary>
    public enum RenderingMode : int
    {
        /// <summary>
        /// 不透明的
        /// </summary>
        Opaque = 0,
        /// <summary>
        /// 剪贴
        /// </summary>
        Cutout,
        /// <summary>
        /// 褪色的
        /// </summary>
        Fade,
        /// <summary>
        /// 透明的
        /// </summary>
        Transparent,
    }
}
