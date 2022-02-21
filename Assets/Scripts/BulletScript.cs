using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Transform player;
    private Transform bulletHolder = null;
    public Transform BulletHolder { set { bulletHolder = value; } }

    private bool facingRight;
    public bool FacingRight { set { facingRight = value; } }
    private bool shotByPlayer;
    public bool ShotByPlayer { set { shotByPlayer = value; } }

    private float bulletSpeed = 10;

    private void Awake()
    {
        this.enabled = true;
    }

    private void Start()
    {
    }

    private void OnEnable()
    {

        if (bulletHolder != null)
            transform.position = bulletHolder.position;
        else
            transform.position = Vector3.zero;

        StartCoroutine(DeactivateBullet(3));
    }

    // Update is called once per frame
    void Update()
    {
        if (facingRight)
        {
            transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
        }
        else if (!facingRight)
        {
            transform.Translate(-Vector3.right * Time.deltaTime * bulletSpeed);
        }

        StartCoroutine(DeactivateBullet(5));
    }

    IEnumerator DeactivateBullet(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);

       // gameObject.transform.SetParent(bulletHolder);
        transform.position = transform.parent.position;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && shotByPlayer)
        {
            if(GameManager.instance.RedActive && collision.gameObject.GetComponent<DroidZapper>().isSpectrumR)
            {
                collision.gameObject.GetComponent<DroidZapper>().takeDamage(5);
                StartCoroutine(DeactivateBullet(0));
            }
            if (GameManager.instance.BlueActive && !collision.gameObject.GetComponent<DroidZapper>().isSpectrumR)
            {
                collision.gameObject.GetComponent<DroidZapper>().takeDamage(5);
                StartCoroutine(DeactivateBullet(0));
            }
        }
        
        /*if(collision.gameObject.CompareTag("Player") && !shotByPlayer)
        {
            EventManager.Instance.UpdateHealth(-20);
            //collision.gameObject.GetComponent<PlayerController>().HurtPlayer(20);
            StartCoroutine(DeactivateBullet(0));
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(DeactivateBullet(0));
        }*/
    }
}
