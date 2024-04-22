using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindMatchUI : MonoBehaviour
{
    [SerializeField] private Button findMatchButton;
    [SerializeField] private Button backButton;

    [SerializeField] private GameObject mainMenu;

    [SerializeField] private bool isFindingMatch = false;

    private void Awake()
    {
        findMatchButton.onClick.AddListener(() =>
        {
            if (!isFindingMatch)
            {
                isFindingMatch = true;
            }

            else
            {
                isFindingMatch = false;
            }
        });

        backButton.onClick.AddListener(() =>
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
