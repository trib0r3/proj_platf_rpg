using UnityEngine;

public class AI_Walker : MonoBehaviour, ICharacterController
{
  public string controllerType
  {
    get
    {
      return "AI_WALKER";
    }
  }

  public bool isAttackClicked
  {
    get
    {
      return false;
    }
  }

  public bool isJumpClicked
  {
    get
    {
      return false;
    }
  }

  public bool isRunningKeyClicked
  {
    get
    {
      return false;
    }
  }

  public float moveDirection
  {
    get
    {
      return m_direction;
    }
  }


  [SerializeField]
  protected float m_timeToChangeDirection = 2.0f;
  protected float m_direction = -1.0f;


  protected void Start()
  {
    InvokeRepeating("change_direction", m_timeToChangeDirection, m_timeToChangeDirection);
  }

  public void Control()
  {
    // nothing here, intentionally
  }

  protected void change_direction()
  {
    m_direction *= -1;
  }
}
