using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Math;
using UnityEngine.UI;
using XFramework.Runtime;
using XFramework.Architectural;
using XFramework.Core;

public class MeshTest : MonoBehaviour
{
    protected static readonly MeshBuilder s_VertexHelper = new MeshBuilder();

    /// <summary>
    /// s_VertexHelperPool
    /// </summary>
    protected static readonly ObjectPool<MeshBuilder> s_VertexHelperPool = new ObjectPool<MeshBuilder>(null, x => x.Clear());

    /// <summary>
    /// s_MeshPool
    /// </summary>
    protected static readonly ObjectPool<Mesh> s_MeshPool = new ObjectPool<Mesh>(null, x => x.Clear());

    /// <summary>
    /// 置顶
    /// </summary>
    public bool TopMost { get; set; }

    private Color color = Color.white;
    private Matrix4x4 matrix = Matrix4x4.identity;
    private void Start()
    {
        MeshBuilder vh = s_VertexHelperPool.Spawn();
        Mesh mesh = s_MeshPool.Spawn();

       // vh.AddWireRect(new Vector3(length * -0.5f + thickness * 0.5f, 0, length * 0.5f), Quaternion.identity, size, Color);
        vh.AddWireArc(new Vector3(0, 0, 0), Vector3.up, 0, Mathf.PI * 0.5f, 10, Color.red);
        vh.FillMesh(mesh, MeshTopology.Lines);

        // 绘制网格
        matrix.SetTRS(new Vector3(0, 0, 0), Quaternion.identity, Vector3.one);
        Graphics.DrawMesh(mesh, matrix, ArchitectSettings.Door.material2d, 0);

        gameObject.GetComponent<MeshFilter>().mesh = mesh;

        s_VertexHelperPool.Despawn(vh);
        s_MeshPool.DespawnEndOfFrame(mesh);
    }

}
