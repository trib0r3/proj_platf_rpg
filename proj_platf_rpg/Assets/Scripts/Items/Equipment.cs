using UnityEngine;
using System.Collections.Generic;

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

  private int m_gold = 0;
  private float m_capacity = 100;
  private float m_weight = 0;

  private int m_nextId = 0;
  private Transform m_itemsParent;

  // key is equipment id, unique only in eq context
  private Dictionary<int, Item> m_items = new Dictionary<int, Item>();


  public bool AddItem(Item item)
  {
    if (weight + item.weight <= capacity)
    {
      m_items.Add(m_nextId++, item);
      item.transform.SetParent(m_itemsParent);
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
      if (destroy)
      {
        Destroy(item.gameObject, 1);
      }
      else
      {
        item.transform.SetParent(transform.parent);
      }

      update_weight(weight - item.weight);
      m_items.Remove(eid);

      return true;
    }

    return false;
  }


  private void Awake()
  {
    m_itemsParent = new GameObject("Items").transform;
    m_itemsParent.SetParent(transform);
  }

  private void Start()
  {
    foreach (Item item in defaultItems)
    {
      AddItem(item);
    }

    defaultItems = null;
  }

  private void update_gold(int amount)
  {
    m_gold = amount;
  }

  private void update_weight(float value)
  {
    m_weight = value;
  }
}
