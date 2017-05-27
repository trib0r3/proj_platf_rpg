using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
  public static GameMaster gm = null;
  public SpecialEffects specialEffects;

  [Header("Overlay")]
  public Text playerHp;
  public string levelDetails = "";

  [Header("Ingame Menu")]
  [SerializeField]
  GameObject m_gameMenuParent;
  [SerializeField]
  Text m_title;
  [SerializeField]
  Text m_gameStatus;
  [SerializeField]
  Text m_missionDetails;
  [SerializeField]
  Button m_continue;

  private float m_storedTime = 1.0f;
  private string m_currentScene;
  private bool m_isGameOver = false;

  void Awake()
  {
    if (gm == null)
      gm = this;
  }

  private void Start()
  {
    if (specialEffects == null)
      Debug.LogWarning("Special Effects not found", this);

    m_currentScene = SceneManager.GetActiveScene().name;

    m_title.text = m_currentScene;
    m_gameStatus.text = "";
    m_missionDetails.text = levelDetails;

#if !UNITY_EDITOR
    ShowMenu();
#endif
  }

  public void NotifySuccess(GameOverCondition cond)
  {
    NotifyGameOver(true, cond.GetProgressInfo());
  }

  public void NotifyFailure(GameOverCondition cond)
  {
    NotifyGameOver(false, cond.GetProgressInfo());
  }

  public void NotifyGameOver(bool success, string message)
  {
    if (!success)
      m_continue.interactable = false;

    string title = (success ? "Congratz!" : "EPIC FAIL!");
    m_isGameOver = true;

    m_gameStatus.text = title;
    m_missionDetails.text = message;

    Invoke("ShowMenu", 0.5f);
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      m_gameStatus.text = "Pause";
      ShowMenu();
    }
  }

  
  /* UI */
  public void ShowMenu()
  {
    m_storedTime = Time.timeScale;
    Time.timeScale = 0;
    if(m_isGameOver && SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCount)
    {
      m_continue.interactable = false;
    }

    m_gameMenuParent.SetActive(true);
  }

  public void HideMenu()
  {
    Time.timeScale = m_storedTime;
    m_gameMenuParent.SetActive(false);
  }

  public void OnClickedContinue()
  {
    if(!m_isGameOver)
    {
      HideMenu();
    }
    else
    {
      int current = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(current + 1);
    }
  }

  public void OnClickedRestart()
  {
    Time.timeScale = m_storedTime;
    SceneManager.LoadScene(m_currentScene);
  }

  public void OnClickedExit()
  {
    Application.Quit();
  }
}
