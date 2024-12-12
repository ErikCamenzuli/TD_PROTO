using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManger : MonoBehaviour
{
    public void Select(string level)
    {
        SceneManager.LoadScene(level);
    }
}
