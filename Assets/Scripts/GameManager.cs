using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject StartUI;

    public Transform OrbitPivot;

    public PlayerController PlayerPrefab;
    public Vector3 StartPosition;


    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        StartUI.SetActive((true));
    }

    public void StartGame(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerController player = Instantiate(PlayerPrefab, StartPosition, Quaternion.identity, null);
            player.SetPlayer(i+1);
            player.OrbitPivot = OrbitPivot;
            player.gameObject.SetActive(true);
        }
        
        StartUI.SetActive(false);
    }
}
