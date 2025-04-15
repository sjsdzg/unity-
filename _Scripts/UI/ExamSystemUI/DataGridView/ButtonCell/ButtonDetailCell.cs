using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.UI
{
    public class ButtonDetailCell : ButtonCell
    {
        public override ButtonCellType GetButtonCellType()
        {
            return ButtonCellType.Detail;
        }
    }
}
