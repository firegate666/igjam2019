using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanelUI : MonoBehaviour
{
	public GameObject Star1Fill;
	public GameObject Star2Fill;
	public GameObject Star3Fill;

	public void SetStars(int stars)
	{
		Star1Fill.SetActive(stars >= 1);
		Star2Fill.SetActive(stars >= 2);
		Star3Fill.SetActive(stars >= 3);
	}
}
