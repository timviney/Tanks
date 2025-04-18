using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadScene("Level1"); // Goes to level 1
    }
}
