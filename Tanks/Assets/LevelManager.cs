using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    private int _levelsUnlocked = 1;
    
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
    
    // Call this when a level is completed
    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex < _levelsUnlocked) return;
        
        _levelsUnlocked = levelIndex + 1;
        PlayerPrefs.SetInt("LevelsUnlocked", _levelsUnlocked);
        PlayerPrefs.Save();
    }
    
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }
    
    public void ReturnToLevelSelector()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    
    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= _levelsUnlocked;
    }
}
