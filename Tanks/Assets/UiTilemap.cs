using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI; 

public class UiTilemap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject textComponent;
    
    void Start()
    {
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
    }
    
    void OnMouseDown()
    {
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
        tilemap.GetComponent<TilemapCollider2D>().enabled = false;
        
        // Make text disappear
        if (textComponent != null)
        {
            textComponent.SetActive(false);
        }
    }
}