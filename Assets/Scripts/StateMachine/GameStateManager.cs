using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
	StateInterface _currentState;

	public void ChangeState(StateInterface state)
	{
		if (_currentState != null)
		{
			LeaveState(_currentState);
		}

		LoadState(state);
		_currentState = state;
	}

	void LeaveState(StateInterface state)
	{
		state.OnStop();
		state.OnUnload();
	}

	void LoadState(StateInterface state)
	{
		state.OnLoad();
		state.OnStart();
	}

    // Start is called before the first frame update
    void Start()
    {
		ChangeState(new StartState());
    }
}
