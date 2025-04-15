using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class PathOffsetterExample : MonoBehaviour
    {
        public LineRenderer inputRenderer;
        public LineRenderer positiveOutputRenderer;
        public LineRenderer negativeOutputRenderer;

        private void Awake()
        {
            //var input = Geometry.StarPolygon2(5, 1, 2);
            var input = new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 0), new Vector2(0, 1 ) };
            //var input1 = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0) };
            //var input2 = new List<Vector2>() { new Vector2(0, 0), new Vector2(-1, -3) };

            SetVertices(inputRenderer, input);

            var output = new List<List<Vector2>>();

            var offsetter = new PathOffsetter();
            offsetter.AddPath(input, ClipperLib.JoinType.jtMiter, ClipperLib.EndType.etOpenButt);
            //offsetter.AddPath(input1, ClipperLib.JoinType.jtMiter, ClipperLib.EndType.etOpenButt);
            //offsetter.AddPath(input2, ClipperLib.JoinType.jtMiter, ClipperLib.EndType.etOpenButt);
            offsetter.Offset(ref output, 0.1);
            SetVertices(positiveOutputRenderer, output[0]);
            //SetVertices(positiveOutputRenderer, output[1]);

            //offsetter.Offset(ref output, -0.5);
            //SetVertices(negativeOutputRenderer, output[0]);
        }

        private void SetVertices(LineRenderer lineRenderer, List<Vector2> vertices)
        {
            lineRenderer.positionCount = vertices.Count;
            for (int i = 0; i < vertices.Count; i++)
            {
                lineRenderer.SetPosition(i, vertices[i]);
            }
        }
    }
}
