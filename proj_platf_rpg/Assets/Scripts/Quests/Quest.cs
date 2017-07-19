using System;
using UnityEngine;

[Serializable]
public class Quest
{
  public string questTitle = "Quest Title";
  public string questDescription = "i.e kill 10 monster to save the princess!";

  [Header("Objectives")]
  public int objectiveAmount = 1;

  // HACK Type of object would be a better idea, 
  // but UnityEditor cannot handle with Type as configurable field from Editor :(
  public string objectiveTag;

  [Header("Rewards")]
  [SerializeField]
  protected int m_gold;
  [SerializeField]
  protected float m_exp;
  [SerializeField]
  protected Item[] m_items;

  protected bool m_isFinished = false;
  protected int m_collectedObjectives = 0;

  public virtual bool IsFinished(string updater, int amount)
  {
    // check if it is interesting type of item
    if (updater == objectiveTag)
    {
      // update quest details
      m_collectedObjectives += Mathf.Abs(amount);

      if (m_collectedObjectives >= objectiveAmount)
      {
        m_isFinished = true;
        return true;
      }
    }

    return false;
  }

  public void GetReward(out int gold, out float exp, out Item[] items)
  {
    // get reward for quest only if we already finished the quest
    if(m_isFinished)
    {
      gold = m_gold;
      exp = m_exp;
      items = m_items;
    }
    else
    {
      gold = 0;
      exp = 0;
      items = null;
    }
  }
}
