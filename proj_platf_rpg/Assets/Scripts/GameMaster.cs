using UnityEngine;

public class GameMaster : MonoBehaviour
{
  public static GameMaster gm = null;

  void Awake()
  {
    if (gm == null)
      gm = this;
  }

  public void NotifySuccess(GameOverCondition cond)
  {
    Debug.Log(cond.GetProgressInfo());
  }

  public void NotifyFailure(GameOverCondition cond)
  {
    Debug.Log(cond.GetProgressInfo());
  }

  private void Update()
  {
    /* FIXME for testing purpose only! */
    if (Input.GetKeyDown(KeyCode.Escape))
      Application.Quit();
  }
}
