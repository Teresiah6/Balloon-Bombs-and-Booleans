using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    public float yRangeDown = 0;
    public float yRangeUp = 25;

    private Rigidbody playerRb;
    public bool isLowEnough = true;
    public bool isOnGround = true;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip BounceSound;



    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        isLowEnough = true;
        
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Force);

    }

    // Update is called once per frame
    void Update()
    {
        //setting lower boundaries
        if(transform.position.y <= yRangeDown)
        {
            transform.position = new Vector3(transform.position.x, yRangeDown, transform.position.z);
            isOnGround = true;
            isLowEnough = true;
            // add bounce
            playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            playerAudio.PlayOneShot(BounceSound, 1.0f);
        }
        //destroy balloon when it's on the ground and game is over
        if(transform.position.y == yRangeDown && gameOver)
        {
            Destroy(gameObject);
        }
        //setting upper boundaries
        if(transform.position.y > yRangeUp)
        {
            transform.position = new Vector3(transform.position.x, yRangeUp, transform.position.z);
            isOnGround = false;
            isLowEnough = false;
        }
        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
            isLowEnough = true;

        } 

    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);

        }
        

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

}
