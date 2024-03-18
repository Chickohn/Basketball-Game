using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Rigidbody rb;
    private bool isDragging = false;
    private Queue<Vector3> mousePositions = new Queue<Vector3>(); // Queue to store the last ten mouse positions
    private float sensitivity = 0.25f; // Adjust for sensitivity of movement
    private Vector3 startPosition = new Vector3(0, 2, 0); // Starting position of the basketball
    private float startTime; // Time when the dragging starts
    private int frameCounter = 0; // Initialize a frame counter
    private bool shotInProgress = false;
    public gameManager gameManager;
    private bool resetInProgress = false;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Initially, we don't want gravity to affect the ball
        transform.position = startPosition; // Set the initial position
    }

    void Update()
    {
        frameCounter++;
        // if (frameCounter % 5000 == 0)
        // {
        //     Debug.Log("bounciness: " + GetComponent<Collider>().material.bounciness.ToString());
        // }
        if (!shotInProgress) {
            if (Input.GetMouseButtonDown(0) && !isDragging)
            {
                StartDragging();
            }
        }
        if (Input.GetMouseButton(0) && isDragging)
        {
            DragBall();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            ReleaseBall();
        }
    
    }

    private void StartDragging()
    {
        shotInProgress = true;
        isDragging = true;
        startTime = Time.time;
        mousePositions.Clear(); // Clear previous positions
        rb.useGravity = false;
        rb.velocity = Vector3.zero; // Reset velocity
        rb.angularVelocity = Vector3.zero; // Reset angular velocity
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        AddMousePosition(Input.mousePosition); // Add the initial mouse position
    }

    private void DragBall()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        // Check if the current position is at or below the floor level (assuming y = 0 is the floor level)
        float floorLevel = 1f; // Adjust this value to match the actual Y position of your floor
        if (curPosition.y <= floorLevel)
        {
            curPosition.y = floorLevel; // Prevent the ball from going below the floor level
        }

        transform.position = curPosition;

        // Increment the frame counter

        // Every twenty-five frames, add the current mouse position to the queue
        if (frameCounter % 25 == 0)
        {
            AddMousePosition(Input.mousePosition);
            // frameCounter = 0; // Reset the frame counter
        }
    }

    private void ReleaseBall()
    {
        isDragging = false;
        rb.useGravity = true; // Let gravity take over again

        Vector3 averageVelocity = CalculateAverageVelocity();
        // Adjust the throw velocity to use the X-axis for forward movement based on the upward swipe.
        // This assumes the averageVelocity.y (vertical swipe component) determines the forward (X-axis) force magnitude.
        Vector3 throwVelocity = new Vector3(Mathf.Abs(averageVelocity.y), averageVelocity.y, -averageVelocity.x) * sensitivity; // Moves the ball along the X-axis

        // Debug.Log("Throw Velocity: " + throwVelocity + ", Average Velocity: " + averageVelocity);
        rb.AddForce(throwVelocity, ForceMode.Impulse);
    }


    private void AddMousePosition(Vector3 position)
    {
        if (mousePositions.Count >= 10)
        {
            mousePositions.Dequeue(); // Remove the oldest position
        }
        mousePositions.Enqueue(position); // Add the new position
    }

    private Vector3 CalculateAverageVelocity()
    {
        if (mousePositions.Count < 2) return Vector3.zero; // Need at least two points to calculate velocity
        string positionsString = "Mouse Positions: ";
        foreach (var position in mousePositions)
        {
            positionsString += position.ToString() + " | ";
        }
        // Debug.Log(positionsString);
        Vector3 sumOfDifferences = Vector3.zero;
        Vector3 previousPosition = mousePositions.Peek(); // Get the oldest position without removing it
        foreach (var position in mousePositions)
        {
            sumOfDifferences += (position - previousPosition);
            previousPosition = position;
        }

        float totalTime = Time.time - startTime;
        // Debug.Log(totalTime);
        return (sumOfDifferences / mousePositions.Count) * sensitivity / 0.15f;// totalTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RimWarn")
        {
            StartCoroutine(LowerBounciness(0.25f));
        }
        if (!isDragging && !resetInProgress) { // Check resetInProgress flag
            if (other.gameObject.CompareTag("Hoop Top")) {
                scoreManager.IncrementScore(); // Increment Score
            }
            if (other.gameObject.CompareTag("Floor")) {
                StartCoroutine(ResetBasketball());
                // Debug.Log("Resetting");
            }
            StartCoroutine(ResetBasketball());  // Reset the basketball after a delay
        }
    }

    IEnumerator LowerBounciness(float bounce)
    {
        // Access the collider's material.
        var colliderMaterial = GetComponent<Collider>().material;
        
        // Check if the material exists.
        if (colliderMaterial != null)
        {
            // Change the bounciness property.
            colliderMaterial.bounciness = bounce;
            Debug.Log("bounciness: " + GetComponent<Collider>().material.bounciness.ToString());
            
            yield return new WaitForSeconds(0.5f);

            colliderMaterial.bounciness = 0.5f;
            Debug.Log("bounciness: " + GetComponent<Collider>().material.bounciness.ToString());
        }
    }

    IEnumerator ResetBasketball()
    {
        if (resetInProgress) {
            yield break; // Exit if a reset is already in progress
        }

        resetInProgress = true; // Indicate that reset is in progress

        yield return new WaitForSeconds(1); // Wait for 2 seconds

        shotInProgress = false;
        Debug.Log("Ball reset");

        rb.velocity = Vector3.zero; // Reset velocity
        rb.angularVelocity = Vector3.zero; // Reset angular velocity
        rb.useGravity = false; // Turn off gravity
        transform.position = startPosition; // Reset position

        gameManager.MoveHoopToNewPosition();

        resetInProgress = false; // Reset complete, allow future resets
    }
}
