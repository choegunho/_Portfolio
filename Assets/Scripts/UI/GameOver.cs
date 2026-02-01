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
        // Stage Àç·Îµå
        GameManager.Instance.RequestLoadStage("Stage1");
        GameManager.Instance.ActivateUI();
    }

    public void OnClickMainMenuButton()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
        GameManager.Instance.MainMenu();
    }
}
