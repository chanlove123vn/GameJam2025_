using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float tileHeight = 10f;
    [SerializeField] private int initialTileCount = 5;
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnThreshold = 10f; // Cách bao xa từ player thì spawn tile mới
    
    private Queue<GameObject> activeTiles = new Queue<GameObject>();
    private float nextSpawnYPosition;
    private float lastSpawnY;

    void Start()
    {
        // Spawn tile ban đầu
        for (int i = 0; i < initialTileCount; i++)
        {
            SpawnTile();
        }
        
        lastSpawnY = nextSpawnYPosition;
    }

    void Update()
    {
        // Di chuyển tất cả tile xuống
        foreach (GameObject tile in activeTiles)
        {
            tile.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        }

        // Kiểm tra và spawn tile mới dựa vào vị trí player
        CheckAndSpawnTile();

        // Xoá tile cũ khi nó quá xa player (phía dưới)
        RemoveOldTiles();
    }

    private void CheckAndSpawnTile()
    {
        // Nếu tile tiếp theo sắp đến gần player, spawn tile mới
        if (nextSpawnYPosition - player.position.y < spawnThreshold)
        {
            SpawnNewTile();
        }
    }

    private void SpawnNewTile()
    {
        nextSpawnYPosition -= tileHeight; // Âm vì tile cuộn xuống

        GameObject newTile = Instantiate(
            tilePrefab, 
            new Vector3(0, nextSpawnYPosition, 0), 
            Quaternion.identity
        );
        
        activeTiles.Enqueue(newTile);
    }

    private void RemoveOldTiles()
    {
        // Xoá tile khi nó đã cuộn quá xa phía dưới player
        while (activeTiles.Count > 0)
        {
            GameObject tile = activeTiles.Peek();

            // Nếu tile ở dưới player quá 15 unit, xoá nó
            if (tile.transform.position.y < player.position.y - 15f)
            {
                activeTiles.Dequeue();
                Destroy(tile);
            }
            else
            {
                break;
            }
        }
    }
    
    private void SpawnTile()
    {
        GameObject newTile = Instantiate(
            tilePrefab, 
            new Vector3(0, nextSpawnYPosition, 0), 
            Quaternion.identity
        );
        
        activeTiles.Enqueue(newTile);
        nextSpawnYPosition -= tileHeight; // Chuẩn bị vị trí cho tile tiếp theo
    }
}
    