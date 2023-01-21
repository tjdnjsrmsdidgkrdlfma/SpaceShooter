using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button start_button;
    public Button option_button;
    public Button shop_button;

    UnityAction action;

    void Start()
    {
        action = () => OnButtonClick(start_button.name);

        start_button.onClick.AddListener(action);

        option_button.onClick.AddListener(delegate { OnButtonClick(option_button.name); });

        shop_button.onClick.AddListener(() => OnButtonClick(shop_button.name));
    }

    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg}");
    }
}
