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
    public float countdown = 2f;

    int _waveNumber = 1;
    bool _spawning = false;

    [Header("UI Feedback")]
    public TextMesh timerText;
    public TextMesh waveText;

    internal void Start()
    {
        _waveNumber = 1;
        _spawning = false;
        waveText.text = _waveNumber + "";
    }

    internal void Stop()
    {
        _spawning = true;
    }

    void Update() {
        if (_spawning)
            return;

        if (countdown <= 0f) {
            _spawning = true;
            StartCoroutine(SpawnWave());
        }

        countdown -= Time.deltaTime;
        timerText.text = "" + Mathf.CeilToInt(countdown);
    }

    IEnumerator SpawnWave() {
        for (int i = 0; i < _waveNumber; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        _waveNumber++;
        waveText.text = _waveNumber + "";

        countdown = timeBetweenWaves;
        _spawning = false;
    }

    void SpawnEnemy() {
        var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, transform);
        enemy.GetComponent<Enemy>().speed *= 1 + _waveNumber * .1f;
    }

    internal void Clean()
    {
        //Clean all the enemies left
        for (int i = transform.childCount - 1; i >= 0; i++)
        {
            Destroy(transform.GetChild(i));
        }
    }
}
