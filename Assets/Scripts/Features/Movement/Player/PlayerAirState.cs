using UnityEngine;

[System.Serializable]
public class PlayerAirState : PlayerState
{
    public float gravity;
    [SerializeField] float fallSpeed;
    [SerializeField] float jumpTime;
    float currentJumpTime;

    public override void StartState(PlayerController player)
    {
        player.CanJump = false;
    }

    public override void UpdateState(PlayerController player)
    {
        float move = InputManager.instance.Move.x;

        if (move != 0f)
        {
            player.HSpeed = player.maxSpeed * Mathf.Sign(move);
        }
        else
        {
            player.HSpeed = 0f;
        }

        if (!InputManager.instance.Jump || Physics2D.OverlapBox(player.Body.position + Vector2.up, new Vector2(0.75f, 1f), 0f, LayerMask.GetMask("Solid")))
        {
            player.VSpeed = Mathf.Min(0f, player.VSpeed);
        }

        player.VSpeed = Mathf.Clamp(player.VSpeed + gravity * Time.deltaTime, fallSpeed, -fallSpeed);

        player.Move();
    }

    public override void ChangeState(PlayerController player)
    {
        if (InputManager.instance.Jump && player.CanJump)
        {
            currentJumpTime += Time.deltaTime;
        }
        else
        {
            currentJumpTime = 0f;
        }

        if (!InputManager.instance.Jump && !player.CanJump)
        {
            player.CanJump = true;
        }

        if (player.VSpeed <= 0f && Physics2D.OverlapBox(player.Body.position + Vector2.up * 0.5f, new Vector2(0.75f, 1f), 0f, LayerMask.GetMask("Solid")))
        {
            if (currentJumpTime <= jumpTime)
            {
                player.VSpeed = player.jumpSpeed;
            }
            else
            {
                player.SetState(player.groundState);
            }
        }
    }

    public override void ExitState(PlayerController player)
    {
        
    }
}
