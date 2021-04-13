using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	bool triggered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !triggered)
		{
			triggered = true;

			QuickSave.Save("autosaves");
		}
	}
}
