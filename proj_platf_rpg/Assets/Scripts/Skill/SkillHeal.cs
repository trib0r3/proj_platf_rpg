using UnityEngine;

public class SkillHeal : Skill
{
  public float restoreHP = 5;

  public override void Use(GameObject target)
  {
    base.Use(target);

    // self-healing skill, logarithmic increase
    m_owner.stats.hp += restoreHP + restoreHP * Mathf.Log(m_skillLevel, 2);
  }
}
