using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void Level1()
    {
        LevelManager.Instance.LoadLevel(1);
    }
}
