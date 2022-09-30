using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;
    private const float TURN_SPEED = 0.05f;
    // Movement
    private CharacterController controller;
    private float jumpForce = 4.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private float speed = 2.0f;// 7
    private int desiredLane = 1; // 0 -left 1 -middle 2-right

    // Animation
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // get user input on which lane he should be
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Move Left
            MoveLane(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLane(true);
        }

        // calculate where player should be
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0) // left
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2) // right
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }

        // move vector calculation
        Vector3 moveVector = Vector3.zero;

        moveVector.x = (targetPosition - transform.position).normalized.x * speed; // where player is supposed to be - where he is at the moment

        bool isGrounded = IsGrounded();
        animator.SetBool("Grounded", isGrounded);

        // calculate Y
        if (isGrounded) //on the ground
        {
            verticalVelocity = -0.1f; // snap to the floor at all times

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Jump
                animator.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // make the num go down
            verticalVelocity -= (gravity * Time.deltaTime);

            // fast falling mechanic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = -jumpForce;
            }
        }


        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        // Move the player
        controller.Move(moveVector * Time.deltaTime);

        // Rotate player to where he is going
        Vector3 direction = controller.velocity;
        if (direction != Vector3.zero)
        {
            direction.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, direction, TURN_SPEED);
        }

    }

    private void MoveLane(bool goingRight)
    {
        //// left
        //if(!goingRight)
        //{
        //    desiredLane--;
        //    if(desiredLane == -1)
        //    {
        //        desiredLane = 0;
        //    }
        //} else
        //{
        //    desiredLane++;
        //    if(desiredLane == 3)
        //    {
        //        desiredLane = 2;
        //    }
        //}
        //The above line of code can be written as 
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        // a ray is a 2 parameter object that takes in an original position and direction
        Ray groundRay = new Ray(new Vector3(
            controller.bounds.center.x,
            (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);
        Debug.DrawLine(groundRay.origin, groundRay.direction, Color.cyan,1.0f);

        return (Physics.Raycast(groundRay, 0.2f + 0.1f));
        
    }
}
