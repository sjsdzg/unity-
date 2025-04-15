using UnityEngine;
using System.Collections;

namespace XFramework.UI
{
    public class PersonalLibraryPanel : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

