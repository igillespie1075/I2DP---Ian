using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitBox : MonoBehaviour
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
    void Update()
    {

        if (parryCount > 0)
        {
            parryTimer -= Time.deltaTime;

            if (parryTimer < 0f)
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
        BossController enemy = GetComponentInParent<BossController>();


        if (player.canParry)
        {

            float force = enemy.attackNumber == 3 ? 1f : 0.5f;
            player.Parry(force);
            parryCount++;

            parryTimer = stunTimeWindow;

            if (parryCount == 10)
            {
                enemy.Stunned(stunTime);
                parryCount = 0;
                parryTimer = 0f;
            }

            return;
        }



        else if (player.isBlocking)
        {
            player.Block(2);
            Debug.Log("Blocked");
            return;
        }

        else
        {
            player.GetHit();
        }



    }
}
