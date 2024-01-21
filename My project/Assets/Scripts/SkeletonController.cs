using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SkeletonController : MonoBehaviour
{
   
    public Animator animator;
    public float hp = 1;
    private Rigidbody2D rbody;

    public Transform[] patrolArea;
    public float speed;
    public int patrolPlace;

    public ParticleSystem dying;


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("Running", true);
    }
    

    // Update is called once per frame
    void Update()
    {
        if(patrolPlace == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolArea[1].position, speed * Time.deltaTime);
            if(Vector2.Distance(transform.position,patrolArea[1].position) <.5f)
            {
                flip();
                transform.Rotate(0,180,0);
                patrolPlace = 0;
            }
        }

        if(patrolPlace == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolArea[0].position, speed * Time.deltaTime);
            if(Vector2.Distance(transform.position,patrolArea[0].position) <.5f)
            {
                flip();
                transform.Rotate(0,180,0);
                patrolPlace = 1;
            }
        }
        
         if(hp == 0)
        {
            animator.SetTrigger("Death");
        }
    }

    public void dyingParticle()
    {
         dying.Play();
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

   public void Dead()
    {   
        speed = 0;
        Debug.Log("Enemy Killed");   
        FindObjectOfType<PlayerControl>().changeScore(1);
        Destroy(gameObject);

    }
    
}
    
    
