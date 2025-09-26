using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class StopwatchUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public Button startPauseButton;
    public GameObject resetButtonObj;   // Usaremos GameObject para SetActive
    public GameObject lapButtonObj;
    public TextMeshProUGUI lapsText;

    private bool isRunning = false;
    private float elapsedTime = 0f;
    private List<string> laps = new List<string>();

    void Start()
    {
        // Asigna los listeners
        startPauseButton.onClick.AddListener(OnStartPause);
        resetButtonObj.GetComponent<Button>().onClick.AddListener(OnReset);
        lapButtonObj.GetComponent<Button>().onClick.AddListener(OnLap);

        // Estado inicial
        resetButtonObj.SetActive(false);
        lapButtonObj.SetActive(false);

        UpdateTimerDisplay();
        UpdateStartPauseLabel();
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
        UpdateStartPauseLabel();

        // Mostrar botones Reset/Lap solo si ya se inició
        if (elapsedTime == 0f && isRunning)
        {
            resetButtonObj.SetActive(true);
            lapButtonObj.SetActive(true);
        }
    }

    void OnReset()
    {
        isRunning = false;
        elapsedTime = 0f;
        laps.Clear();
        lapsText.text = "";

        resetButtonObj.SetActive(false);
        lapButtonObj.SetActive(false);

        UpdateTimerDisplay();
        UpdateStartPauseLabel();
    }

    void OnLap()
    {
        string lapTime = elapsedTime.ToString("F2");
        laps.Add(lapTime);
        lapsText.text = string.Join("\n", laps);
    }

    void UpdateTimerDisplay()
    {
        timerText.text = elapsedTime.ToString("F2");
    }

    void UpdateStartPauseLabel()
    {
        startPauseButton.GetComponentInChildren<TextMeshProUGUI>().text =
            isRunning ? "Pausar" : (elapsedTime == 0f ? "Empezar" : "Reanudar");
    }
}
