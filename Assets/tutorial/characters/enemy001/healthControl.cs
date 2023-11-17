using UnityEngine;
using UnityEngine.SceneManagement;

public class healthControl : MonoBehaviour
{
	[SerializeField] private float health;
	[SerializeField] public GameObject enemy;
	private Animator enemyAnim;
    private enemyMovement mov;

    private void Start()
	{
		enemyAnim = GetComponent<Animator>();
		mov = GetComponent<enemyMovement>();
	}

	public void getHit(float damage)
	{
		health -= damage;
		mov.isAttacked = true;

		if (health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		enemyAnim.SetBool("isDead", true);
		enemy.SetActive(false);
		SceneManager.LoadScene(4);
	}
}
