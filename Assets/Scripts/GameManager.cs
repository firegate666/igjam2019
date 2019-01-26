using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using ProBuilder2.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

	public ScoreManager TheScore;

	public UITimer Timer;
    public GameObject StartUI;
    public GameObject GameOverUI;
    public AlienUI AlienUI;
    private GameState _gameState = GameState.MainMenu;

    private PlayerController[] _playerControllers;
    private TileSystem _tileSystem;
    [SerializeField] private TileSystemPainter _tileSystemPainter;
    [SerializeField] private PlanetOutlinePainter _planetOutlinePainter;
    [SerializeField] private Transform _planetOutlineContainer;

	[SerializeField] private int planetsPast;

    public Transform OrbitPivot;

    public PlayerController PlayerPrefab;
    public Vector3 StartPosition;

    private List<AlienContainer> _aliens;

    public List<AlienContainer> GetAliens()
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
        _planetOutlineContainer.gameObject.SetActive(false);
        StartUI.SetActive(true);
    }

    public void StartGame(int numberOfPlayers)
    {
		planetsPast = 0;

        _aliens = AlienSpawner.Instance.SpawnAliens(numberOfPlayers).ToList();
        AlienUI.AddAliens(_aliens.ToArray());
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
        _planetOutlineContainer.gameObject.SetActive(true);
        _gameState = GameState.Planet;
        Timer.SetRunning(120, () => { Debug.Log("Time is monkey"); });
    }

    public void GameOver()
    {
	    GameOverUI.SetActive(true);

	    foreach (var player in _playerControllers)
	    {
		    player.gameObject.SetActive(false);
	    }
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
			{
				Debug.Log("zero points");
				continue;
			}

			if (alienPoints[i] > maxPoints)
			{
				Debug.Log("new winnner");
				allWinners.Clear();
				allWinners.Add(i);
				maxPoints = alienPoints[i];
			} else if (alienPoints[i] == maxPoints)
			{
				Debug.Log("Additional winner");
				allWinners.Add(i);
			}
		}
		for (int i = 0; i < allWinners.Count; i++)
		{
			Debug.Log("Winner declared");
			TheScore.addScore(alienPoints[allWinners[i]]);
			Debug.Log(TheScore.getScore());
			
			AlienUI.RemoveAlien(_aliens[allWinners[i]]);
			_aliens.Remove(_aliens[allWinners[i]]);
			AlienContainer newAlien = AlienSpawner.Instance.SpawnAlien();
			AlienUI.AddAlien(newAlien);
			_aliens.Add(newAlien);
		}

		_tileSystem.Dispose();
		_tileSystem = new TileSystem(_tileSystemPainter, _planetOutlinePainter);

		planetsPast++;
	}
}
