using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skateboard : MonoBehaviour
{
    //There is the skateboard at the beginning and on respawn 
    public Vector3 spawnPoint;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private bool _grounded = false;
    private bool _finishedLevel = false;

    //Serialized Fields - will be set in Unity editor
    [SerializeField] private float movementSpeed = 100f;
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private AudioClip ollieAudio;
    [SerializeField] private AudioClip firstTryAudio;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_finishedLevel) return;
        
        Move();
        Rotate();
        Jump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Just keep going
                break;
            case "Finish":
                _finishedLevel = true;
                _audioSource.Stop();
                _audioSource.PlayOneShot(firstTryAudio);
                Invoke(nameof(LoadNextLevel), firstTryAudio.length);
                break;
            default:
                SceneManager.LoadScene(1);
                transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
                _rigidbody.velocity = Vector3.zero;
                break;
        }

        _grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _grounded = false;
    }

    private void Jump()
    {
        float jumpThisFrame = jumpForce * Time.deltaTime;

        //Jumping movement and sound
        if (Input.GetKey(KeyCode.Space) && _grounded)
        {
            _rigidbody.AddRelativeForce(Vector3.up * jumpThisFrame);
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(ollieAudio);
            }
        }
    }

    private void Move()
    {
        float moveThisFrame = movementSpeed * Time.deltaTime;

        //Forward and backwards movement
        if (Input.GetKey(KeyCode.W))
        {
            _rigidbody.AddRelativeForce(Vector3.forward * moveThisFrame);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.AddRelativeForce(Vector3.back * moveThisFrame);
        }
    }

    private void Rotate()
    {
        float rotateThisFrame = movementSpeed * Time.deltaTime;

        _rigidbody.freezeRotation = true;
        transform.SetPositionAndRotation(transform.position,
            new Quaternion(transform.rotation.x, transform.rotation.y, 0f, transform.rotation.w));
        //Left and right movement
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * rotateThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotateThisFrame);
        }

        _rigidbody.freezeRotation = false;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _finishedLevel = false;
    }
}