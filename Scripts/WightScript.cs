using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WightScript : MonoBehaviour
{

    public Rigidbody2D enemyRb;
    private GameObject player;
    public float speed = 2f;
    private Animator anim;
    public CapsuleCollider2D collider;
    public bool isAttacking;
    public int attackNumber = 1;
    public bool isBlocking;
    private PlayerController playerController;
    public float blockForce = 7f;
    public float chaseDistance = 5f;
    public float attackDistance = 0.8f;
    private SpriteRenderer spriteRenderer;
    private bool isHit;
    public bool isStunned;
    public int lives = 10;
    public AudioSource audioSource;
    public AudioClip HitSound;

    public AudioClip DeathSound;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Theguy");
        collider.enabled = false;
        playerController = player.GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerController.hasSword)
        {
            lives = 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (isHit || isStunned)
        {
            enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        Vector3 scale = transform.localScale;

        if (player.transform.position.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        transform.localScale = scale;

        if (distance <= attackDistance)
        {
            enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
            anim.SetBool("isWalking", false);

            if (!isAttacking)
            {
                isAttacking = true;
                if (attackNumber == 1)
                    anim.Play("Attack");
                else if (attackNumber == 2)
                    anim.Play("Attack2");
                else
                    anim.Play("Attack3");
            }
            return;
        }


        if (distance <= chaseDistance && !isAttacking)
        {
            float direction = Mathf.Sign(player.transform.position.x - transform.position.x);

            enemyRb.velocity = new Vector2(direction * speed, enemyRb.velocity.y);

            anim.SetBool("isWalking", true);

            return;

        }

        enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
        anim.SetBool("isWalking", false);



    }

    void FinishAttack()
    {
        Debug.Log("finished " + attackNumber);
        isAttacking = false;

        attackNumber++;

        if (attackNumber > 3)
            attackNumber = 1;
    }


    void EnableSwordBox()
    {
        collider.enabled = true;
        Debug.Log("enabled");
    }

    void DisableSwordBox()
    {
        collider.enabled = false;
    }

    public void GetHit()
    {
        if (isDead)
        {
            return;
        }
        lives--;

        if (lives != 0)
        {
            if (isHit)
                return;
            StartCoroutine(HitReaction());
        }

        else
        {
            isDead = true;
            isAttacking = false;
            enemyRb.velocity = Vector2.zero;
            audioSource.PlayOneShot(DeathSound);
            anim.Play("Die");
            Destroy(gameObject, 0.9f);
            PlayerController.lives += 1;
            return;
        }


    }


    private IEnumerator HitReaction()
    {

        isHit = true;
        isAttacking = false;
        collider.enabled = false;
        enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
        anim.SetBool("isWalking", false);
        audioSource.PlayOneShot(HitSound);
        anim.Play("Hit", 0, 0f);
        yield return new WaitForSeconds(0.1f);
        isHit = false;
    }



    public void Stunned(float time)
    {
        StartCoroutine(StunRoutine(time));
    }

    private IEnumerator StunRoutine(float time)
    {
        isStunned = true;
        isAttacking = false;

        enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
        collider.enabled = false;
        anim.SetBool("isWalking", false);

        yield return new WaitForSeconds(time);

        isStunned = false;
    }
}
