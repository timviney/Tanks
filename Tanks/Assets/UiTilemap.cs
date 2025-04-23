using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UiTilemap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject textComponent;

    private UiState _state;
    
    void Start()
    {
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
    }

    public void Win()
    {
        _state = UiState.Win;
        Enable();
        textComponent.GetComponent<TextMeshPro>().text = "You Win!";
    }
    public void Lose()
    {
        _state = UiState.Lose;
        Enable();
        textComponent.GetComponent<TextMeshPro>().text = "You Lose!";
    }

    private void Enable()
    {
        tilemap.GetComponent<TilemapRenderer>().enabled = true;
        tilemap.GetComponent<TilemapCollider2D>().enabled = true;
        textComponent.SetActive(true);
    }
    private void Disable()
    {
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
        tilemap.GetComponent<TilemapCollider2D>().enabled = false;
        textComponent.SetActive(false);
    }

    void OnMouseDown()
    {
        switch (_state)
        {
            case UiState.Start:
            default:
                Disable();
                break;
            case UiState.Win:
                LevelManager.Instance.ReturnToLevelSelector();
                break;
            case UiState.Lose:
                LevelManager.Instance.ReturnToLevelSelector();
                break;
        }
    }

    private enum UiState
    {
        Start,
        Win,
        Lose
    }
}