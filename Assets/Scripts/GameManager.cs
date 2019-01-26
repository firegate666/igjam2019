using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject StartUI;
    public AlienUI AlienUI;

    public Transform OrbitPivot;

    public PlayerController PlayerPrefab;
    public Vector3 StartPosition;

    private AlienContainer[] _aliens;

    public AlienContainer[] GetAliens()
    {
        return _aliens;
    }
    
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        StartUI.SetActive((true));
    }

    public void StartGame(int numberOfPlayers)
    {
        _aliens = AlienSpawner.Instance.SpawnAliens(numberOfPlayers);
        AlienUI.AddAliens(_aliens);
        
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerController player = Instantiate(PlayerPrefab, StartPosition, Quaternion.identity, null);
            player.SetPlayer(i+1);
            player.OrbitPivot = OrbitPivot;
            player.gameObject.SetActive(true);
            player.SetPositionAngle(Random.Range(0, 359));
        }
        
        StartUI.SetActive(false);
    }
}
