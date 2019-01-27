using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using ProBuilder2.Common;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

	public ScoreManager TheScore;

	public UITimer Timer;
	public TextMeshProUGUI gameScore;
	public GameObject scoreAddedPrefab;
	public GameObject MainUI;
    public GameObject StartUI;
    public GameObject GameOverUI;
    public AdvertisementsUI AdvertisementUI;
    public AlienUI AlienUI;
    public GameState gameState = GameState.MainMenu;

    public ParticleSystem PlanetFishedFX;
    public ParticleSystem PlanetCompleteFX;
    public ParticleSystem NextPlanetFX;

    public Animator PlanetAnimatior;

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
		TheScore.setTotalScore(0);
		gameScore.text = "0";

        
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
	    _tileSystem = new TileSystem(_tileSystemPainter, _planetOutlinePainter);
        MainUI.SetActive(true);
        gameState = GameState.Planet;
        Timer.SetRunning(GlobalConfig.GameplaySeconds, () => GameOver());
    }

    public void GameOver()
    {
	    Timer.Pause();
	    _tileSystemPainter.gameObject.SetActive(false);
	    _planetOutlineContainer.gameObject.SetActive(false);
	    
	    GameOverUI.SetActive(true);
	    GameOverUI.GetComponentInChildren<TextMeshProUGUI>().text = "" + TheScore.getTotalScore() * 100;

	    foreach (var player in _playerControllers)
	    {
		    player.gameObject.SetActive(false);
	    }
    }
    
	public bool doDrop(float playerPosition, int playerNo, Elements element)
	{
		if (_tileSystem.DoDrop(playerPosition, element))
		{
			Camera.main.gameObject.GetComponent<Shake>().DoShake();
			_playerControllers[playerNo - 1].assignRandomElement();
//			UpdateScoreText();
			return true;
		}
//		UpdateScoreText();
		return false;
	}

	public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

	public void PlanetFull()
	{
		UpdateScoreText();
		TheScore.addTotalScore(TheScore.getPlanetScore());
		TheScore.setPlanetScore(0);
		Debug.Log(TheScore.getTotalScore());
		gameState = GameState.Advertisements;
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
		yield return new WaitForSeconds(1);
		PlanetAnimatior.SetTrigger("planetOut");
		Timer.Pause();
		yield return new WaitForSeconds(2);
		_tileSystemPainter.gameObject.SetActive(false);
		_planetOutlineContainer.gameObject.SetActive(false);
		gameState = GameState.Advertisements;
		AdvertisementUI.gameObject.SetActive(true);
		PlanetFishedFX.Stop();
		PlanetCompleteFX.Stop();
		PlanetCompleteFX.Clear();
	}

	public void LeaveAdvertisements()
	{
		gameState = GameState.Planet;
		PlanetAnimatior.SetTrigger("planetIn");
		_tileSystemPainter.gameObject.SetActive(true);
		_planetOutlineContainer.gameObject.SetActive(true);
		ClearPlanet();
		Timer.Unpause();
		NextPlanetFX.Play();
		StartCoroutine(StopNextPlanetFXDelayed());
	}

	IEnumerator StopNextPlanetFXDelayed()
	{
		yield return new WaitForSeconds(2);
		NextPlanetFX.Stop();
		NextPlanetFX.Clear();
	}
	
	public void ClearPlanet()
	{
		_tileSystem.Dispose();
		_tileSystem = new TileSystem(_tileSystemPainter, _planetOutlinePainter);
	}

	public void UpdateScoreText(bool showScoreIndicator = true)
	{
		if (gameState != GameState.Planet)
		{
			return;
		}
		int elementCount1 = _tileSystem.CountElement(_aliens[0].Element);
		int elementCount2 = _tileSystem.CountElement(_aliens[1].Element);

		if (showScoreIndicator && TheScore.getPlanetScore() != elementCount1 + elementCount2)
		{
			StartCoroutine(spawnScortextIndicator((elementCount1 + elementCount2- TheScore.getPlanetScore())*100, 0.5f));
		}
		
		TheScore.setPlanetScore(elementCount1 + elementCount2);
		gameScore.text = "" + (TheScore.getTotalScore()+TheScore.getPlanetScore())*100;
	}

	IEnumerator spawnScortextIndicator(int scoreText, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject scoreAddedText = Instantiate(scoreAddedPrefab, gameScore.transform);
		scoreAddedText.GetComponent<TextMeshProUGUI>().text =
			""+scoreText;
		Destroy(scoreAddedText, 2f);
	}
}
