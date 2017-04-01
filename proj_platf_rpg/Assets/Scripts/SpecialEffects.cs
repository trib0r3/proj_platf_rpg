using UnityEngine;
using System.Collections;

public class SpecialEffects : MonoBehaviour
{
  public void ChangeTempColor(SpriteRenderer renderer, Color color, float time)
  {
    Color oldColor = renderer.color;

    StartCoroutine(changeColor(renderer, color));
    StartCoroutine(changeColor(renderer, oldColor, time));
  }

  IEnumerator changeColor(SpriteRenderer renderer, Color color, float wait=0.0f)
  {
    yield return new WaitForSeconds(wait);

    renderer.color = color;
  }
}
