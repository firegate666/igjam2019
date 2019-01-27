using System;
using System.Collections;
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
	public GameObject MainUI;
    public GameObject StartUI;
    public GameObject GameOverUI;
    public AdvertisementsUI AdvertisementUI;
    public AlienUI AlienUI;
    public GameState gameState = GameState.MainMenu;

    public ParticleSystem PlanetFishedFX;
    public ParticleSystem PlanetCompleteFX;
    public ParticleSystem NextPlanetFX;

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
        MainUI.SetActive(true);
        gameState = GameState.Planet;
        Timer.SetRunning(GlobalConfig.GameplaySeconds, () => GameOver());
    }

    public void GameOver()
    {
	    _tileSystemPainter.gameObject.SetActive(false);
	    _planetOutlineContainer.gameObject.SetActive(false);
	    
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
		planetsPast++;

		PlanetFishedFX.Play();
		PlanetCompleteFX.Play();
		StartCoroutine(TriggerAdvertisements());
	}

	IEnumerator TriggerAdvertisements()
	{
		yield return new WaitForSeconds(3);
		Timer.Pause();
		_tileSystemPainter.gameObject.SetActive(false);
		_planetOutlineContainer.gameObject.SetActive(false);
		AdvertisementUI.gameObject.SetActive(true);
		PlanetFishedFX.Stop();
		PlanetCompleteFX.Stop();
		yield return new WaitForSeconds(4);
		_tileSystemPainter.gameObject.SetActive(true);
		_planetOutlineContainer.gameObject.SetActive(true);
		Timer.Unpause();
		ClearPlanet();
		
		NextPlanetFX.Play();

		StartCoroutine(StopNextPlanetFXDelayed());
	}

	IEnumerator StopNextPlanetFXDelayed()
	{
		yield return new WaitForSeconds(2);
		NextPlanetFX.Stop();
	}
	
	public void ClearPlanet()
	{
		_tileSystem.Dispose();
		_tileSystem = new TileSystem(_tileSystemPainter, _planetOutlinePainter);
	}
}
