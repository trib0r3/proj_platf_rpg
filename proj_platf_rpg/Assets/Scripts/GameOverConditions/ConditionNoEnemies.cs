using System;

public class ConditionNoEnemies : GameOverCondition
{
  private string m_text = "Enemies left: ";
  private string m_status;

  public override string GetProgressInfo()
  {
    if (Enemy.spawnedCount > 0)
    {
      m_status = m_text + Enemy.spawnedCount;
    }
    else
    {
      m_status = "All enemies are dead!";
    }
    return m_status;
  }

  protected override bool isFailure()
  {
    return false;
  }

  protected override bool isSuccess()
  {
    return (Enemy.spawnedCount <= 0);
  }
}
