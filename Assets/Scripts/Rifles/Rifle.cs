using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Transform shootingArea;
    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public Animator animator;
    public bool isMoving;
    public PlayerScript playerScript;

    [Header("Rifle Ammunition and reloading")]
    private int maximumAmmunition = 1;
    public int presentAmmunition;
    public int mag;
    public float reloadingTime;
    private bool setReloading;
    public GameObject crosshair;

    public float Timer = 0f;

    private void Awake()
    {
        crosshair = FindObjectOfType<Crosshair>().gameObject;
    }
    private void Start()
    {
        presentAmmunition = maximumAmmunition;
    }

    private void Update()
    {
        if(animator.GetFloat("movementValue") > 0.001f)
        {
            isMoving = true;
        }
        else if(animator.GetFloat("mvovementValue") < 0.0999999f)
        {
            isMoving = false;
        }

        if (setReloading)
            return;

        if(presentAmmunition <= 0 && mag > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0) && isMoving == false)
        {
            animator.SetBool("RifleActive", true);
            animator.SetBool("Shooting", true);
            Shoot();
            Debug.Log("RifleActive Mode On:마우스왼쪽클릭down시마다 Timer=0되며 대전모드On");
            Timer = 0f;
        }
        else if (!Input.GetMouseButtonDown(0))
        {
            Timer += Time.deltaTime;
            animator.SetBool("Shooting", false);
        }

        if (Input.GetMouseButton(1))
        {
            //animator.SetBool("RifleAim", true);
            //crosshair.SetActive(true);
        }
        else if (!Input.GetMouseButton(1))
        {
            //animator.SetBool("RifleAim", false);
            //crosshair.SetActive(false);
        }

        if (Timer > 5f)
        {
            Debug.Log("RifleActive Mode Off, 마우스를 뗀 이후로 5초이상지난 시점에 대전모드Off");
            animator.SetBool("RifleActive", false);
        }

    }

    void Shoot()
    {
        if(mag <= 0)
        {
            //show out UI
            return;
        }

        presentAmmunition--;

        if(presentAmmunition == 0)
        {
            mag--;
        }
        RaycastHit hitInfo;

        if(Physics.Raycast(shootingArea.position,shootingArea.forward,out hitInfo, shootingRange)){
            Debug.Log("[Rifle] [[Hitinfo]]:" + hitInfo.transform.name);

            KnightAI knightAI = hitInfo.transform.GetComponent<KnightAI>();
            KnightAI2 knightAI2 = hitInfo.transform.GetComponent<KnightAI2>();

            if (knightAI != null)
            {
                Debug.Log("Rifle knight1 Damage");
                knightAI.TakeDamage(giveDamage);
            }
            if (knightAI2 != null)
            {
                Debug.Log("Rifle knight2 Damage");
                knightAI2.TakeDamage(giveDamage);
            }
        }
    }

    IEnumerator Reload()
    {
        setReloading = true;
        animator.SetFloat("movementValue", 0f);
        //playerScript.movementSpeed = 0f;
        animator.SetBool("ReloadRifle", true);
        //reloading anim
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("ReloadRifle", false);
        presentAmmunition = maximumAmmunition;
        setReloading = false;
        animator.SetFloat("movementValue",0f);
        //playerScript.movementSpeed = 5f;
    }
}
