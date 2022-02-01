using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public TextMesh txt;
    public int lineIndex = 0;
    public AudioClip voiceBeep;
    public AnimationCurve translateCurve;
    [TextArea]
    public List<string> lines;


    float downY = -1.859f;
    float upY = -1.5f;
    float X = 2.42f;

    Coroutine lineRoutine;

    public void Hide(){
        if(lineRoutine != null){StopCoroutine(lineRoutine);}
        lineRoutine = StartCoroutine(HideLine());
    }

    private void Start() {
        NextLine(true);
    }

    public void NextLine(bool translate){
        if(lineRoutine != null){StopCoroutine(lineRoutine);}
        lineRoutine = StartCoroutine(ShowLine(translate));
    }

    IEnumerator ShowLine(bool translate){
        txt.text = "";
        string line = lines[lineIndex];
        lineIndex++;
        float t = 0f;
        Vector3 upPos = new Vector3(X, upY, 0);
        Vector3 downPos = new Vector3(X, downY, 0);
        if(translate){
            while (t < 1f)
            {
                transform.localPosition = Vector3.Lerp(upPos, downPos, translateCurve.Evaluate(t));
                t += Time.deltaTime * 2f;
                yield return 0;
            }
        }
        transform.localPosition = downPos;

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < line.Length; i++)
        {
            txt.text = line.Substring(0, i);
            GM.I.audio.PlaySFX(voiceBeep, GM.I.cam.transform.position, 2f);
            yield return 0;
            yield return 0;
            yield return 0;
            yield return 0;
        }
        txt.text = line;
        
    }

    IEnumerator HideLine(){
        txt.text = "";
        float t = 0f;
        Vector3 upPos = new Vector3(X, upY, 0);
        Vector3 downPos = new Vector3(X, downY, 0);
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(downPos, upPos, translateCurve.Evaluate(t));
            t += Time.deltaTime * 2f;
            yield return 0;
        }
        transform.localPosition = upPos;
    }


}
