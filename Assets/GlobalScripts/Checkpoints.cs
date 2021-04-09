using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
	LevelSelection selection;
	GameManager gm;
	GameObject player;
	PlayerController pc;
	PlayerWeapon weapons;

	string sceneName;

	// Start is called before the first frame update
	void Start()
    {
		selection = GetComponentInChildren<LevelSelection>();
		gm = GetComponent<GameManager>();
		player = GameObject.FindGameObjectWithTag("Player");
		pc = player.GetComponent<PlayerController>();
		weapons = player.GetComponent<PlayerWeapon>();
		sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
     
		if (Input.GetKey(KeyCode.F5))
		{
			// Quick Save
			LevelInfo lvl = new LevelInfo();
			lvl.abilityUnlocked = pc.isAbilityUnlocked;
			lvl.sceneName = sceneName; // null
			lvl.spawnPoint = player.transform.position;
			lvl.title = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString(); // make this -> mm-dd-yy
			lvl.availableWeapons = weapons.weapons.ToArray();
			lvl.foundKeys = KeycardIcon.keysFound;
			lvl.dimension = (int)GameManager.currentActiveDimension;
		}

		if (Input.GetKey(KeyCode.F6))
		{
			// Quick Load


		}
    }
}
