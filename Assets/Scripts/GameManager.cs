using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class GameManager : MonoBehaviour
{
    enum GameState { Idle, Playing, Pause }
    GameState _currentState = GameState.Idle;

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

    public int initialLifes = 5;
    int _currentLifes;

    public Turret[] turrets;

    public Transform bulletContainer;

    [Header("UI Feedback")]
    public TextMesh lifeText;
    public UnityEngine.UI.Text gameOverText;
    [TextArea]
    public string gameOverMessage;

    public GameObject mainMenuCanvas;
    public GameObject pauseCanvas;
    public GameObject endgameCanvas;

    [Header("Particles")]
    public GameObject playerTower;
    public GameObject endgameParticles;

    void Start()
    {
        foreach (var turret in turrets)
        {
            turret.Start();
        }

        //Disable turrets
        EnableTurrets(false);

        //Stop spawning
        spawner.Stop();
    }

    public void StartGame()
    {
        //Erase any bullets left
        for (int i = bulletContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(bulletContainer.GetChild(i).gameObject);
        }

        //Make the spawner clean itself
        spawner.Clean();

        //Re-Enable Turrets
        EnableTurrets(true);

        //Move torrets back to the initial pos
        foreach (var turret in turrets)
        {
            turret.RestartPos();
        }

        //Initiate Spawns
        spawner.Start();

        //Reset Lifes
        _currentLifes = initialLifes;
        lifeText.text = Mathf.Clamp(_currentLifes, 0, initialLifes) + "";

        //Set the currentState to playing
        _currentState = GameState.Playing;

        //Make sure the tower is active
        playerTower.SetActive(true);
    }

    public void ResumeGame()
    {
        if (_currentState != GameState.Pause)
            return;

        //Resume Spawner
        spawner.Resume();

        //Resume bullets
        for (int i = 0; i < bulletContainer.childCount; i++)
        {
            bulletContainer.GetChild(i).GetComponent<Bullet>().enabled = true;
        }

        //Enable Turrets
        EnableTurrets(true);

        //Hide UI
        pauseCanvas.SetActive(false);

        //Change State
        _currentState = GameState.Playing;
    }

    public void PauseGame()
    {
        if (_currentState != GameState.Playing)
            return;

        //Pause Spawner
        spawner.Stop();

        //Pause Bullets
        for (int i = 0; i < bulletContainer.childCount; i++)
        {
            bulletContainer.GetChild(i).GetComponent<Bullet>().enabled = false;
        }

        //Disable Turrets
        EnableTurrets(false);

        //Show UI
        pauseCanvas.SetActive(true);
        
        //Change State
        _currentState = GameState.Pause;
    }

    void EnableTurrets(bool enable)
    {
        foreach(var tur in turrets)
        {
            var handler = tur.GetComponent<ManipulationHandler>();
            if (handler != null)
                handler.enabled = enable;

            tur.enabled = enable;
        }
    }

    public void Damage()
    {
        _currentLifes--;
        lifeText.text = Mathf.Clamp(_currentLifes, 0, initialLifes) + "";

        // End Game
        if (_currentLifes <= 0)
        {
            Debug.Log("GAME FINISHED", gameObject);

            spawner.Clean();
            spawner.Stop();
            EnableTurrets(false);

            playerTower.SetActive(false);
            Instantiate(endgameParticles, playerTower.transform.position, playerTower.transform.rotation, transform);

            endgameCanvas.SetActive(true);
            gameOverText.text = gameOverMessage + spawner.WaveNumber + " waves!";

            _currentState = GameState.Idle;
        }
    }
}
