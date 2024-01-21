using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public float horizontal;
    public float speed;
    private bool facingRight= true;

    Rigidbody2D rbody;
     
    public float jump;
    
    private bool midJump;

    private bool loser = false;
    private bool winnner = false;

    public Animator animator;

    public Transform swingSphere;
    public float swingRange = .07f;
    public LayerMask enemies;

    public int score;

    public bool runningTimer;
    public float currentTime;
    public float maxTime = 12;

    public TMP_Text timerText;
    public TMP_Text scoreText;
    public GameObject winScreen;
    public GameObject loseScreen;
    
    public GameObject startText;

    public AudioSource audioSource;
    public AudioClip slash;
    public GameObject WinClip;
    public GameObject LoseClip;
    public GameObject BackgroundMusic;


    void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        currentTime = maxTime;
        scoreText.text = score.ToString() + " Skeleton's Exorcised";

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        score = 0;
        startText.SetActive(true);
        BackgroundMusic.SetActive(false);
        WinClip.SetActive(false);
        LoseClip.SetActive(false);
    }

   
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rbody.velocity = new Vector2(horizontal * speed, rbody.velocity.y);

        animator.SetFloat("MoveSpeed", Mathf.Abs(horizontal));

        currentTime -= Time.deltaTime;
        Debug.Log(currentTime + "/" + 10);

        timerText.text = currentTime.ToString ("0.0");

        if(currentTime < 10)
        {
            startText.SetActive(false);
            BackgroundMusic.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.W) && !midJump)
        {
            rbody.velocity = Vector2.up * jump;
            midJump = true;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!winnner && !loser)
            {
                animator.SetBool("Swing", true);
                PlaySound(slash);
            }
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
        }


        flip();

          if(currentTime <= 0)
        {
            if(!winnner)
            {
                lose();
            }
        }

        if (score >= 3)
        {
            if(!loser)
            {
             win();
            }
        }

    
    }

    public void endSwing()
    {
        animator.SetBool("Swing", false);
    }

    public void Swing()
    {
       Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(swingSphere.position, swingRange, enemies);

        foreach(Collider2D enemies in hitEnemy)
        {
            Debug.Log("Enemy Damage");
            enemies.GetComponent<SkeletonController>().hp -= 1;
        }

    }

    void OnDrawGizmosSelected()
    {
        if (swingSphere == null)
            return;

        Gizmos.DrawWireSphere(swingSphere.transform.position, swingRange);
    }


    void flip()
    {
       if(facingRight && horizontal < 0f || !facingRight && horizontal > 0)
       {
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
       }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            midJump = false;
        }
    }

    public void changeScore(int val)
    {
        score += val;
        Debug.Log(score + "/" + 3);
        scoreText.text = score.ToString() + " Skeleton's Exorcised";
    }

    public void win()
    {
        Debug.Log("You win");

        speed = 0;
        winnner = true;
        rbody.simulated = false;
        winScreen.SetActive(true);
        BackgroundMusic.SetActive(false);
         WinClip.SetActive(true);
        

    }

    public void lose()
    {
        speed = 0;
        loser = true;
        rbody.simulated = false;
        BackgroundMusic.SetActive(false);
        if(!winnner)
        {
            loseScreen.SetActive(true);
            LoseClip.SetActive(true);
        }
        
    }

   public void PlaySound(AudioClip clip)
     {
         audioSource.PlayOneShot(clip);
     }



}
