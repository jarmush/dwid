using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    public SaveLoad saveLoadManager;
    public World world;

    public int chunkSize;
    public int xOffset;
    public int yOffset;

    //list of ground tiles prefabs
    public GameObject[] groundTiles;

    //array of spawned tiles gameobjects
    public GameObject[,] tilesInChunk;

    [SerializeField]
    public int[,] tilesMap;



    // Use this for initialization
    void Awake()
    {
        saveLoadManager = GameObject.Find("Game").GetComponent<SaveLoad>();
        world = GameObject.Find("World").GetComponent<World>();

         tilesInChunk = new GameObject[chunkSize, chunkSize];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.name + " collision at coord " + this.xOffset + " " + this.yOffset + " with " + collision.gameObject.name + " at " + Time.time);
        world.Repopulate(this.xOffset, this.yOffset);
    }

    public void CreateNew(int xPos, int yPos)
    {
        name = "Chunk " + xPos + " " + yPos;
        int xCenter = xPos * chunkSize;
        int yCenter = yPos * chunkSize;

        int xMin = xCenter - Mathf.FloorToInt(chunkSize / 2);
        int yMin = yCenter - Mathf.FloorToInt(chunkSize / 2);
        int xMax = xCenter + Mathf.FloorToInt((chunkSize - 1) / 2);
        int yMax = yCenter + Mathf.FloorToInt((chunkSize - 1) / 2);

        xOffset = xPos;
        yOffset = yPos;
        transform.position = transform.position + new Vector3(xCenter, yCenter, 0);
        tilesMap = new int[chunkSize, chunkSize];

        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                int tileToInstantiateIndex = Random.Range(0, groundTiles.Length);
                GameObject toInstantiate = groundTiles[tileToInstantiateIndex];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.parent = transform;
                instance.name = "tile" + x + "." + y;
                tilesInChunk[x - xMin, y - yMin] = instance;
                tilesMap[x - xMin, y - yMin] = tileToInstantiateIndex;
            }
        }

        Debug.Log("new chunk created");
        saveLoadManager.Save(this);
    }

}
