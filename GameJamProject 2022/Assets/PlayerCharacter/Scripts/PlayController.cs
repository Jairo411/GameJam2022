using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayController : MonoBehaviour
{
    public float speed;
    public float jumpingSpeed;
    private bool isgrounded;
    private bool buttonPressed;
    private float verticalAcceleration;
    private float horizontalAcceleration;
    private float verticalDeacceleration;
    private float horizontalDeacceleration;
    private float horizontalAxis;
    private Vector3 dirVector;
    private Vector3 movementVector;
    private Vector2 Velocity;
    private Vector3 fallingVector;
    private int comboStep;
    private int collisonCount;
    private bool comboPossible;
    private bool hold = false;
    private bool bowAnim = false;
    private float totalTime = 0.0f;
 
    // Start is called before the first frame update
    void Start()
    {
        speed = 2.0f;
        jumpingSpeed = 10.0f;
        isgrounded = false;
        verticalAcceleration = 1.0f; //accel is tied to the games framerate 
        horizontalAcceleration = 0.0f; // also tied to the framerate
        horizontalDeacceleration = 0.0f;
        verticalDeacceleration = 0.0f;
        horizontalAxis = 0.0f;
        comboStep = 0;
        collisonCount = 0;
        dirVector = new Vector3();
        fallingVector = new Vector3();
        movementVector = new Vector3(dirVector.x, dirVector.y, 0.0f);
        GetComponent<Animator>().SetBool("Grounded", true);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");

        JumpUpdate();
        InputUpdate();
        UpdateVectors();
        animUpdates();
        GetComponent<Animator>().SetFloat("Movement", horizontalAxis);
        Debug.Log("movement V:" + movementVector);
    }

    void JumpUpdate()
    {
        if (isgrounded == true)
        {
           
            UpdateJumpVector();
            GetComponent<Animator>().SetBool("Fall", false);
        }

        if (Input.GetKey(KeyCode.Space) && isgrounded == true)
        {
            isgrounded = false;
            GetComponent<Animator>().Play("jumping");
        }
        if (isgrounded == false)
        {
            SimulateJump(dirVector);
            gameObject.transform.position += movementVector * Time.deltaTime;
        }

    }
    void animUpdates()
    {
        if (isgrounded==false)
        {
            GetComponent<Animator>().SetBool("Grounded", false);
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
            dirVector *= 1f+Time.deltaTime;
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
            dirVector *= 1f+Time.deltaTime;
            gameObject.transform.position += dirVector * Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetMouseButton(1))
        {
            hold = true;
        }
        if (Input.GetMouseButton(1) && hold == true)
        {
            totalTime += Time.deltaTime;
            if (totalTime < 0.048f && bowAnim == false) // three frames
            {
                GetComponent<Animator>().Play("Bow1");
                bowAnim = true;
            }
            else if (totalTime > 0.064f && bowAnim == false) //four frames
            {
                GetComponent<Animator>().Play("BowHold");
                bowAnim = true;
            }
            GetComponent<Animator>().SetBool("Release", false);
        }
        else if (Input.GetMouseButtonUp(1) && bowAnim == true);
    }
    
    void simulateFalling()
    {
        if (totalTime>0.16f)
        {
            GetComponent<Animator>().SetBool("Fall",true);
        }
    }

    void ReleaseBow()
    {
        GetComponent<Animator>().SetBool("Release", true);
        ResetBow();
    }

    void HoldBow()
    {
        bowAnim = true;
    }

    void ResetBow()
    {
        bowAnim = false;
    }
    
    void Attack()
    {
        if (comboStep == 0)
        {
            GetComponent<Animator>().Play("attack1");
            comboStep = 1;
            return;
        }
        if (comboStep!=0)
        {
            if (comboPossible==true)
            {
                Combo();
                comboPossible = false;
                comboStep += 1;
            }
        }
    }
    void ComboPossible()
    {
        comboPossible = true;
    }

    void Combo()
    {
        if (comboStep==1)
        {
            GetComponent<Animator>().Play("attack2");
        }
        else if (comboStep==2)
        {
            GetComponent<Animator>().Play("attack3");
        }
    }

    public void ComboReset()
    {
        ComboPossible();
        comboStep = 0;
    }
    
    AnimationClip FindAnimation(string animationName, AnimationClip[] animationClips)
    {
        int length = animationClips.Length;
        AnimationClip clip = new AnimationClip();
        for (int i = 0; i < length; i++)
        {
            if (animationClips[i].name==animationName)
            {
                clip = animationClips[i];
            }
        }
        return clip;
    }
    void SimulatePush()
    {
        float push = 1.0f;
        float opposite = dirVector.x * -1.0f;
        movementVector = new Vector3(1.0f, 0.0f, 0.0f);
        SimulateInstantHorizontalAcceleration(0.3f);
        float horizontalSpeed = (0.25f + horizontalAcceleration);
        Vector3 acceleration = new Vector3(horizontalSpeed,0.0f,0.0f);

        movementVector += acceleration;

    }

    void UpdateJumpVector()
    {
        verticalAcceleration =1.0f;
        horizontalAcceleration = 1.0f;
        horizontalDeacceleration = 0.0f;
        verticalDeacceleration = 0.0f;
        movementVector = new Vector3(horizontalAcceleration, verticalAcceleration, 0.0f);
    }
    
    void UpdateVectors() // normalize my vectors
    {
        dirVector.Normalize();
        movementVector = new Vector3(dirVector.x, 1.0f, 0.0f);
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
        Vector3 acceleration = new Vector3();

       

        if (verticalAcceleration !=0.0f)
        {
            SimulateInstantHorizontalAcceleration(1.0f);
            SimulateVerticalAcceleration(negativeDelta);
            float vetricalSpeed = 4.0f + verticalAcceleration; // 1/8 of a pixel persecond 
            float horizontalSpeed = (0.25f + horizontalAcceleration)*direction.x;
            acceleration = new Vector3(horizontalSpeed, vetricalSpeed, 0.0f);
        }
        else if (verticalAcceleration==0.0f)
        {
            SimulateInstantHorizontalAcceleration(1.0f);
            if (movementVector.y!=0.0f)
            {
                SimulateVerticalDeacceleration(negativeDelta);
            }
            float vetricalSpeed = 1.0f + verticalAcceleration; // 1/8 of a pixel persecond 
            float horizontalSpeed = (0.25f + horizontalAcceleration) * direction.x;
            acceleration = new Vector3(horizontalSpeed, vetricalSpeed, 0.0f);
        }
        movementVector.y = Time.deltaTime;
        movementVector += acceleration;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collison" + collision.gameObject.name);
        if (collision.gameObject.tag=="Floor")
        {
            isgrounded = true;
            GetComponent<Animator>().SetBool("Grounded", true);
            Debug.Log("Collison" + collision.gameObject.name);
            collisonCount++;
        }
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
        Vector2 jumpdir = movementVector.normalized;
        Gizmos.DrawLine(new Vector3(pos.x, pos.y, 0.0f), new Vector3(pos.x + jumpdir.x,pos.y+jumpdir.y, 0.0f));


    }
}


