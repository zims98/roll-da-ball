using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Vector3 moveInput;
    [SerializeField] float force = 8;
    [SerializeField] float maxSpeed = 6;
    [SerializeField] float brakeFactor = 4;
    bool isBraking;

    // Particle System references
    [SerializeField] ParticleSystem pickupEffect;
    [SerializeField] ParticleSystem finalPickupEffect;
    [SerializeField] ParticleSystem ballExplosion;

    // Script references
    [SerializeField] ScoreManager scoreScript;
    [SerializeField] LevelLoader loaderScript;
    [SerializeField] GameMaster gameMasterScript;
    [SerializeField] CamShake shakeScript;

    Scene currentScene;    

    // Velocity Directions
    [HideInInspector] public Vector3 positiveVelocityDirPos;
    [HideInInspector] public Vector3 negativeVelocityDirPos;
    [SerializeField] float maxDirLength = 4f;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        if (Input.GetKey(KeyCode.Space))
        {
            isBraking = true;
        }
        else isBraking = false;
        
          
        positiveVelocityDirPos = transform.position + Vector3.ClampMagnitude(moveInput + rb.velocity, maxDirLength); // Velocity's direction forward
        negativeVelocityDirPos = transform.position + Vector3.ClampMagnitude(moveInput - rb.velocity, maxDirLength); // Velocity's direction backwards
    }

    void FixedUpdate()
    {
        if (isBraking)
        {
            rb.AddForce(-brakeFactor * rb.velocity);
        }
        else rb.AddForce(moveInput * force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed); // Limits speed

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup")) // Pickups required to unlock the door
        {
            FindObjectOfType<AudioManager>().Play("Scoring");
            scoreScript.IncrementScore();
            Instantiate(pickupEffect, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);          
        }

        if (other.gameObject.CompareTag("Final Pickup")) // Pickup to proceed to next level or win the game
        {
            FindObjectOfType<AudioManager>().Play("Final Pickup");
            Instantiate(finalPickupEffect, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);

            if (currentScene.name == "Level 1")
            {
                loaderScript.FadeToLevel(1);               
            }
                
            else if (currentScene.name == "Level 2")
            {
                loaderScript.FadeToLevel(2);
            }

            else if (currentScene.name == "Level 3")
            {
                gameMasterScript.Victory();
            }
        }

        if (other.gameObject.CompareTag("Enemy")) // If collision occurs with the Player -> Kill
        {
            FindObjectOfType<AudioManager>().Play("Ball Explosion");
            shakeScript.Shake = 1f;
            Instantiate(ballExplosion, transform.position, Quaternion.identity);
            gameMasterScript.GameOver();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike")) // If collision occurs with the Player -> Kill
        {
            FindObjectOfType<AudioManager>().Play("Ball Explosion");
            shakeScript.Shake = 1f;
            Instantiate(ballExplosion, transform.position, Quaternion.identity);
            gameMasterScript.GameOver();
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos() // Debugging
    {
        Debug.DrawLine(transform.position, positiveVelocityDirPos, Color.blue);
        Debug.DrawLine(transform.position, negativeVelocityDirPos, Color.red);
    }
}
