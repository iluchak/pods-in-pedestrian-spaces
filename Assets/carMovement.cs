using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carMovement : MonoBehaviour
{

    public GameObject car;
    public GameObject centerEye;

    public float newPosition;
    Vector3 velocity1;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = -0.5f;
        velocity1 = new Vector3(0.0f, 0.0f, newPosition); 
    }

    // Update is called once per frame
    void Update()
    {
         car.transform.Translate(velocity1 * Time.deltaTime);
        
    }
}
