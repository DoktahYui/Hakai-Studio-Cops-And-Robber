using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyUITest : MonoBehaviour
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
