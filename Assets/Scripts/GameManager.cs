using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

	public ScoreManager TheScore;

	public UITimer Timer;
	public TextMeshProUGUI gameScore;
	public TextMeshProUGUI highScore;
	public GameObject scoreAddedPrefab;
	public GameObject MainUI;
    public GameObject StartUI;
    public GameObject GameOverUI;
    public IntermissionUI IntermissionUI;
    public ElementSpawner ElementSpawner; 
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
		TheScore.LoadPlayerProgress();
		TheScore.setTotalScore(0);
		TheScore.setPlanetScore(0);
		gameScore.text = "0";

		int highscore = TheScore.getHighestScore() * 100;
		highScore.text = highscore.ToString();
        
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
        ElementSpawner.spawnElements = true;
        Timer.SetRunning(GlobalConfig.GameplaySeconds, () => GameOver());
    }

    public void GameOver()
    {
	    Timer.Pause();
	    _tileSystemPainter.gameObject.SetActive(false);
	    _planetOutlineContainer.gameObject.SetActive(false);
	    
	    GameOverUI.SetActive(true);
	    GameOverUI.GetComponentInChildren<TextMeshProUGUI>().text = "" + TheScore.getTotalScore() * 100;

		TheScore.CheckHighscore();

	    foreach (var player in _playerControllers)
	    {
		    player.gameObject.SetActive(false);
	    }
    }

    private void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
		    Restart();
	    }
    }


    public bool doDrop(float playerPosition, int playerNo, Elements element)
	{
		if (_tileSystem.DoDrop(playerPosition, element))
		{
			Camera.main.gameObject.GetComponent<Shake>().DoShake();
			_playerControllers[playerNo - 1].SetElementToDrop(Elements.NotSet);
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
		int planetScore = TheScore.getPlanetScore();
		TheScore.addTotalScore(planetScore);
		TheScore.setPlanetScore(0);
		//Debug.Log(TheScore.getTotalScore());
		gameState = GameState.Advertisements;
		ElementSpawner.spawnElements = false;
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
				//Debug.Log("zero points");
				continue;
			}

			if (alienPoints[i] > maxPoints)
			{
				//Debug.Log("new winnner");
				allWinners.Clear();
				allWinners.Add(i);
				maxPoints = alienPoints[i];
			} else if (alienPoints[i] == maxPoints)
			{
				//Debug.Log("Additional winner");
				allWinners.Add(i);
			}
		}
		for (int i = 0; i < allWinners.Count; i++)
		{
			//Debug.Log("Winner declared");
			
			AlienUI.RemoveAlien(_aliens[allWinners[i]]);
			_aliens.Remove(_aliens[allWinners[i]]);
			AlienContainer newAlien = AlienSpawner.Instance.SpawnAlien();
			AlienUI.AddAlien(newAlien);
			_aliens.Add(newAlien);
		}
		
		IntermissionUI.SetPlanetScore(planetScore);
		
		planetsPast++;

		Timer.Pause();
		gameState = GameState.Advertisements;
		ElementSpawner.spawnElements = false;
		PlanetFishedFX.Play();
		PlanetCompleteFX.Play();
		StartCoroutine(TriggerAdvertisements());
	}

	IEnumerator TriggerAdvertisements()
	{
		IntermissionUI.gameObject.SetActive(true);
		PlanetAnimatior.SetTrigger("planetOut");
		yield return new WaitForSeconds(2);
		IntermissionUI.gameObject.SetActive(false);
			
			
		
		
		yield return new WaitForSeconds(1.5f);
		_tileSystem.Dispose();
		yield return null;
		_planetOutlineContainer.gameObject.SetActive(false);
		_tileSystemPainter.gameObject.SetActive(false);
		yield return null;
		AdvertisementUI.gameObject.SetActive(true);
		PlanetFishedFX.Stop();
		PlanetFishedFX.Clear();
		PlanetCompleteFX.Stop();
		PlanetCompleteFX.Clear();
	}

	public void LeaveAdvertisements()
	{
		StartCoroutine(LeaveAdvertisementsCoroutine());
	}
	
	private IEnumerator LeaveAdvertisementsCoroutine()
	{
		_tileSystem = new TileSystem(_tileSystemPainter, _planetOutlinePainter);
		_tileSystemPainter.gameObject.SetActive(true);
		_planetOutlineContainer.gameObject.SetActive(true);
		PlanetAnimatior.SetTrigger("planetIn");
		yield return null;
		gameState = GameState.Planet;
		ElementSpawner.spawnElements = true;
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
