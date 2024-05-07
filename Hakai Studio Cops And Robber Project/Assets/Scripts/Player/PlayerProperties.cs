using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    [SerializeField] private int playerGroup = 0;
    public int PlayerGroup { get { return playerGroup; } }

    private void Start()
    {
        gameObject.transform.position = new Vector3(0f, 20f, 0f);
    }
}
