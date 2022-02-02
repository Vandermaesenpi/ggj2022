using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static GM _instance;
    public static GM I
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GM>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("Bicycle");
                    _instance = container.AddComponent<GM>();
                }
            }

            return _instance;
        }
    }
    #endregion

    public CameraManager cam;
    public Player player;
    public EnnemyManager currentEnnemyManager;
    public DialogSystem dialog;
    public AudioManager audio;

    public GameObject gameOverScreen, mainMenuScreen, gameObjects, winScreen;
    public SpriteRenderer goArrow;
    public Vector2 gameBounds;
    public float boundAngle;

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public void Restart() => SceneManager.LoadScene(0);

    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
        gameObjects.SetActive(true);
        audio.PlayGameMusic();
        dialog.enabled = true;
    }

    public void GameOver()
    {
        gameObjects.SetActive(false);
        gameOverScreen.SetActive(true);
    }
}
