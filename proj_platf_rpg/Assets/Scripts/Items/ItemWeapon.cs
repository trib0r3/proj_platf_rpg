using UnityEngine;

public class ItemWeapon : Item
{
  public Weapon weapon;

  protected override void on_item_use(ItemProperty useContext, PlayableCharacter user)
  {
    switch(useContext)
    {
      case ItemProperty.EQUIPPED:
        // called when item is equipped / unequipped
        float modifier = 0;
        if(HasProperty(ItemProperty.EQUIPPED))
        {
          // item is now equipped, so we have to add stats
          modifier = weapon.dmg;
        }
        else
        {
          // item is unequipped, so delete effect from character
          modifier = -weapon.dmg;
        }

        // update stats
        user.stats.dmg += modifier;
        break;

      default:
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

    // NOTICE: every weapon is an EQUIPABLE WEAPON ;)
    enable_property(ItemProperty.EQUIPABLE);
    enable_property(ItemProperty.WEAPON);
  }
}
