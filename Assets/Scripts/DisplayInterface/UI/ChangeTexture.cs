using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataBase;

public class ChangeTexture : MonoBehaviour
{
    private Texture selfTexture;
    private Material selfMaterial;
    private MeshFilter SelfMesh;
    private MeshCollider Collider;
    public float Ratio;
    List<SingleCurtain> m_data = new List<SingleCurtain>();
    public float x;
    public float y;
    public float z;

    void Start()
    {
        selfMaterial = this.GetComponent<Renderer>().material;
        SelfMesh = GetComponent<MeshFilter>();
        Collider = GetComponent<MeshCollider>();
        
        x = SelfMesh.mesh.bounds.size.x;
        y = SelfMesh.mesh.bounds.size.y;
    }

    public void SetData(List<SingleCurtain> Data)
    {
        m_data = Data;
    }

    public void ReceiveMessage(Texture texture)
    {
        selfTexture = texture;
        ChangePicture(selfTexture);
    }

    public void ReceiveMessage(bool IsActive)
    {
        Debug.Log(name + "  " + IsActive);
        gameObject.SetActive(IsActive);
    }
    public void ReceiveMessage(Mesh mesh, Texture texture)
    {
        SelfMesh.mesh = ChangeMesh(x, y, z);
        //Collider.sharedMesh = mesh;

        selfMaterial = this.GetComponent<Renderer>().material;
        selfMaterial.shader = Resources.Load<Shader>("VertexLit");
        selfMaterial.mainTexture = texture;
        GetComponent<CurtainManager>().Material = this.selfMaterial;
        GetComponent<CurtainManager>().IsModel = false;
    }

    void ChangePicture(Texture texture)
    {
        selfMaterial.mainTexture = texture;
    }

    #region mesh
    private Vector3[] m_vertices;
    private Vector2[] m_uv;
    private Color[] m_color;
    private Vector3[] m_normals;
    private int[] m_triangles;
    #endregion

    Mesh ChangeMesh(float height, float width, float depth)
    {
        //Mesh mesh = MsgCenter._instance.AssetMesh;
        Mesh mesh = new Mesh();
        mesh.name = "myPlane";
        m_vertices = new Vector3[4]
        {
            new Vector3(-height/2, -width, depth), // 左下
            new Vector3(height/2, -width, depth), // 右下
            new Vector3(-height/2, 0, depth), // 左上
            new Vector3(height/2, 0, depth), // 右上
        };


        m_uv = new Vector2[4] // 顶点映射的uv位置
        {
            new Vector3(0, 0), // 顶点0的纹理坐标
            new Vector3(1, 0),
            new Vector3(0, 1),
            new Vector3(1, 1),
        };

        m_triangles = new int[6] // 两个三角面的连接
        {
            0,1,2,// 通过顶点012连接形成的三角面
            1,3,2,// 通过顶点132连接形成的三角面
        };

        mesh.Clear();
        mesh.vertices = m_vertices;
        mesh.uv = m_uv;
        mesh.colors = m_color;
        mesh.normals = m_normals;
        mesh.triangles = m_triangles;
        mesh.RecalculateNormals();

        return mesh;
    }



}
