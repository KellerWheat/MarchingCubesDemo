using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TerrainMesh : MonoBehaviour {
    public static System.Diagnostics.Stopwatch s = System.Diagnostics.Stopwatch.StartNew();
    public int terrainIndex;
    public Vector3Int size;
    public TerrainController world;

    public int blocksize;

    public Material m_material;

    List<GameObject> meshes = new List<GameObject>();

    void Start () {
        world = GameObject.FindGameObjectWithTag("World").GetComponent<TerrainController>();
        size = world.terrainData[terrainIndex].size;
        Generate();
    }

    public void Generate()
    {
        byte[] Voxels = world.terrainData[terrainIndex].blocks;
        //byte[] Textures = world.terrainData[terrainIndex].textures;
        Vector3 size = world.terrainData[terrainIndex].size;

        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        //The mesh produced is not optimal. There is one vert for each index.
        //Would need to weld vertices for better quality mesh

        long starttime = s.ElapsedMilliseconds;

        Mesh tempmesh = MarchingCubes.CreateMesh(verts, indices, uvs, Voxels, world.terrainData[terrainIndex].size.x, world.terrainData[terrainIndex].size.y, world.terrainData[terrainIndex].size.z);
        //Mesh tempmesh = MarchingCubes.CreateMesh(verts, indices, uvs, Voxels, Textures, world.terrainData[terrainIndex].size.x, world.terrainData[terrainIndex].size.y, world.terrainData[terrainIndex].size.z);

        Debug.Log("Loaded in " + (s.ElapsedMilliseconds - starttime) + "ms");

        verts.AddRange(tempmesh.vertices);
        for (int x = 0; x < tempmesh.subMeshCount; x++)
        {
            indices.AddRange(tempmesh.GetIndices(x));
        }
        uvs.AddRange(tempmesh.uv);

        //A mesh in unity can only be made up of 65000 verts.
        //Need to split the verts between multiple meshes.

        int maxVertsPerMesh = 30000; //must be divisible by 3, ie 3 verts == 1 triangle
        int numMeshes = verts.Count / maxVertsPerMesh + 1;
        for (int i = 0; i < numMeshes; i++)
        {

            List<Vector3> splitVerts = new List<Vector3>();
            List<int> splitIndices = new List<int>();
            List<Vector2> splitUvs = new List<Vector2>();

            for (int j = 0; j < maxVertsPerMesh; j++)
            {
                int idx = i * maxVertsPerMesh + j;

                if (idx < verts.Count)
                {
                    splitVerts.Add(verts[idx]);
                    splitIndices.Add(j);
                    splitUvs.Add(uvs[idx]);
                }
            }

            if (splitVerts.Count == 0) continue;

            Mesh mesh = new Mesh();

            mesh.SetVertices(splitVerts);
            mesh.SetTriangles(splitIndices, 0);
            mesh.uv = splitUvs.ToArray();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            GameObject go = new GameObject("Mesh");
            go.transform.parent = transform;
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.AddComponent<MeshCollider>();
            go.GetComponent<Renderer>().material = m_material;
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.GetComponent<MeshCollider>().sharedMesh = mesh;
            go.transform.localPosition = new Vector3(-(size.x * blocksize) / 2, -(size.y * blocksize) / 2, -(size.z * blocksize) / 2);
            go.transform.localScale = new Vector3(blocksize, blocksize, blocksize);

            meshes.Add(go);
        }
    }
}
