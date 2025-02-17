using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAI2 : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float runningSpeed;
    public float CurrentmovingSpeed;
    public float maxHealth = 120f;
    public float currentHealth;

    [Header("Knight AI")]
    public GameObject playerBody;
    public LayerMask playerLayer;
    public float visionRadius;
    public float attackRadius;
    public bool playerInvisionRadius;
    public bool playerInattackRadius;

    [Header("Knight Attack Var")]
    public int SingleMeleeVal;
    public Transform attackArea;
    public float giveDamage;
    public float attackingRadius;
    bool previouslyAttack;
    public float timebtwAttack;
    public Animator anim;

    public bool isDied = false;

    private void Start()
    {
        CurrentmovingSpeed = movingSpeed;
        currentHealth = maxHealth;
        playerBody = GameObject.Find("Player");
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInattackRadius = Physics.CheckSphere(transform.position, attackRadius, playerLayer);

        if (!playerInvisionRadius && !playerInattackRadius)
        {
            Idle();
        }

        if (playerInvisionRadius && !playerInattackRadius)
        {
            anim.SetBool("Idle", false);
            ChasePlayer();
        }

        if (playerInvisionRadius && playerInattackRadius)
        {
            anim.SetBool("Idle", true);
            SingleMeleeModes();
        }
    }

    public void Idle()
    {
        anim.SetBool("Run", false);
    }

    void ChasePlayer()
    {
        CurrentmovingSpeed = runningSpeed;
        transform.position += transform.forward * CurrentmovingSpeed * Time.deltaTime;
        transform.LookAt(playerBody.transform);

        anim.SetBool("Attack", false);
        anim.SetBool("Run", true);
    }

    void SingleMeleeModes()
    {
        if (!previouslyAttack)
        {
            Debug.Log("FistFightModes는 마우스왼쪽클릭한 경우에만 대전모드로되며,관련 애니메이션1~5 랜덤진행");
            SingleMeleeVal = Random.Range(1, 5);

            if (SingleMeleeVal == 1)
            {
                Attack();
                //Animation
                StartCoroutine(Attack1());
            }

            if (SingleMeleeVal == 2)
            {
                Attack();
                //Animation
                StartCoroutine(Attack2());
            }

            if (SingleMeleeVal == 3)
            {
                Attack();
                //Animation
                StartCoroutine(Attack3());
            }

            if (SingleMeleeVal == 4)
            {
                Attack();
                //Animation
                StartCoroutine(Attack4());
            }
        }
    }
    void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(attackArea.position, attackingRadius, playerLayer);

        foreach (Collider player in hitPlayer)
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();

            if (playerScript != null)
            {
                Debug.Log("Hitting.Player");
                playerScript.playerHitDamage(giveDamage);
            }
        }

        previouslyAttack = true;
        Invoke(nameof(ActiveAttack), timebtwAttack);
    }
    private void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;

        Gizmos.DrawWireSphere(attackArea.position, attackingRadius);
    }

    private void ActiveAttack()
    {
        previouslyAttack = false;
    }

    IEnumerator Attack1()
    {
        anim.SetBool("Attack1", true);
        movingSpeed = 0f;
        runningSpeed = 0f;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Attack1", false);
        movingSpeed = 1f;
        runningSpeed = 3f;
    }
    IEnumerator Attack2()
    {
        anim.SetBool("Attack2", true);
        movingSpeed = 0f;
        runningSpeed = 0f;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Attack2", false);
        movingSpeed = 1f;
        runningSpeed = 3f;
    }
    IEnumerator Attack3()
    {
        anim.SetBool("Attack3", true);
        movingSpeed = 0f;
        runningSpeed = 0f;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Attack3", false);
        movingSpeed = 1f;
        runningSpeed = 3f;
    }
    IEnumerator Attack4()
    {
        anim.SetBool("Attack4", true);
        movingSpeed = 0f;
        runningSpeed = 0f;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Attack4", false);
        movingSpeed = 1f;
        runningSpeed = 3f;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        //anim.SetTrigger("GetHit");

        if (currentHealth <= 0f)
        {
            if (!isDied)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("isDead Anim 실행>>");
        isDied = true;
        anim.SetBool("isDead", true);
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 6f);
    }
}
