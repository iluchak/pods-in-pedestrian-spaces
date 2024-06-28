using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    [SerializeField] WheelCollider wheels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(0f, 10* Time.deltaTime, 0f, Space.Self);
        Vector3 position;
        Quaternion rotation;
        wheels.GetWorldPose(out position, out rotation);

        //Set wheel transform state
        transform.position = position;
        transform.rotation = rotation;
    }
}
