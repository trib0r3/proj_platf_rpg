using UnityEngine;

public class ItemFood : Item
{
  public float restore_hp = 10;

  public float GetRestorationHP()
  {
    if (quantity == 0)
    {
      Destroy(this, 1.0f);
    }

    return restore_hp * GetStatsMultiplier();
  }

  public void Eat()
  {
    Use(ItemProperty.EATABLE);
  }

  protected override void on_item_use(ItemProperty useContext)
  {
    switch(useContext)
    {
      case ItemProperty.EATABLE:
        // only correct value
        // TODO restore caller hp
        eat();
        break;

      default:
        // should be never called, because of guard in parent
        Debug.LogError("Item cannot be used in following context!", this);
        on_item_use_failure();
        break;
    }
  }

  protected override void on_item_use_failure()
  {
    Debug.LogWarning("Detected invalid use case of item", this);
  }

  protected override void Start()
  {
    base.Start();

    // NOTICE: every food is eatable
    enable_property(ItemProperty.EATABLE);
  }


  private void eat()
  {
    quantity--;
    m_useLock = true;
  }
}
