using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MapManager_1 : Singleton<MapManager_1>
{
    [Header("Background")]
    [SerializeField] private Transform backgroundPrefab;
    [SerializeField] private Transform appearPoint;

    [Header("Wave")]
    [SerializeField] private List<Transform> waves;
    [SerializeField] private int currWaveIndex;

    public void SpawnCloud()
    {
        Transform newClound = SpawnerManager.Instance.Background.SpawnByObj(this.backgroundPrefab, this.appearPoint.position, this.appearPoint.rotation);
        newClound.gameObject.SetActive(true);
    }

    public void NextWave()
    {
        this.waves[this.currWaveIndex].gameObject.SetActive(false);
        this.currWaveIndex++;
        this.waves[this.currWaveIndex].gameObject.SetActive(true);
    }
}
