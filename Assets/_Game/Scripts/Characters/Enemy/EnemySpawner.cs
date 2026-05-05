using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    // ============ DESIGNER SETTINGS ============

    [Header("Spawn Configuration")]
    [SerializeField] 
    [Tooltip("Daftar prefab musuh yang akan di-spawn secara acak.")]
    private GameObject[] _enemyPrefabs;

    [SerializeField] 
    [Tooltip("Interval waktu antar spawn (detik).")]
    private float _spawnInterval = 3f;

    [SerializeField] 
    [Tooltip("Jarak spawn dari titik pusat.")]
    private float _spawnRadius = 15f;

    [Header("Targeting")]
    [SerializeField] 
    [Tooltip("Jika aktif, musuh spawn di sekitar Player. Jika mati, spawn di sekitar posisi objek ini.")]
    private bool _spawnAroundPlayer = true;

    [SerializeField] 
    [Tooltip("Referensi Transform Player. Jika kosong, script akan mencari tag 'Player' saat Start.")]
    private Transform _playerTransform;

    [Header("Performance")]
    [SerializeField] 
    [Tooltip("Jumlah maksimum musuh yang boleh ada di scene sekaligus.")]
    private int _maxEnemies = 50;

    // ============ STATE MANAGEMENT ============

    public int ActiveEnemyCount => _activeEnemies.Count;
    private float _timer;
    private List<GameObject> _activeEnemies = new List<GameObject>();

    // ============ UNITY EVENTS ============

    private void Start()
    {
        InitializePlayerReference();
        
        // Memulai timer pada nilai interval agar spawn pertama langsung terjadi
        _timer = _spawnInterval;
    }

    private void Update()
    {
        HandleSpawnLogic();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = (_spawnAroundPlayer && _playerTransform != null) ? _playerTransform.position : transform.position;
        
        // Menggambar lingkaran di scene view untuk membantu Level Designer
        Gizmos.DrawWireSphere(center, _spawnRadius);
    }

    // ============ LOGIC ============

    private void InitializePlayerReference()
    {
        if (_playerTransform == null && _spawnAroundPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
        }
    }

    private void HandleSpawnLogic()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            CleanupActiveEnemies();
            
            if (_activeEnemies.Count < _maxEnemies)
            {
                SpawnEnemy();
            }
            
            _timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefabs == null || _enemyPrefabs.Length == 0) return;

        Vector3 spawnPosition = CalculateSpawnPosition();
        int randomIndex = Random.Range(0, _enemyPrefabs.Length);
        
        GameObject enemy = Instantiate(_enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        _activeEnemies.Add(enemy);
    }

    private Vector3 CalculateSpawnPosition()
    {
        // Perbaikan Logika: Menggunakan Random.insideUnitCircle untuk distribusi 2D (X dan Z)
        // Jika menggunakan Vector2 direction awalmu (x, y), musuh akan spawn melayang di udara (Y) jika game 3D.
        Vector2 randomPoint = Random.insideUnitCircle.normalized * _spawnRadius;
        
        Vector3 offset = new Vector3(randomPoint.x, 0f, randomPoint.y);
        Vector3 center = (_spawnAroundPlayer && _playerTransform != null) ? _playerTransform.position : transform.position;
        
        return center + offset;
    }

    private void CleanupActiveEnemies()
    {
        _activeEnemies.RemoveAll(item => item == null);
    }
}