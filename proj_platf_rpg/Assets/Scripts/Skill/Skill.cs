using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
  public struct SkillRequirement
  {
    public Skill skill;
    public int requiredLevel;
  }

  public bool isUnlocked
  {
    get { return m_isUnlocked; }
  }

  public int skillLevel
  {
    get { return m_skillLevel; }
  }

  public string skillName = "Skill";

  protected int m_skillLevel = 0;
  protected bool m_isReadyToUse = true;
  protected PlayableCharacter m_owner;
  protected SkillTree m_skillTree;

  // end extra values can be here, like "mana cost"


  [SerializeField]
  protected float m_cooldown = 0;

  [SerializeField]
  protected float m_skillStatsMultiplier = 1.5f;

  [Header("Skill unlocking")]
  [SerializeField]
  protected bool m_isUnlocked = false;

  [SerializeField]
  protected SkillRequirement[] m_unlockRequirements = null;

  [SerializeField]
  protected int m_nextLevelCost = 1;
  protected int m_costUnlockMultiplier = 1;

  public virtual void Use(GameObject target)
  {
    if(!isUnlocked)
    {
      Debug.Log(string.Format("Cannot use {0} because skill is not unlocked", skillName), this);
      return;
    }

    // if skill still needs time to cooldown...
    if (!m_isReadyToUse)
      return;

    // begin waiting time
    Cooldown();

    // NOTICE rest is done by implementation in non-virtual class
  }

  public bool Unlock()
  {
    // check if we can unlock it
    if (m_skillTree.availableSkillpoints < m_nextLevelCost)
      return false;

    // check we have to unlock it, if not check we are passing requirements
    if (!m_isUnlocked && !CheckRequirements())
      return false;

    // unlock
    m_skillTree.availableSkillpoints -= m_nextLevelCost;
    m_nextLevelCost = CalculateNextLevelCost();
    m_skillLevel++;
    m_isUnlocked = true;

    return true;
  }

  public void SetOwner(PlayableCharacter character, SkillTree tree)
  {
    m_skillTree = tree;
    m_owner = character;
  }

  private int CalculateNextLevelCost()
  {
    return m_nextLevelCost * m_costUnlockMultiplier;
  }

  private bool CheckRequirements()
  {
    if(m_unlockRequirements != null)
    {
      foreach (SkillRequirement skillReq in m_unlockRequirements)
      {
        if (skillReq.skill.skillLevel < skillReq.requiredLevel)
          return false;
      }
    }

    return true;
  }

  protected virtual void Start()
  {
    if(isUnlocked)
    {
      m_unlockRequirements = null;
      m_skillLevel = Mathf.Max(m_skillLevel, 1);
    }
  }

  protected IEnumerator Cooldown()
  {
    m_isReadyToUse = false;

    yield return new WaitForSeconds(m_cooldown);

    m_isReadyToUse = true;
  }
}
