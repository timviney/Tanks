using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject uiTilemapGo;

    // Singleton instance
    public static GameManager Instance;

    // Game state tracking
    public bool GameEnded { get; private set; }
    public bool GameStarted { get; private set; }
    private List<GameObject> _enemies = new List<GameObject>();
    private GameObject _player;
    private UiHandler _uiHandler;
    
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
        
        _uiHandler = uiTilemapGo.GetComponent<UiHandler>();
    }

    // Call this when an enemy is destroyed
    public void RegisterDeath(GameObject tank)
    {
        if (_enemies.Contains(tank))
        {
            _enemies.Remove(tank);
            Console.Out.WriteLine(tank.name);
            CheckWinCondition();
        }
        else if (_player == tank)
        {
            GameEnded = true;
            _uiHandler.Lose();
        }
    }

    public void StartGame()
    {
        GameStarted = true;
    }
    
    private void CheckWinCondition()
    {
        Console.Out.WriteLine("Checking win condition");

        if (_enemies.Count > 0 || GameEnded || _player == null) return;
        
        Console.Out.WriteLine("Won!");

        GameEnded = true;
        _uiHandler.Win();
        LevelManager.Instance.CompleteLevel();
    }
}