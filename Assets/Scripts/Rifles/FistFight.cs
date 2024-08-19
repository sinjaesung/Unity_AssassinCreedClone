using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class FistFight : MonoBehaviour
{
    public float Timer = 0f;
    public int FistFightVal;
    public Animator anim;
    public PlayerScript playerScript;

    public Transform attackArea;
    public float giveDamage = 10f;
    public float attackRadius;
    public LayerMask knightLayer;
    public Inventory inventory;

    [SerializeField] Transform LeftHandPunch;
    [SerializeField] Transform RightHandPunch;
    [SerializeField] Transform LeftLegKick;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            Timer += Time.deltaTime;
        }
        else
        {
            Debug.Log("Fist Fight Mode On:마우스왼쪽클릭down시마다 Timer=0되며 대전모드On");
            playerScript.movementSpeed = 3f;
            anim.SetBool("FistFightActive", true);
            Timer = 0f;
        }

        if(Timer > 5f)
        {
            Debug.Log("Fist Fight Mode Off, 마우스를 뗀 이후로 5초이상지난 시점에 대전모드Off");
            playerScript.movementSpeed = 5f;
            anim.SetBool("FistFightActive", false);
            inventory.fistFightMode = false;
            Timer = 0f;
            this.gameObject.GetComponent<FistFight>().enabled = false;
        }

        FistFightModes();
    }

    void FistFightModes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("FistFightModes는 마우스왼쪽클릭한 경우에만 대전모드로되며,관련 애니메이션1~5 랜덤진행");
            FistFightVal = Random.Range(1, 7);

            if(FistFightVal == 1)
            {
                //Attack
                attackArea = LeftHandPunch;
                attackRadius = 0.5f;
                Attack();
                //Animation
                StartCoroutine(SingleFist());
            }

            if(FistFightVal == 2)
            {
                //Attack
                attackArea = RightHandPunch;
                attackRadius = 0.6f;
                Attack();
                //Animation
                StartCoroutine(DoubleFist());
            }

            if(FistFightVal == 3)
            {
                //Attack
                attackArea = RightHandPunch;
                attackArea = LeftLegKick;
                attackRadius = 0.7f;
                Attack();
                //Animation
                StartCoroutine(FirstFistKick());
            }

            if (FistFightVal == 4)
            {
                //Attack
                attackArea = LeftLegKick;
                attackRadius = 0.9f;
                Attack();
                //Animation
                StartCoroutine(KickCombo());
            }

            if(FistFightVal == 5)
            {
                //Attack
                attackArea = LeftLegKick;
                attackRadius = 0.9f;
                Attack();
                //Animation
                StartCoroutine(LeftKick());
            }
        }
    }

    void Attack()
    {
        Collider[] hitKnight = Physics.OverlapSphere(attackArea.position, attackRadius, knightLayer);

        foreach(Collider knight in hitKnight)
        {
            Debug.Log("FistFight [[Hitinfo]]:" + knight.transform.name);

            KnightAI knightAI = knight.GetComponent<KnightAI>();
            KnightAI2 knightAI2 = knight.GetComponent<KnightAI2>();

            if(knightAI != null)
            {
                knightAI.TakeDamage(giveDamage);
            }
            if(knightAI2 != null)
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

    IEnumerator SingleFist()
    {
        anim.SetBool("SingleFist", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("SingleFist", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator DoubleFist()
    {
        anim.SetBool("DoubleFist", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("DoubleFist", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator FirstFistKick()
    {
        anim.SetBool("FirstFistKick", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("FirstFistKick", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator KickCombo()
    {
        anim.SetBool("KickCombo", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("KickCombo", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator LeftKick()
    {
        anim.SetBool("LeftKick", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("LeftKick", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
}
