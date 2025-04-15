using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Module
{
    public class Fault : DataObject<Fault>
    {
        /// <summary>
        /// 故障编号
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// 故障名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 故障现象
        /// </summary>
        [XmlAttribute("phenomena")]
        public string Phenomena { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
        [XmlAttribute("causes")]
        public string Causes { get; set; }

        /// <summary>
        /// 是否有学习模式
        /// </summary>
        [XmlAttribute("study")]
        public bool Study { get; set; }

        /// <summary>
        /// 是否有考核模式
        /// </summary>
        [XmlAttribute("examine")]
        public bool Examine { get; set; }

        /// <summary>
        /// url链接
        /// </summary>
        [XmlAttribute("url")]
        public string URL { get; set; }

        /// <summary>
        /// 附加信息
        /// 1.附加需要加载的车间类型
        /// 2.使用"|"隔开
        /// </summary>
        [XmlAttribute("workshops")]
        public string Workshops { get; set; }

        /// <summary>
        /// 实体列表
        /// </summary>
        [XmlArray("Entities")]
        [XmlArrayItem(typeof(EntityMyself)), XmlArrayItem(typeof(EntityNPC))]
        public List<EntityBase> Entities { get; set; }

        /// <summary>
        /// 处理序列列表
        /// </summary>
        [XmlArray("Sequences")]
        [XmlArrayItem("Sequence")]
        public List<Sequence> Sequences { get; set; }

        /// <summary>
        /// 考核点列表
        /// </summary>
        [XmlArray("AssessmentPoints")]
        [XmlArrayItem("AssessmentPoint")]
        public List<AssessmentPoint> AssessmentPoints { get; set; }

        /// <summary>
        /// 知识点列表
        /// </summary>
        [XmlArray("KnowledgePoints")]
        [XmlArrayItem("KnowledgePoint")]
        public List<KnowledgePoint> KnowledgePoints { get; set; }

        public _Action GetAction(int seqId, int actionId)
        {
            _Action action = Sequences[seqId - 1].Actions[actionId - 1];
            return action;
        }
    }
}
