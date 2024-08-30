using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> enemyPrefab;
    public List<GameObject> powerUpPrefabs;

    private float spawnPos = 9.0f;
    public int enemyCount;
    public int waveCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveCount);
        SpawnPowerUp();
    }

    // Update is called once per frame
    void Update()
    {
        //finds any object that has enemy controlller script.
        enemyCount = FindObjectsOfType<EnemyController>().Length;

        if (enemyCount == 0 )
        {
            waveCount++;
            SpawnEnemyWave(waveCount);
            SpawnPowerUp();
        }
    }

    private Vector3 CreateRandomSpawn()
    {
        float spawnPositionX = Random.Range(-spawnPos, spawnPos);
        float spawnPositionZ = Random.Range(-spawnPos, spawnPos);

        Vector3 RandomPosition = new Vector3(spawnPositionX, 0, spawnPositionZ);

        return RandomPosition;
    }

    private void SpawnPowerUp()
    {
        int powerUpSelected = Random.Range(0, powerUpPrefabs.Count);

        Instantiate(powerUpPrefabs[powerUpSelected], CreateRandomSpawn(), Quaternion.identity);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int chooseEnemy = Random.Range(0, enemyPrefab.Count);
            Instantiate(enemyPrefab[chooseEnemy], CreateRandomSpawn(), Quaternion.identity);
        }
    }
}
