using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
  #region Enums
  public enum CharacterType
  {
    NONE, PLAYER, ENEMY, OTHER
  };

  public enum ControllerType
  {
    KEYBOARD, AI_WALKER, AI_BERSERKER
  };
  #endregion

  #region Variables
  public Equipment equipment = null;

  [Header("Stats")]
  public CharacterStats stats;
  public int jumpLimit = 1;

  [SerializeField]
  protected float m_vspeed = 5;

  [SerializeField]
  protected float m_hspeed = 400;

  [Header("Controller")]
  [SerializeField]
  protected CharacterType m_characterType = CharacterType.NONE;

  [SerializeField]
  protected ControllerType m_controllerType = ControllerType.KEYBOARD;

  [Header("Misc")]
  [SerializeField]
  protected Animator m_animator;

  [SerializeField]
  protected CollisionReceiver m_groundCheckReceiver;

  [SerializeField]
  protected SpriteRenderer m_renderer;

  [SerializeField]
  protected Weapon m_equippedWeapon;

  protected ICharacterController m_controller;
  protected Rigidbody2D m_rigidbody;
  protected int m_availableJumps;
  #endregion

  #region Setters, getters
  public float verticalSpeed
  {
    get
    {
      return m_vspeed;
    }
    set
    {
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
  #endregion


  public virtual void NotifyDamageTaken()
  {
    float hp = stats.hp;

    if (hp == 0)
    {
      OnDeath();
    }
    else
    {
      GameMaster.gm.specialEffects.ChangeTempColor(m_renderer, Color.red, stats.timeInvulnOnHit);
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

    if (m_animator == null)
    {
      Debug.LogWarning("Cannot find animator", this);
    }
  }

  protected virtual void Start()
  {
    m_availableJumps = jumpLimit;
    m_groundCheckReceiver.onTriggerEnterCallbacks += OnGroundEnter;
  }

  protected virtual void FixedUpdate()
  {
    if (stats.isDead)
      return;

    m_controller.Control();

    Move();
    Flip();

    Jump();
    Attack();
  }

  protected virtual void OnTriggerEnter2D(Collider2D collision)
  {
    ItemObject itemObject = collision.gameObject.GetComponent<ItemObject>();
    if(itemObject != null)
    {
      // we are here only during collison with item
      // we are hitting it (ItemObject) -> item is collectable
      CollectItem(itemObject.item);
    }
  }

  protected virtual void Move()
  {
    Vector2 v = m_rigidbody.velocity;
    m_rigidbody.velocity = new Vector2(m_vspeed * m_controller.moveDirection, v.y);

    m_animator.SetFloat("speed", v.x != 0.0f ? 1.0f : -1.0f);
  }

  protected virtual void Flip()
  {
    Vector3 scale = gameObject.transform.localScale;
    if (m_rigidbody.velocity.x < 0.0f)
    {
      scale.x = 1.0f;
    }
    else
    {
      scale.x = -1.0f;
    }
    gameObject.transform.localScale = scale;
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

  protected virtual void Attack()
  {
    if (!m_controller.isAttackClicked)
      return;
    if (m_equippedWeapon == null)
      return;

    m_equippedWeapon.UseWeapon();
  }

  protected ICharacterController ProbeCharacterController()
  {
    ICharacterController conn = null;
    switch (m_controllerType)
    {
      case ControllerType.KEYBOARD:
        conn = GetComponent<ManualKeyboardController>();
        break;

      case ControllerType.AI_WALKER:
        conn = GetComponent<AI_Walker>();
        break;

      case ControllerType.AI_BERSERKER:
        conn = GetComponent<AI_Berserker>();
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

  protected virtual void OnDeath()
  {
    if(m_animator != null)
      m_animator.SetBool("isAlive", false);
  }

  protected void CollectItem(Item item)
  {
    GameMaster.gm.questBoard.NotifyNewEvent(item.tag, item.quantity);
    item.SetPhysicalOnScene(false, item.transform.position);
    equipment.AddItem(item);
  }
}
