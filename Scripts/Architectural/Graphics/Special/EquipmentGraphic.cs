using UnityEngine;
using System.Collections;

namespace XFramework.Architectural
{
    public class EquipmentGraphic : GraphicObject
    {
        private Equipment equipment;

        public Equipment Equipment
        {
            get { return equipment; }
            set 
            { 
                equipment = value;
                equipment.TransformChanged += Equipment_TransformChanged;
            }
        }

        private void Equipment_TransformChanged()
        {
            SetTranformDirty();
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            transform.position = equipment.Position;
        }
    }

}
