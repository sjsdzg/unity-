using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Diagram
{
    public static class GraphUtils
    {
        public static Vector2 GetPosition(Node node, Vector2 anchor)
        {
            anchor -= new Vector2(0.5f, 0.5f);
            Vector2 pos = (Vector2)node.Position + anchor * node.SizeDelta;
            return pos;
        }

        /// <summary>
        /// 获取一个 Widget Id
        /// </summary>
        /// <returns></returns>
        public static string GetWidgetID()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 根据 GameObject, 获取一个 Widget Id
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static string GetWidgetID(GameObject go)
        {
            string[] strs = go.name.Split('#');
            if (strs.Length != 2)
                return null;

            return strs[1];
        }
    }
}
