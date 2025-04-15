using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Architectural
{
    public struct RaycastInfo
    {
        private GameObject m_GameObject; // Game object hit by the raycast

        public GameObject gameObject
        {
            get { return m_GameObject; }
            set { m_GameObject = value; }
        }

        public float distance; // The distance from the origin this hit was.
        public int depth;
        // World-space position where a ray cast into the screen hits something
        public Vector3 worldPosition;
        // World-space normal where a ray cast into the screen hits something
        public Vector3 worldNormal;
        // screenPosition
        public Vector2 screenPosition;

        public bool isValid
        {
            get { return gameObject != null; }
        }

        public void Clear()
        {
            gameObject = null;
            distance = 0;
            depth = 0;
            worldNormal = Vector3.up;
            worldPosition = Vector3.zero;
            screenPosition = Vector2.zero;
        }

        public override string ToString()
        {
            if (!isValid)
                return "";

            return "Name: " + gameObject + "\n" +
                "distance: " + distance + "\n" +
                "depth: " + depth + "\n" +
                "worldNormal: " + worldNormal + "\n" +
                "worldPosition: " + worldPosition + "\n" +
                "screenPosition: " + screenPosition + "\n";
        }
    }
}
