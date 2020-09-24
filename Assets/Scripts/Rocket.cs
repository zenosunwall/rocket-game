using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [Header("Rocket Attributes")]
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float thrustSpeed = 10f;

    [Header("Level End Settings")]
    [SerializeField] float levelLoadDelay = 3f;

    [Header("Audio Settings")]
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelComplete;

    [Header("Partical Effects")]
    [SerializeField] ParticleSystem deathExplosion;

    [SerializeField] ParticleSystem engineEffect;
    [SerializeField] ParticleSystem levelSuccessful;

    AudioSource audioSource;
    Rigidbody myRigidbody;

    enum State { Alive, Dying, Transending }
    State currentState;

    bool collideOn = true;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Alive;
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent <AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Alive)
        {
            RespondToTrustInput();
            RespondToRotationInput();
        }
    }

    // Called when collision occurs.
    void OnCollisionEnter(Collision collision)
    {
        if (currentState != State.Alive || !collideOn) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartCoroutine( NextLevel() );
                break;
            default:
                StartCoroutine( Death() );
                break;
        }
    }

    private void RespondToTrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopThrust();
        }

    }

    private void StopThrust()
    {
        StopAudio();
        engineEffect.Stop();
    }

    private void ApplyThrust()
    {
        myRigidbody.AddRelativeForce(Vector3.up * thrustSpeed);
        PlayAudioClip(mainEngine);
        EngineEffect();
    }

    private void RespondToRotationInput()
    {
        

        if (Input.GetKey(KeyCode.A))
        {
            Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(-Vector3.forward);
        }
    }

    private void Rotate(Vector3 rotationVector)
    {
        myRigidbody.angularVelocity = Vector3.zero;
        transform.Rotate(rotationVector * Time.deltaTime * rotationSpeed);
    }

    private IEnumerator NextLevel()
    {
        StopAudioAndPlayNew(levelComplete);
        levelSuccessful.Play();
        currentState = State.Transending;
        yield return new WaitForSeconds(levelLoadDelay);
        GetNextScene();
    }

    private void GetNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator Death()
    {
        engineEffect.Stop();
        deathExplosion.Play();
        StopAudioAndPlayNew(deathSound);
        currentState = State.Dying;
        yield return new WaitForSeconds(levelLoadDelay);
        SceneManager.LoadScene(0);
    }

    private void StopAudioAndPlayNew(AudioClip clip)
    {
        StopAudio();
        PlayAudioClip(clip);
    }

    private void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void EngineEffect()
    {
        if (!engineEffect.isPlaying)
        {
            engineEffect.Play();
        }
    }

    public void TroggleCollision()
    {
        collideOn = !collideOn;
    }
}
