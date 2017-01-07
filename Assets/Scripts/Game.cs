using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject worldPrefab;
    public GameObject playerPrefab;

    public World world;
    public Player player;
    public SaveLoad saveLoadManager;


    public GameObject ingameMenu;
    public GameObject startMenu;
    GameManager GM = GameManager.Instance;



    private void Awake()
    {
        saveLoadManager = GameObject.Find("Game").GetComponent<SaveLoad>();

        GM.SetGameState(GameState.MAIN_MENU);
        ingameMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void CreateNew()
    {
        saveLoadManager.ClearSave();

        DestroyImmediate(GameObject.Find("World"));
        world = Instantiate(worldPrefab).GetComponent<World>();
        world.name = "World";
        world.CreateNew();
        world.transform.parent = transform;

        DestroyImmediate(GameObject.Find("Player"));
        player = Instantiate(playerPrefab).GetComponent<Player>();
        player.CreateNew();
        player.transform.parent = transform;
        player.name = "Player";

        Debug.Log("new game created");
        saveLoadManager.Save(this);
        GM.SetGameState(GameState.GAME);
        Unpause();
    }

    public void Continue()
    {
        if(world != null)
        {
            DestroyImmediate(world.gameObject);
        }
        world = Instantiate(worldPrefab).GetComponent<World>();
        world.name = "World";
        world.transform.parent = transform;

        if (player != null)
        {
            DestroyImmediate(player.gameObject);
        }
        player = Instantiate(playerPrefab).GetComponent<Player>();
        player.transform.parent = transform;
        player.name = "Player";

        Debug.Log("old game continued");
        //saveLoadManager.Load(this);
        saveLoadManager.Load(world);
        saveLoadManager.Load(player);
        GM.SetGameState(GameState.GAME);
        Unpause();

    }

    public void Save()
    {
        saveLoadManager.Save(this);
    }

    public void Exit()
    {
        saveLoadManager.Save(this);
        Debug.Log("exiting and closing");
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Debug.Log("game is on pause");
        GM.SetGameState(GameState.PAUSED);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        Debug.Log("game is not on pause");
        GM.SetGameState(GameState.GAME);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape pressed");

            if (GM.gameState == GameState.PAUSED)
            {
                Unpause();
                ingameMenu.SetActive(false);
            }
            else if (GM.gameState == GameState.GAME)
            {
                Pause();
                ingameMenu.SetActive(true);
            }
            else if (GM.gameState == GameState.MAIN_MENU)
            {
                Debug.Log("game is in main menu, escape dont make sence");
            }
        }
    }

}
