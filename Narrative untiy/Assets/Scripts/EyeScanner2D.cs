using System;
using DialogueEditor;
using UnityEngine;
using UnityEngine.Events;

public class EyeScanner2D : MonoBehaviour
{
    [Header("View Settings")]
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 60f;

    [Header("Sweep Rotation")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float minSweepAngle = -60f;
    [SerializeField] private float maxSweepAngle = 60f;

    [Header("Layers")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Enemy Link")]
    [SerializeField] private StealthEnemy2D linkedEnemy;

    [Header("Facing (fra fjendens sprite)")]
    [Tooltip("SpriteRenderer på fjenden (den der bliver flipX'et når han vender).")]
    [SerializeField] private SpriteRenderer enemySprite;

    [Tooltip("TRUE hvis fjendens sprite kigger mod højre som default (uden flipX). FALSE hvis den kigger mod venstre.")]
    [SerializeField] private bool spriteFacesRightByDefault = true;

    public UnityEvent OnPlayerSpottedEvent;

    private float baseAngle;
    private float sweepAngle;
    private int sweepDir = 1;

    [SerializeField] private NPCConversation Spotted;
    
    
    private bool detectionLocked = false;   // prevents retriggering
    private bool playerCurrentlyVisible = false; // tracks if player is inside cone

    private void Awake()
    {
        baseAngle = transform.eulerAngles.z;
        sweepAngle = 0f;
    }
    

    private void Update()
    {
        UpdateSweepRotation();
        ScanForPlayer();
    }

    private void UpdateSweepRotation()
    {
        sweepAngle += rotationSpeed * sweepDir * Time.deltaTime;

        if (sweepAngle > maxSweepAngle)
        {
            sweepAngle = maxSweepAngle;
            sweepDir = -1;
        }
        else if (sweepAngle < minSweepAngle)
        {
            sweepAngle = minSweepAngle;
            sweepDir = 1;
        }

        float z = baseAngle + sweepAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, z);
    }

    private Vector2 GetForward()
    {
        // Udgangspunkt: samme som før (cone peger nedad)
        Vector2 forward = -transform.up;

        // Spejl vandret hvis fjenden er vendt (flipX)
        if (enemySprite != null)
        {
            int dir;
            if (spriteFacesRightByDefault)
            {
                // Default = højre. flipX = true => fjenden kigger venstre
                dir = enemySprite.flipX ? -1 : 1;
            }
            else
            {
                // Default = venstre. flipX = true => fjenden kigger højre
                dir = enemySprite.flipX ? 1 : -1;
            }

            // Spejl kun på X-aksen
            forward = new Vector2(forward.x * dir, forward.y);
        }

        return forward.normalized;
    }
    
    
    private void ScanForPlayer()
    {
        bool playerSeenThisFrame = false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerMask);
        Vector2 forward = GetForward();

        foreach (var hit in hits)
        {
            if (hit == null) continue;

            Vector2 dirToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(forward, dirToTarget);

            if (angleToTarget <= viewAngle * 0.5f)
            {
                float distToTarget = Vector2.Distance(transform.position, hit.transform.position);
                int mask = playerMask | obstacleMask;

                RaycastHit2D firstHit = Physics2D.Raycast(
                    transform.position,
                    dirToTarget,
                    distToTarget,
                    mask
                );

                if (firstHit.collider != null && firstHit.collider.CompareTag("Player"))
                {
                    playerSeenThisFrame = true;

                    // Trigger detection ONLY once
                    if (!detectionLocked)
                    {
                        detectionLocked = true;

                        if (linkedEnemy != null)
                            linkedEnemy.OnPlayerSpotted();

                        OnPlayerSpottedEvent?.Invoke();
                    }

                    break; // no need to check more hits
                }
            }
        }

        // Update "player inside cone" state
        playerCurrentlyVisible = playerSeenThisFrame;

        // If player is NOT visible anymore → unlock detection
        if (!playerCurrentlyVisible)
        {
            detectionLocked = false;
        }
    }


    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector2 forward = Application.isPlaying ? GetForward() : -transform.up;
        float halfAngle = viewAngle * 0.5f;
        Vector3 rightDir = Quaternion.Euler(0, 0,  halfAngle) * forward;
        Vector3 leftDir  = Quaternion.Euler(0, 0, -halfAngle) * forward;

        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + leftDir  * viewRadius);
    }
}
