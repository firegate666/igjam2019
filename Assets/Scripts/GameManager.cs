using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

	public ScoreManager TheScore;

    public GameObject StartUI;
    public AlienUI AlienUI;
    private GameState _gameState = GameState.MainMenu;

    private PlayerController[] _playerControllers;
    private TileSystem _tileSystem;
    [SerializeField] private TileSystemPainter _tileSystemPainter;
    [SerializeField] private PlanetOutlinePainter _planetOutlinePainter;

	[SerializeField] private int planetsPast;

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

		TheScore = new ScoreManager();
		TheScore.setScore(0);

        _tileSystem = new TileSystem(_tileSystemPainter, _planetOutlinePainter);
        _tileSystemPainter.gameObject.SetActive(false);
        StartUI.SetActive(true);
    }

    public void StartGame(int numberOfPlayers)
    {
		planetsPast = 0;

        _aliens = AlienSpawner.Instance.SpawnAliens(numberOfPlayers);
        AlienUI.AddAliens(_aliens);
        _playerControllers = new PlayerController[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerController player = Instantiate(PlayerPrefab, StartPosition, Quaternion.identity, null);
            player.SetPlayer(i + 1);
            player.OrbitPivot = OrbitPivot;
            player.gameObject.SetActive(true);
            player.SetPositionAngle(Random.Range(0, 359));

            _playerControllers[i] = player;
        }

        StartUI.SetActive(false);
        _tileSystemPainter.gameObject.SetActive(true);
        _gameState = GameState.Planet;
    }

    private void Update()
    {
    /*    if (_gameState == GameState.Planet)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _tileSystem.DoDrop(_playerControllers[0].positionAngle, Elements.Stone);
            }
        }*/
    }
    
	public void doDrop(float playerPosition, int playerNo, Elements element)
	{
		if (_tileSystem.DoDrop(playerPosition, element))
		{
			_playerControllers[playerNo - 1].assignRandomElement();
		}
	}

	public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }

	public void PlanetFull()
	{
		// Check for winning alien
		List<int> alienPoints = new List<int>();
		foreach (AlienContainer alien in _aliens)
		{
			int points = _tileSystem.CountElement(alien.Element);
			alienPoints.Add(points);
		}
		int maxPoints = 0;
		List<int> allWinners = new List<int>();
		for (int i = 0; i < alienPoints.Count; i++)
		{
			if (alienPoints[i] == 0)
				continue;
			if (alienPoints[i] > maxPoints)
			{
				allWinners.Clear();
				allWinners.Add(i);
				maxPoints = alienPoints[i];
			} else if (alienPoints[i] == maxPoints)
			{
				allWinners.Add(i);
			}
		}
		for (int i = 0; i < allWinners.Count; i++)
		{
			TheScore.addScore(alienPoints[allWinners[i]]);
			Debug.Log(TheScore.getScore());
		}

		//_tileSystem.Dispose();
		_tileSystem = new TileSystem(_tileSystemPainter, _planetOutlinePainter);

		planetsPast++;
	}
}
