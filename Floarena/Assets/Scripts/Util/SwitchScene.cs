using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SwitchScene : MonoBehaviour
{
    [SerializeField]
    string scene;

    public void NextScene()
    {
        SceneManager.LoadScene(scene);
    }
}