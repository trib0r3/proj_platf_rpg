using UnityEngine;

public class CollisionReceiver : MonoBehaviour
{
  public delegate void CallbackCollisionEnter(Collision2D coll);
  public delegate void CallbackTriggerEnter(Collider2D coll);

  public CallbackCollisionEnter onCollisionEnterCallbacks;
  public CallbackTriggerEnter onTriggerEnterCallbacks;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (onCollisionEnterCallbacks != null)
      onCollisionEnterCallbacks(collision);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (onTriggerEnterCallbacks != null)
      onTriggerEnterCallbacks(collision);
  }
}
