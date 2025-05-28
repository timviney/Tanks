using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class UiHandler : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject uiTextComponent;
    [SerializeField] private GameObject okTextComponent;
    [SerializeField] private GameObject returnTextComponent;

    private UiState _state;
    private Vector3 originalCameraPosition;
    private Vector3[] originalUIPositions;
    
    void Start()
    {
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
        RecordOriginalPositions();
    }
    
    void RecordOriginalPositions()
    {
        originalCameraPosition = Camera.main.transform.position;
        originalUIPositions = new Vector3[4];
        originalUIPositions[0] = tilemap.transform.position;
        originalUIPositions[1] = uiTextComponent.transform.position;
        originalUIPositions[2] = okTextComponent.transform.position;
        originalUIPositions[3] = returnTextComponent.transform.position;
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
        // Calculate how much the camera has moved
        Vector3 cameraMovement = Camera.main.transform.position - originalCameraPosition;

        // Apply movement to all UI elements
        // No moving of tilemap as it is a child of the text component
        uiTextComponent.transform.position = originalUIPositions[1] + cameraMovement;
        okTextComponent.transform.position = originalUIPositions[2] + cameraMovement;
        returnTextComponent.transform.position = originalUIPositions[3] + cameraMovement;
    
        // Enable components
        tilemap.GetComponent<TilemapRenderer>().enabled = true;
        tilemap.GetComponent<TilemapCollider2D>().enabled = true;
        uiTextComponent.SetActive(true);
        okTextComponent.SetActive(true);
        returnTextComponent.SetActive(true);
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
                GameManager.Instance.StartGame();
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