using UnityEngine;

public abstract class GameOverCondition : MonoBehaviour
{
  public delegate void Action();
  public enum ConditionResult { NONE, SUCCESS, FAILURE };

  protected bool m_canBeSucceed = false;
  protected bool m_canBeFailed = false;

  protected Action m_actionOnSuccess;
  protected Action m_actionOnFailure;
  protected Action m_actionsOnUpdateCondition;


  public abstract string GetProgressInfo();

  public void AddActionOnSuccess(Action action)
  {
    m_canBeSucceed = true;
    m_actionOnSuccess += action;
  }

  public void AddActionOnFailure(Action action)
  {
    m_canBeFailed = true;
    m_actionOnFailure += action;
  }

  public void AddActionOnUpdate(Action action)
  {
    m_actionsOnUpdateCondition += action;
  }

  public void CheckConditions()
  {
    ConditionResult result = verifyResult();
    switch (result)
    {
      case ConditionResult.SUCCESS:
        m_actionOnSuccess();
        break;

      case ConditionResult.FAILURE:
        m_actionOnFailure();
        break;

      case ConditionResult.NONE:
        // do nothing
        break;
    }
  }


  protected abstract bool isSuccess();
  protected abstract bool isFailure();

  protected virtual ConditionResult verifyResult()
  {
    /* 
     * Success strategy:
     * checks conditions for success, then failure
     */
    if (m_canBeSucceed && isSuccess())
      return ConditionResult.SUCCESS;

    if (m_canBeFailed && isFailure())
      return ConditionResult.FAILURE;

    return ConditionResult.NONE;
  }

  protected virtual void onUpdateCondition()
  {
    if (m_actionsOnUpdateCondition != null)
      m_actionsOnUpdateCondition();
  }
}
