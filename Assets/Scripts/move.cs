using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;

    // Para el movimiento horizontal
    public float horizontalInput;
    public float verticalInput;

    public Transform groundCheck;   
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    // Para controlar el squash & stretch
    [HideInInspector] public Vector3 currentMoveDirection = Vector3.zero;
    [HideInInspector] public float verticalVelocity;

    // Para controlar el salto
    private Rigidbody rb;
    private bool isGrounded = true;
    private bool jumpRequested = false;

    // Para la interacción con obstaculos/hazards
    public GameObject gdicon; // asignar desde el inspector
    public Transform respawnPoint; // asignar desde el inspector
    public GameObject explosionPrefab; // opcional

    // explosion.mp3
    private AudioSource audioSource;
    public AudioClip deathSound;

    // Detener el movimiento cuando muere por 2 segundos
    public bool canMove = true;

    // Musica de fondo
    public AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
      rb = GetComponent<Rigidbody>();
      audioSource = GetComponent<AudioSource>();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float jumpInput = Input.GetAxis("Jump");

        // Movimiento horizontal
        Vector3 moveX = Vector3.right * horizontalInput * speed * Time.deltaTime;
        Vector3 moveZ = Vector3.forward * verticalInput * speed * Time.deltaTime;

        transform.Translate(-moveX, Space.World);
        transform.Translate(-moveZ, Space.World);

        // Actualizar dirección de movimiento visible para playerVisual
        currentMoveDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // Revisar si está en el suelo usando Raycast
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        }
        else
        {
            Debug.LogWarning("Asigna un objeto a 'groundCheck' en el inspector.");
        }

        // Movimiento vertical
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        //(Código viejo, Transform y RigidBody no funcionan muy bien juntos)
        //verticalVelocity = jumpSpeed * jumpInput;
        //Vector3 jump = Vector3.up * verticalVelocity * Time.deltaTime;
        //transform.Translate(jump, Space.World);

        if (Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }

        //Limites del mapa
        if (transform.position.x < -11.25f)
        {
           transform.position = new Vector3(-11.25f, transform.position.y, transform.position.z);
        }  

        if (transform.position.x > 0.65f)
        {
           transform.position = new Vector3(0.65f, transform.position.y, transform.position.z);
        }

        if (transform.position.z < -11.69f)
        {
           transform.position = new Vector3(transform.position.x, transform.position.y, -11.69f);
        }  

        if (transform.position.z > -7.10f)
        {
           transform.position = new Vector3(transform.position.x, transform.position.y, -7.10f);
        }

        if (transform.position.y > 15f)
        {
           transform.position = new Vector3(transform.position.x, 15f, transform.position.z);
        }


        // Hundimiento lento en el agua
        if (isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, -0.5f, rb.velocity.z);
        }

        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    void FixedUpdate()
    {
        // Ejecutar el salto solo si se debería poder
        if (jumpRequested && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Resetear el salto cada FixedUpdate
        jumpRequested = false;
    }

    // Comportamiento del respawn
    public void DieAndRespawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        canMove = false;
        Vector3 deathPosition = transform.position;

        // Parar la musica
        if (backgroundMusic != null)
            backgroundMusic.Pause();

        // Apagar modelo
        gdicon.SetActive(false);
        
        // Apagar movimiento físico
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Efecto de explosión
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, deathPosition, Quaternion.identity);

        // Sonido se reproduce
        audioSource.Play();

        yield return new WaitForSeconds(2f);

        // Vuelve la musica
        if (backgroundMusic != null)
            backgroundMusic.UnPause();

        
        // Respawn
        transform.position = respawnPoint.position;

        // Reactivar modelo y físicas
        gdicon.SetActive(true);
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;

        canMove = true;
    }
}