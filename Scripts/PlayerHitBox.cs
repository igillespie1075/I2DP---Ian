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


        if (other.CompareTag("Enemy"))
        {

            EnemyController enemy = other.GetComponentInParent<EnemyController>();
            PlayerController player = GetComponentInParent<PlayerController>();
           
            enemy.GetHit();
            StartCoroutine(camera.Shake(0.03f, 0.03f));
            player.StartCoroutine(player.HitStop(0.08f));

        }

        if (other.CompareTag("Boss"))
        {

            BossController enemy = other.GetComponentInParent<BossController>();
            PlayerController player = GetComponentInParent<PlayerController>();
            enemy.GetHit();
            StartCoroutine(camera.Shake(0.03f, 0.03f));
            player.StartCoroutine(player.HitStop(0.08f));
        }

        if (other.CompareTag("wight"))
        {

            WightScript enemy = other.GetComponentInParent<WightScript>();
            PlayerController player = GetComponentInParent<PlayerController>();
            enemy.GetHit();
            StartCoroutine(camera.Shake(0.03f, 0.03f));
            player.StartCoroutine(player.HitStop(0.08f));
        }


        else if (other.CompareTag("BreakDoor"))
        {           
            
            Destroy(other.gameObject);
            
        }
        else if (other.CompareTag("BreakYellowDoor"))
        {
            if (PlayerController.hasSword)
            {
                Destroy(other.gameObject);
            }
        }

        

       
        

    }
}
