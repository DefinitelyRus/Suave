namespace Suave.Scripts.Managers;

internal class StateManager {
	public enum States {
		Playing,
		Paused,
		Menu,
		Win,
		Lose,
		Transition
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

	public static async Task StartTransition() {
		CurrentState = States.Transition;

		await Task.Run(() => Thread.Sleep(1000));

		CurrentState = States.Playing;
	}
}
