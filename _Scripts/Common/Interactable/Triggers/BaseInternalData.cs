using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class BaseInternalData
    {
        public GameObject gameObject { get; set; }

        public BaseInternalData(GameObject _gameObject)
        {
            gameObject = _gameObject;
        }
    }
}
