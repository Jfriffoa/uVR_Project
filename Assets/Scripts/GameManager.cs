using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    // Singleton
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Trying to access GameManager when is not created.");
            return _instance;
        }
    }

    // Initialize the Singleton
    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Trying to create another GameManager when is already on scene. Aborting...", gameObject);
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }
    #endregion

    [Header("Game Components")]
    public WaveSpawner spawner;

    [SerializeField]
    int lifes = 5;

    [Header("UI Feedback")]
    public TextMesh lifeText;

    void Start()
    {
        lifeText.text = lifes + "";
    }

    public void Damage()
    {
        lifes--;
        lifeText.text = lifes + "";

        // End Game
        if (lifes <= 0)
        {
            spawner.enabled = false;
            Debug.Log("GAME FINISHED", gameObject);
            Time.timeScale = 0;
        }
    }
}
