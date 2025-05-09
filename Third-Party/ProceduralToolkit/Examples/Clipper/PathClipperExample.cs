using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit.ClipperLib;

namespace ProceduralToolkit.Examples
{
    public class PathClipperExample : MonoBehaviour
    {
        public LineRenderer subjectRenderer;
        public LineRenderer clipRenderer;
        public LineRenderer outputRenderer;

        public MeshFilter meshFilter;

        private void Awake()
        {
            var subject = Geometry.StarPolygon2(5, 1, 2);
            SetVertices(subjectRenderer, subject);

            var clip = Geometry.PointsOnCircle2(1.5f, 50);
            SetVertices(clipRenderer, clip);

            var output = new List<List<Vector2>>();

            var clipper = new PathClipper();
            clipper.AddPath(subject, PolyType.ptSubject);
            clipper.AddPath(clip, PolyType.ptClip);
            clipper.Clip(ClipType.ctIntersection, ref output);
            SetVertices(outputRenderer, output[0]);

            Tessellator tessellator = new Tessellator();
            for (int i = 0; i < output.Count; i++)
            {
                List<Vector2> vector2s = output[i];
                tessellator.AddContour(vector2s);
            }

            tessellator.Tessellate();
            meshFilter.mesh = tessellator.ToMesh();
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
