using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void StartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game1");
    }

}
