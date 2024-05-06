using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyUI : NetworkBehaviour
{
    [SerializeField] private Button readyButton;

    public void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
                PlayerReadyManager.Instance.SetPlayerReady();
        });
    }
}
