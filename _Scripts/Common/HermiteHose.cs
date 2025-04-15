using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HermiteHose : MonoBehaviour
{
    public List<GameObject> controlPoints = new List<GameObject>();

    private MeshFilter meshFilter;
    /// <summary>
    /// MeshFilter
    /// </summary>
    public MeshFilter MeshFilter
    {
        get
        {
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }
            return meshFilter;
        }
    }

    /// <summary>
    /// 分割段
    /// </summary>
    public int circle_segments = 10;

    /// <summary>
    /// hermite分割段
    /// </summary>
    public int hermite_segments = 20;

    /// <summary>
    /// 半径
    /// </summary>
    public float radius = 0.5f;

    /// <summary>
    /// mesh
    /// </summary>
    private Mesh mesh;


    private void Start()
    {
        mesh = new Mesh();
    }

    void Update()
    {
        // not enough points specified
        if (controlPoints == null || controlPoints.Count < 2) return;

        // loop over segments of spline
        Vector3 p0, p1, m0, m1;
        //vertices
        int vertices_count = (circle_segments + 1) * hermite_segments * (controlPoints.Count - 1) + (circle_segments + 1);
        Vector3[] vertices = new Vector3[vertices_count];

        //normals
        Vector3[] normals = new Vector3[vertices_count];

        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            // check control points
            if (controlPoints[i] == null ||
                controlPoints[i + 1] == null ||
                (i > 0 && controlPoints[i - 1] == null) ||
                (i < controlPoints.Count - 2 && controlPoints[i + 2] == null))
            {
                return;
            }
            // determine control points of segment
            p0 = controlPoints[i].transform.position - transform.position;
            p1 = controlPoints[i + 1].transform.position - transform.position;

            if (i > 0)
            {
                m0 = 0.5f * (controlPoints[i + 1].transform.position
                - controlPoints[i - 1].transform.position);
            }
            else
            {
                m0 = controlPoints[i + 1].transform.position
                    - controlPoints[i].transform.position;
            }
            if (i < controlPoints.Count - 2)
            {
                m1 = 0.5f * (controlPoints[i + 2].transform.position
                    - controlPoints[i].transform.position);
            }
            else
            {
                m1 = controlPoints[i + 1].transform.position
                    - controlPoints[i].transform.position;
            }

            // set points of Hermite curve
            float t = 0;
            float hermite_delta = 1.0f / hermite_segments;
            float circle_delta = 2 * Mathf.PI / circle_segments;
            float length = (i == controlPoints.Count - 2) ? hermite_segments + 1 : hermite_segments;

            for (int j = 0; j < length; j++)
            {
                Vector3 point = GetPoint(p0, m0, p1, m1, t);
                Vector3 normal = GetTangent(p0, m0, p1, m1, t);

                float angle = 0;
                for (int k = 0; k < circle_segments + 1; k++)
                {
                    int vi = (circle_segments + 1) * hermite_segments * i + (circle_segments + 1) * j + k;

                    Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, normal);
                    vertices[vi] = point + rotation * new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);

                    normals[vi] = vertices[vi] - point;

                    angle += circle_delta;
                }

                t += hermite_delta;
            }
        }

        // triangles
        int triangles_count = (controlPoints.Count - 1) * hermite_segments * circle_segments * 2 * 3;
        int[] triangles = new int[triangles_count];

        int index = 0;
        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            for (int j = 0; j < hermite_segments; j++)
            {
                for (int k = 0; k < circle_segments; k++)
                {
                    int vi = (circle_segments + 1) * hermite_segments * i + (circle_segments + 1) * j + k;
                    int nvi = (circle_segments + 1) * hermite_segments * i + (circle_segments + 1) * (j + 1) + k;

                    triangles[index] = vi;
                    triangles[index + 1] = vi + 1;
                    triangles[index + 2] = nvi;
                    triangles[index + 3] = vi + 1;
                    triangles[index + 4] = nvi + 1;
                    triangles[index + 5] = nvi;

                    index += 6;
                }
            }
        }

        // uv
        Vector2[] uvs = new Vector2[vertices_count];
        float v_space = 1.0f / hermite_segments;
        float u_space = 1.0f / circle_segments;

        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            float v = 0;
            for (int j = 0; j < hermite_segments + 1; j++)
            {
                float u = 0;
                for (int k = 0; k < circle_segments + 1; k++)
                {
                    int vi = (circle_segments + 1) * hermite_segments * i + (circle_segments + 1) * j + k;
                    uvs[vi] = new Vector2(u, v);
                    u += u_space;
                }
                v += v_space;
            }
        }

        // mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;

        MeshFilter.mesh = mesh;
    }


    public Vector3 GetPoint(Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1, float t)
    {
        Vector3 point = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
                    + (t * t * t - 2.0f * t * t + t) * m0
                    + (-2.0f * t * t * t + 3.0f * t * t) * p1
                    + (t * t * t - t * t) * m1;

        return point;
    }

    public Vector3 GetTangent(Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1, float t)
    {
        Vector3 tangent = (6.0f * t * t - 6 * t) * p0
                      + (3.0f * t * t - 4.0f * t + 1) * m0
                      + (-6.0f * t * t + 6.0f * t) * p1
                      + (3.0f * t * t - 2.0f * t) * m1;

        return tangent;
    }
}