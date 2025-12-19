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
			// Stop music when transitioning to Win or Lose states
			if ((value == States.Win || value == States.Lose) && _currentState == States.Playing) {
				SoundPlayer.StopMusic();
			}
			// Start music when transitioning to Playing state
			else if (value == States.Playing && _currentState != States.Playing) {
				SoundPlayer.PlayMusic("Gerudo Valley Remix  Super Smash Bros. Ultimate.wav");
			}

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
