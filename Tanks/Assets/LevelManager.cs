using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    private int _levelsUnlocked = 1;
    private int _currentLevel = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            _levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 1);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void CompleteLevel()
    {
        if (_currentLevel < _levelsUnlocked) return;
        
        _levelsUnlocked = _currentLevel + 1;
        PlayerPrefs.SetInt("LevelsUnlocked", _levelsUnlocked);
        PlayerPrefs.Save();
    }
    
    public void LoadLevel(int levelIndex)
    {
        _currentLevel = levelIndex;
        SceneManager.LoadScene("Level" + levelIndex);
    }
    
    public void ReturnToLevelSelector()
    {
        _currentLevel = 0;
        SceneManager.LoadScene("LevelSelector");
    }
    
    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= _levelsUnlocked;
    }
}
