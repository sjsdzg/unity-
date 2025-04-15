using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Simulation
{
    /// <summary>
    /// 离心车间
    /// </summary>
    public partial class CentrifugeWorkshop : CustomWorkshop
    {
        public override EnumWorkshopType GetWorkshopType()
        {
            return EnumWorkshopType.CentrifugeWorkshop;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void InitStandard()
        {
            base.InitStandard();
            InitializeComponent();
            InitializeOperate();
        }
    }
}
