using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingManager : MonoBehaviour
{

    InputAction movementAction;



    private Directions facingDirection = Directions.Up;

    IRoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    //// Update is called once per frame
    void Update()
    {

        List<Directions> listDirection = new List<Directions>();
        //listDirection = roomManager.GetPossibleDirections(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            //Debug.Log("up");
            //if (listDirection.Contains(facingDirection))
            //{

            //}
            Vector3 movementWithFacing = movementOnFacingDirection(facingDirection);
            Debug.Log(movementWithFacing);
            //Debug.Log(facingDirection);
            transform.position += movementWithFacing;
        }
        if (Keyboard.current.sKey.wasPressedThisFrame) facingDirection = rotate180(facingDirection);

        if (Keyboard.current.aKey.wasPressedThisFrame) facingDirection = turnLeft(facingDirection); 

        if (Keyboard.current.dKey.wasPressedThisFrame) facingDirection = turnRight(facingDirection);


    }


    private Directions turnRight(Directions direction)
    {
        switch(direction)
        {
            case Directions.Up:
                return Directions.Right;
            case Directions.Down:
                return Directions.Left;
            case Directions.Left:
                return Directions.Up;
            case Directions.Right:
                return Directions.Down;
            default:
                return direction;
        }
    }

    private Directions turnLeft(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                return Directions.Left;
            case Directions.Down:
                return Directions.Right;
            case Directions.Left:
                return Directions.Down;
            case Directions.Right:
                return Directions.Up;
            default:
                return direction;
        }
    }
    private Directions rotate180(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                return Directions.Down;
            case Directions.Down:
                return Directions.Up;
            case Directions.Left:
                return Directions.Right;
            case Directions.Right:
                return Directions.Left;
            default:
                return direction;
        }
    }

    private Vector3 movementOnFacingDirection(Directions direction)
    {

        switch (direction)
        {
            case Directions.Up:
                return new Vector3(0, 0, 1); 
            case Directions.Down:
                return new Vector3(0, 0, -1);
            case Directions.Left:
                return Vector3.left;
            case Directions.Right:
                return Vector3.right;
            default:
                return new Vector3(0,0,0);
        }
    }




    //public void setRoomManager(IRoomManager roomManager)
    //{
    //    this.roomManager = roomManager;
    //}


}
