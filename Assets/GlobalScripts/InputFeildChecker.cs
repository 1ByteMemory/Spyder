using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Property
{
	speed,
	jumpForce,
	jumpTime,
	fall,
	look
}

public class InputFeildChecker : MonoBehaviour
{
	public Text text;
	public Property property;
	PlayerMovement c;

	private void Start()
	{
		c = FindObjectOfType<PlayerMovement>();

		switch (property)
		{
			case Property.speed:
				text.text = c.playerSpeed.ToString();
				break;
			case Property.jumpForce:
				text.text = c.playerJumpForce.ToString();
				break;
			case Property.jumpTime:
				text.text = c.maxJumpTime.ToString();
				break;
			case Property.fall:
				text.text = c.fallMultiplayer.ToString();
				break;
			case Property.look:
				text.text = c.lookSpeed.ToString();
				break;
			default:
				break;
		}
	}


	public void CheckInput(string number)
	{
		if (IsDigitsOnly(number))
		{
			text.text = number;

			switch (property)
			{
				case Property.speed:
					c.playerSpeed = float.Parse(number);
					break;
				case Property.jumpForce:
					c.playerJumpForce = float.Parse(number);
					break;
				case Property.jumpTime:
					c.maxJumpTime = float.Parse(number);
					break;
				case Property.fall:
					c.fallMultiplayer = float.Parse(number);
					break;
				case Property.look:
					c.lookSpeed = float.Parse(number);
					break;
				default:
					break;
			}
		}
		else
		{
			text.text = "";
		}
	}


	bool IsDigitsOnly(string str)
	{
		foreach (char c in str)
		{
			if ((c > '0' || c < '9') || c == 46)
				return true;
		}

		return false;
	}

}
