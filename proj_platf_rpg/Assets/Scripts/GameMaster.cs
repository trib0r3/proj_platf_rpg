using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
  public static GameMaster gm = null;
  public SpecialEffects specialEffects;
  public Text playerHp;

  void Awake()
  {
    if (gm == null)
      gm = this;
  }

  private void Start()
  {
    if (specialEffects == null)
      Debug.LogWarning("Special Effects not found", this);
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
