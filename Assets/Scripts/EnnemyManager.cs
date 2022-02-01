using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnnemyManager : MonoBehaviour
{
    public Transform cameraTarget;
    public List<Ennemy> ennemies;
    public bool active = false;
    public bool bossPhase = false;

    private void Awake() {
        ennemies = GetComponentsInChildren<Ennemy>(true).ToList();
    }
    public void Activate(bool cam = true){
        GM.I.currentEnnemyManager = this;
        if(cam){
            GM.I.cam.SetTarget(cameraTarget);
        }
        foreach (Ennemy ennemy in ennemies)
        {
            ennemy.gameObject.SetActive(true);
        }
        GM.I.goArrow.enabled = false;
        active = true;
    }

    private void Update() {
        if(!active){return;}
        bool allDead = true;
        foreach (Ennemy ennemy in ennemies)
        {
            if(ennemy.currentState != EntityState.Dead && !ennemy.isBoss){
                allDead = false;
            }
        }

        if(allDead){
            GoToNext();
        }
    }

    private void GoToNext()
    {
        active = false;
        if(!bossPhase){
            GM.I.goArrow.enabled = true;
            GM.I.cam.FocusPlayer();
        }
    }
}
