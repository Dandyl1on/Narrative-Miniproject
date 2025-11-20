using DialogueEditor;
using UnityEngine;

public class Convo : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }
}
