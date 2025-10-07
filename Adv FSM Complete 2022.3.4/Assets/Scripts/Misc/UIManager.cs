using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Mission Intro")]
    [SerializeField] private TextMeshProUGUI missionText;
    [TextArea] public string missionInstructions;
    [SerializeField] private float typeSpeed = 0.05f;

    [Header("Win / Loss Screens")]
    [SerializeField] private CanvasGroup winScreen;
    [SerializeField] private CanvasGroup lossScreen;
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        if (missionText != null)
        {
            StartCoroutine(PlayTypewriterEffect(missionInstructions));
        }

        InitCanvasGroup(winScreen);
        InitCanvasGroup(lossScreen);
    }

    private void InitCanvasGroup(CanvasGroup cg)
    {
        if (cg != null)
        {
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            cg.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayTypewriterEffect(string text)
    {
        missionText.text = "";
        foreach (char c in text)
        {
            missionText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void ShowWinScreen() => ShowCanvasGroup(winScreen);
    public void ShowLossScreen() => ShowCanvasGroup(lossScreen);
    public void HideWinScreen() => HideCanvasGroup(winScreen);
    public void HideLossScreen() => HideCanvasGroup(lossScreen);

    private void ShowCanvasGroup(CanvasGroup cg)
    {
        if (cg != null)
        {
            cg.gameObject.SetActive(true);
            cg.interactable = true;
            cg.blocksRaycasts = true;
            StartCoroutine(FadeCanvasGroup(cg, 0, 1, fadeDuration));
        }
    }

    private void HideCanvasGroup(CanvasGroup cg)
    {
        if (cg != null)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
            StartCoroutine(FadeOutAndDisable(cg, fadeDuration));
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float elapsed = 0f;
        cg.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;
    }

    private IEnumerator FadeOutAndDisable(CanvasGroup cg, float duration)
    {
        yield return FadeCanvasGroup(cg, cg.alpha, 0, duration);
        cg.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
