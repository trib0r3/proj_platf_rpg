public class ConditionPlayerDead : GameOverCondition
{
  public CharacterStats targetStats;
  string message = "Player is alive";

  public override string GetProgressInfo()
  {
    return message;
  }

  protected override bool isFailure()
  {
    return false;
  }

  protected override bool isSuccess()
  {
    if (targetStats.hp == 0.0f)
    {
      message = "Player is dead :(";
    }
    return false;
  }

  private void Start()
  {
    AddActionOnSuccess(() => { GameMaster.gm.NotifyFailure(this); });
  }
}
