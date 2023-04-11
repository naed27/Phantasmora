using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    // Properties
    [SerializeField]
    private float moveSpeed = 1;
    Animator animator;
    Rigidbody2D rigidBody2D;
    float scaleX = 1;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * moveSpeed;
        rigidBody2D.velocity = velocity;
        animator.SetFloat("Speed", velocity.sqrMagnitude);


        if(velocity.x != 0)
        {
            // Flip Animation
            Vector3 scale = transform.localScale;
            scale.x = scaleX * (velocity.x >= 0 ? 1 : -1);
            transform.localScale = scale;
        }
     
    }
}
