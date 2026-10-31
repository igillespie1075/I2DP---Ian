using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBack : MonoBehaviour
{
    private PlayerController player;
    public AudioSource audioSource;
    public AudioClip backSound;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            audioSource.Play();

        }        

    }

    //checks if player is inside of sword background collider and grabs the sword
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
       
        if (player.isGrabbing)
        {
            anim.Play("GrabSword");
        }

    }


}

