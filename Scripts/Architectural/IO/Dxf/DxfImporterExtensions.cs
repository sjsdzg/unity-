using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Architectural
{
    public static class DxfImporterExtensions
    {
        /// <summary>
        /// 更新事件
        /// </summary>
        /// <param name="importer"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static DxfImporter OnUpdate(this DxfImporter importer, Action<DxfImporter> action)
        {
            importer.onUpdate += action;
            return importer;
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        /// <param name="importer"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static DxfImporter OnCompleted(this DxfImporter importer, Action<DxfImporter> action)
        {
            importer.onCompleted += action;
            return importer;
        }
    }
}
