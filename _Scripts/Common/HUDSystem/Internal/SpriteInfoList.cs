using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class SpriteInfoList
    {
        [SerializeField]
        public List<SpriteInfo> spriteInfos = new List<SpriteInfo>();

        /// <summary>
        /// 根据名称，获取Sprite。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Sprite this[string key]
        {
            get
            {
                if (spriteInfos == null)
                    return null;

                SpriteInfo imageInfo = spriteInfos.FirstOrDefault(x => x.id == key);

                if (imageInfo != null)
                {
                    return imageInfo.sprite;
                }
                else
                    return null;
            }
        }
    }
}
