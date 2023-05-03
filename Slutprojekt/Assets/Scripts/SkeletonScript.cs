using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkeletonScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private float attacktime;
    private BoxCollider2D hitbox;
    private float attackdelay;
    private float dielay;
    public GameObject Skeleton;
    public GameObject player;
    private float dist;
    private bool FacingRight;
    private bool AttackReady = true;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI restart;
    public TextMeshProUGUI finalScore;
    private int Score = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
        finalScore.enabled = false;
        restart.enabled = false;
    }

    void Attack()
    {
        AttackReady = false;
        anim.SetBool("Attacking", true);
        gameObject.tag = "Attacking";
        hitbox.enabled = true;
        attacktime = Time.time + 1f;
        attackdelay = Time.time + 3f;
    }

    void Walk()
    {
        anim.SetBool("Running", true);
        if (FacingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(speed * -1f, rb.velocity.y);
        }
    }



    // Update is called once per frame
    void Update()
    {
        dist = (Mathf.Abs(Skeleton.transform.position.x - player.transform.position.x));
        
        if (Time.time > attacktime)
        {
            anim.SetBool("Attacking", false);
            gameObject.tag = "Untagged";
            hitbox.enabled = false;
        }
        if (Time.time > attackdelay) { 
            AttackReady = true;
        }
        if (player.transform.position.x > Skeleton.transform.position.x)
        {
            FacingRight = true;
            sprite.flipX = false;
            hitbox.offset = new Vector2(0.35f, 0f);
        }
        else
        {
            hitbox.offset = new Vector2(-0.35f, 0f);
            FacingRight = false;
            sprite.flipX = true;
        }
        if (10 > dist && dist > 5)
        {
            Walk();
        }
        else
        {
            anim.SetBool("Running", false);
        }
        if (dist < 5 && AttackReady)
        {
            Attack();
        }
        if (Time.timeScale == 0f)
        {
            finalScore.enabled = true;
            restart.enabled = true;
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        
        
        if (col.gameObject.CompareTag("Attacking"))
        {
            if (FacingRight)
            {
                rb.velocity = new Vector2(-3f, 3f);
            }
            else
            {
                rb.velocity = new Vector2(3f, 3f);
            }
            anim.ResetTrigger("Hit");
            anim.SetTrigger("Hit");
            Score += 1;
            ScoreText.text = "Score: " + Score.ToString();
            finalScore.text = "Final score: " + Score.ToString();
        }
        if (col.gameObject.CompareTag("DEATHPLANE"))
        {
            Score += 10;
            ScoreText.text = "Score: " + Score.ToString();
            Skeleton.transform.position = new Vector2(0, 0);
        }
    }

    
}
