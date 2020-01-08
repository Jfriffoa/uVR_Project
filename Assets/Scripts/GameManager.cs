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

    public GameObject[] turrets;
    PosAndRot[] _turretsInitialState;

    public Transform bulletContainer;

    [Header("UI Feedback")]
    public TextMesh lifeText;
    public GameObject mainMenuCanvas;
    public GameObject pauseCanvas;
    public GameObject endgameCanvas;

    void Start()
    {
        //Save initial states
        _turretsInitialState = new PosAndRot[turrets.Length];
        for (int i = 0; i < turrets.Length; i++)
        {
            _turretsInitialState[i].position = turrets[i].transform.localPosition;
            _turretsInitialState[i].rotation = turrets[i].transform.localRotation;
        }

        //Disable turrets
        EnableTurrets(false);

        //Stop spawning
        spawner.Stop();
    }

    public void StartGame()
    {
        //Move torrets back to the initial pos
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].transform.localPosition = _turretsInitialState[i].position;
            turrets[i].transform.localRotation = _turretsInitialState[i].rotation;
        }

        //Erase any bullets left
        for (int i = bulletContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(bulletContainer.GetChild(i).gameObject);
        }

        //Make the spawner clean itself
        spawner.Clean();

        //Re-Enable Turrets
        EnableTurrets(true);

        //Initiate Spawns
        spawner.Start();

        //Reset Lifes
        _currentLifes = initialLifes;
        lifeText.text = Mathf.Clamp(_currentLifes, 0, initialLifes) + "";

        //Set the currentState to playing
        _currentState = GameState.Playing;
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

            var turret = tur.GetComponentInChildren<Turret>();
            if (turret != null)
                turret.enabled = enable;
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

            //TODO: INSTANTIATE PARTICLES
            //TODO: SHOW SCREEN

            _currentState = GameState.Idle;
        }
    }

    private struct PosAndRot
    {
        internal Vector3 position;
        internal Quaternion rotation;
    }
}
