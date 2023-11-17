using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;


public class LoginDB : MonoBehaviour
{
	public TMP_InputField username;
	public TMP_InputField password;
	public Button submit;
	public Button showPassword;
	public Button close;
	public TMP_Text profileText;
	public TMP_Text profilePanelText;

	public Sprite shownImg;
	public Sprite hiddenImg;

	public void OnClickLogin()
	{
		StartCoroutine(Login());
	}

    public void showPass()
    {
    	if (password.contentType == TMP_InputField.ContentType.Password) {
        	password.contentType = TMP_InputField.ContentType.Standard;
        	showPassword.image.sprite = shownImg;
        } else if (password.contentType == TMP_InputField.ContentType.Standard) {
        	password.contentType = TMP_InputField.ContentType.Password;
        	showPassword.image.sprite = hiddenImg;
        }
        password.ForceLabelUpdate();
    }

	IEnumerator Login()
	{
		WWWForm data = new WWWForm();
		data.AddField("username", username.text);
		data.AddField("password", password.text);
		WWW web = new WWW("http://localhost/STOS/login_unity.php", data);
		yield return web;
		if (web.text[0] == '0') {
			ManageDB.sessionUser = username.text;
			ManageDB.saveState1 = int.Parse(web.text.Split("\t")[1]);
			ManageDB.saveState2 = int.Parse(web.text.Split("\t")[2]);
			ManageDB.saveState3 = int.Parse(web.text.Split("\t")[3]);
			gameObject.SetActive(false);
			profileText.text = ManageDB.sessionUser;
			profilePanelText.text = ManageDB.sessionUser;
			Debug.Log("Inicio de sesión exitoso, bienvenido " + username.text + ". " + "SesionIniciada = " + ManageDB.logged);
		}
		else
		{
			Debug.Log("El inicio de sesión ha fallado. Error #" + web.text);
		}
	}

	public void VerifyData()
	{
		submit.interactable = (username.text.Length > 3 && password.text.Length >= 8);
		showPassword.gameObject.SetActive(password.text.Length >= 1);
		if (password.text.Length == 0) {
			password.contentType = TMP_InputField.ContentType.Password;
			showPassword.image.sprite = hiddenImg;
		}
	}

	private void Start()
	{
		showPassword.onClick.AddListener(showPass);
		close.onClick.AddListener(CloseFrame);
	}

	private void FixedUpdate() {
		VerifyData();
	}

	void CloseFrame() {
		gameObject.SetActive(false);
		showPassword.gameObject.SetActive(false);
		password.text = "";
	}
}
