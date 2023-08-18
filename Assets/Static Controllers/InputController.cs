using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    public PlayerInputActions PlayerInput { get; private set; }

    // TODO: FIgure out why PlayerInput is null when scene is changed back to main menu

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        PlayerInput = new PlayerInputActions();
    }


    public event Action<bool> OnToggleMenuActions;
    private void Start()
    {
        PlayerInput.Enable();
        PlayerInput.MenuActions.Exit.performed += ctx => ToggleMenu();
        PlayerInput.GameActions.Pause.performed += ctx => ToggleMenu();
    }

    private void OnDisable()
    {
        PlayerInput.Disable();
        PlayerInput.MenuActions.Exit.performed -= ctx => ToggleMenu();
        PlayerInput.GameActions.Pause.performed -= ctx => ToggleMenu();
    }

    void ToggleMenu()
    {
        if (!GameController.instance.State.Equals(GameController.GAMESTATE.PAUSED))
        {
            GameController.instance.SetState(GameController.GAMESTATE.PAUSED);
            PlayerInput.MenuActions.Enable();
            PlayerInput.GameActions.Disable();
            OnToggleMenuActions?.Invoke(true);
            return;
        }

        GameController.instance.SetState(GameController.instance.PreviousState);
        PlayerInput.GameActions.Enable();
        PlayerInput.MenuActions.Disable();
        OnToggleMenuActions?.Invoke(false);
    }
}
