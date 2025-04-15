using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class TransparentHelper
    {
        /// <summary>
        /// 渲染信息
        /// </summary>
        public class RendererInfo
        {
            public Renderer renderer;//物体渲染信息
            public List<Shader> shaders = new List<Shader>();//物体材质对应的shader列表
            public List<Color> colors = new List<Color>();//颜色列表
        }

        private static Dictionary<GameObject, List<RendererInfo>> RendererInfoDict = new Dictionary<GameObject, List<RendererInfo>>();//物体材质字典
        private static Shader DiffuseShader = Shader.Find("Legacy Shaders/Transparent/Diffuse");//透明shader

        /// <summary>
        /// 设置物体透明度
        /// </summary>
        /// <param name="obj"></param>
        public static void SetObjectAlpha(GameObject obj, float _alpha)
        {
            List<RendererInfo> rendererInfos = new List<RendererInfo>();
            

            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                RendererInfo item = new RendererInfo();//物体渲染信息
                item.renderer = renderer;

                int length = item.renderer.materials.Length;
                for (int i = 0; i < length; i++)
                {
                    item.shaders.Add(item.renderer.materials[i].shader);//物体材质对应的shader列表
                    if (item.renderer.materials[i].HasProperty("_Color"))
                    {
                        item.colors.Add(item.renderer.materials[i].color);
                    }
                    else
                    {
                        item.colors.Add(Color.clear);
                    }
                }
                rendererInfos.Add(item);
                //设置
                SetMaterialsColor(item.renderer, DiffuseShader, _alpha);//设置物体为透明
            }

            if (!RendererInfoDict.ContainsKey(obj))
            {
                RendererInfoDict.Add(obj, rendererInfos); 
            }
        }

        /// <summary>
        /// 设置物体颜色
        /// </summary>
        /// <param name="obj"></param>
        public static void SetObjectAlpha(GameObject obj, Color _color)
        {
            List<RendererInfo> rendererInfos = new List<RendererInfo>();

            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                RendererInfo item = new RendererInfo();//物体渲染信息
                item.renderer = renderer;

                int length = item.renderer.materials.Length;
                for (int i = 0; i < length; i++)
                {
                    item.shaders.Add(item.renderer.materials[i].shader);//物体材质对应的shader列表
                    item.colors.Add(item.renderer.materials[i].color);
                }
                rendererInfos.Add(item);
                //设置
                SetMaterialsColor(item.renderer, DiffuseShader, _color);//设置物体为透明
            }

            if (!RendererInfoDict.ContainsKey(obj))
            {
                RendererInfoDict.Add(obj, rendererInfos);
            }
        }

        /// <summary>
        /// 还原物体渲染信息
        /// </summary>
        /// <param name="obj"></param>
        public static void RestoreBack(GameObject obj)
        {
            List<RendererInfo> rendererInfos = null;
            if (!RendererInfoDict.TryGetValue(obj, out rendererInfos))
            {
                return;
            }

            foreach (var item in rendererInfos)
            {
                SetMaterialsColor(item.renderer, item.shaders, item.colors);
            }
            
            RendererInfoDict.Remove(obj);
        }

        /// <summary>
        /// 还原所有物体的渲染信息
        /// </summary>
        public static void RestoreBackAll()
        {
            foreach (var rendererInfos in RendererInfoDict.Values)
            {
                foreach (var item in rendererInfos)
                {
                    if (item.renderer == null)
                        continue;

                    SetMaterialsColor(item.renderer, item.shaders, item.colors);
                }
            }
            RendererInfoDict.Clear();
        }

        /// <summary>
        /// 修改物体所有材质 对应单一shader
        /// </summary>
        /// <param name="_renderer">材质</param>
        /// <param name="_shader">shader</param>
        /// <param name="_alpha">透明度</param>
        private static void SetMaterialsColor(Renderer _renderer, Shader _shader, float _alpha)
        {
            //获取当前物体的材质球
            int materialsNumber = _renderer.sharedMaterials.Length;
            for (int i = 0; i < materialsNumber; i++)
            {
                //换Shader或者修改材质
                _renderer.materials[i].shader = _shader;
                //获取当前材质球颜色
                Color color = _renderer.materials[i].color;
                //设置透明度 0-1 0 = 完全透明
                color.a = _alpha;
                //置当前材质球颜色
                if (color!=Color.clear)
                {
                    _renderer.materials[i].SetColor("_Color", color);
                }
            }
        }

        /// <summary>
        /// 修改物体所有材质 对应单一shader
        /// </summary>
        /// <param name="_renderer">材质</param>
        /// <param name="_shader">shader</param>
        /// <param name="_color">颜色</param>
        private static void SetMaterialsColor(Renderer _renderer, Shader _shader, Color _color)
        {
            //获取当前物体的材质球
            int materialsNumber = _renderer.sharedMaterials.Length;
            for (int i = 0; i < materialsNumber; i++)
            {
                //换Shader或者修改材质
                _renderer.materials[i].shader = _shader;
                //置当前材质球颜色
                _renderer.materials[i].SetColor("_Color", _color);
            }
        }

        /// <summary>
        /// 修改物体所有材质 对应shader列表
        /// </summary>
        /// <param name="_renderer">材质</param>
        /// <param name="_shader">shader</param>
        /// <param name="_alpha">透明度</param>
        private static void SetMaterialsColor(Renderer _renderer, List<Shader> _shaders, List<Color> _colors)
        {
            //获取当前物体的材质球
            int materialsNumber = _renderer.sharedMaterials.Length;
            if (materialsNumber != _shaders.Count)
            {
                Debug.LogError("材质数量和shader数量不对应！");
                return;
            }

            for (int i = 0; i < materialsNumber; i++)
            {
                //换Shader或者修改材质
                _renderer.materials[i].shader = _shaders[i];
                //置当前材质球颜色
                _renderer.materials[i].SetColor("_Color", _colors[i]);
            }
        }
    }
}
