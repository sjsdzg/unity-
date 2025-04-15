using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 粒子类型的知识点
    /// </summary>
    public class ParticleKnowledgePoint : BestViewKnowledgePoint
    {
        /// <summary>
        /// 粒子效果
        /// </summary>
        private ParticleSystem particle;

        protected override void OnAwake()
        {
            base.OnAwake();
            particle = transform.GetComponent<ParticleSystem>();
            particle.Stop();
        }

        public override void Display()
        {
           // base.Display();
            particle.Play();
        }

        public override void Close()
        {
            particle.Stop();
        }

    }
}
