using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Editor
{
    interface IExcelToUnity
    {
        void Convert(string excelPath, string savePath, Encoding encoding);
    }
}
