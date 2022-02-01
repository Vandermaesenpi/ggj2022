using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : Entity
{
    public float patrolIdleTime;
    public bool active = false;
    public Boss boss;

    [Header("Work variables")]
    public float waitCounter = 0;
    public bool patrolWaiting;
    Vector3 patrolTarget;

    public EnnemyMovementState currentMovementState;
    public override void UpdateBehaviour()
    {
        UpdateState();
        base.UpdateBehaviour();
    }

    public override Bounds HitBox{
        get
        {
            return new Bounds(Vector3.up * 100f, new Vector3(hitboxSize.x, hitboxSize.y, 1));
        }
    } 

    public override Bounds AttackBounds()
    {
        Vector3 boundsCenter = transform.position;
        return new Bounds(boundsCenter, new Vector3(attackWindow.x, attackWindow.y, 1f));
    }

    void UpdateState(){
        float playerDistance = Vector3.Distance(transform.position, GM.I.player.transform.position);
        currentInput = new EntityInput(Vector2.zero, ButtonPress.None);
        TryPatrol();
    }

    private void TryPatrol()
    {
        if(currentState == EntityState.Attacking || currentState == EntityState.Dead || currentState == EntityState.Taunting 
        || currentState == EntityState.Hurt){
            return;
        }

        anim.lockedFlip = LockedFlip.None;
        
        bool changedState = currentMovementState != EnnemyMovementState.Patrolling; 
        currentMovementState = EnnemyMovementState.Patrolling;
        // Si pas de point ou point visé atteint?
        if (changedState){
            patrolTarget = GM.RandomPointInBounds(GM.I.cam.CamBounds()); 
        }if(Vector3.Distance(patrolTarget, transform.position) < 0.01f){
            if(!patrolWaiting){
                waitCounter = patrolIdleTime;
                patrolWaiting = true;
                TryAttack();
                return;
            }else{
                waitCounter -= Time.deltaTime;
                if(waitCounter <= 0){
                    patrolWaiting = false;
                    currentMovementState = EnnemyMovementState.Idle;
                }
            }
        }
        Vector3 target = active? patrolTarget : GM.I.cam.transform.position + Vector3.left * 4f;
        Vector3 dir = (target - transform.position).normalized;
        //Bouger vers point
        if(patrolWaiting){
            dir = Vector3.zero;
        }
        currentInput = new EntityInput(dir, ButtonPress.None);
    }

    private void TryAttack()
    {
        if(currentState == EntityState.Attacking || currentState == EntityState.Dead 
        || currentState == EntityState.Hurt || currentState == EntityState.Taunting){
            return;
        }
        GM.I.audio.PlaySFX(hurt, transform.position);

        Debug.Log("ATTACK !!!!");
        currentMovementState = EnnemyMovementState.Idle;
        currentInput = new EntityInput(Vector3.zero, ButtonPress.Taunt);
        patrolWaiting = false;
    }

    
}
