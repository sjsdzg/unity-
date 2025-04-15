using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Simulation;

namespace XFramework.Module
{
    /// <summary>
    /// 工段
    /// </summary>
    [XmlType("Stage")]
    public class Stage
    {
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 工段名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 工段类型
        /// </summary>
        [XmlAttribute("type")]
        public StageType Type { get; set; }

        /// <summary>
        /// 故障信息Url
        /// </summary>
        [XmlAttribute("faultInfoUrl")]
        public string FaultInfoUrl { get; set; }

        [XmlArray("Procedures")]
        [XmlArrayItem("Procedure")]
        public List<Procedure> Procedures { get; set; }

        [XmlArray("Faults")]
        [XmlArrayItem("Fault")]
        public List<Fault> Faults { get; set; }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="procedureType"></param>
        /// <returns></returns>
        public Procedure GetProcedure(ProcedureType procedureType)
        {
            Procedure procedure = null;
            try
            {
                procedure = Procedures.Find(x => x.Type == procedureType);
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
        /// <param name="faultID"></param>
        /// <returns></returns>
        public Fault GetFault(string faultID)
        {
            Fault fault = null;
            try
            {
                fault = Faults.Find(x => x.Id == faultID);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return fault;
        }

        /// <summary>
        /// 获取故障
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Fault GetFaultByName(string name)
        {
            Fault fault = null;
            try
            {
                fault = Faults.Find(x => x.Name == name);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return fault;
        }
    }
}
