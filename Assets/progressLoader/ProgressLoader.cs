using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressLoader : MonoBehaviour
{
	public Button goBackButton;
	public Button continueNotLoad;
	public Button progress1;
	public Button progress2;
	public Button progress3;

	void Start()
	{
		goBackButton.onClick.AddListener(returnToMenu);
		continueNotLoad.onClick.AddListener(beginGame);
	}

	private void beginGame() {
		SceneManager.LoadScene("tutorial");
	}

	private void returnToMenu() {
		SceneManager.LoadScene(0);
	}
}