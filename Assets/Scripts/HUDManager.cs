using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public List<Sprite> healthSprites;
    public List<Sprite> chargeSprites;

    public SpriteRenderer healthBar, chargeBar, alert;

    private void Update()
    {
        int hp = Mathf.Max(0, GM.I.player.hp);
        healthBar.sprite = healthSprites[20 - hp];
        chargeBar.sprite = chargeSprites[10 - GM.I.player.charge];
        alert.enabled = GM.I.player.charge == 10;
    }
}
