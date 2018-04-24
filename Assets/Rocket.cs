using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    bool inTransition = false;


    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inTransition)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            print("space");
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        mainEngineParticles.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (inTransition) { return;  }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                print("OK");    // todo remove
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        inTransition = true;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", 1f);
    }

    private void StartSuccessSequence()
    {
        inTransition = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", 1f);  // todo parameterize time
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);                // todo kill player
        inTransition = false;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
        inTransition = false;
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero;

        float rotationThisFrame = rotationThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        
    }
}
