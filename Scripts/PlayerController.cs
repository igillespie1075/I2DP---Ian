using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float upInput;
    public float speed = 8f;
    private Rigidbody2D rb;
    public Animator playerAnim;
    private bool isFacingRight;
    private SpriteRenderer spriteRenderer;
    public float jumpForce = 8f;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    public float doubleJumpForce = 4f;
    private bool isAttacking;
    public bool comboQueued;
    public bool canQueueNextAttack;
    private int currentAttack = 1;
    public float attackForce = 1f;
    private bool attackedOnce;
    public bool isBlocking;
    private int parryNum = 1;
    public bool canParry;
    public bool isDead;
    public CapsuleCollider2D collider;
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioClip clickSound;
    public AudioClip clickSound2;
    public AudioClip clickSound3;
    public AudioClip DeathSound;
    public AudioClip HitSound;
    public AudioClip BlockSound;
    public AudioClip ShadowMusic;
    public AudioClip attackSound;


    public int parrySoundNum = 1;
    public int maxHealth = 10;
    public static int lives = 10;


    EnemyController enemy;

    [SerializeField] private float parryWindow = 0.2f;

    private float parryTimer;
    private bool isPushed;

    private float parryForce = 8f;
    [SerializeField] private float KnockBackTime = 0.2f;

    private int staminaNumber = 5;
    private float noAttackTime = 1.5f;
    private float attackTimer = 0f;

    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public float wallCheckRadius = 0.1f;
    public LayerMask wallLayer;
    public float wallJumpForceX = 6f;
    public float wallJumpForceY = 8f;
    private bool touchingLeftWall;
    private bool touchingRightWall;
    private bool isWallJumping = false;
    public float wallJumpLockTime = 0.15f;

    [SerializeField] private RuntimeAnimatorController ogController;
    [SerializeField] private RuntimeAnimatorController newController;

    private bool usingNewController;
    private bool canSwapAnimator;
    public bool isGrabbing = false;
    public static bool hasSword = false;    
    public static int spawnToUse = 0;

    public GameObject door;
    private Animator doorAnim;
    private static bool hasKey = false;
    private bool doorDestroyed;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        doorAnim = door.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider.enabled = false;
        if (hasSword)
        {
            SwapAnimator();
        }

        if (spawnToUse == 1)
        {
            GameObject spawn = GameObject.Find("Spawn1");
            if(spawn != null)
            {
                transform.position = spawn.transform.position;
            }
        }

        if (spawnToUse == 2)
        {
            GameObject spawn = GameObject.Find("Spawn2");
            if (spawn != null)
            {
                transform.position = spawn.transform.position;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

        

        if(canSwapAnimator && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(GrabSword());
        }        

        if (isDead)
        {
            return;
        }        
        
        horizontalInput = Input.GetAxis("Horizontal");
        upInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1"))
        {
            if(!isJumping && !isAttacking && staminaNumber != 0)
            {
                Attack();                
            }           

        }

        if (staminaNumber <= 0)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer < 0f)
            {
                staminaNumber = 5;
            }
        }



        if (horizontalInput < 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            playerAnim.SetBool("isWalking", true);


        }

        else if (horizontalInput > 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
            playerAnim.SetBool("isWalking", true);

        }
        else
        {
            playerAnim.SetBool("isWalking", false);
        }

        touchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, wallCheckRadius, wallLayer);
        touchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, wallCheckRadius, wallLayer);



        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (touchingLeftWall && !isAttacking)
            {
                rb.velocity = new Vector2(wallJumpForceX, wallJumpForceY);
                isWallJumping = true;
                isJumping = true;
                isDoubleJumping = false;
                Invoke(nameof(EndWallJump), wallJumpLockTime);
                isDoubleJumping = false;
                playerAnim.Play("Jump", 0, 0f);


            }
            else if (touchingRightWall && !isAttacking)
            {
                rb.velocity = new Vector2(wallJumpForceX, wallJumpForceY);
                isWallJumping = true;
                isJumping = true;
                isDoubleJumping = false;
                Invoke(nameof(EndWallJump), wallJumpLockTime);
                isDoubleJumping = false;
                playerAnim.Play("Jump", 0, 0f);

            }
            else if (!isJumping && !isAttacking)
            {
                rb.velocity = new Vector2(-rb.velocity.x, jumpForce);
                playerAnim.Play("Jump", 0, 0f);
                isJumping = true;
            }


            else if (!isDoubleJumping && isJumping && !isAttacking)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                playerAnim.Play("DoubleJump");
                isDoubleJumping = true;
            }
        }
       

        if (!isAttacking)
        {
            isBlocking = Input.GetMouseButton(1);
            playerAnim.SetBool("isBlocking", isBlocking);
           
        }

        else
        {
            isBlocking = false;
            playerAnim.SetBool("isBlocking", false);
        }

        if (Input.GetMouseButtonDown(1) && !isAttacking)
        {
            canParry = true;
            parryTimer = parryWindow;
        }

        if (canParry)
        {
            parryTimer -= Time.deltaTime;

            if (parryTimer <= 0f)
            {
                canParry = false;
            }
        }



    }
    private void EndWallJump()
    {
        isWallJumping = false;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        if (isPushed || isWallJumping)
        {
            return;
        }
        if (!isAttacking && !isBlocking)
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ShadowFloor"))
        {
            audioSource3.PlayOneShot(ShadowMusic);
        }

        if (collision.gameObject.CompareTag("DurendalBackground"))
        {
            canSwapAnimator = true;
        }
        isJumping = false;
        isDoubleJumping = false;

        if (collision.gameObject.CompareTag("SpecialDoor"))
        {
            if (hasKey)
            {
                Destroy(collision.gameObject);
                doorDestroyed = true;
            }
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {       

        if (collision.gameObject.CompareTag("DurendalBackground"))
        {
            canSwapAnimator = false;
        }       

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.CompareTag("Door"))
        {
            SceneManager.LoadScene("RightRoom");
        }
        else if (other.CompareTag("Door2"))
        {
            SceneManager.LoadScene("LeftRoom");
        }
        else if (other.CompareTag("Door3"))
        {
            if (hasKey && doorDestroyed)
            {
                SceneManager.LoadScene("UpRoom");
            }
        }
        else if (other.CompareTag("Door4"))
        {
            SceneManager.LoadScene("SampleScene");
            spawnToUse = 1;
        }
        else if (other.CompareTag("Door5"))
        {
            SceneManager.LoadScene("SampleScene");
            spawnToUse = 2;
        }
        else if (other.CompareTag("Key")){
            Destroy(other.gameObject);
            hasKey = true;
        }
    }


    public void Attack()
    {
        attackTimer = noAttackTime;
        isAttacking = true;
        audioSource3.PlayOneShot(attackSound);


        float direction = FacingDirection();

        staminaNumber -= 1;
        if(staminaNumber <= 0)
        {
            staminaNumber = 0;
            attackTimer = noAttackTime;
        }

        if(currentAttack == 1)
        {
            playerAnim.Play("Attack");
            rb.AddForce(Vector2.right * direction * attackForce, ForceMode2D.Impulse);
            currentAttack = 2;
        }
        else if(currentAttack == 2)
        {
            playerAnim.Play("Attack2");
            rb.AddForce(Vector2.right * direction * attackForce, ForceMode2D.Impulse);
            currentAttack = 3;
        }
        else if (currentAttack == 3)
        {
            playerAnim.Play("Attack3");
            rb.AddForce(Vector2.right * direction * attackForce, ForceMode2D.Impulse);
            currentAttack = 1;
        }
        


    }

    public void Parry(float force)
    {
        isAttacking = false;
        canParry = false;
        isPushed = true;       

        playerAnim.Play("Parry");
        rb.velocity = Vector2.zero;
        float direction = FacingDirection();
        rb.AddForce(Vector2.right * -direction * force, ForceMode2D.Impulse);


        if (parrySoundNum == 1)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else if(parrySoundNum == 2)
        {
            audioSource.PlayOneShot(clickSound2);
        }
        else
        {
            audioSource.PlayOneShot(clickSound3);
        }
        parrySoundNum += 1;
        
        if(parrySoundNum == 4)
        {
            parrySoundNum = 1;
        }

        Invoke(nameof(EndKnockBack), KnockBackTime);
    }

    public void Block(float force)
    {
        audioSource2.PlayOneShot(BlockSound);
        rb.velocity = Vector2.zero;

        float direction = FacingDirection();
        rb.AddForce(Vector2.right * -direction * force, ForceMode2D.Impulse);

    }

    private void EndKnockBack()
    {
        isPushed = false;
    }

    private float FacingDirection()
    {
        return transform.localScale.x > 0 ? 1f : -1f; 
    }


    public void EndAttack()
    {
        isAttacking = false;        

    }

    public void GetHit()
    {
        if(isDead) 
            return;

        lives--;
        if (lives != 0)
        {
            audioSource2.PlayOneShot(HitSound);
            isAttacking = false;
            playerAnim.Play("Hit");
        }
        else
        {
            isDead = true;
            isAttacking = false;
            isBlocking = false;
            canParry = false;
            rb.velocity = Vector2.zero;
            audioSource2.PlayOneShot(DeathSound);
            playerAnim.Play("Die");
            Destroy(gameObject, 1f);
        }        
    }
    
    void EnableSwordBox()
    {
        collider.enabled = true;
    }

    void DisableSwordBox()
    {
        collider.enabled = false;
    }

    public IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0.05f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
    }

    private IEnumerator GrabSword()
    {
        isGrabbing = true;
        playerAnim.Play("GrabSword");
        yield return new WaitForSeconds(2.7f);
        SceneManager.LoadScene("Alt1stScene");
        hasSword = true;

    }

    public void SwapAnimator()
    {
        if (!usingNewController)
        {
            playerAnim.runtimeAnimatorController = newController;
            usingNewController = true;
        }
        else
        {
            playerAnim.runtimeAnimatorController = ogController;
            usingNewController = false;
        }
    }




}
