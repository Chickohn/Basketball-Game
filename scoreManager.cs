using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreManager : MonoBehaviour
{
    public static int score = 0; // Global score

    public static void IncrementScore()
    {
        score++;
        Debug.Log("Score: " + score); // Print the new score to the console
    }
}
