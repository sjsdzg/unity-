using UnityEngine;
using System.Collections;
using XFramework.Common;
using System;

namespace XFramework.UI
{
    public class GroupLibraryPanel : ListViewCustom<GroupLibraryItem, GroupLibraryItemData>
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowItem(string name)
        {
            foreach (var item in m_Items)
            {
                if (item.Data.GroupInfo.Name.Equals(name))
                {
                    item.gameObject.SetActive(true);
                    break;
                }
            }
        }

        public void HideItem(string name)
        {
            foreach (var item in m_Items)
            {
                if (item.Data.GroupInfo.Name.Equals(name))
                {
                    item.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}

