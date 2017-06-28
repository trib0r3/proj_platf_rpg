abstract public class Item
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


  protected ItemProperty m_properties;
  protected ItemQuality m_quality = ItemQuality.NORMAL;

  protected float m_baseWeight = 1.0f;
  protected int m_quantity = 1;


  public virtual void Use(ItemProperty useContext)
  {
    if(HasProperty(ItemProperty.DISABLED))
    {
      // item is broken, so cannot be used
      on_item_use_failure();
      return;
    }

    // more actions should be implemented by user
    on_item_use(useContext);
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
}
