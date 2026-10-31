using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{

    private int parryCount = 0;
    private float stunTime = 2f;
    private float stunTimeWindow = 1f;
    private float parryTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    //Checks if the player parries a certain amount of attacks within a time frame so that enemies can be stunned
    void Update()
    {
        if(parryCount > 0)
        {
            parryTimer -= Time.deltaTime;

            if(parryTimer < 0f)
            {
                parryCount = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerController player = other.GetComponent<PlayerController>();
        EnemyController enemy = GetComponentInParent<EnemyController>();

        //Checks if player can parry (right clicks when enemy hitbox hits player)
        if (player.canParry)
        {
            //applies varried forces to player based on enemy attack number

            float force = enemy.attackNumber == 3 ? 1f : 0.5f;
            player.Parry(force);
            parryCount++;

            parryTimer = stunTimeWindow;

            //stuns enemy if parried enough within time frame
            if (parryCount == 3)
            {
                enemy.Stunned(stunTime);
                parryCount = 0;
                parryTimer = 0f;
            }

            return;
        }
        
       
        //checks if player blocked and applies force to player
        else if (player.isBlocking)
        {
            player.Block(2);
            Debug.Log("Blocked");
            return;
        }

        //otherwise the player was hit
        else
        {
            player.GetHit();            
        }

       

    }

    
}
