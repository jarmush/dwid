using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public SaveLoad saveLoadManager;
    public Player cameraPlayer;

    [SerializeField]
    public float xPlayerPositionToSave;
    [SerializeField]
    public float yPlayerPositionToSave;

    private void Awake()
    {
        saveLoadManager = GameObject.Find("Game").GetComponent<SaveLoad>();
        Camera.main.gameObject.GetComponent<CameraSmoothFollow>().enabled = true;
        Camera.main.gameObject.GetComponent<CameraSmoothFollow>().player = this;
    }

    private void Start()
    {

    }

    public void CreateNew()
    {
        Debug.Log("new player created");
    }

}
