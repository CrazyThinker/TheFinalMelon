using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
	private GameManager gameManager;

	public void PauseGame()
    {
		gameManager.PauseGame(true);
		gameManager.SaveMelons();
	}

	public void ResumeGame()
	{
		gameManager.PauseGame(false);
	}

	public void MainMenu()
	{
		SceneManager.LoadScene("OpeningScene");
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		gameManager.PauseGame(false);

		gameManager.ClearMelons();
	}

	// Start is called before the first frame update
	void Start()
	{
		gameManager = UnityEngine.Object.FindFirstObjectByType<GameManager>();
	}

}
