using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Health & Energy")]
    private float playerHealth = 8000f;
    public float presentHealth;
    public HealthBar healthbar;
    private float playerEnergy = 100f;
    public float presentEnergy;
    public EnergyBar energybar;
    public GameObject DamageIndicator;

    [Header("Player Movement")]
    public float movementSpeed = 5f;
    public float rotSpeed = 450f;
    public MainCameraController MCC;
    public EnvironmentChecker environmentChecker;
    Quaternion requireRotation;
    bool playerControl = true;

    public bool playerInAction { get; private set; }

    [Header("Player Animator")]
    public Animator animator;

    [Header("Player Collision & Gravity")]
    public CharacterController Cc;
    public float surfaceCheckRadius = 0.1f;
    public Vector3 surfaceCheckOffset;
    public LayerMask surfaceLayer;
    public bool onSurface;
    public bool playerOnLedge { get; set; }
    public bool playerHanging { get; set; }
    public LedgeInfo LedgeInfo { get; set; }
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;
    [SerializeField] Vector3 requiredMoveDir;
    Vector3 velocity;

    public PickupItem[] pickupItems;
    public Inventory inventory;
    private void Awake()
    {
        Debug.Log("프리팹 캐릭터 스폰"+transform.name);
        healthbar = FindObjectOfType<HealthBar>();
        energybar = FindObjectOfType<EnergyBar>();
        DamageIndicator = FindObjectOfType<DamageIndicator>().gameObject;
        DamageIndicator.SetActive(false);
        MCC = FindObjectOfType<MainCameraController>();
        MCC.SetCharacterTarget(transform);
        presentHealth = playerHealth;
        presentEnergy = playerEnergy;
        healthbar.GiveFullHealth(presentHealth);
        energybar.GiveFullEnergy(presentEnergy);
        pickupItems = FindObjectsOfType<PickupItem>();
        for(int e=0; e<pickupItems.Length; e++)
        {
            var item = pickupItems[e];
            item.SetData(transform, inventory);
        }
    }

    private void Update()
    {
        if(presentEnergy <= 0)
        {
            movementSpeed = 2f;

            if(!Input.GetButton("Horizontal") || !Input.GetButton("Vertical"))
            {
                animator.SetFloat("movementValue", 0f);
            }

            if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                animator.SetFloat("movementValue", 0.5f);
                StartCoroutine(setEnergy());
            }
        }

        if(presentEnergy >= 1)
        {
            movementSpeed = 5f;
        }

        if(animator.GetFloat("movementValue") >= 0.9999)
        {
            playerEnergyDecrease(0.02f);
        }

        if (!playerControl)
            return;

        if (playerHanging)
            return;

        velocity = Vector3.zero;
        if (onSurface)
        {
            fallingSpeed = -0.5f;
            velocity = moveDir * movementSpeed;

            playerOnLedge = environmentChecker.CheckLedge(moveDir,out LedgeInfo ledgeInfo);
            if (playerOnLedge)
            {
                LedgeInfo = ledgeInfo;
                PlayerLedgeMovement();
                Debug.Log("player on ledge");
            }

            Debug.Log("Ledge에 있었을땐 velocity.magnitude:" + velocity.magnitude);
            animator.SetFloat("movementValue", velocity.magnitude / movementSpeed, 0.2f, Time.deltaTime);
        }
        else
        {
            fallingSpeed += Physics.gravity.y * Time.deltaTime;

            velocity = transform.forward * movementSpeed / 2;
        }

        velocity.y = fallingSpeed;

        PlayerMovement();
        SurfaceCheck();
        animator.SetBool("onSurface", onSurface);
        Debug.Log("Player on Surface"+ onSurface);
    }
    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;

        requiredMoveDir = MCC.flatRotation * movementInput;

        Cc.Move(velocity * Time.deltaTime);

        if (movementAmount > 0 && moveDir.magnitude > 0.2f)
        {
            requireRotation = Quaternion.LookRotation(moveDir);
        }

        moveDir = requiredMoveDir;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requireRotation, rotSpeed * Time.deltaTime);

    }

    void SurfaceCheck()
    {
        Debug.Log("SurfaceCheck: transformPos,surfaceCheckOffsetPos" + transform.position + "," + transform.TransformPoint(surfaceCheckOffset));
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
    }

    void PlayerLedgeMovement()
    {
        float angle = Vector3.Angle(LedgeInfo.surfaceHit.normal, requiredMoveDir);

        Debug.Log("PlayerLedgeMovement:" + angle);
        if (angle < 90)
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
    }

    public IEnumerator PerformAction(string AnimationName,CompareTargetParameter ctp=null,
        Quaternion RequiredRotation = new Quaternion(), bool LookAtObstacle = false, float ParkourActionDelay = 0f)
    {
        playerInAction = true;

        animator.CrossFadeInFixedTime(AnimationName, 0.2f);
        yield return null;

        var animationState = animator.GetNextAnimatorStateInfo(0);
        if(!animationState.IsName(AnimationName))
            Debug.Log("Animation Name is Incorrect");

        float rotateStartTime = (ctp != null) ? ctp.StartTime : 0f;
        float timerCounter = 0f;

        while(timerCounter < animationState.length)
        {
            timerCounter += Time.deltaTime;

            float normalizedTimerCounter = timerCounter / animationState.length;

            //make player to look towards the obstacle
            if (LookAtObstacle && normalizedTimerCounter > rotateStartTime)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, RequiredRotation, rotSpeed * Time.deltaTime);
            }

            if (ctp != null)
            {
                CompareTarget(ctp);
            }

            if (animator.IsInTransition(0) && timerCounter > 0.5f)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(ParkourActionDelay);

        playerInAction = false;
    }
    void CompareTarget(CompareTargetParameter compareTargetParameter)
    {
        animator.MatchTarget(compareTargetParameter.position, transform.rotation, compareTargetParameter.bodyPart,
            new MatchTargetWeightMask(compareTargetParameter.positionWeight, 0), compareTargetParameter.StartTime, compareTargetParameter.endTime);
    }
    public void SetControl(bool hasControl)
    {
        this.playerControl = hasControl;
        Cc.enabled = hasControl;

        if (!hasControl)
        {
            animator.SetFloat("movementValue", 0f);
            requireRotation = transform.rotation;
        }
    }
    public void EnableCC(bool enabled)
    {
        Cc.enabled = enabled;
    }
    public void ResetRequiredRotation()
    {
        requireRotation = transform.rotation;
    }
   
    public bool HasPlayerControl
    {
         get => playerControl;
         set => playerControl = value;
    }

    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthbar.SetHealth(presentHealth);
        StartCoroutine(showDamage());

        if(presentHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject, 1.0f);
    }

    public void playerEnergyDecrease(float energyDecrease)
    {
        presentEnergy -= energyDecrease;
        energybar.SetEnergy(presentEnergy);
    }

    IEnumerator setEnergy()
    {
        presentEnergy = 0f;
        yield return new WaitForSeconds(5f);
        energybar.GiveFullEnergy(presentEnergy);
        presentEnergy = 100f;
    }

    IEnumerator showDamage()
    {
        DamageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        DamageIndicator.SetActive(false);
    }
}

public class CompareTargetParameter
{
    public Vector3 position;
    public AvatarTarget bodyPart;
    public Vector3 positionWeight;
    public float StartTime;
    public float endTime;
}
