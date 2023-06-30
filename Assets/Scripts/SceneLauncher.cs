using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLauncher : MonoBehaviour
{
    public string sceneName; // The name of the scene to launch

    public void LaunchScene()
    {
        SceneManager.LoadScene(sceneName); // Load the specified scene
    }
}
