using UnityEngine.SceneManagement;

public class StartState : StateInterface
{
	public void OnLoad()
	{
		SceneManager.LoadSceneAsync("Start", LoadSceneMode.Additive);
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
		SceneManager.UnloadSceneAsync("Start");
	}

	public void OnUnpause()
	{
	}
}
