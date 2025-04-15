using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.UI
{
    public class ButtonCreateDirCell : ButtonCell
    {
        public override ButtonCellType GetButtonCellType()
        {
            return ButtonCellType.CreateDir;
        }
    }
}
