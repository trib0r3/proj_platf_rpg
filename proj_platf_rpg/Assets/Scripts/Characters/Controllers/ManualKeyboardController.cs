using UnityEngine;

public class ManualKeyboardController : MonoBehaviour, ICharacterController
{
  const float MOV_LEFT = -1.0f;
  const float MOV_RIGHT = 1.0f;
  const float MOV_STOP = 0.0f;

  public KeyCode keyMoveLeft = KeyCode.LeftArrow;
  public KeyCode keyMoveRight = KeyCode.RightArrow;
  public KeyCode keyJump = KeyCode.Space;
  public KeyCode keyRun = KeyCode.LeftShift;

  protected float m_movdir = 0.0f;
  protected bool m_jump = false;
  protected bool m_isRunning = false;


  public string controllerType
  {
    get
    {
      return "MANUAL_KEYBOARD";
    }
  }

  public float moveDirection
  {
    get
    {
      return m_movdir;
    }
  }

  public bool isJumpClicked
  {
    get
    {
      if(m_jump)
      {
        m_jump = false;
        return true;
      }

      return false;
    }
  }

  public bool isRunningKeyClicked
  {
    get
    {
      return m_isRunning;
    }
  }

  public void Control()
  {
    // HACK Temporary change, fix later
    m_movdir = Input.GetAxis("Horizontal");

    /* Stop Character */
 //   if (Input.GetKeyUp(keyMoveLeft) || Input.GetKeyUp(keyMoveRight))
 //   {
 //     m_movdir = MOV_STOP;
 //   }

    /* Moving */ // FIXME
 //   if (Input.GetKeyDown(keyMoveLeft))
 //   {
 //     m_movdir = MOV_LEFT;
 //   }

 //   if (Input.GetKeyDown(keyMoveRight))
 //   {
 //     m_movdir = MOV_RIGHT;
 //   }

    //m_movdir = Input.GetAxis("Horizontal");    
    /* Jumping */
    if(Input.GetKeyDown(keyJump))
    {
      m_jump = true;
    }

    /* Running */
    if(Input.GetKeyDown(keyRun))
    {
      m_isRunning = true;
    }
    else if(Input.GetKeyUp(keyRun))
    {
      m_isRunning = false;
    }
  }
}
