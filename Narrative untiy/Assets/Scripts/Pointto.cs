using System;
using Unity.Mathematics;
using UnityEngine;

public class Pointto : MonoBehaviour
{
    public Transform animal;

    private float HideDistance = 10f;

    public Transform player;

    public Transform icon;
    
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

        var dir = animal.position - transform.position;

        if (dir.magnitude < HideDistance)
        {
            setChildrenActive(false);
        }
        else
        {
            setChildrenActive(true);
            
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            icon.rotation = Quaternion.identity;
        }
    }

    void setChildrenActive(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}
