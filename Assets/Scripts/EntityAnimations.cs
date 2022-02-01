using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimations : AnimationPlayer
{
    public Entity myEntity;
    public bool isMaster;
    public LockedFlip lockedFlip = LockedFlip.None;
    public List<EntityAnimations> linkedAnimations;
    [Header("Animations")]
    public List<Sprite> idleAnimation;
    public List<Sprite> moveAnimation;
    public List<Sprite> tauntAnimation;
    public List<Sprite> attackAnimation;
    public List<Sprite> attackEmpty;

    public List<Sprite> hurtAnimation;
    public List<Sprite> deathAnimation;


    private void Update() {
        if(lockedFlip == LockedFlip.None){
            rend.flipX = !myEntity.isFacingRight;
        }else{
            rend.flipX = lockedFlip == LockedFlip.Left;
        }
    }

    public void SetAnimation(EntityState state, bool forceRefresh){
        if(state == myEntity.currentState && !forceRefresh){return;}
        foreach (EntityAnimations linkedAnim in linkedAnimations)
        {
            linkedAnim.SetAnimation(state, forceRefresh);
        }
        switch (state)
        {
            case EntityState.Idle:
                PlayAnimation(idleAnimation, true);
            break;
            case EntityState.Moving:
                PlayAnimation(moveAnimation, true);
            break;
            case EntityState.Taunting:
                PlayAnimation(tauntAnimation, false, AIAttack);
            break;
            case EntityState.Attacking:
                if(myEntity.isPlayer && ((Player)myEntity).isOffense && ((Player)myEntity).charge == 0){
                    PlayAnimation(attackEmpty, false, ResetState);
                    GM.I.audio.PlaySFX(((Player)myEntity).empty, transform.position, ((Player)myEntity).emptyV);
                }else{
                    PlayAnimation(attackAnimation, false, ResetState);
                    if(myEntity.isPlayer && !((Player)myEntity).isOffense){
                        GM.I.audio.PlaySFX(((Player)myEntity).crowd, transform.position, ((Player)myEntity).crowdV);
                    }else{
                        GM.I.audio.PlaySFX(myEntity.hit, transform.position, myEntity.hitV);
                    }
                }
            break;
            case EntityState.Hurt:
                PlayAnimation(hurtAnimation, false, ResetState);
            break;
            case EntityState.Dead:
                PlayAnimation(deathAnimation, false, DestroyEntity);
                foreach (EntityAnimations linkedAnim in linkedAnimations)
                {
                    linkedAnim.gameObject.SetActive(false);
                }
            break;
            
        }
    }

    private void DestroyEntity()
    {
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine(){
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 10; i++)
        {
            rend.enabled = !rend.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        if(myEntity.isPlayer){
            yield return new WaitForSeconds(1f);
            GM.I.GameOver();
        }
        myEntity.gameObject.SetActive(false);
    }

    void ResetState(){
        if(isMaster)
            myEntity.SetState(EntityState.Idle);    
    }  

    void AIAttack(){
        SetState(EntityState.Attacking);
        if(myEntity.AttackBounds().Intersects(GM.I.player.HitBox)){
            GM.I.player.Hurt((GM.I.player.transform.position - transform.position).normalized, 1);
        }
    } 

    void SetState(EntityState state){
        if(isMaster)
            myEntity.SetState(state);    
    }    
}

public enum LockedFlip{
    None,
    Left,
    Right
}
