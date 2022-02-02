using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public int charge = 5;
    public EntityAnimations defenseAnim, offenseAnim;
    public ChargeAppearanceManager defenseCharge, offenseCharge;
    public bool isOffense;

    [Header("Player Audio")]
    public AudioClip swap;
    public AudioClip recharge, crowd, empty;
    public float swapV, rechargeV, crowdV, emptyV;

    private void Start()
    {
        GM.I.cam.FocusPlayer();
        anim.SetAnimation(EntityState.Idle, true);
        defenseCharge.Show(false);
        offenseCharge.Show(true);
    }

    public override Bounds AttackBounds()
    {
        if (isOffense)
        {
            Vector3 boundsCenter = transform.position + ((isFacingRight ? Vector3.right : Vector3.left) * attackWindow.x / 2f);
            return new Bounds(boundsCenter, new Vector3(attackWindow.x, attackWindow.y, 1f));
        }
        else
        {
            Vector3 boundsCenter = transform.position;
            return new Bounds(boundsCenter, new Vector3(attackWindow.x * 3f, attackWindow.y * 3f, 1f));
        }
    }
    public override void UpdateBehaviour()
    {
        if (GM.I.mainMenuScreen.activeSelf)
            return;

        currentInput = EntityInput.Get();
        SwapIfNecessary();
        base.UpdateBehaviour();
        AttackIfNecessary();
        KeepPlayerInBounds();
    }

    private void AttackIfNecessary()
    {
        if (currentInput.pressedButton == ButtonPress.Attack && charge > 0)
        {
            foreach (Ennemy ennemy in GM.I.currentEnnemyManager.ennemies)
            {
                if (AttackBounds().Intersects(ennemy.HitBox))
                {
                    Vector3 knockbackDir = (ennemy.transform.position - transform.position).normalized;
                    if (!isOffense)
                    {
                        knockbackDir = knockbackDir * 2f;
                    }
                    ennemy.Hurt(knockbackDir, isOffense ? 1 : 0);
                }
            }
            charge--;
        }
    }

    private void SwapIfNecessary()
    {
        if (currentInput.pressedButton == ButtonPress.ChangeState)
        {
            isOffense = !isOffense;
            anim = isOffense ? offenseAnim : defenseAnim;
            currentState = EntityState.Idle;
            GM.I.audio.PlaySFX(swap, transform.position, swapV);
        }
        offenseAnim.rend.enabled = isOffense;
        defenseAnim.rend.enabled = !isOffense;
        offenseCharge.Show(isOffense);
        defenseCharge.Show(!isOffense);
    }

    private void KeepPlayerInBounds()
    {
        Vector4 bounds = new Vector4();
        bounds.x = GM.I.cam.transform.position.x - GM.I.cam.cameraBounds.x;
        bounds.y = GM.I.cam.transform.position.x + GM.I.cam.cameraBounds.y;
        bounds.z = GM.I.cam.transform.position.y - GM.I.cam.cameraBounds.z;
        bounds.w = GM.I.cam.transform.position.y + GM.I.cam.cameraBounds.w;

        if (transform.position.x < bounds.x) { transform.position += Vector3.right * (bounds.x - transform.position.x); }
        if (transform.position.x > bounds.y) { transform.position += Vector3.left * (transform.position.x - bounds.y); }
        if (transform.position.y < bounds.z) { transform.position += Vector3.up * (bounds.z - transform.position.y); }
        if (transform.position.y > bounds.w) { transform.position += Vector3.down * (transform.position.y - bounds.w); }
        float angleOffset = GM.I.boundAngle * (transform.position.y);
        if (transform.position.x < GM.I.gameBounds.x + angleOffset) { transform.position += Vector3.right * (GM.I.gameBounds.x - transform.position.x) + Vector3.right * angleOffset; }
        if (transform.position.x > GM.I.gameBounds.y - angleOffset) { transform.position += Vector3.left * (transform.position.x - GM.I.gameBounds.y) + Vector3.left * angleOffset; }

    }

    public override void Hurt(Vector3 knockBackDir, int damage)
    {
        if (isOffense || charge == 10)
        {
            hp -= damage;
            knockBackDirection = knockBackDir;
        }
        else
        {
            charge += damage;
            GM.I.audio.PlaySFX(recharge, transform.position, rechargeV);
        }
        if (hp <= 0)
        {
            SetState(EntityState.Dead);
            GM.I.audio.PlaySFX(death, transform.position, deathV);

        }
        else
        {
            SetState(EntityState.Hurt);
            if (isOffense || charge == 10)
            {
                GM.I.audio.PlaySFX(hurt, transform.position, hurtV);
            }
        }
    }
}

public class EntityInput
{
    public Vector2 direction;
    public ButtonPress pressedButton;

    public EntityInput(Vector2 direction, ButtonPress pressedButton)
    {
        this.direction = direction;
        this.pressedButton = pressedButton;
    }

    public static EntityInput Get()
    {
        Vector2 dir = new Vector2();
        dir.x = Input.GetAxis("Horizontal");
        dir.y = Input.GetAxis("Vertical");

        ButtonPress bp = ButtonPress.None;
        if (Input.GetButtonDown("Swap"))
        {
            bp = ButtonPress.ChangeState;
        }
        else if (Input.GetButtonDown("Attack"))
        {
            bp = ButtonPress.Attack;
        }
        return new EntityInput(dir, bp);
    }
}

public enum ButtonPress
{
    ChangeState,
    Attack,
    Taunt,
    None
}


