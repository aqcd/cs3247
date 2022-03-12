using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeyScript : MonoBehaviour {

    public Animator snakeyAnimator;
    public CharacterController charaControl;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float turnSpeed = 90.0f;

    Vector3 moveDirection;
    Vector3 lastPosition;

    // Start is called before the first frame update
    void Start() {


    }

    // Update is called once per frame
    void Update() {

        float velocity = ( ( charaControl.transform.position - lastPosition ) / Time.deltaTime ).magnitude;
        snakeyAnimator.SetFloat("moveSpeed", velocity);
        lastPosition = charaControl.transform.position;

        float sideMove = 0;
        float forwardMove = 0;

        sideMove += Input.GetAxis("Horizontal");
        forwardMove += Input.GetAxis("Vertical");

        moveDirection = sideMove * Vector3.right + forwardMove * Vector3.forward;
        charaControl.Move(moveDirection * speed * Time.deltaTime);

        Quaternion charaLookDir = Quaternion.LookRotation(moveDirection);

        if (moveDirection.magnitude > 0.0f) {
            charaControl.transform.rotation = Quaternion.Slerp(charaControl.transform.rotation, charaLookDir, turnSpeed * Time.deltaTime);
        }

    }
}
