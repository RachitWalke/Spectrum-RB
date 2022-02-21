using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{

    private float vertical;
    private float speed = 5.0f;
    private bool isLadder;
    public bool isClimbing;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if(isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if(isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
            anim.SetBool("IsClimbing", true);
        }
        else
        {
            rb.gravityScale = 3.0f;
            anim.SetBool("IsClimbing", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
