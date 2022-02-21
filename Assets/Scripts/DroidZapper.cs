using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidZapper : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private float dirx = -1;
    public Patrol patrol;
    public bool isSpectrumR;

    //for attacking
    public Transform EnemyattackPos;
    public float EnemyattackRangeX;
    public float EnemyattackRangeY;
    public LayerMask whatIsPlayer;
    public int damage;
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    //detecting
    public float detectRange;

    //following
    public Transform target;
    public float stopingDistance;

    private bool isFollowing = false;

    private Animator anim;

    public int ZapperHealth;
    //effects

    public SpriteRenderer spriter;

    //public AudioSource audsrc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        //audsrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (patrol.isFacingRight)
        {
            dirx = 1;
        }
        else
        {
            dirx = -1;
        }

        //detecting
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer < detectRange)
        {
            followPlayer();
            isFollowing = true;
        }
        else isFollowing = false;

        Move();
        
        if(isSpectrumR)
        {
            if (GameManager.instance.RedActive == true)
            {
                spriter.enabled = true;
            }
            else
            {
                spriter.enabled = false;
            }
        }
        
        else if(!isSpectrumR)
        {
            if (GameManager.instance.BlueActive == true)
            {
                spriter.enabled = true;
            }
            else
            {
                spriter.enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isFollowing)
        {
            patrol.patrol();
        }
    }

    private void Move()
    {
        if (patrol.isFacingRight && isFollowing && target.position.x < transform.position.x + stopingDistance)
        {
            rb.velocity = Vector2.zero;
            attackPlayer();
        }
        else if (!patrol.isFacingRight && isFollowing && target.position.x > transform.position.x - stopingDistance)
        {
            rb.velocity = Vector2.zero;
            attackPlayer();
        }
        else
        {
            anim.SetBool("IsAttacking", false);
            rb.velocity = Vector2.right * dirx * moveSpeed;
        }
    }

    public void followPlayer()
    {
        //following
        if (target.position.y < transform.position.y + 2.0f)
        {
            if (target.position.x > transform.position.x && !patrol.isFacingRight) patrol.Flip();
            if (target.position.x < transform.position.x && patrol.isFacingRight) patrol.Flip();
        }
    }

    private void attackPlayer()
    {
        if (timeBtwAttack <= 0)
        {
            anim.SetBool("IsAttacking", true);
            Collider2D[] ToHit = Physics2D.OverlapBoxAll(EnemyattackPos.position,new Vector2(EnemyattackRangeX,EnemyattackRangeY),0, whatIsPlayer);
            for (int i = 0; i < ToHit.Length; i++)
            {
                //ToHit[i].GetComponent<PlayerMovement>().TakeDamage(damage);
                EventManager.Instance.UpdateHealth(-5);
                Debug.Log("Player Damaged");
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void takeDamage(int Damage)
    {
        ZapperHealth -= Damage;
        anim.SetTrigger("IsHurt");
        if (ZapperHealth == 0)
        {
            Destroy(this.gameObject);

        }
    }

   /* public void setVolume(float musicvol)
    {
        audsrc.volume = musicvol;
    }

    public void Mute()
    {
        if (audsrc.mute)
        {
            audsrc.mute = false;
        }
        else
        {
            audsrc.mute = true;
        }
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireCube(EnemyattackPos.position, new Vector3(EnemyattackRangeX,EnemyattackRangeY,0));
    }
}
