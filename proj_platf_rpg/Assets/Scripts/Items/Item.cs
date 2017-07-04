using UnityEngine;

abstract public class Item : MonoBehaviour
{
  const int MAX_STACK_SIZE = 16;

  public enum ItemProperty
  {
    NONE      = 0,

    EATABLE   = (1 << 1),
    EQUIPABLE = (1 << 2),
    STACKABLE = (1 << 3),

    ARMOUR    = (1 << 4),
    WEAPON    = (1 << 5),
    
    DISABLED  = (1 << 6) // "broken"
  };

  public enum ItemQuality
  {
    BROKEN = 0,
    NORMAL = 1,
    SUPER  = 2
  };

  public float prize = 10; // per unit

  [HideInInspector]
  public int eid = -1; // equipment id

  public ItemQuality quality
  {
    get { return m_quality; }
    set { on_item_quality_changed(value); }
  }

  public string itemName;
  public string itemDescription;

  public float weight
  {
    get { return get_real_weight(); }
  }

  public float baseWeight
  {
    get { return baseWeight; }
  }

  public int quantity
  {
    get { return m_quantity; }
    set { on_item_quantity_changed(value); }
  }

  // properties enabled only on start
  // array is cleared after initialization
  public ItemProperty[] initialProperties;

  protected ItemProperty m_properties;
  protected ItemQuality m_quality = ItemQuality.NORMAL;

  protected float m_baseWeight = 1.0f;
  protected int m_quantity = 1;

  // use lock - concept
  // we need to protect 1-use items against destroy
  //
  // example scenario:
  //  1) we are using potion by Use()
  //  2) if we have > 0 items in stack
  //    2.1) we can get restoring mana amount by GetMana() method
  //  3) else
  //    3.1 item is destroyed (because is empty)
  //    3.2 we cannot use GetMana() method because Item is destroyed :(
  //
  // solution:
  //   lock item destroying during on time between Use() & GetMana(), then check conditions
  //
  // notice:
  //   lock is set in child class, because we don't know correct context of setting lock on item
  //   guard in Item does not guarantee, that correct context for item is correct of setting lock
  //   so we have to do it manually in each class
  protected bool m_useLock = false;

  public virtual void Use(ItemProperty useContext)
  {
    if (HasProperty(ItemProperty.DISABLED) || !HasProperty(useContext))
    {
      // item is broken, so cannot be used
      // or if item haven't ctx property
      on_item_use_failure();
      return;
    }

    else if (HasProperty(ItemProperty.STACKABLE) && m_useLock)
    {
      // item is locked, just skip use & belive in user multi-click
      return;
    }

    else
    {
      // more actions should be implemented by user
      on_item_use(useContext);
    }
  }

  public void SetProperties(ItemProperty properties)
  {
    m_properties = properties;
  }

  public bool HasProperty(ItemProperty property)
  {
    if ((m_properties & property) != ItemProperty.NONE)
      return true;

    return false;
  }

  public int GetStatsMultiplier()
  {
    // disable lock
    // important note: 
    //   we dont know the context of use GetStats() 
    //   & we fooly belive in that the call is made once when needed to get calculated value
    //   see: GetRestorationHP() from ItemFood.cs 
    m_useLock = false;

    return (int)m_quality;
  }

  public void Fix()
  {
    disable_property(ItemProperty.DISABLED);
  }


  protected abstract void on_item_use(ItemProperty useContext);
  protected abstract void on_item_use_failure();

  protected void on_item_quality_changed(ItemQuality q)
  {
    if (q == ItemQuality.BROKEN)
    {
      enable_property(ItemProperty.DISABLED);
    }

    else
    {
      m_quality = q;
    }
  }

  protected void enable_property(ItemProperty property)
  {
    m_properties |= property;
  }

  protected void disable_property(ItemProperty property)
  {
    m_properties &= ~(property);
  }

  protected float get_real_weight()
  {
    return m_quantity * baseWeight;
  }

  protected void on_item_quantity_changed(int amount)
  {
    amount = (amount > 0 ? amount : 0);

    if(!HasProperty(ItemProperty.STACKABLE) && amount > 1)
    {
      // if item is not stackable ignore values greater than 1
      // and stay with old value
      return;
    }

    if(amount > MAX_STACK_SIZE)
    {
      // if we are trying to store more than stack size -> ignore action
      // FIXME expected behaviour:
      //   1. Store in this item stored = MAX - quantity
      //   2. Create new item with initial quantity: amount - stored
      return;
    }

    m_quantity = amount;
  }

  protected virtual void Start()
  {
    foreach(ItemProperty prop in initialProperties)
    {
      enable_property(prop);
    }

    initialProperties = null;
  }
}
