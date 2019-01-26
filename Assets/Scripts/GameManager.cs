using System;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject StartUI;
    public AlienUI AlienUI;
    private GameState _gameState = GameState.MainMenu;

    private PlayerController[] _playerControllers;
    private TileSystem _tileSystem;
    [SerializeField] private TileSystemPainter _tileSystemPainter;
    [SerializeField] private PlanetOutlinePainter _planetOutlinePainter;

    public Transform OrbitPivot;

    public PlayerController PlayerPrefab;
    public Vector3 StartPosition;

    private AlienContainer[] _aliens;

    public AlienContainer[] GetAliens()
    {
        return _aliens;
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception("Only one instance of GameManager!");
        }

        _tileSystem = new TileSystem(_tileSystemPainter, _planetOutlinePainter);
        StartUI.SetActive(true);
    }

    public void StartGame(int numberOfPlayers)
    {
        _aliens = AlienSpawner.Instance.SpawnAliens(numberOfPlayers);
        AlienUI.AddAliens(_aliens);
        _playerControllers = new PlayerController[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerController player = Instantiate(PlayerPrefab, StartPosition, Quaternion.identity, null);
            player.SetPlayer(i + 1);
            player.OrbitPivot = OrbitPivot;
            player.gameObject.SetActive(true);

            _playerControllers[i] = player;
        }

        StartUI.SetActive(false);
        _gameState = GameState.Planet;
    }

    private void Update()
    {
        if (_gameState == GameState.Planet)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _tileSystem.DoDrop(_playerControllers[0].positionAngle, Elements.Stone);
            }
        }
    }
}
