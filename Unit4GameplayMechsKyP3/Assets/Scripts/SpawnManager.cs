using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    private float spawnPos = 9.0f;
    public int enemyCount;
    public int waveCount;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(3);
    }

    // Update is called once per frame
    void Update()
    {
        //finds any object that has enemy controlller script.
        enemyCount = FindObjectsOfType<EnemyController>().Length;

        if (enemyCount == 0 )
        {
            SpawnEnemyWave(3);
            waveCount++;
        }
    }

    private Vector3 CreateRandomEnemySpawn()
    {
        float spawnPositionX = Random.Range(-spawnPos, spawnPos);
        float spawnPositionZ = Random.Range(-spawnPos, spawnPos);

        Vector3 RandomPosition = new Vector3(spawnPositionX, 0, spawnPositionZ);

        return RandomPosition;
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, CreateRandomEnemySpawn(), Quaternion.identity);
        }
    }
}
