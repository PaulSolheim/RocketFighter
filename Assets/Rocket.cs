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
            Thrust();
            Rotation();
        }

    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            print("space");
        }
        else
            audioSource.Stop();
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
                LoadNextLevel();
                break;
            default:
                inTransition = true;
                LoadFirstLevel();
                break;
        }
    }

    private static void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);                // todo kill player
    }

    private static void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void Rotation()
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
