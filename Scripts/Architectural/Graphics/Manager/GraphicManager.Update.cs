using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Architectural
{
    public partial class GraphicManager
    {

        private readonly List<GraphicObject> m_GraphicRebuildQueue = new List<GraphicObject>();

        private bool ObjectValidForUpdate(IGraphicObject graphic)
        {
            var valid = graphic != null;

            var isUnityObject = graphic is UnityEngine.Object;
            if (isUnityObject)
                valid = (graphic as UnityEngine.Object) != null;

            return valid;
        }

        private void CleanInvalidGraphics()
        {
            for (int i = m_GraphicRebuildQueue.Count - 1; i >= 0; --i)
            {
                var item = m_GraphicRebuildQueue[i];
                if (item == null)
                {
                    m_GraphicRebuildQueue.RemoveAt(i);
                }
            }
        }

        public static void RegisterGraphicForRebuild(GraphicObject graphic)
        {
            Instance.InternalRegisterGraphicForRebuild(graphic);
        }

        public static bool TryRegisterGraphicForRebuild(GraphicObject graphic)
        {
            return Instance.InternalRegisterGraphicForRebuild(graphic);
        }

        private bool InternalRegisterGraphicForRebuild(GraphicObject graphic)
        {
            if (m_GraphicRebuildQueue.Contains(graphic))
                return false;

            m_GraphicRebuildQueue.Add(graphic);

            return true;
        }

        public static void UnRegisterGraphicForRebuild(GraphicObject graphic)
        {
            Instance.InternalUnRegisterGraphicForRebuild(graphic);
        }

        private void InternalUnRegisterGraphicForRebuild(GraphicObject graphic)
        {
            m_GraphicRebuildQueue.Remove(graphic);
        }
    }
}
