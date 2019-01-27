using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuotePanelUI : MonoBehaviour
{
	public GameObject Star1Quote;
	public GameObject Star2Quote;
	public GameObject Star3Quote;

	public void SetStars(int stars)
	{
		Star1Quote.SetActive(stars == 1);
		Star2Quote.SetActive(stars == 2);
		Star3Quote.SetActive(stars == 3);
	}
}
