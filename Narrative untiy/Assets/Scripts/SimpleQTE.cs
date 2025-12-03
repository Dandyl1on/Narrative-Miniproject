using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SimpleQTE : MonoBehaviour
{
    [Header("QTE Settings")]
    [Tooltip("Rækkefølgen af bogstaver der skal trykkes, fx WASD")]
    [SerializeField] private string sequence = "WASD";

    [Tooltip("Samlet tid (sekunder) til hele sekvensen")]
    [SerializeField] private float totalTimeLimit = 3.0f;

    [Header("UI (valgfrit)")]
    [SerializeField] private GameObject uiRoot;          // Panel der kan aktiveres/deaktiveres
    [SerializeField] private TextMeshProUGUI promptText; // Viser fx "Press: W"
    [SerializeField] private Image timerFill;            // Progress bar (0–1), valgfri

    [Header("Events")]
    public UnityEvent OnQTESuccess;
    public UnityEvent OnQTEFail;

    private int currentIndex = 0;
    private float elapsed = 0f;
    private bool isActive = false;

    private void Start()
    {
        if (uiRoot != null)
            uiRoot.SetActive(false);
    }

    private void Update()
    {
        if (!isActive)
            return;

        // Opdater tid
        elapsed += Time.deltaTime;
        float remaining = totalTimeLimit - elapsed;

        // Opdater evt. timer-bar
        if (timerFill != null && totalTimeLimit > 0f)
        {
            timerFill.fillAmount = Mathf.Clamp01(remaining / totalTimeLimit);
        }

        // Tid løbet ud → fail
        if (remaining <= 0f)
        {
            Debug.Log("[SimpleQTE] Time out – FAIL");
            Fail();
            return;
        }

        // Færdig med sekvensen? (burde være håndteret i Success, men bare ekstra guard)
        if (currentIndex >= sequence.Length)
            return;

        // Nuværende forventede tast
        char currentChar = char.ToUpper(sequence[currentIndex]);
        KeyCode expectedKey = CharToKeyCode(currentChar);

        // Kun reaktion på den RIGTIGE tast – andre taster ignoreres
        if (Input.GetKeyDown(expectedKey))
        {
            Debug.Log($"[SimpleQTE] Correct key {currentChar} pressed.");

            currentIndex++;

            // Hele sekvensen gennemført
            if (currentIndex >= sequence.Length)
            {
                Debug.Log("[SimpleQTE] Sequence completed – SUCCESS");
                Success();
            }
            else
            {
                UpdateUI();
            }
        }

        // Ingen fail på "forkert tast" her – de ignoreres bare
    }

    /// <summary>
    /// Kald denne for at starte QTE'en.
    /// </summary>
    public void StartQTE()
    {
        if (string.IsNullOrWhiteSpace(sequence))
        {
            Debug.LogWarning("SimpleQTE: sequence er tom.");
            return;
        }

        if (totalTimeLimit <= 0f)
        {
            Debug.LogWarning("SimpleQTE: totalTimeLimit <= 0, sætter den til 1 sekund.");
            totalTimeLimit = 1f;
        }

        Debug.Log("[SimpleQTE] StartQTE() kaldt.");

        isActive = true;
        currentIndex = 0;
        elapsed = 0f;

        if (uiRoot != null)
            uiRoot.SetActive(true);

        if (timerFill != null)
            timerFill.fillAmount = 1f;

        UpdateUI();
    }
    
    public void SetSequence(string newSequence)
    {
        sequence = newSequence.ToUpper();
    }

    private void Success()
    {
        isActive = false;

        if (uiRoot != null)
            uiRoot.SetActive(false);

        OnQTESuccess?.Invoke();
    }

    private void Fail()
    {
        isActive = false;

        if (uiRoot != null)
            uiRoot.SetActive(false);

        OnQTEFail?.Invoke();
    }

    private void UpdateUI()
    {
        if (promptText == null)
            return;

        char currentChar = char.ToUpper(sequence[currentIndex]);
        promptText.text = $"Press: {currentChar}";
    }

    private KeyCode CharToKeyCode(char c)
    {
        // Virker for A–Z (KeyCode.A, KeyCode.B osv.)
        string name = c.ToString().ToUpper();
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), name);
    }
}
