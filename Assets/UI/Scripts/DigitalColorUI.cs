using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void SetColor(Color col);

public class DigitalColorUI : MonoBehaviour
{
    public static Color color;
    public Image imgCol;
    public TextMeshProUGUI textCol;
	
	public static SetColor setColDelagate;

	private void Start()
	{

		setColDelagate += ChangeCol;
		setColDelagate(color);
	}
	private void OnDisable()
	{
		if (imgCol != null)
			setColDelagate -= ChangeCol;
	}


	private void ChangeCol(Color col)
	{
        if (imgCol != null)
            imgCol.color = col;
		if (textCol != null)
		{
			textCol.color = col;
		}
	}
}
