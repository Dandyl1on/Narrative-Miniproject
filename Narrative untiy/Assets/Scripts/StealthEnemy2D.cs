using DialogueEditor;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class StealthEnemy2D : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Chase
    }

    [Header("Patrol")]
    [SerializeField] private Transform patrolPointA;
    [SerializeField] private Transform patrolPointB;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolSnapDistance = 0.1f;    // hvor tæt på før vi snapper/skifter

    [Header("Chase")]
    [SerializeField] private Transform player;          // Drag Player her
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float attackDistance = 1.5f;

    [Header("QTE / Story")]
    [SerializeField] private StoryController storyController;

    [Header("Sprite")]
    [SerializeField] private bool spriteFacesRightByDefault = true;

   

    private State state = State.Patrol;
    private Transform currentPatrolTarget;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private bool qteActive = false;
    

    [SerializeField] private NPCConversation nazChoice;

    public bool DisableChase;
    public float chaseTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        if (sprite == null)
        {
            Debug.LogWarning("[StealthEnemy2D] Ingen SpriteRenderer fundet på enemy eller children.");
        }
    }

    private void Start()
    {
        currentPatrolTarget = patrolPointA;
    }

    public void stopmoving(float s)
    {
        patrolSpeed = s;
        chaseSpeed = s;
    }

    private void Update()
    {
        if (DisableChase)
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0)
            {
                DisableChase = false;
            }
        }
        
        // Hvis QTE kører, står han stille vandret
        if (qteActive)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        switch (state)
        {
            case State.Patrol:
                PatrolUpdate();
                break;

            case State.Chase:
                ChaseUpdate();
                break;
        }

        //AntiStuckCheck();
    }

    // ---------------- PATROL ----------------

    private void PatrolUpdate()
    {
        if (patrolPointA == null || patrolPointB == null)
            return;

        float targetX = currentPatrolTarget.position.x;
        float diff = targetX - transform.position.x;

        // Hvis vi er meget tæt på målet → snap og skift waypoint
        if (Mathf.Abs(diff) <= patrolSnapDistance)
        {
            Vector3 pos = transform.position;
            pos.x = targetX;
            transform.position = pos;

            currentPatrolTarget = (currentPatrolTarget == patrolPointA) ? patrolPointB : patrolPointA;
            diff = currentPatrolTarget.position.x - transform.position.x;
        }

        float dir = Mathf.Sign(diff);
        rb.linearVelocity = new Vector2(dir * patrolSpeed, rb.linearVelocity.y);

        UpdateSpriteFlip(dir);
    }

    // ---------------- CHASE ----------------

    private void ChaseUpdate()
    {

        if (player == null)
            return;

        float diff = player.position.x - transform.position.x;
        float dir = Mathf.Sign(diff);

        rb.linearVelocity = new Vector2(dir * chaseSpeed, rb.linearVelocity.y);
        UpdateSpriteFlip(dir);

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackDistance && !qteActive)
        {
            StartChaseQTE();
        }
    }
    
    // ---------------- HJÆLPEMETODER ----------------

    private void UpdateSpriteFlip(float dir)
    {
        if (sprite == null) return;
        if (Mathf.Abs(dir) < 0.01f) return;

        if (spriteFacesRightByDefault)
        {
            sprite.flipX = dir < 0f;
        }
        else
        {
            sprite.flipX = dir > 0f;
        }
    }

    private void StartChaseQTE()
    {
        if (qteActive) return;

        qteActive = true;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        if (storyController != null)
        {
            Debug.Log("[StealthEnemy2D] Starting chase QTE.");
            storyController.TriggerQTE();
        }
        else
        {
            Debug.LogWarning("[StealthEnemy2D] StoryController missing.");
        }
    }

    // ---------------- KALDES FRA EYESCANNER2D ----------------

    public void OnPlayerSpotted()
    {
        if (DisableChase)
        {
            return;
        }
        
        if (!qteActive && state == State.Patrol)
        {
            ConversationManager.Instance.StartConversation(nazChoice);
            Debug.Log("[StealthEnemy2D] Player spotted – switching to Chase.");
            state = State.Chase;
        }
    }

    public void DisableDetection(float duration)
    {
        DisableChase = true;
        chaseTimer = duration;
    }

    // ---------------- KALDES FRA STORYCONTROLLER ----------------

    /// <summary>
    /// Kald denne fra StoryController når QTE er slut (success eller fail).
    /// </summary>
    public void ResetAfterQTE()
    {
        Debug.Log("[StealthEnemy2D] ResetAfterQTE – back to patrol.");
        qteActive = false;
        state = State.Patrol;

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // Reset anti-stuck timers
    }
}
