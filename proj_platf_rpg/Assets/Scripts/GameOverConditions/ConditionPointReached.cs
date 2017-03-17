using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ConditionPointReached : GameOverCondition
{
  private BoxCollider2D m_box2d;
  private bool m_playerReached = false;
  private string m_info = "";

  public override string GetProgressInfo()
  {
    return m_info;
  }


  protected override bool isFailure()
  {
    /* NOTICE */
    /* There is no possibility for failure, so always return false */
    return false;
  }

  protected override bool isSuccess()
  {
    return m_playerReached;
  }

  protected void processSuccess()
  {
    m_info = "Point Reached!";
    GameMaster.gm.NotifySuccess(this);
  }


  private void Awake()
  {
    m_box2d = GetComponent<BoxCollider2D>();
  }

  private void Start()
  {
    if (!m_box2d.isTrigger)
    {
      Debug.LogWarning("Condition is set, but there is no trigger option set. Fixing...", this);
      m_box2d.isTrigger = true;
    }

    AddActionOnSuccess(processSuccess);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!m_playerReached && collision.tag == "Player")
    {
      m_playerReached = true;
      CheckConditions(); // force to update conditions
    }
  }
}
