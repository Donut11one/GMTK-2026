using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private GameObject buttons;
    [SerializeField] private float deathDelay = 1f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string gameScene = "Game";

    private void Start()
    {
        GameManager.Instance.GameOver += Begin;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver -= Begin;
        }
    }

    private void Begin()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        yield return new WaitForSeconds(deathDelay);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fade.alpha = elapsed / fadeDuration;
            yield return null;
        }

        fade.alpha = 1f;
        buttons.SetActive(true);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
