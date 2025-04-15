using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.Simulation
{
    public class PipeFittingManager : Singleton<PipeFittingManager>
    {
        /// <summary>
        /// 当前SeqID
        /// </summary>
        public int CurrentSeqId { get; set; }

        /// <summary>
        /// 当前ActionId
        /// </summary>
        public int CurrentActionId { get; set; }

        /// <summary>
        /// 管件
        /// </summary>
        private PipeFitting m_PipeFitting;

        /// <summary>
        /// 管道组件字典
        /// </summary>
        private Dictionary<string, PipeComponent> m_PipeComponents = new Dictionary<string, PipeComponent>();

        /// <summary>
        /// 流体组件字典
        /// </summary>
        private Dictionary<string, FluidComponent> m_FluidComponents = new Dictionary<string, FluidComponent>();

        /// <summary>
        /// uniquePipeNames
        /// </summary>
        private List<string> uniquePipeNames = new List<string>();

        /// <summary>
        /// uniqueFluidNames
        /// </summary>
        private List<string> uniqueFluidNames = new List<string>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configPath">管道流体配置表</param>
        /// <param name="pipeParent">管道父节点</param>
        /// <param name="fluidParent">流体父节点</param>
        public void Init(string configPath, Transform pipeParent, Transform fluidParent)
        {
            m_PipeFitting = PipeFitting.Parser.ParseXmlFromResources(configPath);
            //m_PipeFitting = PipeFitting.Load(configPath, Encoding.UTF8);
            if (m_PipeFitting != null)
            {
                foreach (FittingNode node in m_PipeFitting.FittingNodes)
                {
                    //uniquePipeNames
                    foreach (Pipe pipe in node.Pipes)
                    {
                        if (!uniquePipeNames.Contains(pipe.Name))
                        {
                            uniquePipeNames.Add(pipe.Name);
                        }
                    }
                    //uniqueFluidNames
                    foreach (Fluid fluid in node.Fluids)
                    {
                        if (!uniqueFluidNames.Contains(fluid.Name))
                        {
                            uniqueFluidNames.Add(fluid.Name);
                        }
                    }
                }
                //Loading
                CoroutineManager.Instance.StartCoroutine(Loading(pipeParent, fluidParent));
            }
            else
            {
                Debug.LogError("管道流体配置表路径不存在或配置表为空！");
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="pipeParent"></param>
        /// <param name="fluidParent"></param>
        /// <returns></returns>
        IEnumerator Loading(Transform pipeParent, Transform fluidParent)
        {
            yield return new WaitForEndOfFrame();
            foreach (string pipeName in uniquePipeNames)
            {
                Transform child = pipeParent.Find(pipeName);
                if (child != null)
                {
                    PipeComponent component = child.GetOrAddComponent<PipeComponent>();
                    m_PipeComponents.Add(child.name, component);
                }
                else
                {
                    Debug.LogErrorFormat("未查找管道：{0}", pipeName);
                }
            }
            yield return new WaitForEndOfFrame();
            foreach (string fluidName in uniqueFluidNames)
            {
                Transform child = fluidParent.Find(fluidName);
                if (child != null)
                {
                    FluidComponent component = child.GetOrAddComponent<FluidComponent>();
                    m_FluidComponents.Add(child.name, component);
                }
                else
                {
                    Debug.LogErrorFormat("未查找流体：{0}", fluidName);
                }
            }
        }

        /// <summary>
        /// 调用当前节点管道和流体
        /// </summary>
        public void InvokeCurrentFitting()
        {
            string name = string.Format("{0}-{1}", CurrentSeqId, CurrentActionId);
            InvokeFitting(name);
        }

        /// <summary>
        /// 调用当前节点管道和流体
        /// </summary>
        public void InvokeCurrentFitting(int number)
        {
            string name = string.Format("{0}-{1}-{2}", CurrentSeqId, CurrentActionId, number);
            InvokeFitting(name);
        }

        /// <summary>
        /// 根据节点名称，调用节点
        /// </summary>
        /// <param name="nodeName"></param>
        public void InvokeFitting(string nodeName)
        {
            FittingNode node = m_PipeFitting.FittingNodes.Find(x => x.Name == nodeName);
            if (node != null)
            {
                //pipe
                foreach (Pipe pipe in node.Pipes)
                {
                    PipeComponent component = null;
                    if (m_PipeComponents.TryGetValue(pipe.Name, out component))
                    {
                        component.Transparent = pipe.Transparent;
                        Debug.LogFormat("管道:{0}设置为透明", component.name);
                    }
                }
                //Fluid
                foreach (Fluid fluid in node.Fluids)
                {
                    FluidComponent component = null;
                    if (m_FluidComponents.TryGetValue(fluid.Name, out component))
                    {
                        component.UV = Converter.ToVector2(fluid.UV);
                        component.Velocity = fluid.Velocity;
                        Debug.LogFormat("流体:{0}设置为{1}", component.name, component.Velocity);
                    }
                }
            }
        }

        /// <summary>
        /// 关闭所有
        /// </summary>
        public void CloseAll()
        {
            foreach (var key in m_PipeComponents.Keys)
            {
                m_PipeComponents[key].Transparent = false;
            }

            foreach (var key in m_FluidComponents.Keys)
            {
                m_FluidComponents[key].Velocity = 0;
            }
        }

        /// <summary>
        /// 测试流体方向
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="pipeParent"></param>
        /// <param name="fluidParent"></param>
        public void TestPipeFitting()
        {
            foreach (FluidComponent item in m_FluidComponents.Values)
            {
                Debug.Log("流体名称：" + item.name);
                item.UV = Converter.ToVector2("0,1");
                item.Velocity = 1;
            }
            foreach (PipeComponent item in m_PipeComponents.Values)
            {
                item.Transparent = true;
                Debug.Log("管道名称：" + item.name);
            }
        }

    }
}
