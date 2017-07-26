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

  [HideInInspector]
  private PlayableCharacter m_owner;

  public void Learn(Skill skill)
  {
    if(skill.Unlock())
    {
      Debug.Log(string.Format("Skill {0} learnt! Current level: {1}", skill.skillName, skill.skillLevel));
    }
    else
    {
      Debug.Log(string.Format("Cannot learn or upgrade skill: {0}, not enough XP", skill.skillName));
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
}
