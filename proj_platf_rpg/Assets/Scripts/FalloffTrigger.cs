using UnityEngine;

public class FalloffTrigger : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    del(collision.gameObject);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    del(collision.gameObject);
  }

  private void del(GameObject obj)
  {
    switch (obj.tag)
    {
      case "Player":
        GameMaster.gm.NotifyGameOver(false, "Gravity is brutal :(");
        break;

      default:
        if (obj.transform.parent == null)
        {
          Debug.Log("Falloff: Destroying: " + obj.name);
          Destroy(obj);
        }
        break;
    }
  }
}
