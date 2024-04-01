using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    [SerializeField] private int playerGroup = 0;
    public int PlayerGroup { get { return playerGroup; } }
}
