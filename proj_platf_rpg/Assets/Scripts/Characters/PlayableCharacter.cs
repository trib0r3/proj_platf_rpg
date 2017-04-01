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

  public CharacterStats stats;
  public int jumpLimit = 1;

  [SerializeField]
  protected float m_vspeed = 5;

  [SerializeField]
  protected float m_hspeed = 400;

  [SerializeField]
  protected CharacterType m_characterType = CharacterType.NONE;

  [SerializeField]
  protected ControllerType m_controllerType = ControllerType.KEYBOARD;

  [SerializeField]
  protected CollisionReceiver m_groundCheckReceiver;

  [SerializeField]
  protected SpriteRenderer m_renderer;

  protected ICharacterController m_controller;
  protected Rigidbody2D m_rigidbody;
  protected int m_availableJumps;


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

  public virtual void NotifyDamageTaken()
  {
    float hp = stats.hp;
    Debug.Log("Got hit, current hp: " + hp.ToString(), this);
    GameMaster.gm.specialEffects.ChangeTempColor(m_renderer, Color.red, stats.timeInvulnOnHit);

    if (hp == 0)
    {
      Debug.Log("Play death anim!");
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

    m_controller = ProbeCharacterController();
    if (m_controller == null)
    {
      Debug.LogWarning("Character controller not set!", this);
    }

    // just checks...
    if(m_groundCheckReceiver == null)
    {
      Debug.LogWarning("Collision Receiver for ground detector is not set!", this);
    }

    if (m_renderer == null)
    {
      Debug.LogWarning("Cannot find sprite renderer", this);
    }

  }

  protected virtual void Start()
  {
    m_availableJumps = jumpLimit;
    m_groundCheckReceiver.onTriggerEnterCallbacks += OnGroundEnter;
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
    if (m_controller.isJumpClicked && m_availableJumps > 0)
    {
      m_availableJumps--;
      m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0.0f); // reset falling speed
      m_rigidbody.AddForce(new Vector2(0, m_hspeed * mass));
    }
  }

  protected ICharacterController ProbeCharacterController()
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

  protected void OnGroundEnter(Collider2D other)
  {
    m_availableJumps = jumpLimit;
  }
}
