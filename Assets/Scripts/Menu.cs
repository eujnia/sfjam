using UnityEngine;

public class Menu : MonoBehaviour
{
    void Start()
    {
        transform.Find("StartButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        Music.Instance.PlaySong("karma");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game1");
    }

}
