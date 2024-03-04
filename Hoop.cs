using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    public GameObject hoopSensor;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = hoopSensor.transform.position + new Vector3(0,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = hoopSensor.transform.position + new Vector3(0,1,0);
    }
}
