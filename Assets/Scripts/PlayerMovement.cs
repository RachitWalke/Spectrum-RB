using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    //[SerializeField] private LayerMask whatIsGround;
    //[SerializeField] private GameObject GroundCheckPos;
    //[SerializeField] private float GroundCheckRadius;
    public Animator anim;

    public float horizontal;
 
    public float RunSpeed;
    bool jump = false;
    bool Crouch = false;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal") * RunSpeed;
        anim.SetFloat("Speed",Mathf.Abs(horizontal));
        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            anim.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            Crouch = true;
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            Crouch = false;
        }
    }

    public void OnLanding()
    {
        anim.SetBool("IsJumping", false);
        Debug.Log("landing");
    }

    private void FixedUpdate()
    {
        controller.Move(horizontal * Time.fixedDeltaTime, Crouch, jump);
        jump = false;
    }
}
