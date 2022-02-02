using System.Collections.Generic;
using UnityEngine;

public class ChargeAppearanceManager : MonoBehaviour
{
    public List<SpriteRenderer> bars;
    public bool shown = true;

    public void Show(bool onOff)
    {
        if (onOff == shown) { return; }
        foreach (SpriteRenderer bar in bars)
        {
            bar.enabled = onOff;
        }
        shown = onOff;
    }

    private void Update()
    {
        if (!shown) { return; }
        for (int i = 0; i < bars.Count; i++)
        {
            bars[i].enabled = (i < GM.I.player.charge);
        }
    }
}
