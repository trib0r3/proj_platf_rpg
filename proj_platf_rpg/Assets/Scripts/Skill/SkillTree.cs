using UnityEngine;
using System.Collections.Generic;

public class SkillTree : MonoBehaviour
{
  public int availableSkillpoints = 0;

  // needed only in object creation as initial list
  public Skill[] availableSkills;

  [HideInInspector]
  public List<Skill> learntSkills;
  [HideInInspector]
  public List<Skill> unknownSkills;

  [SerializeField]
  private Transform m_containerLearnt;
  [SerializeField]
  private Transform m_containerUnknown;

  private PlayableCharacter m_owner;
  private Skill m_activeSkill;

  public bool Learn(Skill skill)
  {
    return skill.Unlock();
  }

  public void UseSkill()
  {
    // Use active skill, in our implementation we are allowing for only 1 active skill
    if(m_activeSkill != null)
    {
      m_activeSkill.Use(null);
    }
  }

  public void SetActiveSkillOnSlot(Skill skill)
  {
    // here we can add param "slot_number" to this method
    // that defines assigning slot id
    // we have only one active skill, so we dont need this

    if(skill.isUnlocked && learntSkills.Contains(skill))
    {
      // simple lock-check that evades activating skill from outside the skill tree
      m_activeSkill = skill;
    }
  }

  private void Awake()
  {
    learntSkills = new List<Skill>();
    unknownSkills = new List<Skill>();

    m_owner = GetComponent<PlayableCharacter>();
  }

  private void Start()
  {
    if (availableSkills != null)
    {
      foreach (Skill skill in availableSkills)
      {
        skill.SetOwner(m_owner, this);

        if (skill.isUnlocked)
        {
          learntSkills.Add(skill);
          skill.transform.SetParent(m_containerLearnt);
        }
        else
        {
          unknownSkills.Add(skill);
          skill.transform.SetParent(m_containerUnknown);
        }

        skill.transform.localScale = Vector3.one;
      }

      availableSkills = null;
    }
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.H) && m_activeSkill != null)
    {
      m_activeSkill.Use(null);
    }
  }
}
