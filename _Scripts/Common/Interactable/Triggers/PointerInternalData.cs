using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class PointerInternalData : BaseInternalData
    {
        public InputButton button { get; set; }

        public PointerInternalData(GameObject _gameObject, InputButton _button) : base(_gameObject)
        {
            button = _button;
        }
    }
}
