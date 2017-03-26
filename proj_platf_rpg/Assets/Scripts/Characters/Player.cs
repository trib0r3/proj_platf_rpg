using UnityEngine;

public class Player : PlayableCharacter
{
  public float runningSpeedMultiplier = 2.0f;

  protected override void Move()
  {
    float vy = m_rigidbody.velocity.y;
    float vx = m_vspeed * m_controller.moveDirection;

    if (m_controller.isRunningKeyClicked)
      vx *= runningSpeedMultiplier;

    m_rigidbody.velocity = new Vector2(vx, vy);
  }
}
