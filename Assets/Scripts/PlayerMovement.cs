using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator anim;
    public SpectrumBar SB;
    public LadderMovement LM;

    public HealthBar healthbar;

    //player Movement
    public float horizontal;
 
    public float RunSpeed;
    public float WalkSpeed;
    private float CurrentSpeed;

    bool jump = false;
    bool Crouch = false;
    bool isWalking = false;


    //player shoot
    [SerializeField] private BulletPooling bulletList = null;
    [SerializeField] Transform ShootPoint = null;
    [SerializeField] Transform ShootPoint2 = null;
    [SerializeField] Transform ShootPoint3 = null;
    private float timeBetweenShots;
    public float StartTimeBtwShots;

    //player health
    private const int MAXHEALTH = 100;
    private int m_health;

    private void Start()
    {
        timeBetweenShots = StartTimeBtwShots;

        m_health = MAXHEALTH;
        //health event
        EventManager.Instance.OnUpdateHealth += UpdatePlayerHealth;
        
    }

    // Update is called once per frame
    void Update()
    {

        //Running and Walking

        if (Input.GetKeyDown(KeyCode.LeftShift) && horizontal != 0)
            isWalking = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            isWalking = false;

        if (isWalking == true)
        {
            CurrentSpeed = WalkSpeed;
        }
        else
            CurrentSpeed = RunSpeed;

        if(Crouch == false)
        {
            horizontal = Input.GetAxis("Horizontal") * CurrentSpeed;
        }
       
        if (!isWalking)
        {
            anim.SetBool("IsWalking", false);
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        else if (isWalking)
        {
            anim.SetFloat("Speed", 0);
            anim.SetBool("IsWalking", true);
        }
           
       
        //Jumping and Crouching
        if(Input.GetButtonDown("Jump") && controller.m_Grounded)
        {
            jump = true;
            anim.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown("Crouch") && horizontal == 0 && !LM.isClimbing)
        {
            Crouch = true;
            anim.SetBool("IsCrouching", true);
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            Crouch = false;
            anim.SetBool("IsCrouching", false);
        }

        //Shooting
        if(timeBetweenShots <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("IsShooting", true);
                Shoot();
                timeBetweenShots = StartTimeBtwShots;
            }
            else
                anim.SetBool("IsShooting", false);
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }

        //changing Spectrum
        if(Input.GetKeyDown(KeyCode.E) && SB.currentRedValue > 0)
        {
            GameManager.instance.RedActive = !GameManager.instance.RedActive;
            GameManager.instance.SetRedActive();
            SB.startRedBar();
        }
        if (Input.GetKeyDown(KeyCode.Q) && SB.currentBlueValue > 0)
        {
            GameManager.instance.BlueActive = !GameManager.instance.BlueActive;
            GameManager.instance.SetBlueActive();
            SB.startBlueBar();
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

    public void Shoot()
    {
        GameObject bullet = bulletList.GetBullet();
        if(!Crouch)
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint;
        }
        else
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint2;
        }
        if(horizontal != 0 && !isWalking)
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint3;
        }
        bullet.GetComponent<BulletScript>().ShotByPlayer = true;
        bullet.GetComponent<BulletScript>().FacingRight = controller.m_FacingRight;
        bullet.SetActive(true);
    }

    public void UpdatePlayerHealth(int health)
    {
        //Changing health
        m_health += health;

        //If player hurt
        if (health < 0)
        {
            anim.SetTrigger("IsHurt");
            Debug.Log(m_health);

            if (m_health == 0)
            {
                gameObject.SetActive(false);
            }
        }

        healthbar.setHealth(m_health);
    }
}
