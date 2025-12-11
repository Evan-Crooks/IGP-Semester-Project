using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{

    private int NumberOfEnemiesSpawned = 0;
    public int MaxEnemies = 10;
    public Tilemap tilemap;
    public GameObject enemyPrefab;

    public static EnemySpawner instance { get; set; }

    private void Awake()
    {
        instance = this;
    }

    private void spawnEnemy()
    {
        int enemyCount = 10;

        for (int tries = 0; tries < 1000; tries++)
        {
            int x = UnityEngine.Random.Range(0, tilemap.cellBounds.size.x);
            int y = UnityEngine.Random.Range(0, tilemap.cellBounds.size.y);
            Vector3Int cellPos = new Vector3Int(x, y, 0);

            //spawn if empty tile
            if (tilemap.GetTile(cellPos) == null)
            {
                Vector3 worldPos = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);
                GameObject enemy = Instantiate(enemyPrefab, worldPos, Quaternion.identity);
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If there are not 10 enemies
    }
}
