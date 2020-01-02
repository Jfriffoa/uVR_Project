using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    public float countdown = 2f;

    int _waveNumber = 1;
    bool _spawning = false;

    void Update() {
        if (countdown <= 0f && !_spawning) {
            _spawning = true;
            StartCoroutine(SpawnWave());
        } 

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave() {
        for (int i = 0; i < _waveNumber; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(.5f);
        }

        _waveNumber++;

        countdown = timeBetweenWaves;
        _spawning = false;
    }

    void SpawnEnemy() {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, transform);
    }
}
