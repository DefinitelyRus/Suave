namespace Suave.Scripts.Managers;

internal class StateManager {
	public enum States {
		Playing,
		Paused,
		Menu,
		Win,
		Lose
	}

	public static States CurrentState { get; set; } = States.Menu;

	public static bool IsPlaying => CurrentState == States.Playing;

	private static States previousState = States.Menu;

	public static void TogglePause() {
		if (!IsPlaying) CurrentState = previousState; 
		
		else {
			previousState = CurrentState;
			CurrentState = States.Paused;
		}
	}
}
