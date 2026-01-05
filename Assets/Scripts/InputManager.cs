using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public Vector2 Move { get { return move; } }
    public bool Jump { get { return jump; } }


    public static InputManager instance;

    PlayerInput input;

    Vector2 move;
    bool jump;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        input = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        input.onActionTriggered += OnAction;
    }

    void OnDisable()
    {
        input.onActionTriggered -= OnAction;
    }

    void OnAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                move = context.ReadValue<Vector2>();
                break;
            case "Jump":
                SetBool(context, ref jump);
                break;
        }
    }

    void SetBool(InputAction.CallbackContext context, ref bool input)
    {
        if (context.performed)
        {
            input = true;
        }

        if (context.canceled)
        {
            input = false;
        }
    }
}
