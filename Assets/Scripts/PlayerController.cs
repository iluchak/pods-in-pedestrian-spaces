using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
// [RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidBody;
    [SerializeField] private bool walk;
    [SerializeField] private bool walkInCircle;
    [SerializeField] private bool idling;
    float TimeAmount = 19f;
    bool firstTimeTurn;

    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // rigidBody = GetComponent<Rigidbody>();
        walk = true;
        walkInCircle = false;
        //finding all active cameras - there are LeftEyeAnchor, RightEyeAnchor and CenterEyeAnchor
        // Camera[] allCameras = FindObjectsOfType<Camera>();
        // foreach(Camera c in allCameras)
        //     print(c.name);
        // TimeAmount = 0.05f; 
        currentTime = TimeAmount;
        firstTimeTurn = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if button is pressed
        if(walk){
            //play charcater animation in order
            animator.SetBool("walking", true);
        }
        // currentTime -= Time.deltaTime;
        // // print(currentTime);

        // if(!walk || currentTime <= 0){
        //     //play charcater animation in order

        //     walk = false;
        //     animator.SetBool("walking", false);
        //     animator.SetBool("idling", true);
        //     // print("char walking should be false " + walk + " " + animator.GetBool("walking"));
        // }

        // if(animator.bodyPosition.z > 8.0f && firstTimeTurn){
        //     walkInCircle = true;
        //     firstTimeTurn = false;
        // }
        // // if(animator.bodyPosition.x < -9f || animator.bodyPosition.z < -15f){
        // //        animator.SetBool("walking", false);
        // // }

        // if(walkInCircle){
        //     //play charcater animation in order
        //     animator.SetTrigger("right turning");
        //     walkInCircle= false;
        // }

        // // if (currentTime <= 0 || idling)
        // // {
        // //     animator.SetBool("idling", true);
        // // }
    }
}
