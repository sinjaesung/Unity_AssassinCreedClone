using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMeleeAttack : MonoBehaviour
{
    public float Timer = 0f;
    public int SingleMeleeVal;
    public Animator anim;
    public PlayerScript playerScript;

    public Transform attackArea;
    public float giveDamage = 10f;
    public float attackRadius;
    public LayerMask knightLayer;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            Timer += Time.deltaTime;
        }
        else
        {
            Debug.Log("SingleMeleeAttack Mode On:마우스왼쪽클릭down시마다 Timer=0되며 대전모드On");
            anim.SetBool("SingleHandAttackActive", true);
            Timer = 0f;
        }

        if (Timer > 5f)
        {
            Debug.Log("FistSingleMeleeAttack Mode Off, 마우스를 뗀 이후로 5초이상지난 시점에 대전모드Off");
            anim.SetBool("SingleHandAttackActive", false);
        }

        SingleMeleeModes();
    }

    void SingleMeleeModes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SingleMeleeVal = Random.Range(1, 7);

            if (SingleMeleeVal == 1)
            {        
                Attack();
                //Animation
                StartCoroutine(SingleAttack1());
            }

            if (SingleMeleeVal == 2)
            {               
                Attack();
                //Animation
                StartCoroutine(SingleAttack2());
            }

            if (SingleMeleeVal == 3)
            {             
                Attack();
                //Animation
                StartCoroutine(SingleAttack3());
            }

            if (SingleMeleeVal == 4)
            {            
                Attack();
                //Animation
                StartCoroutine(SingleAttack4());
            }

            if (SingleMeleeVal == 5)
            {            
                Attack();
                //Animation
                StartCoroutine(SingleAttack5());
            }
        }
    }

    void Attack()
    {
        Collider[] hitKnight = Physics.OverlapSphere(attackArea.position, attackRadius, knightLayer);

        foreach (Collider knight in hitKnight)
        {
            Debug.Log("SingleMeleeAttack [[Hitinfo]]:" + knight.transform.name);

            KnightAI knightAI = knight.GetComponent<KnightAI>();
            KnightAI2 knightAI2 = knight.GetComponent<KnightAI2>();

            if (knightAI != null)
            {
                knightAI.TakeDamage(giveDamage);
            }
            if (knightAI2 != null)
            {
                knightAI2.TakeDamage(giveDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;

        Gizmos.DrawWireSphere(attackArea.position, attackRadius);
    }

    IEnumerator SingleAttack1()
    {
        anim.SetBool("SingleAttack1", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack1", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack2()
    {
        anim.SetBool("SingleAttack2", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack2", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack3()
    {
        anim.SetBool("SingleAttack3", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack3", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack4()
    {
        anim.SetBool("SingleAttack4", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack4", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack5()
    {
        anim.SetBool("SingleAttack5", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack5", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
}
