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
      // default behaviour:
      // enable attack only if player is not invulnerable
      return !m_invulnerable;
    }
  }

  public bool hitable = true;
  public bool ownerNotificationsEnabled = true;
  public float timeInvulnOnHit = 2.0f;

  [SerializeField]
  private float m_hp = 100.0f;

  [SerializeField]
  private float m_dmg = 1.0f;

  private bool m_invulnerable = false;


  private void Awake()
  {
    if(owner == null && ownerNotificationsEnabled)
    {
      Debug.LogWarning("Owner is not set!", this);
    }
  }

  protected void GetHit(CharacterStats attackerStats)
  {
    if (!hitable) // if cannot hurt object...
      return;     // skip hitting it!

    if (m_invulnerable) // if got hit...
      return;           // stay invulnerable on hit for seconds!

    if (attackerStats.dmg == 0) // if base dmg is 0...
      return;                   // attacking is disabled! Skip it!

    m_hp = Mathf.Max(m_hp - attackerStats.dmg, 0.0f);

    StartCoroutine(StayInvulnOnHit()); // temporary disable hitting player

    if (ownerNotificationsEnabled && owner != null)
      owner.NotifyDamageTaken();
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    CharacterStats enemy = collision.gameObject.GetComponent<CharacterStats>();
    ReceiveDamage(enemy);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    CharacterStats enemy = other.gameObject.GetComponent<CharacterStats>();
    ReceiveDamage(enemy);
  }

  private IEnumerator StayInvulnOnHit()
  {
    m_invulnerable = true;
    yield return new WaitForSeconds(timeInvulnOnHit);
    m_invulnerable = false;
  }

  private void ReceiveDamage(CharacterStats enemy)
  {
    if (enemy == null)
      return;

    if (enemy.canAtttack) // receive damage only if enemy can attack
      GetHit(enemy);  // example skill is ready, character is not invuln.
  }
}
