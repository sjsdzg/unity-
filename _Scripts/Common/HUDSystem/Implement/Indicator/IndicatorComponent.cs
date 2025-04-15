using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    public class IndicatorComponent : HUDComponent<IndicatorInfo>
    {
        public override void show()
        {
            if (HasCreated)
            {
                hudInfo.onScreenArgs.visible = true;
                hudInfo.offScreenArgs.visible = true;
            }
            else
            {
                CreateHUD();
            }

        }

        public override void hide()
        {
            if (HasCreated)
            {
                hudInfo.onScreenArgs.visible = false;
                hudInfo.offScreenArgs.visible = false;
            }
        }
    }
}
