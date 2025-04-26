using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;

    private void Start()
    {
        SetButtons();
    }
    
    private void OnEnable()
    {
        Debug.Log($"Refreshing button states");
        SetButtons();
    }
    
    private void SetButtons()
    {
        Debug.Log($"Set button states {levelButtons.Length}");
        for (var i = 0; i < levelButtons.Length; i++)
        {
            var level = i + 1;
            
            // Clear existing listeners first
            levelButtons[i].onClick.RemoveAllListeners();
            
            levelButtons[i].onClick.AddListener(() => {
                Debug.Log($"Loading level {level}");
                LevelManager.Instance.LoadLevel(level);
            });
            
            levelButtons[i].interactable = LevelManager.Instance.IsLevelUnlocked(level);
            Debug.Log($"Level {level} is {levelButtons[i].interactable}");
        }
    }
}