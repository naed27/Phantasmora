using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    // Properties
    [SerializeField] float moveSpeed;

    Rigidbody2D rb;
    Vector2 movement;
    Animator animator;

    private float scaleX = 1;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0)
        {
            // Flip Animation
            Vector3 scale = transform.localScale;
            scale.x = scaleX * (movement.x >= 0 ? 1 : -1);
            transform.localScale = scale;
        }
    }
     
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement.normalized);
    }
}
