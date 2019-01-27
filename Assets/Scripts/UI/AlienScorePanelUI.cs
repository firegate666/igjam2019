using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienScorePanelUI : MonoBehaviour
{
	public Image AlienPanel1;
	public Image AlienPanel2;

	private int _winnerCount = 0;

	public void AddWinner(AlienContainer alien)
	{
		if (_winnerCount == 0)
		{
			AlienPanel1.sprite = alien.AlienWinnerImage.sprite;
			_winnerCount++;
			AlienPanel1.gameObject.SetActive(true);
		}
		else
		{
			AlienPanel2.sprite = alien.AlienWinnerImage.sprite;
			AlienPanel2.gameObject.SetActive(true);
		}
	}

	public void Reset()
	{
		_winnerCount = 0;
		AlienPanel1.gameObject.SetActive(false);
		AlienPanel2.gameObject.SetActive(false);
	}
}
