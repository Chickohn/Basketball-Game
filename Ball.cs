using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool hasHitRim = false;
    privavte bool swish = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rim") && collision.gameObject.CompareTag("Net"))
        {
            hasHitRim = true;
        } 
        else if (!hasHitRim && collision.gameObject.CompareTag("Net")) 
        {
            swish = true;
        }

    }
    
    public bool HasHitRim()
    {
        return hasHitRim;
    }

    public bool Swish()
    {
        return swish;
    }


}