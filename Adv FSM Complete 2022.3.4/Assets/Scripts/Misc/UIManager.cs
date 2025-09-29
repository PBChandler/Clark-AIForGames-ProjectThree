using UnityEngine;
using TMPro;
using System.Collections;

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

        if (winScreen != null) winScreen.alpha = 0;
        if (lossScreen != null) lossScreen.alpha = 0;
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

    public void ShowWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(winScreen, 0, 1, fadeDuration));
        }
    }

    public void ShowLossScreen()
    {
        if (lossScreen != null)
        {
            lossScreen.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(lossScreen, 0, 1, fadeDuration));
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
}
