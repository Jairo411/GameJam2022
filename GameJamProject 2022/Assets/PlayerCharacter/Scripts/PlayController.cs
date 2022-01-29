using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    public float speed;
    public float jumpingSpeed;
    private bool isgrounded;
    private float verticalAcceleration;
    private float horizontalAcceleration;
    private float verticalDeacceleration;
    private float horizontalDeacceleration;
    private float horizontalAxis;
    private Vector3 dirVector;
    private Vector3 jumpVector;
    private Vector2 Velocity;
    // Start is called before the first frame update
    void Start()
    {
        speed = 2.0f;
        jumpingSpeed = 10.0f;
        isgrounded = true;
        verticalAcceleration = 1.0f; //accel is tied to the games framerate 
        horizontalAcceleration = 0.0f; // also tied to the framerate
        horizontalDeacceleration = 0.0f;
        verticalDeacceleration = 0.0f;
        horizontalAxis = 0.0f;
        dirVector = new Vector3();
        jumpVector = new Vector3(dirVector.x, dirVector.y, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");

        JumpUpdate();
        InputUpdate();
        UpdateVectors();
    }

    void JumpUpdate()
    {
        if (isgrounded == true)
        {
            UpdateJumpVector();
        }

        if (Input.GetKey(KeyCode.Space) && isgrounded == true)
        {
            isgrounded = false;
        }
        if (isgrounded == false)
        {
            SimulateJump(dirVector);
            gameObject.transform.position += jumpVector * Time.deltaTime;
        }
    }
    void InputUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            //If moving right
            GetComponent<SpriteRenderer>().flipX = false;
            Velocity = new Vector2((speed) * horizontalAxis, 0.0f);
            if (Input.GetKey(KeyCode.Space))
            {
                //If space is pressed
                isgrounded = false;
            }
            dirVector = new Vector3(Velocity.x, Velocity.y, 0.0f);
            gameObject.transform.position += dirVector * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            Velocity = new Vector2((speed) * horizontalAxis, 0.0f);
            //IF moving left 
            if (Input.GetKey(KeyCode.Space))
            {
                //If space is pressed
                isgrounded = false;

            }
            dirVector = new Vector3(Velocity.x, Velocity.y, 0.0f);
            gameObject.transform.position += dirVector * Time.deltaTime;
        }
       
    }

    //If is ground is false the have this code running
    void UpdateJumpVector()
    {
        verticalAcceleration = 1.0f;
        horizontalAcceleration = 1.0f;
        horizontalDeacceleration = 0.0f;
        verticalDeacceleration = 0.0f;
        jumpVector = new Vector3(horizontalAcceleration, verticalAcceleration, 0.0f);
    }
    
    void UpdateVectors() // normalize my vectors
    {
        dirVector.Normalize();
        jumpVector = new Vector3(dirVector.x, 1.0f, 0.0f);
    }
    void SimulateInstantHorizontalAcceleration(float maxvelocity) // instant acceleration assuming this is in px/ms
    {
        horizontalAcceleration = maxvelocity;
    }

    void SimulateHorizontalAcceleration(float maxvelocity, float deltaTime)
    {

    }

    void SimulateHorizontalDeacceleration(float deltaTime)
    {

    }
    void SimulateVerticalAcceleration(float deltaTime) //Start with whatever intial velocity you have then only slow down when you land
    {
        if (verticalAcceleration>0.0f)
        {
            verticalAcceleration += deltaTime;
        }
        else if (verticalAcceleration<=0.0f)
        {
            verticalAcceleration = 0.0f;
        }
    }
    void SimulateVerticalDeacceleration(float deltaTime) // Start with intial velocity then bring the velocity down 
    {
        if (verticalDeacceleration != -1.0f)
        {
            verticalDeacceleration += deltaTime;
        }
        else if (verticalDeacceleration <= -1.0f)
        {
            verticalDeacceleration = -1.0f;
        }
    }

    void SimulateJump(Vector2 direction) //Simulate the jump using my own physics 
    {
        float negativeDelta = Time.deltaTime * -1.0f;
        Vector3 Accelerator = new Vector3();

        if (verticalAcceleration !=0.0f)
        {
            SimulateInstantHorizontalAcceleration(1.0f);
            SimulateVerticalAcceleration(negativeDelta);
            float vetricalSpeed = 2.0f + verticalAcceleration; // 1/8 of a pixel persecond 
            float horizontalSpeed = (0.25f + horizontalAcceleration)*direction.x;
            Accelerator = new Vector3(horizontalSpeed, vetricalSpeed, 0.0f);
        }
        else if (verticalAcceleration==0.0f)
        {
            SimulateInstantHorizontalAcceleration(1.0f);
            if (jumpVector.y!=0.0f)
            {
                SimulateVerticalDeacceleration(negativeDelta);
            }
            float vetricalSpeed = 1.0f + verticalAcceleration; // 1/8 of a pixel persecond 
            float horizontalSpeed = (0.25f + horizontalAcceleration) * direction.x;
            Accelerator = new Vector3(horizontalSpeed, vetricalSpeed, 0.0f);
        }
        
        jumpVector += Accelerator;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isgrounded = true;
        Debug.Log("Collison" + collision.gameObject.name);
    }
    void OnDrawGizmosSelected()
    {
        /* 
         *Direction vector debug code
         */
        Gizmos.color = Color.green;
        Vector2 dir=Velocity.normalized;
        Vector2 pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Gizmos.DrawLine( new Vector3(pos.x, pos.y, 0.0f), new Vector3(pos.x+dir.x,pos.y+dir.y,0.0f));

        Gizmos.color = Color.red;
        Vector2 jumpdir = jumpVector.normalized;
        Gizmos.DrawLine(new Vector3(pos.x, pos.y, 0.0f), new Vector3(pos.x + jumpdir.x,pos.y+jumpdir.y, 0.0f));


    }
}


