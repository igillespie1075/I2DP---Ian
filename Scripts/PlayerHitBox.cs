using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private CameraController camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        //Checks if player hitbox hits enemy, shakes screen if hits, and stops frame if hits 
        if (other.CompareTag("Enemy"))
        {

            EnemyController enemy = other.GetComponentInParent<EnemyController>();
            PlayerController player = GetComponentInParent<PlayerController>();
           
            enemy.GetHit();
            StartCoroutine(camera.Shake(0.03f, 0.03f));
            player.StartCoroutine(player.HitStop(0.08f));

        }

        //Same for boss
        if (other.CompareTag("Boss"))
        {

            BossController enemy = other.GetComponentInParent<BossController>();
            PlayerController player = GetComponentInParent<PlayerController>();
            enemy.GetHit();
            StartCoroutine(camera.Shake(0.03f, 0.03f));
            player.StartCoroutine(player.HitStop(0.08f));
        }

        //Same for the tall enemy
        if (other.CompareTag("wight"))
        {

            WightScript enemy = other.GetComponentInParent<WightScript>();
            PlayerController player = GetComponentInParent<PlayerController>();
            enemy.GetHit();
            StartCoroutine(camera.Shake(0.03f, 0.03f));
            player.StartCoroutine(player.HitStop(0.08f));
        }

        //Breaks the blue doors when player hits them
        else if (other.CompareTag("BreakDoor"))
        {           
            
            Destroy(other.gameObject);
            
        }

        //Checks if player has the new sword that can break the yellow doors

        else if (other.CompareTag("BreakYellowDoor"))
        {
            if (PlayerController.hasSword)
            {
                Destroy(other.gameObject);
            }
        }

        

       
        

    }
}
