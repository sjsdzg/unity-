using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Text;

namespace XFramework.Module
{
    public class EquipmentAssemblyModule : BaseModule
    {
        private Equipment m_Equipment;
        /// <summary>
        /// 设备信息
        /// </summary>
        public Equipment Equipment
        {
            get { return m_Equipment; }
            set { m_Equipment = value; }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path"></param>
        public void Init(string path)
        {
            m_Equipment = Equipment.LoadFromResources(path, Encoding.UTF8);
        }
    }

}
