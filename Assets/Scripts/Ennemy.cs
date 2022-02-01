using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Entity
{
    public float seeingDistance;
    public float patrolIdleTime;

    [Header("Work variables")]
    public float waitCounter = 0;
    public bool patrolWaiting;
    public bool isBoss;
    Vector3 patrolTarget;
    public EnnemyMovementState currentMovementState;
    public override void UpdateBehaviour()
    {
        UpdateState();
        base.UpdateBehaviour();
    }

    

    void UpdateState(){
        float playerDistance = Vector3.Distance(transform.position, GM.I.player.transform.position);
        bool seePlayer = playerDistance < seeingDistance;
        Bounds attackBound = AttackBounds();
        bool canAttack = attackBound.Contains(GM.I.player.transform.position);
        if(GM.I.player.currentState == EntityState.Dead){
            TryPatrol();
        }
        else if(canAttack){
            TryAttack();
        }
        else if (seePlayer){
            TryChase();
        }
        else{
            TryPatrol();
        }

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
            }else{
                waitCounter -= Time.deltaTime;
                if(waitCounter <= 0){
                    patrolWaiting = false;
                    currentMovementState = EnnemyMovementState.Idle;
                }
            }
        }
        Vector3 dir = (patrolTarget - transform.position).normalized;
        //Bouger vers point
        if(patrolWaiting){
            dir = Vector3.zero;
        }
        currentInput = new EntityInput(dir, ButtonPress.None);
    }

    private void TryChase()
    {
        if(currentState == EntityState.Attacking || currentState == EntityState.Dead 
        || currentState == EntityState.Taunting || currentState == EntityState.Hurt){
            return;
        }
        currentMovementState = EnnemyMovementState.Chasing;
        
        Vector3 targetPoint = GM.I.player.transform.position;
        float sens = Mathf.Sign(targetPoint.x - transform.position.x);
        targetPoint += Vector3.left * sens * 0.2f;
        anim.lockedFlip = sens < 0 ? LockedFlip.Left : LockedFlip.Right;

        // Si pas de point ou point visé atteint?
        Vector3 dir = (targetPoint - transform.position).normalized;
        currentInput = new EntityInput(dir, ButtonPress.None);
    }

    private void TryAttack()
    {
        if(currentState == EntityState.Attacking || currentState == EntityState.Dead 
        || currentState == EntityState.Hurt || currentState == EntityState.Taunting){
            return;
        }
        currentMovementState = EnnemyMovementState.Idle;
        currentInput = new EntityInput(Vector3.zero, ButtonPress.Taunt);
        patrolWaiting = false;
    }

    
}

public enum EnnemyMovementState{
    Idle,
    Patrolling,
    Chasing,
}
