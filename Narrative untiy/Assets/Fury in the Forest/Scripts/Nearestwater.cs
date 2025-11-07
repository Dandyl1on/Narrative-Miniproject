using System;
using Unity.Mathematics;
using UnityEngine;

public class Nearestwater : MonoBehaviour
{

    public Transform player;
    public Transform Pondtransform;
    private float HideDistance = 3f;
    Vector3 faces = new Vector3(1, 1, 1);
    
    void Update()
    {
        if (player.transform.localScale == faces)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        Transform nearestPond = FindNearestPond();
        if (nearestPond == null) return;

        Vector3 dir = nearestPond.position - transform.position;

        if (dir.magnitude < HideDistance)
        {
            setChildrenActive(false);
        }
        else
        {
            setChildrenActive(true);
            
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Pondtransform.rotation = Quaternion.identity;
        }
        
    }

    Transform FindNearestPond()
    {
        GameObject[] ponds = GameObject.FindGameObjectsWithTag("Pond");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject pond in ponds)
        {
            float dist = Vector3.Distance(player.position, pond.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = pond.transform;
            }
        }

        return nearest;
    }
    void setChildrenActive(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}