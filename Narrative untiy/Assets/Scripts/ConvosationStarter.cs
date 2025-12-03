using System;
using DialogueEditor;
using UnityEngine;

public class Convo : MonoBehaviour
{
    public NPCConversation myConversation;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }
}
