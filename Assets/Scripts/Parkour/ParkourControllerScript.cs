using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourControllerScript : MonoBehaviour
{
    public EnvironmentChecker environmentChecker;
    bool playerInAction;
    public Animator animator;
    public PlayerScript playerScript;
    [SerializeField] NewParkourAction jumpDownParkourAction;
    float autoJumpHeightLimit = 2f;

    [Header("Parkour Action Area")]
    public List<NewParkourAction> newParkourActions;

    private void Update()
    {

        var hitData = environmentChecker.CheckObstacle();
        if (Input.GetButton("Jump") && !playerScript.playerInAction && !playerScript.playerHanging)
        {
            if (hitData.hitFound)
            {
                foreach(var action in newParkourActions)
                {
                    if (action.CheckIfAvailable(hitData,transform))
                    {
                        StartCoroutine(PerformParkourAction(action));
                        break;
                    }
                }
            }
        }

        if(playerScript.playerOnLedge && !playerInAction && !hitData.hitFound )
        {
           // bool canJump = true;
            //if (playerScript.LedgeInfo.height > autoJumpHeightLimit && !Input.GetButton("Jump"))
                //canJump = false;

            Debug.Log("PlayerScript LedgeInfo angle:" + playerScript.LedgeInfo.angle);
            if (playerScript.LedgeInfo.angle <= 90 && Input.GetButton("Jump"))
            {
                Debug.Log("jumpDownParkourAction가능한 경우" + playerScript.LedgeInfo.angle);
                playerScript.playerOnLedge = false;
                StartCoroutine(PerformParkourAction(jumpDownParkourAction));//JumpDown->exitTime시 FallingIdle -> OnSurface시에 landing
            }
        }
    }
    IEnumerator PerformParkourAction(NewParkourAction action)
    {
        playerScript.SetControl(false);

        CompareTargetParameter compareTargetParameter = null;
        if (action.AllowTargetMatching)
        {
            compareTargetParameter = new CompareTargetParameter()
            {
                position = action.ComparePosition,
                bodyPart = action.CompareBodyPart,
                positionWeight = action.ComparePositionWeight,
                StartTime = action.CompareStartTime,
                endTime = action.CompareEndTime
            };
        }

        Debug.Log("Perform ParkourActionName>>" + action.AnimationName);
        yield return playerScript.PerformAction(action.AnimationName, compareTargetParameter, action.RequiredRotation,
            action.LookAtObstacle, action.ParkourActionDelay);
        playerScript.SetControl(true);
    }
    /*IEnumerator PerformParkourAction(NewParkourAction action)
    {
        Debug.Log("PerformParkourAction 액션 시행시에는 모든 대전모드형상태해제" + action.AnimationName);
        animator.SetBool("FistFightActive", false);
        animator.SetBool("SingleHandAttackActive", false);
        animator.SetBool("RifleActive", false);
        animator.SetBool("BazookaActive", false);

        playerInAction = true;
        playerScript.SetControl(false);

        animator.CrossFade(action.AnimationName, 0.2f);
        yield return null;

        Debug.Log("PerformParkourAction 액션 애니메이션 실행"+ action.AnimationName);
       

        var animationState = animator.GetNextAnimatorStateInfo(0);
        if (!animationState.IsName(action.AnimationName))
            Debug.Log("Animations Name is Incorrect");

        float timerCounter = 0f;

        while(timerCounter <= animationState.length)
        {
            timerCounter += Time.deltaTime;

            //Make player to look towards the obstacle
            if (action.LookAtObstacle)
            {
                Debug.Log("PerformParkourAction 액션 동작중 파쿠르대상물체방향으로의 캐릭터 방향회전"+ action.RequiredRotation);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.RequiredRotation, playerScript.rotSpeed * Time.deltaTime);
            }

            if (action.AllowTargetMatching)
            {
                CompareTarget(action);
            }
            Debug.Log("animator.IsInTransition(0) , timerCounter:" + animator.IsInTransition(0) + "," + timerCounter);
            if (animator.IsInTransition(0) && timerCounter > 0.5f)
            {
                Debug.Log("animator.IsInTransition(0) True && timerCounter > 0.5f:" + animator.IsInTransition(0) + "," + timerCounter);
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(action.ParkourActionDelay);

        playerScript.SetControl(true);
        playerInAction = false;
    }

    void CompareTarget(NewParkourAction action)
    {
        animator.MatchTarget(action.ComparePosition, transform.rotation, action.CompareBodyPart, new MatchTargetWeightMask(action.ComparePositionWeight,0), action.CompareStartTime, action.CompareEndTime);
    }*/

}
