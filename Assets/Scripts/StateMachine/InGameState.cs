using UnityEngine.SceneManagement;

public class InGameState : StateInterface
{
	public void OnLoad()
	{
		SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
	}

	public void OnPause()
	{
	}

	public void OnStart()
	{
	}

	public void OnStop()
	{
	}

	public void OnUnload()
	{
		SceneManager.UnloadSceneAsync("Game");
	}

	public void OnUnpause()
	{
	}
}
