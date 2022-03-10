using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovements : MonoBehaviour
{
    public float speedWalking;
    public float speedRunning;
    public float jumpHeight = 1f;

    // Private vars
    float speed;
    float speedTarget;
    float animationSpeed;
    float animationSpeedTarget;
    float lerpSpeed;

    float groundDistance = 0.25f;
    LayerMask groundLayerMask = 1;
    Vector3 moveDirection;
    Rigidbody rb;
    bool isGrounded;
    Animator animator;

    float vertical, horizontal;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Ground check
        // Créer un layer pour le personnage pour qu'il évite de se détecter lui-même
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundLayerMask, QueryTriggerInteraction.Ignore);

        animator.SetBool("inAit", !isGrounded);

        // 1.2 Inputs
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        // Animations de déplacement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Vitesse de déplacement et animation
            speedTarget = speedRunning;
            animationSpeedTarget = 2f;
        }
        else
        {
            // Vitesse de déplacement et animation
            speedTarget = speedWalking;
            animationSpeedTarget = 1f;
        }

        // Interpolations
        lerpSpeed = Time.deltaTime * 3f;

        speed = Mathf.Lerp(speed, speedTarget, lerpSpeed);
        animationSpeed = Mathf.Lerp(animationSpeed, animationSpeedTarget, lerpSpeed);

        // Horizontal
        animator.SetFloat("horizontal", horizontal * animationSpeed);
        // Vertical
        animator.SetFloat("vertical", vertical * animationSpeed);

        // Déplacements
        moveDirection = transform.forward * vertical;
        moveDirection += transform.right * horizontal;

        // ------------------------------------------------------------

        // Jump -------------------------------------------------------
        if (Input.GetButtonDown("Jump") && isGrounded)
        {            
            Jump();
            
        }

        // Respawn ------------------------------------------------
        if (transform.position.y < -15f)
            transform.position = Vector3.zero;
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        animator.SetTrigger("Jump");
    }

    private void FixedUpdate()
    {        
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }
}
