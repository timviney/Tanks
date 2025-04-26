using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class UiTilemap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject uiTextComponent;
    [SerializeField] private GameObject okTextComponent;
    [SerializeField] private GameObject returnTextComponent;

    private UiState _state;
    
    void Start()
    {
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
    }

    public void Win()
    {
        _state = UiState.Win;
        uiTextComponent.GetComponent<TextMeshPro>().text = "You Win!";
        okTextComponent.GetComponent<TextMeshPro>().text = "Next";
        
        Enable();
    }
    public void Lose()
    {
        _state = UiState.Lose;
        uiTextComponent.GetComponent<TextMeshPro>().text = "You Lose!";
        okTextComponent.GetComponent<TextMeshPro>().text = "Retry";

        Enable();
    }

    private void Enable()
    {
        tilemap.GetComponent<TilemapRenderer>().enabled = true;
        tilemap.GetComponent<TilemapCollider2D>().enabled = true;
        uiTextComponent.SetActive(true);
    }
    private void Disable()
    {
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
        tilemap.GetComponent<TilemapCollider2D>().enabled = false;
        uiTextComponent.SetActive(false);
    }

    public void WhenOkayClicked()
    {
        switch (_state)
        {
            case UiState.Start:
            default:
                Disable();
                break;
            case UiState.Win:
                LevelManager.Instance.NextLevel();
                break;
            case UiState.Lose:
                LevelManager.Instance.ReloadLevel();
                break;
        }
    }

    public void WhenReturnClicked()
    {
        LevelManager.Instance.ReturnToLevelSelector();
    }

    private enum UiState
    {
        Start,
        Win,
        Lose
    }
}