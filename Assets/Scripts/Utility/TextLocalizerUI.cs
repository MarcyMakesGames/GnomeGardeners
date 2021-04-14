using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalizerUI : MonoBehaviour
{
    TextMeshProUGUI textField;

    public LocalizedString localizedString;

    #region Unity Methods

    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = localizedString.Value;
    }

    #endregion
}
