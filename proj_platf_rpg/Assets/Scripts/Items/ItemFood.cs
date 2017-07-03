using UnityEngine;

public class ItemFood : Item
{
  public float restore_hp = 10;
  bool useLock = false;

  private void Awake()
  {
    itemName = "Food";
    itemDescription = "Use it to restore HP";

    enable_property(ItemProperty.EATABLE);
  }

  protected override void on_item_use(ItemProperty useContext)
  {
    if (useLock)
      return;

    switch(useContext)
    {
      case ItemProperty.EATABLE:
        // only correct value
        // TODO restore caller hp
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

  public float GetRestorationHP()
  {
    useLock = false;
    if(quantity == 0)
    {
      Destroy(this, 1.0f);
    }

    return restore_hp * GetStatsMultiplier();
  }

  private void eat()
  {
    quantity--;
    useLock = true;
  }
}
