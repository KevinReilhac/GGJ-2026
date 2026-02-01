using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingManager : MonoBehaviour
{
       
    [SerializeField]
    private float offsetMovement=1.0f;

    [SerializeField]
    private AnimationCurve MoveSpeedCurve;

    [SerializeField]
    private AnimationCurve RotationSpeedCurve;

    [SerializeField]
    private float moveDuration = 0.3f;

    [SerializeField]
    private float rotationDuration = 0.3f;

    [SerializeField]
    private GameObject roomManagerObject;

    private Directions facingDirection = Directions.Down;
    private Vector2Int gridPosition = Vector2Int.zero;
    private Dictionary<Directions, Vector2Int> _directionToVector = null;

    Vector3 futureLocation;
    Quaternion futureRotation;

    Vector3 startPosition;
    float traveledDistance;
    float rotationDistance;

    bool isRotating = false;
    bool isMoving = false;

    private IRoomManager roomManager;

    private float forwardBufferTime = 0.2f;
    private float forwardBufferCounter;

    // Start is called before the first frame update
    void Start()
    {
        _directionToVector = new Dictionary<Directions, Vector2Int>
        {
            [Directions.Up] = new Vector2Int(0, -1),
            [Directions.Down] = new Vector2Int(0, 1),
            [Directions.Right] = new Vector2Int(1, 0),
            [Directions.Left] = new Vector2Int(-1, 0)
        };

        futureLocation = transform.position;
        futureRotation = transform.rotation;


        startPosition = transform.position;
        traveledDistance = 0f;
        rotationDistance = 0f;

        roomManager = roomManagerObject.GetComponent<IRoomManager>();
        transform.position = roomManager.GetStartPosition(out gridPosition);

    }

    //// Update is called once per frame
    void Update()
    {
        List<Directions> listDirection = new List<Directions>();
        listDirection = roomManager.GetPossibleDirections(gridPosition);
        
        if (Keyboard.current.wKey.wasPressedThisFrame && !isMoving)
        {
            if(listDirection.Contains(facingDirection))
            {

                forwardBufferCounter = forwardBufferTime;
            }
        } else
        {
            forwardBufferCounter -= Time.deltaTime;
        }

        if (forwardBufferCounter > 0f)
        {
            if (!isMoving)
                gridPosition += _directionToVector[facingDirection];

            Vector3 movementWithFacing = movementOnFacingDirection(facingDirection);

            startPosition = transform.position;

            traveledDistance = 0f;


            futureLocation = transform.position + movementWithFacing;

            switch (roomManager.GetDungeonEvents(getGridPosition(futureLocation)))
            {
                case DungeonEvents.None:
                    isMoving = true;
                    break;
                case DungeonEvents.Boss:
                case DungeonEvents.Fight:
                    Debug.Log("Unhandled");
                    break;
                case DungeonEvents.Chest:
                    roomManager.GetChestLoot(getGridPosition(futureLocation));
                    isMoving = true;
                    break;
            }

            



        }
        if (Keyboard.current.sKey.wasPressedThisFrame&& !isRotating)
        {

            futureRotation = Quaternion.Inverse(Quaternion.Euler(new Vector3(0f, 180f, 0f))) * transform.rotation;
            facingDirection = rotate180(facingDirection);
            rotationDistance = 0f;
            isRotating = true;
        }

        if (Keyboard.current.aKey.wasPressedThisFrame && !isRotating)
        {
            futureRotation = Quaternion.Inverse(Quaternion.Euler(new Vector3(0f, 90f, 0f))) * transform.rotation;

            facingDirection = turnLeft(facingDirection);
            rotationDistance = 0f;
            isRotating = true;
        };

        if (Keyboard.current.dKey.wasPressedThisFrame && !isRotating)
        {
            futureRotation = Quaternion.Inverse(Quaternion.Euler(new Vector3(0f, -90f, 0f))) * transform.rotation;

            facingDirection = turnRight(facingDirection);
            rotationDistance = 0f;
            isRotating = true;
        }

        if (isMoving)
        {
            traveledDistance += Time.deltaTime / moveDuration;

            float t = Mathf.Clamp01(traveledDistance); 
            float curvedT = MoveSpeedCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, futureLocation, curvedT);

            if(t >= 1f)
            {
                transform.position = futureLocation;
                isMoving = false;
            }
        }

        if (isRotating)
        {
            rotationDistance += Time.deltaTime / rotationDuration;
            float t = Mathf.Clamp01(rotationDistance);
            float curvedT = RotationSpeedCurve.Evaluate(t);

            //var alphaRotation = RotationSpeedCurve.Evaluate(Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, futureRotation, curvedT);

            if(t >= 1f)
            {
                transform.rotation = futureRotation;
                isRotating = false;
            }



        }




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
                return new Vector3(0, 0, offsetMovement); 
            case Directions.Down:
                return new Vector3(0, 0, -offsetMovement);
            case Directions.Left:
                return new Vector3(-offsetMovement,0,0);
            case Directions.Right:
                return new Vector3(offsetMovement, 0, 0);
            default:
                return new Vector3(0,0,0);
        }
    }

    private Vector2Int getGridPosition(Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }




}
