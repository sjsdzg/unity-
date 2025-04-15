using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace XFramework.Component
{
  public   class UpdateMaterialComponent :MonoBehaviour
    {
        [SerializeField]
        private  Material oldMaterial;
        [SerializeField]
        private  Material newMaterial;

        public Material OldMaterial { get => oldMaterial; set => oldMaterial = value; }
        public Material NewMaterial { get => newMaterial; set => newMaterial = value; }
    }
}
