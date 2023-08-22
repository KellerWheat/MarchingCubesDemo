using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GenerationSettings")]
public class GenerationSettings : ScriptableObject
{
    public int terrainCount;
    public Vector3Int terrainSize;
    public Vector3Int worldSize;
    public Texture2D shapemap;
}
