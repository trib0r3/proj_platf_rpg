using UnityEngine;
using System.Collections;

public class Weapon : CharacterStats
{
  public bool isInfinite
  {
    get { return m_isInfinite; }
  }

  public int ammunition
  {
    get { return m_ammunition; }
  }

  /* Default setup is set to short distance weapon (i.e. sword) */
  [Header("Weapon")]
  [SerializeField]
  protected bool m_isInfinite = true;

  [SerializeField]
  protected int m_ammunition = 1;

  [SerializeField]
  protected float m_cooldown = 1.0f;

  [SerializeField]
  protected Animator m_animator;

  protected bool m_isCooldown = false;


  protected void Awake()
  {
    m_animator = GetComponent<Animator>();
    if(m_animator == null)
    {
      Debug.LogWarning("Cannot find animator for weapon", this);
    }
  }

  virtual public bool UseWeapon()
  {
    if (!canAtttack)
      return false;

    m_animator.SetBool("isAttacking", true);
    return true;
  }

  protected override bool CanAttack()
  {
    if (m_isCooldown) // cooldown in progress
      return false;

    if (!isInfinite && m_ammunition <= 0) // missing ammo
      return false;

    return true;
  }

  protected IEnumerator BeginCooldown()
  {
    m_isCooldown = true;
    yield return new WaitForSeconds(m_cooldown);
    m_isCooldown = false;
    m_animator.SetBool("isAttacking", false);
  }

  override protected void OnCollisionEnter2D(Collision2D collision)
  {
    base.OnCollisionEnter2D(collision);
    use_weapon();
  }

  protected override void OnTriggerEnter2D(Collider2D other)
  {
    base.OnTriggerEnter2D(other);
    use_weapon();
  }

  protected void use_weapon()
  {
    if (!isInfinite)
    {
      m_ammunition--;
    }

    StartCoroutine(BeginCooldown());
  }
}
