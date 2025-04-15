using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    [Serializable]
    public class ImageList : MonoBehaviour
    {
        [SerializeField]
        public List<ImageInfo> imageInfos = new List<ImageInfo>();

        /// <summary>
        /// 根据名称，获取Sprite。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Sprite this[string key]
        {
            get
            {
                if (imageInfos == null)
                    return null;

                ImageInfo imageInfo = imageInfos.FirstOrDefault(x => x.name == key);

                if (imageInfo != null)
                {
                    return imageInfo.sprite;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (this[key] == null)
                return false;

            return true;
        }

        [ContextMenu("自动生成名称")]
        void AutoBuildName()
        {
            foreach (var imageInfo in imageInfos)
            {
                imageInfo.name = imageInfo.sprite.name;
            }
        }
    }
}
