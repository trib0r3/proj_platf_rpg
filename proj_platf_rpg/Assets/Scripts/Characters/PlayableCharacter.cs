using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
  public enum CharacterType
  {
    NONE, PLAYER, ENEMY, OTHER
  };

  public enum ControllerType
  {
    KEYBOARD, AI
  };

  [SerializeField]
  protected int m_hp = 100;

  [SerializeField]
  protected float m_dmg = 1;

  [SerializeField]
  protected float m_vspeed = 5;

  [SerializeField]
  protected float m_hspeed = 400;

  [SerializeField]
  protected CharacterType m_characterType = CharacterType.NONE;

  [SerializeField]
  protected ControllerType m_controllerType = ControllerType.KEYBOARD;

  protected ICharacterController m_controller;
  protected Rigidbody2D m_rigidbody;


  public int hp
  {
    get
    {
      return m_hp;
    }
    set
    {
      // TODO add special effects
      m_hp = value;
    }
  }

  public float dmg
  {
    get
    {
      return m_dmg;
    }
    set
    {
      // TODO add special effects
      m_dmg = value;
    }
  }

  public float verticalSpeed
  {
    get
    {
      return m_vspeed;
    }
    set
    {
      // TODO add special effects
      m_vspeed = value;
    }
  }

  public float jumpPower
  {
    get
    {
      return m_hspeed;
    }
    set
    {
      // TODO add special effects
      m_hspeed = value;
    }
  }

  public float mass
  {
    get
    {
      return m_rigidbody.mass;
    }
    set
    {
      // TODO add special effects
      m_rigidbody.mass = value;
    }
  }

  public Vector2 velocity
  {
    get
    {
      return m_rigidbody.velocity;
    }
  }

  public CharacterType characterType
  {
    get
    {
      return m_characterType;
    }
  }

  protected virtual void Awake()
  {
    m_rigidbody = GetComponent<Rigidbody2D>();
    if (m_rigidbody == null)
    {
      Debug.LogError("Cannot find rigidbody component!", this);
      return;
    }

    if (m_characterType == CharacterType.NONE)
    {
      Debug.LogWarning("Character Type is not set!", this);
    }

    m_controller = probe_character_controller();
    if (m_controller == null)
    {
      Debug.LogWarning("Character controller not set!", this);
    }
  }

  protected virtual void FixedUpdate()
  {
    m_controller.Control();

    Move();
    Jump();
  }

  protected virtual void Move()
  {
    Vector2 v = m_rigidbody.velocity;
    m_rigidbody.velocity = new Vector2(m_vspeed * m_controller.moveDirection, v.y);
  }

  protected virtual void Jump()
  {
    if(m_controller.isJumpClicked)
      m_rigidbody.AddForce(new Vector2(0, m_hspeed * mass));
  }

  protected ICharacterController probe_character_controller()
  {
    ICharacterController conn = null;
    switch (m_controllerType)
    {
      case ControllerType.KEYBOARD:
        conn = GetComponent<ManualKeyboardController>();
        break;

      default:
        Debug.LogError("Invalid controller type!", this);
        break;
    }

    if(conn == null)
    {
      Debug.LogError("Cannot find proper controller attached to object!", this);
    }

    return conn;
  }
}
