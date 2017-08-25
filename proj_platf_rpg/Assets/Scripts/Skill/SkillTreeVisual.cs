using UnityEngine;
using UnityEngine.UI;

public class SkillTreeVisual : MonoBehaviour
{
  public const string TEXT_BUTTOIN_LEARN_UNKNOWN = "LEARN";
  public const string TEXT_BUTTOIN_LEARN_KNOWN = "UPGRADE";

  public SkillTree skillTree;

  public Transform skillsLearnt;
  public Transform skillsUnknown;

  public Button skillBtnActivate;
  public Button skillBtnUpgrade;

  public Text skillSelectedName;
  public Text skillSelectedProps;

  protected Skill m_selectedSkill = null;
  protected Color m_oldColor;

  public void OnSkillSelect(Skill skill)
  {
    // restore color
    if(m_selectedSkill != null)
    {
      // reset color change
      m_selectedSkill.GetComponent<Image>().color = m_oldColor;
    }

    // update selection
    m_selectedSkill = skill;

    // modify & store color
    Image img = m_selectedSkill.GetComponent<Image>();
    m_oldColor = img.color;
    img.color = new Color(0.5f, 1.0f, 0.25f);

    // update ui
    toggle_ui(true);
  }

  public void OnActivateClicked()
  {
    skillBtnActivate.interactable = false;
    skillTree.SetActiveSkillOnSlot(m_selectedSkill);
  }

  public void OnUpgradeClicked()
  {
    skillTree.Learn(m_selectedSkill);
    toggle_ui(true); // refresh ui
  }

  protected void toggle_ui(bool active)
  {
    skillBtnActivate.interactable = active;
    skillBtnUpgrade.interactable = active;

    if(active)
    {
      // update gui props
      skillBtnUpgrade.GetComponentInChildren<Text>().text =
        m_selectedSkill.isUnlocked ?
        TEXT_BUTTOIN_LEARN_KNOWN :
        TEXT_BUTTOIN_LEARN_UNKNOWN;

      skillSelectedName.text = m_selectedSkill.skillName;
      skillSelectedProps.text = string.Format(
          "{0}\n{1}", m_selectedSkill.skillLevel, m_selectedSkill.CalculateNextLevelCost()
      );

      // check xp
      skillBtnUpgrade.interactable = m_selectedSkill.CheckRequirements();
    }
  }

  private void Start()
  {
    toggle_ui(false);

    for(int i=0; i<skillsUnknown.childCount; ++i)
    {
      Transform child = skillsUnknown.GetChild(i);
      init_buttons(child);
    }

    for (int i = 0; i < skillsLearnt.childCount; ++i)
    {
      Transform child = skillsLearnt.GetChild(i);
      init_buttons(child);
    }

    // NOTICE We are not loading learnt skills from previous game / game save.
    // NOTICE You can do this here :)
  }

  private void init_buttons(Transform child)
  {
    Button button = child.GetComponent<Button>();
    Skill skill = child.GetComponent<Skill>();

    if (skill == null)
      Debug.LogWarning("Cannot find attached Skill to this game object!", this);

    button.onClick.AddListener(() => OnSkillSelect(skill));
  }
}
