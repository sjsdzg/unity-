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
    public class EngineeringDesignFluidManager : Singleton<EngineeringDesignFluidManager>
    {
        /// <summary>
        /// 管件
        /// </summary>
        private PipeFitting m_PipeFitting;

        /// <summary>
        /// 知识点
        /// </summary>
        private KnowledgePointCollection m_KnowledgePointCollection;

        /// <summary>
        /// 管道组件字典
        /// </summary>
        private Dictionary<string, PipeComponent> m_PipeComponents = new Dictionary<string, PipeComponent>();

        /// <summary>
        /// 流体组件字典
        /// </summary>
        private Dictionary<string, FluidComponent> m_FluidComponents = new Dictionary<string, FluidComponent>();

        /// <summary>
        /// 唯一的流体名字
        /// </summary>
        private List<string> uniqueFluidNames = new List<string>();

        /// <summary>
        /// 唯一的管道名字(key:管道名称  value: 管道所属的节点名称)
        /// </summary>
        private Dictionary<string, string> uniquePipeNames = new Dictionary<string, string>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configFluidPath">工程设计-管道流体配置表路径</param>
        /// <param name="configKnowledgePointPath">工程设计-流体对应的知识点配置表路径</param>
        /// <param name="pipeParent">所有管道的父物体</param>
        /// <param name="fluidParent">所有流体的父物体</param>
        public void Init(string configFluidPath,string configKnowledgePointPath, Transform pipeParent, Transform fluidParent)
        {
            m_KnowledgePointCollection = KnowledgePointCollection.Parser.ParseXmlFromResources(configKnowledgePointPath);
            m_PipeFitting = PipeFitting.Parser.ParseXmlFromResources(configFluidPath);
            if (m_PipeFitting != null)
            {
                foreach (FittingNode node in m_PipeFitting.FittingNodes)
                {
                    //uniquePipeNames
                    foreach (Pipe pipe in node.Pipes)
                    {
                        if (!uniquePipeNames.Keys.Contains(pipe.Name))
                        {
                            uniquePipeNames.Add(pipe.Name,node.Name);
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
            foreach (string pipeName in uniquePipeNames.Keys)
            {
                string parentName = uniquePipeNames[pipeName].Split('-')[0];
                string pipeType = uniquePipeNames[pipeName].Split('-')[1];
                Transform child = pipeParent.Find(parentName + "/" + pipeType + "/" +pipeName);
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
        /// 根据车间名称/具体流体名称，调用节点
        /// </summary>
        /// <param name="parentName">流体所属房间</param>
        /// <param name="fluidName">流体所属类型</param>
        /// <param name="fluidState">流体状态</param>
        public void InvokeFitting(string parentName, string fluidName, bool fluidState)
        {
            FittingNode node = m_PipeFitting.FittingNodes.Find(x => x.Name == parentName+"-"+fluidName);
            //EngineeringDesignFluidNode node = m_Fitting.FluidNodes.Find(x => x.ParentName == parentName);
            if (node != null)
            {
                //pipe
                foreach (Pipe pipe in node.Pipes)
                {
                    PipeComponent component = null;
                    if (m_PipeComponents.TryGetValue(pipe.Name, out component))
                    {
                        component.Transparent = fluidState;
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
                        if (fluidState)
                        {
                            component.Velocity = fluid.Velocity;
                        }
                        else
                        {
                            component.Velocity = 0;
                        }
                        Debug.LogFormat("流体:{0}设置为{1}", component.name, component.Velocity);
                    }
                }
            }
            else
            {
                Debug.LogError("没有该节点："+ parentName + "-" + fluidName);
            }
        }

        public void InvokeCloseAllFitting(string parentName, string fluidName, bool fluidState)
        {

        }

        /// <summary>
        /// 流体显示时候的描述
        /// </summary>
        /// <returns></returns>
        public string InvokeFittingInfo(string parentName, string fluidName)
        {
            string content = null;
            KnowledgePoint knowledgePoint = m_KnowledgePointCollection.KnowledgePoints.Find(x => x.Id == parentName+"-"+fluidName);
            if (knowledgePoint != null)
            {
                content = knowledgePoint.Description;
            }
            else
            {
                Debug.LogError("没有该知识点："+ parentName + "-" + fluidName);
            }
            return content;
        }
    }
}
