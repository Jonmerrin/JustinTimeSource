using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeController : MonoBehaviour
{
    public Animator animator;
    public GameObject healthIndicator;
    public Vector2 gridPos;
    public int health;
    public int startingHealth;
    public int attackRange;
    public GridCharacterController gridController;
    public int deathWaitTime = 2;
    public int damageDealt = 1;

    // Start is called before the first frame update
    void Start()
    {
        health = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Make sure the enemy is always facing the correct way
        if(gridPos.y < 3)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            healthIndicator.GetComponent<Animator>().SetBool("isFlipped", false);
        }
        else
        {
            transform.localScale = new Vector3(-1*Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            healthIndicator.GetComponent<Animator>().SetBool("isFlipped", true);
        }
        //Make animator display attack animation or not depending on distance to player
        animator.SetBool("isAttacking", gridPos.x < attackRange);
        //Update health and check for death
        healthIndicator.GetComponentInChildren<TMPro.TextMeshPro>().text = health.ToString();
    }

    private IEnumerator Death()
    {
        health = 0;
        healthIndicator.SetActive(false);
        animator.SetTrigger("Death");
        EnemyManager.Instance.RemoveEnemy(gridController);
        yield return new WaitForSeconds(deathWaitTime);
        GameObject.Destroy(this.gameObject);
    }

    public void TakeTurn()
    {
        if(gridPos.x < attackRange)
        {
            Attack();
        }
        else
        {
            //Move on grid, is this going to be a function of the grid itself?
            //How to check grid for traffic?
            if(GridManager.Instance.GetInfoFromCoords(Mathf.FloorToInt(gridPos.x) - 1, Mathf.FloorToInt(gridPos.y)).gridCharacter == null)
            {
                gridController.Move(-1, 0);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
    }

    private void Attack()
    {
        GameManager.Instance.TakeDamage(damageDealt);
    }
}
