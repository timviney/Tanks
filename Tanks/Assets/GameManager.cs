using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject uiTilemapGo;

    // Singleton instance
    public static GameManager Instance;

    // Game state tracking
    private List<GameObject> _enemies = new List<GameObject>();
    private GameObject _player;
    private bool _gameEnded = false;
    private UiTilemap _uiTilemap;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find player (tag your player with "Player")
        _player = GameObject.FindGameObjectWithTag("Player");
        
        // Find all enemies (tag your enemies with "Enemy")
        var enemyArray = GameObject.FindGameObjectsWithTag("Enemies");
        _enemies = new List<GameObject>(enemyArray);
        
        _uiTilemap = uiTilemapGo.GetComponent<UiTilemap>();
    }

    // Call this when an enemy is destroyed
    public void RegisterDeath(GameObject tank)
    {
        if (_enemies.Contains(tank))
        {
            _enemies.Remove(tank);
            CheckWinCondition();
        }
        else if (_player == tank)
        {
            _gameEnded = true;
            _uiTilemap.Lose();
        }
    }

    private void CheckWinCondition()
    {
        if (_enemies.Count > 0 || _gameEnded || _player == null) return;
        
        _gameEnded = true;
        _uiTilemap.Win();
    }

    private int GetCurrentLevelIndex()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level"))
        {
            string levelNum = sceneName.Replace("Level", "");
            int index;
            if (int.TryParse(levelNum, out index))
            {
                return index;
            }
        }
        return 1; // Default to level 1 if can't parse
    }
}