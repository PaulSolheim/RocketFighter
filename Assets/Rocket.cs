using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] float mainThrust = 100f;

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
            audioSource.Stop();
    }

    private void ApplyThrust()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
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
                inTransition = true;
                Invoke("LoadNextLevel", 1f);  // todo parameterize time
                break;
            default:
                inTransition = true;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
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
