using UnityEngine;

public class StoryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SimpleQTE qte;  // Reference til dit QTE-script

    /// <summary>
    /// Kald denne metode, når QTE'en skal starte.
    /// </summary>
    public void TriggerQTE()
    {
        if (qte != null)
        {
            qte.StartQTE();
        }
        else
        {
            Debug.LogWarning("StoryController: QTE reference mangler.");
        }
    }

    /// <summary>
    /// Kaldes når spilleren klarer QTE'en.
    /// Tilknyttes via OnQTESuccess i SimpleQTE.
    /// </summary>
    public void OnQTESuccessHandler()
    {
        Debug.Log("QTE success – fortsæt historien her.");
        // TODO: indsæt din logik: næste scene, dialog, animation osv.
    }

    /// <summary>
    /// Kaldes når spilleren fejler QTE'en.
    /// Tilknyttes via OnQTEFail i SimpleQTE.
    /// </summary>
    public void OnQTEFailHandler()
    {
        Debug.Log("QTE fail – branch/følger her.");
        // TODO: indsæt din logik: død, alternativ gren, skade osv.
    }
}
