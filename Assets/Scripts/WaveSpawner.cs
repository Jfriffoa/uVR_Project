using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spanwer Config")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    public float timeBetweenEnemies = .5f;
    public float initialCountdown = 10f;

    int _waveNumber = 1;
    float _countdown = 2f;
    bool _spawning = false;
    bool _pause = false;

    [Header("UI Feedback")]
    public TextMesh timerText;
    public TextMesh waveText;

    internal void Start()
    {
        _waveNumber = 1;
        _spawning = false;
        _pause = false;
        _countdown = initialCountdown;
        waveText.text = _waveNumber + "";
    }

    internal void Resume()
    {
        _pause = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var script = child.GetComponent<Enemy>();
            if (script != null)
                script.enabled = true;
        }
    }
    internal void Stop()
    {
        _pause = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var script = child.GetComponent<Enemy>();
            if (script != null)
                script.enabled = false;
        }
    }

    void Update() {
        if (_spawning || _pause)
            return;

        if (_countdown <= 0f) {
            _spawning = true;
            StartCoroutine(SpawnWave());
        }

        _countdown -= Time.deltaTime;
        timerText.text = "" + Mathf.CeilToInt(_countdown);
    }

    IEnumerator SpawnWave() {
        for (int i = 0; i < _waveNumber; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
            yield return new WaitWhile(() => _pause);
        }

        _waveNumber++;
        waveText.text = _waveNumber + "";

        _countdown = timeBetweenWaves;
        _spawning = false;
    }

    void SpawnEnemy() {
        var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, transform);
        enemy.GetComponent<Enemy>().speed *= 1 + _waveNumber * .1f;
    }

    internal void Clean()
    {
        //Clean all the enemies left
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
