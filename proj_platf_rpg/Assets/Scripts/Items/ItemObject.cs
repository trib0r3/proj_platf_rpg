using UnityEngine;

// Wrapper on real object, needed only for visibility in scene
public class ItemObject : MonoBehaviour
{
  public Item item; // real item object

  private CircleCollider2D _triggerCollider;

  [SerializeField]
  private SpriteRenderer _renderer;

  public void SetVisibleOnScene(bool visible, Vector3 position)
  {
    _triggerCollider.enabled = visible;
    _renderer.enabled = visible;

    transform.position = position;
  }

  protected virtual void OnTriggerEnter2D(Collider2D collision)
  {
    item.OnItemTrigger(collision);
  }

  protected virtual void OnCollisionEnter2D(Collision2D collision)
  {
    item.OnItemCollide(collision);
  }

  private void Awake()
  {
    _triggerCollider = GetComponent<CircleCollider2D>();
    _renderer = GetComponent<SpriteRenderer>();
  }
}
