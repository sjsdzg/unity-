using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Simulation.Base
{
    /// <summary>
    /// 元件类型
    /// </summary>
    public static class ElementType
    {
        //元件大类
        public const int DEVICE = 1001;//设备
        public const int VALVE = 1002;//阀门
        public const int PIPE = 1003;//管道
        public const int METER = 1004;//仪表

        //设备
        public const int Tank = 100101;//反应罐

        //各种罐
        public const int Seed_Tank = 10010101;//种子罐
        public const int Fermentation_tank = 10010102;//发酵罐
        public const int Feed_Tank = 10010103;//补料罐
        public const int Hold_Tank = 10010104;//菌体收集罐

        //阀门大类
        public const int Ball_Valve = 100201;//球阀
        public const int T_Type_Valve = 100202;//T形阀
        public const int Diaphragm_Valve = 100203;//隔膜阀
        public const int Tank_Bottom_Valve = 100204;//罐底阀

        //阀门细类
        public const int Ball_Valve_Handle = 10020101;//手动球阀
        public const int Ball_Valve_Gas = 10020102;//气动球阀
        public const int T_Type_Valve_Handle = 10020201;//手动T形阀
        public const int T_Type_Valve_Gas = 10020202;//气动T形阀
        public const int Diaphragm_Valve_Handle = 10020301;//手动隔膜阀
        public const int Diaphragm_Valve_Gas = 10020301;//气动隔膜阀
        public const int Tank_Bottom_Valve_Handle = 10020401;//手动隔膜阀
        public const int Tank_Bottom_Valve_Gas = 10020402;//气动隔膜阀

        //仪表
        public const int Digital_Meter = 100401;//数字仪表
        public const int Analog_Meter = 100402;//模拟仪表

        //其他组件
        public const int Liquid = 1005;//液体
        public const int Fluid = 1006;//流体
        public const int Agitator = 1007;//搅拌器
        public const int Effect = 1008;//特效
        public const int Lid = 1009;//反应釜盖子
        public const int Tank_Body = 1010;//罐体
        public const int Tank_Jacket = 1011;//罐夹套
        public const int Tank_Lid_Handle = 1012;//罐子盖子把手

        //特殊组件
        public const int MagnifyComponent = 1013;//放大镜组件
        public const int GlassDoorComponent = 1014;//玻璃门
        public const int BreathingBagComponent = 1015;//呼吸袋
        public const int BottlingVialDiskComponent = 1016;//理瓶盘
        public const int PowerSwitchComponent = 1017;//电源开关组件
        public const int ControlPanelComponent = 1018;//控制面板组件
        public const int SamplingVialsComponent = 1019;//取样西林瓶
        public const int GlassDoorMonitorComponent = 1020;//玻璃门监视器
        public const int PlugDiskComponent = 1021;
        public const int SurplusVialsComponent = 1022;//生产剩余西林瓶组件
        public const int SurplusPlugComponent = 1023;//生产剩余胶塞组件


        //通用组件 1024开始
        public const int HighlighterComponent = 1024;//高亮组件        
        public const int TwinklingComponent = 1025;//闪烁组件
        public const int HighlighterShowComponent = 1026;//特定时间闪烁组件
        public const int FollowComponent = 1027;//特定时间闪烁组件
    }
}
