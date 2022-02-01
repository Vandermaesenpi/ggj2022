using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBlinker : AnimationPlayer
{
    public List<Sprite> sprites;
    private void Start() {
        PlayAnimation(sprites, true);
    }
}
