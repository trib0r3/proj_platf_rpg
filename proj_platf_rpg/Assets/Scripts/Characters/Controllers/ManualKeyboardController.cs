using UnityEngine;

public class ManualKeyboardController : MonoBehaviour, ICharacterController
{
  const float MOV_LEFT = -1.0f;
  const float MOV_RIGHT = 1.0f;
  const float MOV_STOP = 0.0f;

  public KeyCode keyMoveLeft = KeyCode.LeftArrow;
  public KeyCode keyMoveRight = KeyCode.RightArrow;
  public KeyCode keyJump = KeyCode.Space;

  float m_movdir = 0.0f;
  bool m_jump = false;


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

  public void Control()
  {
    if(Input.GetKeyDown(keyMoveLeft))
    {
      m_movdir = MOV_LEFT;
    }
    else if(Input.GetKeyDown(keyMoveRight))
    {
      m_movdir = MOV_RIGHT;
    }

    if ((m_movdir == MOV_LEFT && Input.GetKeyUp(keyMoveLeft)) || 
       (m_movdir == MOV_RIGHT && Input.GetKeyUp(keyMoveRight)))
    {
      m_movdir = MOV_STOP;
    }

    if(Input.GetKeyDown(keyJump))
    {
      m_jump = true;
    }
  }
}
