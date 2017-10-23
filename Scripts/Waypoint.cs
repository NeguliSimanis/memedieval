using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint previous;
    [SerializeField] private Waypoint next;
    [SerializeField] private bool Castle;
    [SerializeField] private bool Player;


    void OnDrawGizmos()
    {
        if (Next == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, Next.transform.position);
    }


    public Waypoint Next
    {
        get
        {
            return next;
        }
    }


    public Waypoint Previous
    {
        get
        {
            return previous;
        }
    }
}
