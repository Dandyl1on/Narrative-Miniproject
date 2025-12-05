using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    [Header("QTE")]
    [SerializeField] private SimpleQTE qte;          // Drag dit QTE_Controller-objekt ind
    [Tooltip("Skal QTE kun køre én ad gangen?")]
    [SerializeField] private bool preventMultipleQTEs = true;

    private bool qteRunning = false;

    [Header("Death / Scene Flow (optional)")]
    [Tooltip("Navn på death-scene der skal loades ved QTE-fail. Hvis tom, loades der ingen scene.")]
    [SerializeField] private string deathSceneName;

    [Header("Stealth Enemy (optional)")]
    [Tooltip("Fjenden som skal resettes efter QTE (for at undgå freeze).")]
    [SerializeField] private StealthEnemy2D[] stealthEnemy;

    private bool Mountdoombool;
    


    // --------------------------------------------------------------------
    // PUBLIC API – kaldes fra andre scripts (EyeScanner2D, Convo, osv.)
    // --------------------------------------------------------------------

    /// <summary>
    /// Start en QTE-sekvens via SimpleQTE.
    /// Kaldes f.eks. fra StealthEnemy2D, EyeScanner2D, Convo-trigger osv.
    /// </summary>
    public void SetQTESequence(string seq)
    {
        if (qte == null)
        {
            Debug.LogWarning("StoryController: No QTE reference.");
            return;
        }
        qte.SetSequence(seq);
    }
    public void TriggerQTE()
    {
        if (qte == null)
        {
            Debug.LogWarning("[StoryController] TriggerQTE kaldt, men SimpleQTE-reference mangler.");
            return;
        }

        if (preventMultipleQTEs && qteRunning)
        {
            Debug.Log("[StoryController] TriggerQTE ignoreret – QTE kører allerede.");
            return;
        }

        Debug.Log("[StoryController] TriggerQTE – starter QTE.");
        qteRunning = true;


        // Antager at SimpleQTE har en public StartQTE()-metode,
        // som bruger sine egne serialized settings (sequence, timer osv.).
        
        //qte.StartQTE();
    }

    // --------------------------------------------------------------------
    // CALLBACKS FRA SIMPLEQTE (hookes via UnityEvents på SimpleQTE)
    // --------------------------------------------------------------------

    /// <summary>
    /// Kaldet af SimpleQTE.OnQTESuccess (via UnityEvent).
    /// </summary>
    public void OnQTESuccessHandler()
    {
        Debug.Log("[StoryController] QTE SUCCESS.");
        qteRunning = false;

        // Fjende må ikke blive frosset – reset bevægelse/patrol.
        if (stealthEnemy != null)
        {
            foreach (var enemy in stealthEnemy)
            {
                enemy.ResetAfterQTE();
            }
        }

        if (Mountdoombool)
        {
            MountDoom();
        }
        
        // Her kan du fortsætte story logik, hvis du har behov.
        // Fx: gå til næste story step, enable næste trigger osv.
    }

    /// <summary>
    /// Kaldet af SimpleQTE.OnQTEFail (via UnityEvent).
    /// </summary>
    public void OnQTEFailHandler()
    {
        Debug.Log("[StoryController] QTE FAIL.");
        qteRunning = false;

        // Reset fjenden, så han ikke hænger fast i QTE-state,
        // selvom vi evt. loader en ny scene bagefter.
        if (stealthEnemy != null)
        {
            foreach (var enemy in stealthEnemy)
            {
                enemy.ResetAfterQTE();
            }
        }

        // Hvis du vil loade en death-scene ved fail:
        if (!string.IsNullOrEmpty(deathSceneName))
        {
            Debug.Log($"[StoryController] Loader death scene '{deathSceneName}'.");
            SceneManager.LoadScene(deathSceneName);
        }

        // Hvis du ikke vil loade scene, men fx bare resette level eller skade spilleren,
        // kan du lægge den logik her i stedet.
    }

    public void setMountdoombool()
    {
        Mountdoombool = true;
    }

    public void MountDoom()
    {
        SceneManager.LoadScene("Mount Doom");
    }
}
