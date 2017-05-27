using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
  public PlayableCharacter owner;

  public float hp
  {
    get
    {
      return m_hp;
    }
  }

  public float dmg
  {
    get
    {
      return m_dmg;
    }
  }

  public bool canAtttack
  {
    get
    {
      return CanAttack();
    }
  }

  public bool isDead
  {
    get { return m_isDead; }
  }

  public bool hitable = true;
  public bool ownerNotificationsEnabled = true;
  public float timeInvulnOnHit = 2.0f;

  [SerializeField]
  protected float m_hp = 100.0f;

  [SerializeField]
  protected float m_dmg = 1.0f;

  protected bool m_invulnerable = false;
  protected bool m_isDead = false;


  private void Awake()
  {
    if(owner == null && ownerNotificationsEnabled)
    {
      Debug.LogWarning("Owner is not set!", this);
    }
  }

  protected void GetHit(CharacterStats attackerStats)
  {
    if (!hitable ||              // if cannot hurt object...
        m_invulnerable ||        // if got hit...
        isDead ||                // if is dead...
        attackerStats.dmg == 0   // if base dmg is 0...
      )
      return;                    // ...then attacking is disabled, skip it!

    m_hp = Mathf.Max(m_hp - attackerStats.dmg, 0.0f);

    if (m_hp > 0)
    {
      StartCoroutine(StayInvulnOnHit()); // temporary disable hitting player
    }
    else
    {
      m_isDead = true;
    }

    if (ownerNotificationsEnabled && owner != null)
      owner.NotifyDamageTaken();
  }

  protected virtual void OnCollisionEnter2D(Collision2D collision)
  {
    CharacterStats enemy = collision.gameObject.GetComponent<CharacterStats>();
    ReceiveDamage(enemy);
  }

  protected virtual void OnTriggerEnter2D(Collider2D other)
  {
    CharacterStats enemy = other.gameObject.GetComponent<CharacterStats>();
    ReceiveDamage(enemy);
  }

  protected IEnumerator StayInvulnOnHit()
  {
    m_invulnerable = true;
    yield return new WaitForSeconds(timeInvulnOnHit);
    m_invulnerable = false;
  }

  protected void ReceiveDamage(CharacterStats enemy)
  {
    if (enemy == null)
      return;

    if (enemy.canAtttack) // receive damage only if enemy can attack
      GetHit(enemy);  // example skill is ready, character is not invuln.
  }

  virtual protected bool CanAttack()
  {
    // default behaviour:
    // enable attack only if player is not invulnerable
    return !m_invulnerable && !isDead;
  }
}
