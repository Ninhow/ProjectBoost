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
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem sucessParticles;
    [SerializeField] ParticleSystem deathParticles;

    enum State
    {
        Alive,
        Dying,
        Transcending
    }

    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Rotate();
            Thrust();
        }
        
    }

    private void LoadNextScene()
    {
        sucessParticles.Stop();
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        deathParticles.Stop();
        SceneManager.LoadScene(0);
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
        if(state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly");
                break;
            case "Finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(success);
                sucessParticles.Play();
                Invoke("LoadNextScene", 1f);
                
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticles.Play();
                Invoke("LoadFirstLevel", 1f);
                print("Dead");
                break;
        }

    }

    private void Thrust()
    {
   
        if (Input.GetKey(KeyCode.Space))
        {
            //rigidBody.AddRelativeForce(Vector3.up);
            rigidBody.AddRelativeForce(new Vector3(0, 1, 0) * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
                
            }
            mainEngineParticles.Play();
        }
        else
        {
            mainEngineParticles.Stop();
            audioSource.Stop();
        }
    }
}