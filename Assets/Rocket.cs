using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;

    AudioSource audioSource;

    [SerializeField]float rcsThrust = 100f;

    [SerializeField] float mainThrust = 1000f;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Thrust();
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //Take manue control of rotation

       
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)){
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }else if(Input.GetKey(KeyCode.D)){
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //Resume physics
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly");
                break;
            case "Finish":
                SceneManager.LoadScene(1);
                break;
            default:
                SceneManager.LoadScene(0);
                print("Dead");
                break;
        }
    }

    private void Thrust()
    {
   
        if (Input.GetKey(KeyCode.Space))
        {
            //rigidBody.AddRelativeForce(Vector3.up);
            rigidBody.AddRelativeForce(new Vector3(0, 1, 0) * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }else
        {
            audioSource.Stop();
        }
    }
}