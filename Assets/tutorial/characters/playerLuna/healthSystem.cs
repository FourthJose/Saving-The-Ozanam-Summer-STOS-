using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class healthSystem : MonoBehaviour
{
	public int maxHealth;
	public TMP_Text barText;
	public Slider bar;
	public barManagement barMg;
	private int health;
	void Start()
	{
		barText.text = gameObject.name;
		barMg = bar.GetComponent<barManagement>();
		health = maxHealth;
		barMg.setMaxHP(maxHealth);
	}

	public void GetHit(int dmg)
	{
		health -= dmg;
		barMg.setHealth(health);
		if (health <= 0)
		{
			Die();
		}
	}

	public void Die()
	{

	}
}
