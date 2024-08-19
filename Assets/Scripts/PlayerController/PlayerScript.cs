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

    [Header("Player Animator")]
    public Animator animator;

    [Header("Player Collision & Gravity")]
    public CharacterController Cc;
    public float surfaceCheckRadius = 0.1f;
    public Vector3 surfaceCheckOffset;
    public LayerMask surfaceLayer;
    bool onSurface;
    public bool playerOnLedge { get; set; }
    public LedgeInfo LedgeInfo { get; set; }
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;
    [SerializeField] Vector3 requiredMoveDir;
    Vector3 velocity;

    private void Awake()
    {
        presentHealth = playerHealth;
        presentEnergy = playerEnergy;
        healthbar.GiveFullHealth(presentHealth);
        energybar.GiveFullEnergy(presentEnergy);
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

            Debug.Log("Ledge¿¡ ÀÖ¾úÀ»¶© velocity.magnitude:" + velocity.magnitude);
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
