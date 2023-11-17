using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
	public Button jugarButton;
	public Button visitanosButton;
	public Button salirButton;
	public Button perfilButton;
	public Button loginButton;
	public GameObject panelInicioSesion;
	public GameObject panelPerfil;
	public Animator logoAnimator;
	public TMP_Text profileText;
	public TMP_Text profilePanelText;
	public TMP_Text respectiveAlert;

	private bool SesionIniciada;

	private void Start()
	{
		logoAnimator.Play("init");

		jugarButton.onClick.AddListener(OnJugarButtonClick);
		visitanosButton.onClick.AddListener(OnVisitanosButtonClick);
		salirButton.onClick.AddListener(OnSalirButtonClick);
		perfilButton.onClick.AddListener(OnPerfilButtonClick);
		loginButton.onClick.AddListener(OnLoginButtonClick);

		panelInicioSesion.gameObject.SetActive(false);
		perfilButton.gameObject.SetActive(false);
		panelPerfil.gameObject.SetActive(false);
		profileText.text = ManageDB.sessionUser;
	}

	void OnJugarButtonClick()
	{
		if (SesionIniciada)
		{
			SceneManager.LoadScene("progressLoader");
		}
		else
		{
			panelInicioSesion.gameObject.SetActive(true);
			respectiveAlert.text = "Inicia sesión antes de jugar";
		}
	}

	void OnVisitanosButtonClick()
	{
		Application.OpenURL("http://localhost/STOS");
	}

	void OnSalirButtonClick()
	{
		Application.Quit();
	}

	void OnPerfilButtonClick()
	{
		panelPerfil.gameObject.SetActive(true);
	}

	void OnLoginButtonClick() {
		panelInicioSesion.gameObject.SetActive(true);
		respectiveAlert.text = "Iniciar sesión";
	}

	private void FixedUpdate() {
		SesionIniciada = ManageDB.logged;
		perfilButton.gameObject.SetActive(SesionIniciada);
		loginButton.gameObject.SetActive(!SesionIniciada);
	}
}