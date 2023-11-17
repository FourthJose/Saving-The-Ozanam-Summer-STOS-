using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeSystem : MonoBehaviour
{
	[SerializeField] private Transform hitController;
	[SerializeField] private float hitRadius;
	[SerializeField] private float hitDamage;
	[SerializeField] private ParticleSystem particles;
	[SerializeField] private GameObject hitSprite;

	private int hitNumber = 0;
	private bool canAttackAgain = true;
	private bool keyDropped = true;
	internal bool canEnableHit = true;
	Rigidbody2D rb;
	public Animator playerAnim;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		hitSprite.SetActive(false);
	}

	void Update()
	{
		if (playerMovement.canMove)
		{
			if (Input.GetKey("t") && canAttackAgain && keyDropped && groundDetection.isOverGround)
			{
				if (playerMovement.isWalking)
				{
					StartCoroutine(HitDash());
				}
				Hit();
				hitNumber += 1;
				canAttackAgain = false;
				keyDropped = false;
				Invoke("Disable", 0.5f);
			}
		}
	}

	void FixedUpdate()
	{
		if (!Input.GetKey("t"))
		{
			keyDropped = true;
		}
	}

	private void Hit()
	{
		if (hitNumber % 2 == 0)
		{
			playerAnim.SetBool("isAttacking2", true);
			Invoke("AnimDisable", 0.4f);
		}
		else
		{
			playerAnim.SetBool("isAttacking1", true);
			Invoke("AnimDisable", 0.4f);
		}

		Collider2D[] inTouch = Physics2D.OverlapCircleAll(hitController.position, hitRadius);
		foreach (Collider2D collider in inTouch)
		{
			if (collider.CompareTag("Enemy"))
			{
				StartCoroutine(HitSprite(collider));
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(hitController.position, hitRadius);
	}

	private void Disable()
	{
		canAttackAgain = true;
	}

	private IEnumerator HitSprite(Collider2D collider)
	{
		if (canEnableHit)
		{
			canEnableHit = false;
			yield return new WaitForSeconds(0.15f);
			collider.GetComponent<healthControl>().getHit(hitDamage);
			hitSprite.SetActive(true);
			yield return new WaitForSeconds(0.42f);
			hitSprite.SetActive(false);
		}
	}

	private void AnimDisable()
	{
		playerAnim.SetBool("isAttacking1", false);
		playerAnim.SetBool("isAttacking2", false);
	}

	private IEnumerator HitDash()
	{
		playerMovement.canMove = false;
		rb.velocity = new Vector2(14.5f * transform.localScale.x, 0);
		particles.Play();

		yield return new WaitForSeconds(0.2f);

		particles.Stop();
		playerMovement.canMove = true;
	}
}
