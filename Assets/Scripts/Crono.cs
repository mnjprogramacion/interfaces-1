using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StopwatchUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public Button startPauseButton;
    public GameObject resetButtonObj;
    public GameObject lapButtonObj;

    [Header("Lap UI")]
    public Transform lapsContainer;
    public GameObject lapItemPrefab;

    [Header("Start/Pause Visuals")]
    public Image startPauseImage;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Color playColor = Color.green;
    public Color pauseColor = Color.red;

    [Header("Lap Colors")]
    public Color bestColor = Color.blue;
    public Color worstColor = Color.red;
    public Color defaultColor = Color.white;

    private bool isRunning = false;
    private float elapsedTime = 0f;

    // Guardamos tiempos y referencias a los objetos
    private List<float> lapTimes = new List<float>();
    private List<GameObject> lapObjects = new List<GameObject>();
    private int lapCounter = 0;

    void Start()
    {
        startPauseButton.onClick.AddListener(OnStartPause);
        resetButtonObj.GetComponent<Button>().onClick.AddListener(OnReset);
        lapButtonObj.GetComponent<Button>().onClick.AddListener(OnLap);

        resetButtonObj.SetActive(false);
        lapButtonObj.SetActive(false);

        if (startPauseImage != null && playSprite != null)
        {
            startPauseImage.sprite = playSprite;
            startPauseImage.color = playColor;
        }

        UpdateTimerDisplay();
        UpdateStartPauseVisuals();
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void OnStartPause()
    {
        isRunning = !isRunning;
        if (elapsedTime == 0f && isRunning)
        {
            resetButtonObj.SetActive(true);
            lapButtonObj.SetActive(true);
        }
        UpdateStartPauseVisuals();
    }

    void OnReset()
    {
        isRunning = false;
        elapsedTime = 0f;
        lapCounter = 0;
        lapTimes.Clear();

        foreach (var obj in lapObjects)
            Destroy(obj);
        lapObjects.Clear();

        resetButtonObj.SetActive(false);
        lapButtonObj.SetActive(false);
        UpdateTimerDisplay();
        UpdateStartPauseVisuals();
    }

    void OnLap()
    {
        lapCounter++;
        float currentTime = elapsedTime;
        lapTimes.Add(currentTime);

        GameObject item = Instantiate(lapItemPrefab, lapsContainer);
        lapObjects.Add(item);

        TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = $"{lapCounter}";
        texts[1].text = FormatTime(currentTime);

        if (lapsContainer.childCount > 3)
        {
            Destroy(lapsContainer.GetChild(0).gameObject);
            lapObjects.RemoveAt(0);
            lapTimes.RemoveAt(0);
        }

        UpdateLapColors();
    }

    void UpdateLapColors()
    {
        if (lapTimes.Count == 0) return;

        float minTime = Mathf.Min(lapTimes.ToArray());
        float maxTime = Mathf.Max(lapTimes.ToArray());

        for (int i = 0; i < lapObjects.Count; i++)
        {
            var item = lapObjects[i];
            var texts = item.GetComponentsInChildren<TextMeshProUGUI>();
            var image = item.GetComponent<Image>();      // fondo con esquinas curvas
            var outline = item.GetComponent<Outline>();  // outline

            Color c = defaultColor;
            if (lapTimes[i] == minTime) c = bestColor;
            else if (lapTimes[i] == maxTime) c = worstColor;

            // Cambiar color de texto y borde
            texts[0].color = c;
            texts[1].color = c;
            if (image != null) image.color = new Color(image.color.r, image.color.g, image.color.b, 0.2f); // fondo semitransparente opcional
            if (outline != null) outline.effectColor = c;
        }
    }

    void UpdateTimerDisplay()
    {
        timerText.text = FormatTime(elapsedTime);
    }

    void UpdateStartPauseVisuals()
    {
        var txt = startPauseButton.GetComponentInChildren<TextMeshProUGUI>();
        txt.text = isRunning ? "Pausar" : (elapsedTime == 0f ? "Empezar" : "Reanudar");

        if (startPauseImage != null)
        {
            startPauseImage.sprite = isRunning ? pauseSprite : playSprite;
            startPauseImage.color = isRunning ? pauseColor : playColor;
        }
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int centiseconds = (int)((time * 100f) % 100f);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, centiseconds);
    }
}
