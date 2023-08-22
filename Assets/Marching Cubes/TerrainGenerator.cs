using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    // Settings
    public GenerationSettings g;

	public Dictionary<int, Terrain> Generate()
   {

        //Terrain Data
        Vector3Int worldSize = g.worldSize;

        //Terrains
        Dictionary<int, Terrain> terrains = new Dictionary<int, Terrain>();

        for (int n = 0; n < g.terrainCount; n++)
        {
            Vector3Int terrainSize = g.terrainSize;
            float seed1 = Random.Range(0, 100000);
            float seed2 = Random.Range(0, 100000);


            Terrain i = new Terrain();
            i.pos = new Vector3(Random.Range(0, worldSize.x), Random.Range(0, worldSize.y), Random.Range(0, worldSize.z));
            i.size = terrainSize;
            i.biome = 0;
            i.difficulty = (byte)Random.Range(0, 5);

            int sizeX = terrainSize.x;
            int sizeY = terrainSize.y;
            int sizeZ = terrainSize.z;


            int[,] bottomground = new int[sizeX, sizeZ];
            int surfaceheight = sizeY - 2;

            //Generate shape of terrain
            for (int x = 1; x < sizeX - 1; x++)
            {
                for (int z = 1; z < sizeZ - 1; z++)
                {
                    //if (Vector2.Distance(new Vector2(x, z), new Vector2(sizeX / 2, sizeZ / 2)) < (sizeX / 2.0f) * (0.5f + Mathf.PerlinNoise(seed1 + (float)x / sizeX, seed2 + (float)z / sizeX) / 2.0f))
                    if(g.shapemap.GetPixel(x,z).grayscale < 0.5f)
                    {
                        bottomground[x, z] = surfaceheight - 1;
                    }
                    else
                    {
                        bottomground[x, z] = sizeY;
                    }
                }
            }

            int minheight = surfaceheight - 1;

            //Generate bottom points of terrain
            for (int w = sizeY - 2; w > 0; w--)
            {
                int[,] nbg = (int[,])bottomground.Clone();
                for (int x = 1; x < sizeX - 1; x++)
                {
                    for (int z = 1; z < sizeZ - 1; z++)
                    {
                        if ((bottomground[x - 1, z - 1] == w) && (bottomground[x - 1, z] == w) && (bottomground[x - 1, z + 1] == w) && (bottomground[x, z - 1] == w) && (bottomground[x, z] == w) && (bottomground[x, z + 1] == w) && (bottomground[x + 1, z - 1] == w) && (bottomground[x + 1, z] == w) && (bottomground[x + 1, z + 1] == w))
                        {
                            nbg[x, z] -= 1;
                            minheight = w;
                        }   
                    }
                }
                bottomground = (int[,])nbg.Clone();
            }

            //Minimize y size
            sizeY = surfaceheight - minheight + 2;
            i.size.y = sizeY;
            Debug.Log(sizeY);


            i.blocks = new byte[sizeX * sizeY * sizeZ];

            //Generate block data
            for (int x = 1; x < sizeX - 1; x++)
            {
                for (int y = 1; y < sizeY - 1; y++)
                {
                    for (int z = 1; z < sizeZ - 1; z++)
                    {
                        int ny = y + minheight - 1;
                        //Blocks
                        if(ny <= surfaceheight && ny > bottomground[x,z])
                        {
                            i.blocks[(x * sizeY * sizeZ) + (y * sizeZ) + z] = 128;
                        }

                        //Textures
                        if (ny >= surfaceheight - 1)
                        {
                            i.blocks[(x * sizeY * sizeZ) + (y * sizeZ) + z] += 1;
                        }
                        else
                        {
                            i.blocks[(x * sizeY * sizeZ) + (y * sizeZ) + z] += 2;
                        }
                    }
                }
            }
            for (int x = 1; x < sizeX - 1; x++)
            {
                for (int z = 1; z < sizeZ - 1; z++)
                {
                    i.blocks[(x * sizeY * sizeZ) + (0 * sizeZ) + z] += 2;
                }
            }
            Debug.Log(n);
            Debug.Log(i);
            terrains.Add(n, i);
        }

        return terrains;
    }
}

public class Terrain
{
    public Vector3 pos;
    public Vector3Int size;
    public byte[] blocks;
    //public byte[] textures;
    public byte biome;
    public byte difficulty;
    public bool downloaded = true;
}