using UnityEngine;

[System.Serializable]
public class PlayerGroundState : PlayerState
{
    [SerializeField] float accel;
    [SerializeField] float decel;
    [SerializeField] float fric;
    [SerializeField] float coyoteTime;
    private float currentCoyoteTime;

    public override void StartState(PlayerController player)
    {
        player.VSpeed = 0f;
        player.CanJump = false;
    }

    public override void UpdateState(PlayerController player)
    {
        float move = InputManager.instance.Move.x;

        if (move > 0f)
        {
            if (player.HSpeed < 0f)
            {
                player.HSpeed += decel * Time.deltaTime;
            }
            else if (player.HSpeed < player.maxSpeed)
            {
                player.HSpeed += accel * Time.deltaTime;
            }
            else
            {
                player.HSpeed = player.maxSpeed;
            }
        }
        else if (move < 0f)
        {
            if (player.HSpeed > 0f)
            {
                player.HSpeed -= decel * Time.deltaTime;
            }
            else if (player.HSpeed > -player.maxSpeed)
            {
                player.HSpeed -= accel * Time.deltaTime;
            }
            else
            {
                player.HSpeed = -player.maxSpeed;
            }
        }
        else
        {
            player.HSpeed -= Mathf.Min(fric * Time.deltaTime, Mathf.Abs(player.HSpeed)) * Mathf.Sign(player.HSpeed);
        }

        if (InputManager.instance.Jump && player.CanJump)
        {
            player.VSpeed = player.jumpSpeed;
        }

        if (!InputManager.instance.Jump && !player.CanJump)
        {
            player.CanJump = true;
        }

        player.Move();
    }

    public override void ChangeState(PlayerController player)
    {
        if (!Physics2D.OverlapBox(player.Body.position + Vector2.up * 0.5f, Vector2.one, 0f, LayerMask.GetMask("Solid")))
        {
            currentCoyoteTime -= Time.deltaTime;
        }
        else
        {
            currentCoyoteTime = coyoteTime;
        }

        if (player.VSpeed > 0f || currentCoyoteTime <= 0f)
        {
            player.SetState(player.airState);
        }
    }

    public override void ExitState(PlayerController player)
    {
        
    }
}
