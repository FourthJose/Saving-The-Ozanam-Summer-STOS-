using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] private float detectionRadius;
    [SerializeField] private float hitRadius;
    [SerializeField] private Transform detectionController;
    [SerializeField] private float runSpeed;
    [SerializeField] private int damage;

    private Animator enemyAnim;
    private Rigidbody2D enemyRb;
    private float dir;
    private bool canPush = false;
    internal bool isAttacked = false;
    internal bool canHit = true;
    internal bool canGetHit = true;

    void Start()
    {
        playerMovement.canMove = true;
        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void DisableAnims()
    {
        enemyAnim.SetBool("isWalking", false);
        enemyAnim.SetBool("isHitting", false);
    }

    void FixedUpdate()
    {
        Collider2D[] inEnter = Physics2D.OverlapCircleAll(detectionController.position, detectionRadius);

        foreach (Collider2D collision in inEnter)
        {
            if (collision.CompareTag("Player"))
            {
                if (isAttacked)
                {
                    DisableAnims();
                    enemyRb.velocity = new Vector2(0, 0);
                    StartCoroutine(GetHit(collision));
                    StartCoroutine(Push());
                    StopCoroutine(ChasePlayer(collision));
                }
                else
                {
                    StartCoroutine(ChasePlayer(collision));
                }
            }
        }
    }

    private IEnumerator ChasePlayer(Collider2D objective)
    {
        yield return new WaitForSeconds(0.4f);

        if (Mathf.Abs(transform.position.x - objective.transform.position.x) < detectionRadius)
        {
            if (transform.position.x < objective.transform.position.x)
            {
                if (Mathf.Abs(transform.position.x - objective.transform.position.x) > hitRadius)
                {
                    enemyAnim.SetBool("isWalking", true);
                    enemyRb.velocity = new Vector2(runSpeed, enemyRb.velocity.y);
                }
                else
                {
                    if (objective.GetComponent<Rigidbody2D>().velocity.x == 0)
                    {
                        enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
                        enemyAnim.SetBool("isWalking", false);
                        yield break;
                    }
                    else
                    {
                        if (objective.transform.localScale == transform.localScale)
                        {
                            enemyAnim.SetBool("isWalking", true);
                            enemyRb.velocity = new Vector2(objective.GetComponent<Rigidbody2D>().velocity.x, enemyRb.velocity.y);
                        }
                        else
                        {
                            enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
                            enemyAnim.SetBool("isWalking", false);
                            yield break;
                        }


                    }
                }
                Vector3 scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
            }
            else if (transform.position.x > objective.transform.position.x)
            {
                if (Mathf.Abs(transform.position.x - objective.transform.position.x) > hitRadius)
                {
                    enemyAnim.SetBool("isWalking", true);
                    enemyRb.velocity = new Vector2(-runSpeed, enemyRb.velocity.y);
                }
                else
                {
                    if (objective.GetComponent<Rigidbody2D>().velocity.x == 0)
                    {
                        enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
                        enemyAnim.SetBool("isWalking", false);
                        yield break;
                    }
                    else
                    {
                        if (objective.transform.localScale == transform.localScale)
                        {
                            enemyAnim.SetBool("isWalking", true);
                            enemyRb.velocity = new Vector2(objective.GetComponent<Rigidbody2D>().velocity.x, enemyRb.velocity.y);
                        }
                        else
                        {
                            enemyRb.velocity = new Vector2(0, enemyRb.velocity.y);
                            enemyAnim.SetBool("isWalking", false);
                            yield break;
                        }


                    }
                }
                Vector3 scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
            }
            else
            {
                DisableAnims();
                enemyRb.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            DisableAnims();
            enemyRb.velocity = new Vector2(0, 0);
        }
    }
    private IEnumerator HitPlayer(Collider2D player)
    {
        /*if (canHit)
		{
			canHit = false;
			playerMovement.canMove = false;
			player.transform.Translate(Vector3.left * transform.localScale.x * 2.0f * Time.deltaTime);
			healthSystem script = player.GetComponent<healthSystem>();
			script.GetHit(damage);
			yield return new WaitForSeconds(1f);
			playerMovement.canMove = true;
			canHit = true;
		}*/
        yield return null;
    }
    private IEnumerator GetHit(Collider2D collision)
    {
        if (canGetHit)
        {
            dir = collision.transform.localScale.x;
            canPush = true;
            canGetHit = false;
            enemyAnim.SetBool("isAttacked", true);
            Vector3 scale = transform.localScale;
            scale.x = -dir;
            transform.localScale = scale;
            yield return new WaitForSeconds(1f);
            isAttacked = false;
            enemyAnim.SetBool("isAttacked", false);
            meleeSystem meleeSystem = collision.GetComponent<meleeSystem>();
            meleeSystem.canEnableHit = true;
            canGetHit = true;
            yield break;
        }
    }

    private IEnumerator Push()
    {
        if (canPush)
        {
            enemyRb.velocity = new Vector2(dir * 20.0f, 0);
            yield return new WaitForSeconds(0.1f);
            enemyRb.velocity = new Vector2(0, 0);
            canPush = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionController.position, hitRadius);
        Gizmos.DrawWireSphere(detectionController.position, detectionRadius);
    }
}
