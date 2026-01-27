using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;

    public void OnClickRestartButton()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        string currentStage = GameManager.Instance.CurrentStage;

        // Stage Àç·Îµå
        GameManager.Instance.RequestLoadStage(currentStage);
    }

    public void OnClickMainMenuButton()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
