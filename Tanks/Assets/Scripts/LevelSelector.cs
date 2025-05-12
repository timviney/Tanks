using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;

    private void Start()
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
                LevelManager.Instance.LoadLevel(level);
            });
            
            levelButtons[i].interactable = LevelManager.Instance?.IsLevelUnlocked(level)??false;
        }
    }
}