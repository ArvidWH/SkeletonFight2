using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrolls : MonoBehaviour
{
    public float speed;
    public float jumpHeight;
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded = true;
    private SpriteRenderer sprite;
    private float attacktime;
    private bool attacking;
    private BoxCollider2D hitbox;
    private int HP = 3;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    private float Iframes;
    private bool gameOver;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
    }

    // Update is called once per frame
    void heartupdate(int heart)
    {
        if (heart == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        else if (heart == 2)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
        }
        else if (heart == 1)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }
        else if (heart == 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
            Time.timeScale = 0f;
            gameOver = true;
        }
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("ferger");
            Application.LoadLevel(Application.loadedLevel);
            Time.timeScale = 1f;
        }

    }
    void FixedUpdate()
    {
        float dir = Input.GetAxisRaw("Horizontal");
        if (attacking == false)
        {
            rb.velocity = new Vector2(dir * speed, rb.velocity.y);
        
            if (dir > 0f)
            {
                anim.SetBool("running", true);
                sprite.flipX = false;
                hitbox.offset = new Vector2(0.3f, 0f);
            }
            else if (dir < 0f)
            {
                anim.SetBool("running", true);
                sprite.flipX = true;
                hitbox.offset = new Vector2(-0.3f, 0f);
            }
            else
            {
                anim.SetBool("running", false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && grounded == true && attacking == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            grounded = false;
            anim.SetBool("jumping", true);
        }
        if (Input.GetKeyDown(KeyCode.E) && grounded == true && attacking == false)
        {
            anim.SetBool("attack", true);
            attacktime = Time.time + 0.5f;
            attacking = true;
            gameObject.tag = "Attacking";
            hitbox.enabled = true;
        }
        if ( Time.time > attacktime)
        {
            anim.SetBool("attack", false);
            attacking = false;
            gameObject.tag = "Untagged";
            hitbox.enabled = false;
        }
       

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            grounded = true;
            anim.SetBool("jumping", false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Attacking") && Time.time > Iframes)
        {
            Iframes = Time.time + 2f;
            HP -= 1;
            anim.ResetTrigger("Hit");
            anim.SetTrigger("Hit");
            heartupdate(HP);
        }
        if (col.gameObject.CompareTag("DEATHPLANE"))
        {
            heartupdate(0);
        }
    }


}
