using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
	[SerializeField] private float walkSpeed = 0f;
	[SerializeField] private float jumpStrength = 0f;
	[SerializeField] private float jumpDelay = 0f;
	[SerializeField] private ParticleSystem particles;
	public Animator playerAnim;
	internal bool isStaying;
	private float lastJump;
	public static bool isWalking;
	public static bool canMove = true;
	private bool canJump = true;
	private bool flip = true;
	Rigidbody2D rb;
//Resto del código...

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		lastJump = -jumpDelay;
	}

	void FixedUpdate()
	{
		if (canMove)
		{
			if (Input.GetKey("d") || Input.GetKey("right")) {
				rb.velocity = new Vector2 (walkSpeed, rb.velocity.y);
				playerAnim.SetBool("isWalking", true);
				isWalking = true;
			}
			else if (Input.GetKey("a") || Input.GetKey("left"))
			{
				rb.velocity = new Vector2 (-walkSpeed, rb.velocity.y);
				playerAnim.SetBool("isWalking", true);
				isWalking = true;
			}
			else
			{
				rb.velocity = new Vector2 (0, rb.velocity.y);
				playerAnim.SetBool("isWalking", false);
				isWalking = false;
			}

			if ((Input.GetKey("a") || Input.GetKey("left")) && (Input.GetKey("d") || Input.GetKey("right")))
			{
				rb.velocity = new Vector2 (0, rb.velocity.y);
				playerAnim.SetBool("isWalking", false);
				isWalking = false;
			}

			if (rb.velocity.x > 0 && isWalking && !flip) {
				Flip();
			}
			else if (rb.velocity.x < 0 && isWalking && flip) {
				Flip();
			}

			if ((Input.GetKey("space") || Input.GetKey("w") || Input.GetKey("up")) && groundDetection.isOverGround && canJump && Time.time - lastJump >= jumpDelay)
			{
				lastJump = Time.time;
				rb.velocity = new Vector2 (rb.velocity.x, jumpStrength);
				canJump = false;
			}

			if (!Input.GetKey("space") && !Input.GetKey("w") && !Input.GetKey("up"))
			{
				canJump = true;
			}
		} else {
			playerAnim.SetBool("isWalking", false);
			isWalking = false;
		}

		if (rb.velocity.y < 0 && !groundDetection.isOverGround)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpStrength / (jumpStrength * jumpStrength)) * Time.deltaTime;
			playerAnim.SetBool("isFalling", true);
			playerAnim.SetBool("isJumping", false);
		}

		if (rb.velocity.y > 0 && (!Input.GetKey("space") && !Input.GetKey("w") && !Input.GetKey("up"))) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * jumpStrength * Time.deltaTime;
		}

		if (rb.velocity.y > 0) {
			playerAnim.SetBool("isJumping", true);
			playerAnim.SetBool("isFalling", false);
		}

		if (groundDetection.isOverGround)
		{
			playerAnim.SetBool("isJumping", false);
			playerAnim.SetBool("isFalling", false);
		}
	}

    private void Flip()
	{
		flip = !flip;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}