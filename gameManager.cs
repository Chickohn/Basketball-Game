using UnityEngine;

public class gameManager : MonoBehaviour
{
    public GameObject basketballHoop; // Assign in the inspector
    public Vector3 positionOrigin;
    public Vector3 positionThreshold; // Max distance the hoop can move from its original position

    private enum GameMode { Easy, Normal, Hard }
    private GameMode currentGameMode = GameMode.Normal;
    private enum MovementPattern { AlongX, AlongZ, AlongXZ, ArcYZ }
    private MovementPattern currentPattern;

    private Vector3 endPointA, endPointB;
    private float moveTime; // Timer to control the interpolation
    private float moveSpeed = 0.5f; // Speed at which the hoop moves between points
    private float arcHeight = 1.5f; // Adjust for higher or lower arcs

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGameMode = GameMode.Easy;
            MoveHoopToOrigin();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentGameMode = GameMode.Normal;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentGameMode = GameMode.Hard;
            SelectRandomMovementPattern();
            PrepareMovement();
        }

        if (currentGameMode == GameMode.Hard)
        {
            ExecuteMovementPattern();
        }
    }

    public void MoveHoopToNewPosition()
    {
        switch (currentGameMode)
        {
            case GameMode.Easy:
                MoveHoopToOrigin();
                break;
            case GameMode.Normal:
                MoveHoopToRandomPosition();
                break;
            case GameMode.Hard:
                MoveHoopToRandomPosition();
                SelectRandomMovementPattern();
                PrepareMovement();
                break;
        }
    }

    private void SelectRandomMovementPattern()
    {
        currentPattern = (MovementPattern)Random.Range(0, System.Enum.GetValues(typeof(MovementPattern)).Length);
    }

    private void PrepareMovement()
    {
        endPointA = basketballHoop.transform.position;

        // Choose the direction based on the selected pattern
        switch (currentPattern)
        {
            case MovementPattern.AlongX:
                endPointB = endPointA + Vector3.right * positionThreshold.x;
                break;
            case MovementPattern.AlongZ:
                endPointB = endPointA + Vector3.forward * positionThreshold.z;
                break;
            case MovementPattern.AlongXZ:
                endPointB = endPointA + new Vector3(positionThreshold.x, 0, positionThreshold.z);
                break;
            case MovementPattern.ArcYZ:
                endPointB = endPointA + Vector3.forward * positionThreshold.z; // Arc will use this as base direction
                break;
        }

        moveTime = 0f;
    }

    private void ExecuteMovementPattern()
    {
        moveTime += Time.deltaTime * moveSpeed;
        float t = Mathf.PingPong(moveTime, 1f);
        
        switch (currentPattern)
        {
            case MovementPattern.AlongX:
            case MovementPattern.AlongZ:
            case MovementPattern.AlongXZ:
                basketballHoop.transform.position = Vector3.Lerp(endPointA, endPointB, t);
                break;
            case MovementPattern.ArcYZ:
                ExecuteArcMovement(t);
                break;
        }
    }

    private void ExecuteArcMovement(float t)
    {
        Vector3 baseLine = Vector3.Lerp(endPointA, endPointB, t);
        float arc = (Mathf.Sin(t * Mathf.PI) * arcHeight); // Sine wave for arc height
        basketballHoop.transform.position = new Vector3(baseLine.x, baseLine.y + arc, baseLine.z);
    }

    private void MoveHoopToOrigin()
    {
        basketballHoop.transform.position = positionOrigin;
    }

    private void MoveHoopToRandomPosition()
    {
        Vector3 newPosition = new Vector3(
            Random.Range(-positionThreshold.x, positionThreshold.x),
            Random.Range(-positionThreshold.y, positionThreshold.y),
            Random.Range(-positionThreshold.z, positionThreshold.z)
        ) + positionOrigin;

        basketballHoop.transform.position = newPosition;
    }
}
