using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    private float currentSpeed = 0.0f;
    public float turnSpeed = 180;


    public List<KeyCode> upButton;
    public List<KeyCode> downButton;
    public List<KeyCode> leftButton;
    public List<KeyCode> rightButton;
    public List<KeyCode> runButton;

    private Vector3 lastMovement = new Vector3();

    void Awake()
    {

    }

    void Start () {
		
	}
	
	void Update () {

        Movement();
        Rotation();

    }

    void Movement()
    {
        Vector3 movement = new Vector3();
        movement += MoveIfPressed(upButton, Vector3.up);
        movement += MoveIfPressed(downButton, Vector3.down);
        movement += MoveIfPressed(leftButton, Vector3.left);
        movement += MoveIfPressed(rightButton, Vector3.right);
        movement.Normalize();
        if (movement.magnitude > 0)
        {
            if (IsRunning())
            {
                currentSpeed = runSpeed;
                this.transform.Translate(movement * Time.deltaTime * runSpeed, Space.World);
            }
            else
            {
                currentSpeed = walkSpeed;
                this.transform.Translate(movement * Time.deltaTime * walkSpeed, Space.World);
            }
            lastMovement = movement;

        }
        else
        {
            this.transform.Translate(lastMovement * Time.deltaTime * currentSpeed, Space.World);
            currentSpeed *= 0.9f;
        }
    }

    Vector3 MoveIfPressed(List<KeyCode> keyList, Vector3 Movement)
    {
        foreach (KeyCode element in keyList)
        {
            if (Input.GetKey(element))
            {
                return Movement;
            }
        }
        return Vector3.zero;
    }


    bool IsRunning()
    {
        foreach (KeyCode element in runButton)
        {
            if (Input.GetKey(element))
            {
                return true;
            }
        }
        return false;
    }

    void Rotation()
    {
        Vector3 worldPos = Input.mousePosition;
        worldPos = Camera.main.ScreenToWorldPoint(worldPos);
        float dx = this.transform.position.x - worldPos.x;
        float dy = this.transform.position.y - worldPos.y;
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rot, turnSpeed * Time.deltaTime);
    }
}
