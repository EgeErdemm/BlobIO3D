using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Image _bgImage;
    [SerializeField] Canvas _canvas;
    [SerializeField] private Button _RestartButton;
    [SerializeField] private TextMeshProUGUI _ScoreText;
    [SerializeField] private Player player;


    private void Start()
    {
        if (_bgImage == null)
        {
            Debug.LogError("Background Image is null");
        }
    }

    private void Awake()
    {
        _RestartButton.onClick.AddListener(OnRestartButtonClicked);
        Debug.Log("RESTART BUTTON CLİCKED");
    }


    public void GameOver()
    {

        StartCoroutine(BlackOutCoroutine());
        _canvas.gameObject.SetActive(true);
        _ScoreText.text ="Score: " +player.Level;
    }



    private void OnRestartButtonClicked()
    {
        _RestartButton.interactable = false;
        Time.timeScale = 1f;
        Scene activeScene = SceneManager.GetActiveScene();
        int buildIndex = activeScene.buildIndex;
        SceneManager.LoadScene(buildIndex);
    }

    private IEnumerator BlackOutCoroutine()
    {
        float timer = 2f;
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            Color color = _bgImage.color;
            color.a = Mathf.Clamp01(color.a + Time.deltaTime);
            _bgImage.color = color;
            yield return null;
        }
        Time.timeScale = 0f;
    }

}
