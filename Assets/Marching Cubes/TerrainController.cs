using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TerrainController : MonoBehaviour
{
    public GameObject terrainPrefab;

    public Dictionary<int, Terrain> terrainData;

    public Dictionary<int, GameObject> terrains;

    public ArrayList _terrains = new ArrayList(); 

    public TerrainGenerator generator;

    public void Start()
    {
        terrainData = generator.Generate();
        foreach (KeyValuePair<int,Terrain> i in terrainData)
        {
            Debug.Log("REE");
            _terrains.Add(i.Value);
            GameObject c = GameObject.Instantiate(terrainPrefab, i.Value.pos, transform.rotation);
            TerrainMesh ni = c.GetComponent<TerrainMesh>();
            ni.terrainIndex = i.Key;
            terrains.Add(i.Key, ni.gameObject);
        }
	}
}
