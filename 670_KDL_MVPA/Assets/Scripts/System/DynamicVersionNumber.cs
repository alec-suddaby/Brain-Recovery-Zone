using UnityEngine;
using System.Collections;
using TMPro;

public class DynamicVersionNumber : MonoBehaviour
{
    void Awake()
    {
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();
        textmeshPro.text = "Application Version : " + Application.version;
        Debug.Log("Application Version : " + Application.version);
    }
}
