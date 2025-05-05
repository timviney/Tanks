using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        SetButtons();
    }
    
    private void OnEnable()
    {
        SetButtons();
    }
    
    private void SetButtons()
    {
        for (var i = 0; i < levelButtons.Length; i++)
        {
            var level = i + 1;
            
            levelButtons[i].onClick.RemoveAllListeners();
            
            levelButtons[i].onClick.AddListener(() => {
                levelManager.LoadLevel(level);
            });
            
            levelButtons[i].interactable = levelManager.IsLevelUnlocked(level);
        }
    }
}