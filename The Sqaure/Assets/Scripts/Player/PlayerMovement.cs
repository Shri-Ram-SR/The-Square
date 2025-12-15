using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    int Direction;
    bool Grounded;
    [SerializeField] GravityValues Gravity;
    [SerializeField] PlayerState State;
    [Header("Moving")]
    public float Speed;
    [SerializeField] float MaxGroundSpeed = 12;
    [SerializeField] float MaxAirSpeed = 20;
    [SerializeField] float Acceleration = 45;
    [SerializeField] float Deceleration = 65;
    public bool IgnoreMovement;

    [Header("Jumps")]
    [SerializeField] float JumpForce = 40;

    [Header("Coyote jump")]
    [SerializeField] float CoyoteTime = 0.1f;
    [SerializeField] float CoyoteTimer;

    [Header("Buffer jump")]
    [SerializeField] float BufferTime = 0.1f;
    float BufferTimer;

    [Header("Elevate")]
    [SerializeField] float ElevatePower = 60;

    [Header("Abilities")]
    [SerializeField] bool HasElevate;
    [SerializeField] bool HasRoll;
    [SerializeField] bool HasStoneGrip;
    [SerializeField] bool HasSlam;
    [SerializeField] bool HasVinePull;
    [SerializeField] bool HasDash;

    [Header("Dash")]
    [SerializeField] float DashCoolDownTime;
    [SerializeField] float DashCoolDownTimer;
    [SerializeField] float DashDurTime;
    [SerializeField] float DashPower;


    [Header("Slam")]
    [SerializeField] float SlamCoolDownTime = 1;
    [SerializeField] float SlamPower = 100;
    float SlamTimer;

    [Header("Misc")]
    [SerializeField] float ExtraInvincibilityDur;
    [SerializeField] LayerMask Ground_Layer;
    [SerializeField] LayerMask Ground_Wall_Layer;
    [SerializeField] Transform GO_Ground_Check;
    [SerializeField] Transform GO_Front_Check;

    [Header("References")]
    PlayerHealthSystem PHS;
    PlayerCombat PC;
    Rigidbody2D rb;

    public static PlayerMovement Instance;
    #endregion
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        RespawnManager.Instance.UpdateRespawnPoint(transform.position);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        rb = GetComponent<Rigidbody2D>();
        PHS = GetComponent<PlayerHealthSystem>();
        PC = GetComponent<PlayerCombat>();
    }
    //Calls neccessary functions and runs timers
    private void FixedUpdate()
    {
        ChangeSpeed();
        FrontCheck();
        CheckGround();
        StateManager();
        //Coyote Timer
        if (Grounded)
            CoyoteTimer = CoyoteTime;
        else
            CoyoteTimer -= Time.deltaTime;

        //Buffer Timer;
        if (BufferTimer > 0)
            BufferTimer -= Time.deltaTime;
        else
            BufferTimer = 0;

        //Slam timer
        if (SlamTimer > 0)
            SlamTimer -= Time.deltaTime;
        else
            SlamTimer = 0;

        //Dash Timer
        if (DashCoolDownTimer > 0)
            DashCoolDownTimer -= Time.deltaTime;
        else
            DashCoolDownTimer = 0;
    }
    void StateManager()
    {
        //Dash
        if (State == PlayerState.Dash)
        {
            rb.gravityScale = Gravity.Dash;
            rb.linearVelocityX = DashPower * transform.right.x;
            return;
        }
        //In air
        if (rb.linearVelocityY > 0)
        {
            State = PlayerState.InAir;
            rb.gravityScale = Gravity.Regular;
            return;
        }
        //Moving or Idle
        if (Grounded)
        {
            if (Speed == 0)
                State = PlayerState.Idle;
            else
                State = PlayerState.Moving;
            rb.gravityScale = Gravity.Regular;
            return;
        }
        //Sliding
        if (State == PlayerState.Sliding)
        {
            rb.gravityScale = Gravity.Sliding;
            if (rb.linearVelocityY < -Gravity.Sliding * 3)
                rb.linearVelocityY = -Gravity.Sliding * 3;
            return;
        }
        //Slam
        if (State == PlayerState.Slam)
        {
            rb.gravityScale = Gravity.Slam;
            return;
        }
        //Jump peak
        if (Mathf.Abs(rb.linearVelocityY) < 2)
        {
            State = PlayerState.JumpPeak;
            rb.gravityScale = Gravity.JumpPeak;
            return;
        }
        //Falling
        if (rb.linearVelocityY < 0)
        {
            State = PlayerState.Falling;
            rb.gravityScale = Gravity.Falling;
            if (rb.linearVelocityY < -Gravity.Falling * 3)
                rb.linearVelocityY = -Gravity.Falling * 3;
            return;
        }
        Debug.LogWarning("No state found");
        return;
    }
    //Checks if grounded
    void CheckGround()
    {
        if (Physics2D.OverlapCircle(GO_Ground_Check.transform.position, .1f, Ground_Layer))
        {
            if (State == PlayerState.Slam)
            {
                PC.Slam();
            }
            Grounded = true;
            //Checks if jump has been presses just before landing
            if (BufferTimer > 0)
            {
                rb.linearVelocityY = JumpForce;
            }
        }
        else
            Grounded = false;
    }
    //Checks if pressed against a wall
    void FrontCheck()
    {
        if (Grounded) return;
        //Sets speed to 0 and sets up sliding
        if (Physics2D.OverlapBox(GO_Front_Check.transform.position, new Vector2(.2f, .2f), 0, Ground_Wall_Layer))
            Speed = 0;
        if (Physics2D.OverlapBox(GO_Front_Check.transform.position, new Vector2(.2f, .2f), 0, Ground_Layer))
        {
            //When jumps while sliding
            if (rb.linearVelocityY > 0)
            {
                State = PlayerState.InAir;
            }
            else if (HasStoneGrip)
            {
                State = PlayerState.Sliding;
                CoyoteTimer = CoyoteTime;
            }
        }
        //Checks if fallen off the wall
        else if (State == PlayerState.Sliding)
        {
            State = PlayerState.Falling;
        }
    }

    #region Movement
    public void JumpAction(bool b)
    {
        bool jump = false;
        if (State == PlayerState.Slam)
            return;
        //Variable Jump height
        if (!b)
        {
            if (State == PlayerState.InAir)
                rb.linearVelocityY /= 2;
            return;
        }

        //Checks if original jumps is still viable due to coyote time
        if (CoyoteTimer > 0 || State == PlayerState.Sliding)
        {
            jump = true;
        }
        //Buffer jump initiated
        else
        {
            BufferTimer = BufferTime;
        }

        if (jump)
        {
            if (State == PlayerState.Sliding)
            {
                rb.linearVelocityY = JumpForce * 1.2f;
                rb.linearVelocityX += -transform.right.x * MaxAirSpeed;
                Ignoremovement(.2f);
            }
            else
                rb.linearVelocityY = JumpForce;
        }
    }
    //Accelearate
    void Accelearation()
    {
        float MaxSpeed = Grounded ? MaxGroundSpeed : MaxAirSpeed;
        if (Mathf.Abs(Speed) < MaxSpeed)
            Speed += Time.deltaTime * Acceleration * Direction;
        else
            Speed = MaxSpeed * Direction;
    }
    //Decelearate
    void Decelearation()
    {
        if (Speed < 0)
        {
            Speed += Time.deltaTime * Deceleration;
            if (Speed > 0)
                Speed = 0;
        }
        else
        {
            Speed -= Time.deltaTime * Deceleration;
            if (Speed < 0)
                Speed = 0;
        }
    }
    //Handles accelearation and decelearation and applying of speed
    void ChangeSpeed()
    {
        if (IgnoreMovement) return;

        int d = Direction;
        if (Direction == 0)
            Direction = GetJoystickValue.Instance.GetDirection();

        //Detects if changing direction
        if (d != Direction)
            Speed = 0;
        //Apply acceleration and Ignore movement if needed
        if (Direction != 0)
        {
            if (State != PlayerState.Sliding && State != PlayerState.Slam)
                Accelearation();
            transform.rotation = Quaternion.Euler(new Vector3(0, Direction == 1 ? 0 : 180, 0));
        }
        else
        {
            if (Speed != 0)
                Decelearation();
            //If there is no input but has speed
            else if (rb.linearVelocityX != 0)
            {
                Speed = rb.linearVelocityX;
                Decelearation();
            }
        }
        rb.linearVelocityX = Speed;
    }
    #endregion


    #region Abilities
    public void Slam()
    {
        if (!HasSlam)
            return;
        if (SlamTimer == 0)
        {
            //Negates all movement the player had
            rb.linearVelocity = Vector2.zero;
            Speed = 0;
            rb.AddForce(transform.up * -SlamPower);
            //Prep
            SlamTimer = SlamCoolDownTime;
            State = PlayerState.Slam;
        }
    }
    public void Elevate()
    {
        if (!HasElevate)
            return;
        if (Grounded)
        {
            rb.linearVelocityY = ElevatePower;
        }
    }
    public void VinePull()
    {
        if (!HasVinePull) return;
        Debug.Log("Vine pull");
    }
    public void Dash()
    {
        if (HasDash && DashCoolDownTimer <= 0)
        {
            Ignoremovement(DashDurTime);
            rb.linearVelocity = Vector2.zero;
            State = PlayerState.Dash;
            DashCoolDownTimer = DashCoolDownTime;
            Invoke("StopDash", DashDurTime);
        }
    }
    void StopDash()
    {
        Debug.Log("Stop dash");
        State = PlayerState.Idle;
        rb.linearVelocityX = 0;
    }
    #endregion


    #region Misc
    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.started)
            JumpAction(true);
        else if (context.canceled)
            JumpAction(false);
    }
    void SetIgnoreMovement()
    {
        IgnoreMovement = false;
    }
    public void Ignoremovement(float f)
    {
        IgnoreMovement = true;
        Invoke("SetIgnoreMovement", f);
    }
    public int GetDir()
    {
        return Direction;
    }
    public void PutDir(InputAction.CallbackContext context)
    {
        int d = Direction;
        Direction = (int)context.ReadValue<float>();
        if (d != Direction) Speed = 0;
    }
    public PlayerState GetState()
    {
        return State;
    }
    public void SetState(PlayerState state)
    {
        State = state;
    }
    public void SetVelocity(Vector2 v)
    {
        rb.linearVelocity = v;
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(GO_Front_Check.position, .1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(GO_Front_Check.position, new Vector2(.2f, .2f));
    }
    #endregion
}
public enum PlayerState
{
    Idle,
    InAir,
    Moving,
    JumpPeak,
    Falling,
    Slam,
    Sliding,
    Dash,
    Zero
}
[System.Serializable]
class GravityValues
{
    public float Regular;
    public float JumpPeak;
    public float Falling;
    public float Sliding;
    public float Slam;
    public float Dash;
    public float Zero;
}