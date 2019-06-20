using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// class for reading all inputs
public class ThirdPersonInput : MonoBehaviour
{
    #region public variables 
    public string horizontalInput = "Horizontal";
    public string verticallInput = "Vertical";

    public KeyCode m_jumpInput = KeyCode.Space;
   // public KeyCode m_climbInput = KeyCode.Space;
    public KeyCode m_interactionInputKey = KeyCode.Space;
    
    public delegate  void KeyPressDelegate();
    public event KeyPressDelegate OnJumpKeyEvent;
    public event KeyPressDelegate OnInteractionKeyDownEvent;
    public event KeyPressDelegate OnInteractionKeyUpEvent;


    public KeyCode m_jumpInputWindows = KeyCode.Space;
    // public KeyCode m_climbInput = KeyCode.Space;
    public KeyCode m_interactionInputKeyWindows = KeyCode.Space;


    // public event KeyPressDelegate OnClimbKeyDownEvent;

    public Vector2 InputMoveAxis
    {
        get{ return inputMoveAxis; }
    }

    #endregion
    private Vector2 inputMoveAxis;

    #region unity callbacks
    // Use this for initialization
    void Start ()
    {

	}

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }
    #endregion

    #region protected methods
    protected virtual void HandleInputs()
    {
        ReadMoveAxis();
        ReadJumpInput();
        ReadInteractionInput();
        //ReadClimbInput();
    }

    protected virtual void ReadMoveAxis()
    {
        inputMoveAxis.x = Input.GetAxis(horizontalInput);
        inputMoveAxis.y = Input.GetAxis(verticallInput);
    }

    protected virtual void ReadJumpInput()
    {
        if (Input.GetKeyDown(m_jumpInput) || Input.GetKeyDown(m_jumpInputWindows))
        {
            if(OnJumpKeyEvent != null) OnJumpKeyEvent();
        }
    }

    //protected virtual void ReadClimbInput()
    //{
    //    if (Input.GetKeyDown(m_climbInput))
    //    {
    //        if(OnClimbKeyDownEvent != null) OnClimbKeyDownEvent();
    //    }
    //}

    protected virtual void ReadInteractionInput()
    {
        if (Input.GetKeyDown(m_interactionInputKey) || Input.GetKeyDown(m_interactionInputKeyWindows))
        {
            if (OnInteractionKeyDownEvent != null) OnInteractionKeyDownEvent();
        }

        if (Input.GetKeyUp(m_interactionInputKey) || Input.GetKeyUp(m_interactionInputKeyWindows))
        {
            if (OnInteractionKeyUpEvent != null) OnInteractionKeyUpEvent();
        }
    }

    #endregion
}


/*  ps4 butttons
 * Buttons
    Square  = joystick button 0
    X       = joystick button 1
    Circle  = joystick button 2
    Triangle= joystick button 3
    L1      = joystick button 4
    R1      = joystick button 5
    L2      = joystick button 6
    R2      = joystick button 7
    Share	= joystick button 8
    Options = joystick button 9
    L3      = joystick button 10
    R3      = joystick button 11
    PS      = joystick button 12
    PadPress= joystick button 13

Axes:
    LeftStickX      = X-Axis
    LeftStickY      = Y-Axis (Inverted?)
    RightStickX     = 3rd Axis
    RightStickY     = 4th Axis (Inverted?)
    L2              = 5th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
    R2              = 6th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
    DPadX           = 7th Axis
    DPadY           = 8th Axis (Inverted?)
*/