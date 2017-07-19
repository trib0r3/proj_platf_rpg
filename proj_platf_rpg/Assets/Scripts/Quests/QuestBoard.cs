using UnityEngine;
using System.Collections;

public class QuestBoard : MonoBehaviour
{
  public Quest[] initialQuests;

  private Player m_player;
  private ArrayList m_activeQuests;
  private ArrayList m_questViews;

  [SerializeField]
  private Transform m_questBoardView;
  [SerializeField]
  private QuestView m_viewPrefab;

  public void NotifyNewEvent(string type, int amount=1)
  {
    for(int i=0; i<m_activeQuests.Count; ++i)
    {
      Quest quest = (Quest)m_activeQuests[i];
      if(quest.IsFinished(type, amount))
      {
        // acquire rewards
        int g;
        float exp;
        Item[] items;
        quest.GetReward(out g, out exp, out items);
        Debug.Log(string.Format("Finished Quest: {0}. Rewards: {1}g, {2}exp", quest.questTitle, g, exp));

        m_player.equipment.gold += g;
        // TODO add exp to player
        if (items != null)
        {
          foreach (Item item in items)
            m_player.equipment.AddItem(item);
        }

        m_activeQuests.Remove(quest);
      }

      // <- here we can update progress (if we want)
    }
  }

  public void AddQuest(Quest quest)
  {
    m_activeQuests.Add(quest);
    QuestView qv = Instantiate(m_viewPrefab, m_questBoardView);
    qv.transform.localScale = Vector3.one;

    qv.title.text = quest.questTitle;
    qv.description.text = quest.questDescription;

    m_questViews.Add(qv); // for optional updating progress
  }

  private void Start()
  {
    m_player = GameMaster.gm.player;

    m_activeQuests = new ArrayList();
    m_questViews = new ArrayList();

    foreach (Quest q in initialQuests)
    {
      AddQuest(q);
    }
  }
}
