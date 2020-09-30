using UnityEngine;
using UnityEngine.SceneManagement;

public class Skateboard : MonoBehaviour
{
    // TODO: fix jumping bug
    
    public Vector3 spawnPoint;
    private Rigidbody _rigidbody;
    private bool _grounded = true;
    private AudioSource _jumpAudio;
    [SerializeField] private float movementSpeed = 100f;
    [SerializeField] private float jumpForce = 100f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _jumpAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        Jump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Collided with friendly object");
                break;
            case "Finish":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        
        //Jumping movement
        if (Input.GetKey(KeyCode.Space) && _grounded)
        {
            _rigidbody.AddRelativeForce(Vector3.up * jumpThisFrame);
            if (!_jumpAudio.isPlaying)
            {
                _jumpAudio.Play();
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
        transform.SetPositionAndRotation(transform.position, new Quaternion(transform.rotation.x, transform.rotation.y, 0f, transform.rotation.w));
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
}