using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager SMInstance; // Singleton instance
    private int swishScoreValue = 2; // Score value for a swish
    private int regularScoreValue = 1; // Score value for a regular shot
    private Ball ball; 
    int score = 000;

    private void Awake()
    {
        ball = FindObjectOfType<Ball>()
        // Set up the singleton instance
        if (SMInstance == null)
            SMInstance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (ball == null){
            ball = GameObject.Find("Ball").GetComponent<Ball>();
        } 

        if (ball == null)
            return;
        
        if (ball.hasHitRim){
            RegularShot();
        }

        if (ball.swish){
            SwishShot();
        }


    }

    public void RegularShot()
    {
            AddScore(regularScoreValue);
    }
    public void SwishShot()
    {
        AddScore(swishScoreValue);
    }
            
    private void AddScore(int score)
    {
        this.score += score
    }
}