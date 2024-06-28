using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField] public Vector3 _rotation;

    // Update is called once per frame
    void Update()
    {
        // rotationChange += new Vector3(Input.GetAxis("Vertical")*sensitivity), Input.GetAxis("Horizontal")
        transform.Rotate(_rotation *Time.deltaTime);

        //  transform.RotateAround(rotatingPoint, _rotation *Time.deltaTime, 0f);
    }
}
