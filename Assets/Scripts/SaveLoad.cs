using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    public void Save(World world)
    {
        foreach (Chunk chunk in world.spawnedChunkList)
        {
            Save(chunk);
        }

        Debug.Log("world is saved");
    }

    public void Save(Game game)
    {
        Save(game.player);
        Save(game.world);
        Debug.Log("game is saved");
    }

    public void Save(Player player)
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        /*
        //save inventory
        Debug.Log("starting to save " + Application.persistentDataPath + "/player/" + player.name + ".inventory");
        file = File.Create(Application.persistentDataPath + "/player/" + player.name + ".inventory");
        bf.Serialize(file, player.inventory.inventory);
        file.Close();
        */

        //save position
        //TODO make player name dependent
        Debug.Log("starting to save " + Application.persistentDataPath + "/player/last.position");
        file = File.Create(Application.persistentDataPath + "/player/last.position");
        float[] playerCoords = new float[2];
        playerCoords[0] = player.transform.position.x;
        playerCoords[1] = player.transform.position.y;
        bf.Serialize(file, playerCoords);
        file.Close();

        Debug.Log("player is saved");
    }

    public void Save(Chunk chunk)
    {

        BinaryFormatter bf = new BinaryFormatter();

        //save tiles
        Debug.Log("starting to save " + Application.persistentDataPath + "/chunks/" + chunk.name);
        FileStream file = File.Create(Application.persistentDataPath + "/chunks/" + chunk.name);
        bf.Serialize(file, chunk.tilesMap);
        file.Close();

        /*
        //save objects
        Debug.Log("starting to save " + Application.persistentDataPath + "/chunks/" + chunk.name + ".objects. itemList is " + chunk.spawnedItemsList.Count + " items");
        file = File.Create(Application.persistentDataPath + "/chunks/" + chunk.name + ".objects");
        bf.Serialize(file, chunk.spawnedItemsList);
        file.Close();
        */

        Debug.Log("chunk is saved");
    }




    public void Load(World world)
    {

        if (File.Exists(Application.persistentDataPath + "/player/last.position"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/player/last.position", FileMode.Open);
            float[] playerCoords = new float[2];
            playerCoords = (float[])bf.Deserialize(file);
            file.Close();

            int chunkSize = world.chunkPrefab.GetComponent<Chunk>().chunkSize;

            int xWorldOffset = (int)(playerCoords[0] / chunkSize);
            int yWorldOffset = (int)(playerCoords[1] / chunkSize);

            for (int x = xWorldOffset - 1; x <= xWorldOffset + 1; x++)
            {
                for (int y = yWorldOffset - 1; y <= yWorldOffset + 1; y++)
                {
                    Debug.Log("try to instantiate " + world.chunkPrefab);
                    Chunk newChunk = Instantiate(world.chunkPrefab).GetComponent<Chunk>();
                    newChunk.name = "Chunk " + x + " " + y;
                    newChunk.xOffset = x;
                    newChunk.yOffset = y;
                    Load(newChunk);
                    newChunk.transform.parent = world.transform;
                    world.spawnedChunkList.Add(newChunk);
                }
            }
            Debug.Log("old world loaded");
        }
    }

    public void Load(Game game)
    {
        Load(game.world);
        Load(game.player);
        Debug.Log("old game loaded");
    }

    public void Load(Player player)
    {
        if (File.Exists(Application.persistentDataPath + "/player/" + player.name + ".inventory"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;

            /*
            Debug.Log("starting to load " + Application.persistentDataPath + "/player/" + player.name + ".inventory");
            FileStream file = File.Open(Application.persistentDataPath + "/player/" + player.name + ".inventory", FileMode.Open);
            player.inventory.inventory = (List<InventoryItem>)bf.Deserialize(file);
            file.Close();
            Debug.Log("old player is loaded");
            */
        }

        if (File.Exists(Application.persistentDataPath + "/player/last.position"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            Debug.Log("starting to load " + Application.persistentDataPath + "/player/last.position");
            FileStream file = File.Open(Application.persistentDataPath + "/player/last.position", FileMode.Open);
            float[] playerCoords = new float[2];
            playerCoords = (float[])bf.Deserialize(file);
            file.Close();
            player.transform.position = new Vector3(playerCoords[0], playerCoords[1], 0);
            Debug.Log("old player is loaded");
        }
    }

    public void Load(Chunk chunk)
    {

        //check autocreating and autoplacing on load
        if (Exists(chunk))
        {
            BinaryFormatter bf = new BinaryFormatter();

            Debug.Log("starting to load " + Application.persistentDataPath + "/chunks/" + chunk.name);
            chunk.tilesMap = new int[chunk.chunkSize, chunk.chunkSize];
            FileStream file = File.Open(Application.persistentDataPath + "/chunks/" + chunk.name, FileMode.Open);
            chunk.tilesMap = (int[,])bf.Deserialize(file);
            file.Close();

            Debug.Log("loaded " + Application.persistentDataPath + "/chunks/" + chunk.name);

            int xCenter = chunk.xOffset * chunk.chunkSize;
            int yCenter = chunk.yOffset * chunk.chunkSize;

            int xMin = xCenter - Mathf.FloorToInt(chunk.chunkSize / 2);
            int yMin = yCenter - Mathf.FloorToInt(chunk.chunkSize / 2);
            int xMax = xCenter + Mathf.FloorToInt((chunk.chunkSize - 1) / 2);
            int yMax = yCenter + Mathf.FloorToInt((chunk.chunkSize - 1) / 2);

            chunk.transform.position = chunk.transform.position + new Vector3(xCenter, yCenter, 0);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    int tileToInstantiateIndex = chunk.tilesMap[x - xMin, y - yMin];
                    GameObject toInstantiate = chunk.groundTiles[tileToInstantiateIndex];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(chunk.transform);
                }
            }

            /*
            Debug.Log("starting to load " + Application.persistentDataPath + "/chunks/" + chunk.name + ".objects");
            chunk.spawnedItemsList = new List<WorldItem>();
            file = File.Open(Application.persistentDataPath + "/chunks/" + chunk.name + ".objects", FileMode.Open);
            List<WorldItem> enumeratorWorldItem = (List<WorldItem>)bf.Deserialize(file);
            file.Close();

            Debug.Log("load function itemManager found as " + GameObject.Find("Game").GetComponent<ItemManager>() + ". itemlist is " + enumeratorWorldItem.Count + " items");
            foreach (WorldItem item in enumeratorWorldItem)
            {
                Debug.Log("spawning " + item.itemName + " at " + item.xPos + " " + item.yPos);

                //TODO rewrite that fuck!

                itemManager.SpawnNewItem(itemManager.FindItem(item.itemName), item.itemQuallity, item.xPos, item.yPos);
            }
            */

        }
        else
        {
            Debug.Log("something goes wrong");
        }

        Debug.Log("old chunk is loaded");
    }

    public void ClearSave()
    {
        if (Directory.Exists(Application.persistentDataPath + "/player"))
        {
            Directory.Delete(Application.persistentDataPath + "/player", true);
        }
        if (Directory.Exists(Application.persistentDataPath + "/chunks"))
        {
            Directory.Delete(Application.persistentDataPath + "/chunks", true);
        }
        Directory.CreateDirectory(Application.persistentDataPath + "/player");
        Directory.CreateDirectory(Application.persistentDataPath + "/chunks");

    }

    public bool Exists(Chunk chunk)
    {
        if (File.Exists(Application.persistentDataPath + "/chunks/" + chunk.name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
