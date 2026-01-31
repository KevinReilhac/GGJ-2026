using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingManager : MonoBehaviour
{

    InputAction movementAction;

    [SerializeField]
    private int offsetMovement=1;

    [SerializeField]
    private AnimationCurve MoveSpeedCurve;

    [SerializeField]
    private AnimationCurve RotationSpeedCurve;

    [SerializeField]
    private float moveDuration = 0.3f;

    [SerializeField]
    private float rotationDuration = 0.3f;

    private Directions facingDirection = Directions.Up;

    Vector3 futureLocation;
    Quaternion futureRotation;

    Vector3 startPosition;
    float traveledDistance;
    float rotationDistance;


    bool isRotating = false;
    bool isMoving = false;

    IRoomManager roomManager;


    private float forwardBufferTime = 0.2f;
    private float forwardBufferCounter;

    // Start is called before the first frame update
    void Start()
    {
        futureLocation = transform.position;
        futureRotation = transform.rotation;


        startPosition = transform.position;
        traveledDistance = 0f;
        rotationDistance = 0f;
    }

    //// Update is called once per frame
    void Update()
    {
        List<Directions> listDirection = new List<Directions>();
        //listDirection = roomManager.GetPossibleDirections(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));

        if(Keyboard.current.wKey.wasPressedThisFrame && !isMoving)
        {
            forwardBufferCounter = forwardBufferTime;
        } else
        {
            forwardBufferCounter -= Time.deltaTime;
        }

        if (forwardBufferCounter > 0f)
        {

            Vector3 movementWithFacing = movementOnFacingDirection(facingDirection);

            startPosition = transform.position;

            traveledDistance = 0f;


            futureLocation = transform.position + movementWithFacing;

            isMoving = true;



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
