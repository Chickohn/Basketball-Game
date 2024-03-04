using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public GameObject basketballHoop; // Assign in inspector
    public Vector3 positionOrigin;
    public Vector3 positionThreshold; // Max distance the hoop can move from its original position

    // Call this method every time the ball is reset
    public void MoveHoopToNewPosition()
    {
        Vector3 newPosition = new Vector3(
            Random.Range(-positionThreshold.x, positionThreshold.x),
            Random.Range(-positionThreshold.y, positionThreshold.y),
            Random.Range(-positionThreshold.z, positionThreshold.z)
        ) + positionOrigin;

        basketballHoop.transform.position = newPosition;
    }
}
