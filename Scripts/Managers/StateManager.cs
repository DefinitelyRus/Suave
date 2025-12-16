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

	public static bool IsPaused => CurrentState != States.Playing;
}
