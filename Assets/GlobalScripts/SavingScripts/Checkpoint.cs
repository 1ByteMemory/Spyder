using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	bool triggered;
	public string checkpointName;


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !triggered && !GameManager.loadedFromSave)
		{
			triggered = true;

			if (checkpointName == "")
			{
				QuickSave.Save("autosaves", checkpointName);
			}
			else
			{
				QuickSave.Save("autosaves");
			}
		}
	}
}
