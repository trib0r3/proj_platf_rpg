using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public static GameMaster gm = null;

	void Awake() {
		if (gm == null)
			gm = this;
	}
}
