using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public SpriteRenderer rend;
    
    public float animationSpeed;

    Coroutine currentAnimation;

    public void PlayAnimation(List<Sprite> sprites, bool looping, Action onComplete = null)
    {
        if(currentAnimation != null){ StopCoroutine(currentAnimation);}
        currentAnimation = StartCoroutine(AnimationRoutine(sprites, looping, onComplete));
    }

    IEnumerator AnimationRoutine(List<Sprite> sprites, bool looping, Action onComplete){
        bool passedOnce = false;
        while(looping || !passedOnce){
            foreach (var sprite in sprites)
            {
                rend.sprite = sprite;
                yield return new WaitForSeconds(1f/animationSpeed);
            }
            passedOnce = true;
        }
        onComplete?.Invoke();
    }
}
