using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : Ennemy
{
    public BossState currentBossState;
    public List<Claw> cables;
    public List<GameObject> phaseEnnemies;
    public Transform cameraTarget;

    private void OnEnable()
    {
        StartCoroutine(BossRoutine());
    }

    public override Bounds HitBox
    {
        get
        {
            if (currentBossState == BossState.Tired)
            {
                return new Bounds(transform.position, new Vector3(hitboxSize.x, hitboxSize.y, 1));
            }
            else
            {
                return new Bounds(Vector3.up * 100f, new Vector3(hitboxSize.x, hitboxSize.y, 1));
            }

        }
    }

    public override void UpdateBehaviour()
    {
    }

    IEnumerator BossRoutine()
    {
        GM.I.audio.PlayBossMusic();
        GM.I.cam.SetTarget(cameraTarget);
        GM.I.goArrow.enabled = false;
        anim.SetAnimation(EntityState.Idle, true);
        yield return new WaitForSeconds(2f);
        foreach (Claw claw in cables)
        {
            claw.gameObject.SetActive(true);
        }
        for (int i = 0; i < 4; i++)
        {

            currentBossState = BossState.Attacking;
            anim.SetAnimation(EntityState.Idle, true);
            foreach (Claw claw in cables)
            {
                claw.active = true;
            }
            yield return new WaitForSeconds(20f);
            foreach (Claw claw in cables)
            {
                claw.active = false;
            }
            EnnemyManager currentPhase = GameObject.Instantiate(phaseEnnemies[i], transform.parent).GetComponentInChildren<EnnemyManager>();
            currentPhase.Activate(false);
            currentPhase.ennemies.Add(this);
            currentBossState = BossState.Tired;
            anim.SetAnimation(EntityState.Moving, true);
            while (hp == 4 - i)
            {
                if (!currentPhase.active)
                {
                    Destroy(currentPhase.transform.parent.gameObject);
                    currentPhase = GameObject.Instantiate(phaseEnnemies[i], transform.parent).GetComponentInChildren<EnnemyManager>();
                    currentPhase.Activate(false);
                    currentPhase.ennemies.Add(this);
                }
                yield return 0;
            }

            yield return new WaitForSeconds(0.2f);
        }

        List<Ennemy> lastAliveEnnemies = FindObjectsOfType<Ennemy>().ToList();
        foreach (Ennemy ennemy in lastAliveEnnemies)
        {
            ennemy.Hurt(Vector3.zero, 1000);
        }

        yield return new WaitForSeconds(2f);
        GM.I.dialog.NextLine(true);
    }
}

public enum BossState
{
    Idle,
    Attacking,
    Tired
}
