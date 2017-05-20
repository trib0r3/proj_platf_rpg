using UnityEngine;

public class AI_Berserker : MonoBehaviour, ICharacterController
{
  public string controllerType
  {
    get
    {
      return "AI_BERSERKER";
    }
  }

  public bool isAttackClicked
  {
    get
    {
      return m_playerInView;
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
      return m_playerInView;
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
  protected float m_timeToChangeDirection = 1.0f;
  protected float m_direction = -1.0f;

  protected float m_lastUpdate;
  protected bool m_playerInView = false;

  private void Start()
  {
    m_lastUpdate = Time.time;
  }

  public void Control()
  {
    if (!m_playerInView) // is not charging
    {
      if (Time.time - m_lastUpdate > m_timeToChangeDirection) // time to change direction
      {
        m_direction *= -1.0f;
        m_lastUpdate = Time.time;
      }
    }
    else // player, so charge!
    {
      m_lastUpdate = Time.time;
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if(other.tag == "Player")
    {
      m_playerInView = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.tag == "Player")
    {
      m_playerInView = false;
    }
  }
}
