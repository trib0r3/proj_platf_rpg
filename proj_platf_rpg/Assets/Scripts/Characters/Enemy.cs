public class Enemy : PlayableCharacter
{
  public static int spawnedCount = 0;

  protected override void Start()
  {
    base.Start();
    spawnedCount++;
  }

  protected override void OnDeath()
  {
    base.OnDeath();
    spawnedCount--;
    GameMaster.gm.NotifyEnemyDeath();
  }

  private void OnDestroy()
  {
    OnDeath();
  }
}
