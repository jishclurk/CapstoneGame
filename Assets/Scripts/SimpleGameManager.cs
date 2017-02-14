using UnityEngine;
using System.Collections;

// Game States
// for now we are only using these two
public enum State { INTRO, MAIN_MENU, PLAY }

public delegate void OnStateChangeHandler();

public class SimpleGameManager {
	protected SimpleGameManager() {}
	private static SimpleGameManager instance = null;
	public event OnStateChangeHandler OnStateChange;
	public  State gameState { get; private set; }

	public static SimpleGameManager Instance{
		get {
			if (SimpleGameManager.instance == null){
				//DontDestroyOnLoad(SimpleGameManager.instance);
				SimpleGameManager.instance = new SimpleGameManager();
			}
			return SimpleGameManager.instance;
		}

	}

	public void SetGameState(State state){
		this.gameState = state;
		OnStateChange();
	}

	public void OnApplicationQuit(){
		SimpleGameManager.instance = null;
	}

}