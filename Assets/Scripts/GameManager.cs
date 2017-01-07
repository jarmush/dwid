using UnityEngine;
using System.Collections;

// Game States
public enum GameState { MAIN_MENU, PAUSED, GAME }

public class GameManager
{
    protected GameManager() { }
    private static GameManager instance = null;
    public GameState gameState { get; private set; }

    public static GameManager Instance
    {
        get
        {
            if (GameManager.instance == null)
            {
                GameManager.instance = new GameManager();
            }
            return GameManager.instance;
        }
    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
    }
}