using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Body {  get; private set; }

    public float HSpeed { get; set; }
    public float VSpeed { get; set; }
    public bool CanJump { get; set; }

    public PlayerState CurrentState { get; private set; }

    [Header("Universal Variables")]
    public float maxSpeed;
    public float jumpSpeed;

    public PlayerGroundState groundState = new PlayerGroundState();
    public PlayerAirState airState = new PlayerAirState();

    void Start()
    {
        Body = GetComponent<Rigidbody2D>();

        SetState(groundState);
    }

    void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.UpdateState(this);
            CurrentState.ChangeState(this);
        }
    }

    public void SetState(PlayerState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState(this);
        }

        CurrentState = newState;

        if (CurrentState != null)
        {
            CurrentState.StartState(this);
        }
    }

    public void Move()
    {
        Vector2 velocity = Vector2.right * HSpeed;
        velocity.y = VSpeed;

        Body.MovePosition(Body.position + velocity * Time.deltaTime);
    }
}
