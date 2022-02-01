using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int hp;
    public float walkSpeed = 1f;
    public float knockBackSpeed;
    [HideInInspector] public Vector3 knockBackDirection;
    public EntityAnimations anim;
    public EntityMovement movement;
    public EntityInput currentInput;

    public Vector2 hitboxSize;
    public Vector2 attackWindow;

    public EntityState currentState;
    public bool isFacingRight;
    public bool isPlayer;

    [Header("Audio")]
    public AudioClip hit;
    public AudioClip hurt, death;
    public float hitV, hurtV, deathV;

    public virtual Bounds HitBox => new Bounds(transform.position, new Vector3(hitboxSize.x, hitboxSize.y, 1));
    public virtual Bounds AttackBounds(){
        Vector3 boundsCenter = transform.position + ((isFacingRight? Vector3.right : Vector3.left) * attackWindow.x /2f);
        return new Bounds(boundsCenter, new Vector3(attackWindow.x, attackWindow.y, 1f));
    }

    internal void SetState(EntityState state, bool forceRefresh = false)
    {
        anim.SetAnimation(state, forceRefresh);
        currentState = state;
    }
    
    private void Update() {
        UpdateBehaviour();
    }
    public virtual void UpdateBehaviour(){
        UpdateStateFromInput(currentInput);
        if(currentState == EntityState.Moving){
            isFacingRight = currentInput.direction.x > 0;
            movement.Move(currentInput.direction, walkSpeed);
        }else if(currentState == EntityState.Hurt){
            movement.Move(knockBackDirection, knockBackSpeed);
        }
    }

    public void UpdateStateFromInput(EntityInput currentInput)
    {
        switch (currentState)
        {
            case EntityState.Idle:
                if(currentInput.pressedButton == ButtonPress.Attack){
                    SetState(EntityState.Attacking);
                }else if (currentInput.pressedButton == ButtonPress.ChangeState){
                    SetState(EntityState.Idle, true);
                }else if (currentInput.pressedButton == ButtonPress.Taunt){
                    Debug.Log("TAUNT IDLE");
                    SetState(EntityState.Taunting, true);
                }
                else if(currentInput.direction.sqrMagnitude > 0){
                    SetState(EntityState.Moving);
                }
            break;

            case EntityState.Moving:
                
                if(currentInput.pressedButton == ButtonPress.Attack){
                    SetState(EntityState.Attacking);
                }else if (currentInput.pressedButton == ButtonPress.ChangeState){
                    SetState(EntityState.Moving, true);
                }else if (currentInput.pressedButton == ButtonPress.Taunt){
                    Debug.Log("TAUNT MOVE");
                    SetState(EntityState.Taunting, true);
                }
                else if(currentInput.direction.sqrMagnitude == 0){
                    SetState(EntityState.Idle);
                }
            break;
        }
    }

    public virtual void Hurt(Vector3 knockBackDir, int damage){
        if(currentState == EntityState.Dead){return;}
        hp -= damage;
        SetState(EntityState.Hurt);
        knockBackDirection = knockBackDir;
        if(hp <= 0){
            SetState(EntityState.Dead);
            GM.I.audio.PlaySFX(death, transform.position);
        }else{
            GM.I.audio.PlaySFX(hurt, transform.position);
        }
    }

    private void OnDrawGizmos() {
        Bounds attackBound = AttackBounds();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackBound.center, attackBound.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(HitBox.center, HitBox.size);
    }
}

public enum EntityState{
    Idle,
    Moving,
    Taunting,
    Attacking,
    Swapping,
    Hurt,
    Dead
}
