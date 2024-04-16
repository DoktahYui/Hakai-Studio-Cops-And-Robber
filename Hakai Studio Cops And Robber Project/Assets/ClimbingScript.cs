using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingScript : MonoBehaviour
{
    [SerializeField] private GameObject topPosition;
    [SerializeField] private GameObject bottomPosition;
    [SerializeField] private GameObject leftPosition;
    [SerializeField] private GameObject rightPosition;

    private Vector3 topDir;
    private Vector3 bottomDir;
    private Vector3 leftDir;
    private Vector3 rightDir;

    public void Update()
    {
        topDir = (topPosition.transform.position - transform.position).normalized;
        bottomDir = (bottomPosition.transform.position - transform.position).normalized;
        leftDir = (leftPosition.transform.position - transform.position).normalized;
        rightDir = (rightPosition.transform.position - transform.position).normalized;

        ShowRay(topDir, bottomDir, leftDir, rightDir);
    }

    void ShowRay(Vector3 top, Vector3 bottom, Vector3 left, Vector3 right)
    {
        Debug.DrawRay(transform.position, top, Color.red, 0.1f);
        Debug.DrawRay(transform.position, bottom, Color.red, 0.1f);
        Debug.DrawRay(transform.position, left, Color.red, 0.1f);
        Debug.DrawRay(transform.position, right, Color.red, 0.1f);
    }
}
