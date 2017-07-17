using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
  public int gold
  {
    get { return m_gold; }
    set { update_gold(value); }
  }

  public float capacity
  {
    get { return m_capacity; }
  }

  public float weight
  {
    get { return m_weight; }
  }

  public Item[] defaultItems; // default items for eq added after eq creation

  [SerializeField]
  private int m_gold = 0;

  [SerializeField]
  private float m_capacity = 100;
  private float m_weight = 0;

  private int m_nextId = 0;

  [SerializeField]
  private Transform m_itemsParent;

  // key is equipment id, unique only in eq context
  private Dictionary<int, Item> m_items = new Dictionary<int, Item>();

  [SerializeField]
  private Text m_textGold;
  [SerializeField]
  private Text m_textCapacity;

  // Notice: items can be clicked only from player inventory
  // So we haven't to check null-valued gui objects
  [Header("Visual fields")]
  [SerializeField]
  private Button[] m_actionButtons;
  [SerializeField]
  private Text[] m_selectedTexts;

  private Item m_selectedItem = null;


  public bool AddItem(Item item)
  {
    // Adds item only if can store additional items (based on item weight)
    if (weight + item.weight <= capacity)
    {
      m_items.Add(m_nextId, item);
      item.eid = m_nextId++;

      item.transform.SetParent(m_itemsParent);
      item.transform.localScale = Vector3.one;

      update_weight(weight + item.weight);
      return true;
    }

    return false;
  }

  public bool DeleteItem(int eid, bool destroy = false)
  {
    // Check if item is available
    // if yes, check it should be destryoed from game
    //  if yes, then destroy it
    //  else change parent
    if (m_items.ContainsKey(eid))
    {
      Item item = m_items[eid];
      float itemWeight = item.weight;
      
      item.transform.SetParent(null);
      item.transform.localScale = Vector3.one;

      update_weight(weight - itemWeight);
      m_items.Remove(eid);

      if (destroy)
        Destroy(item.gameObject, 1);

      return true;
    }

    return false;
  }

  public virtual void OnSelectItem(Item item)
  {
    update_selected(true, item);
  }

  public void EquipSelected()
  {
    // properly equip/unequip item
    m_selectedItem.SetEquipped(!m_selectedItem.HasProperty(Item.ItemProperty.EQUIPPED));

    // TODO update stats
  }

  public void UseSelected()
  {
    // HACK temporary solution
    // at this moment we have only one context for using item from menu
    // if more, then we have to choose in some way
    //
    // idea: lets add new item property "USABLE" and define new method "DefaultUseBehaviour"
    //   then each time we use item in USABLE ctx the DefaultUseBehaviour will be called
    m_selectedItem.Use(Item.ItemProperty.EATABLE);
    update_weight(weight - m_selectedItem.baseWeight);

    if(m_selectedItem.quantity == 0)
    {
      // item is used & no items left in stack, so delete it
      DeleteItem(m_selectedItem.eid, true);
      update_selected(false);
    }
  }

  public void DropSelected()
  {
    DeleteItem(m_selectedItem.eid);
    m_selectedItem.SetPhysicalOnScene(
      true, 
      GameMaster.gm.player.transform.position + new Vector3(0, 2, 0)
    );

    update_selected(false);
  }


  private void Start()
  {
    foreach (Item item in defaultItems)
    {
      if(!AddItem(item))
      {
        Debug.LogWarning(string.Format(
          "{0}: item weight ({1}) exceeds available capacity (uses / available: {2} / {3})",
          gameObject.name,
          item.itemName,
          item.weight,
          capacity - weight)
        );
      }
    }

    defaultItems = null;

    update_gold(gold); // force to update gui
    update_weight(weight);
  }

  private void update_gold(int amount)
  {
    m_gold = amount;

    if(m_textGold != null)
      m_textGold.text = string.Format("{0}g", m_gold);
  }

  private void update_weight(float value)
  {
    m_weight = value;

    if(m_textCapacity != null)
      m_textCapacity.text = string.Format("{0} / {1}", weight, capacity);
  }

  private void update_selected(bool selected, Item item = null)
  {
    foreach(Button button in m_actionButtons)
    {
      button.interactable = selected;
    }

    if(selected)
    {
      // fix buttons
      if (!item.HasProperty(Item.ItemProperty.EQUIPABLE))
        m_actionButtons[0].interactable = false;

      if (!item.HasProperty(Item.ItemProperty.EATABLE) /* or any other "USABLE" property */)
        m_actionButtons[1].interactable = false;
    }

    if(m_selectedItem != null)
    {
      // if we selected earlier something lets recolor it
      m_selectedItem.GetComponent<Image>().color = new Color(1, 1, 1);
    }

    if (selected)
    {
      // recolor item to make item visually selected
      item.GetComponent<Image>().color = new Color32(155, 155, 155, 255);

      // update values
      m_selectedTexts[0].text = item.itemName;
      m_selectedTexts[1].text = item.itemDescription;
    }
    else
    {
      foreach(Text text in m_selectedTexts)
      {
        text.text = "";
      }
    }

    m_selectedItem = item;
  }
}
