using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    public string Url;

    public void Open() {
        Application.OpenURL(Url);
    }
}
