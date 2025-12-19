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

	private static States _currentState = States.Menu;

	public static States CurrentState {
		get => _currentState;
		set {
			bool isGameEnd = value == States.Win || value == States.Lose;
			bool isCurrentlyPlaying = _currentState == States.Playing;
			bool willPlay = value == States.Playing;

			// Stop music when transitioning to game end
			if (isGameEnd && isCurrentlyPlaying) SoundPlayer.StopMusic();
			
			// Start music when transitioning to game start
			else if (willPlay && !isCurrentlyPlaying) SoundPlayer.PlayMusic("Gerudo Valley Remix  Super Smash Bros. Ultimate.wav");

			_currentState = value;
		}
	}

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
