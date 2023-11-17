using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class profilePanel : MonoBehaviour
{
	public Button closeBtn;
	void Start()
	{
		closeBtn.onClick.AddListener(closeThis);
	}

	private void closeThis() {
		gameObject.SetActive(false);
	}

	void Update()
	{
		
	}
}
