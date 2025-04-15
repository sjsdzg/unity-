using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.Module
{
    [XmlType("Project")]
    public class Project : DataObject<Project>
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("Stages")]
        [XmlArrayItem("Stage")]
        public List<Stage> Stages { get; set; }

        /// <summary>
        /// 获取工段
        /// </summary>
        /// <param name="stageType"></param>
        /// <returns></returns>
        public Stage GetStage(StageType stageType)
        {
            Stage stage = null;
            try
            {
                stage = Stages.Find(x => x.Type == stageType);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return stage;
        }

        /// <summary>
        /// 获取工段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Stage GetStage(string name)
        {
            Stage stage = null;
            try
            {
                stage = Stages.Find(x => x.Name == name);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return stage;
        }

        /// <summary>
        /// 根据工段id（工段id是按顺序排列），获取工段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Stage GetStageById(string id)
        {
            Stage stage = null;
            try
            {
                stage = Stages.Find(x => x.ID == id);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return stage;
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="stageType"></param>
        /// <param name="procedureType"></param>
        /// <returns></returns>
        public Procedure GetProcedure(StageType stageType, ProcedureType procedureType)
        {
            Procedure procedure = null;
            try
            {
                Stage stage = GetStage(stageType);
                procedure = stage.GetProcedure(procedureType);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return procedure;
        }

        /// <summary>
        /// 获取故障
        /// </summary>
        /// <param name="stageType"></param>
        /// <param name="faultID"></param>
        /// <returns></returns>
        public Fault GetFault(StageType stageType, string faultID)
        {
            Fault fault = null;
            try
            {
                Stage stage = GetStage(stageType);
                fault = stage.GetFault(faultID);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return fault;
        }
    }
}
