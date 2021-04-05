using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : MonoBehaviour
{

    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
       text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Level.IsCurrent)
        {
            text.text = GameManager.Instance.Level.GetTimeAsString();
        }
        else
        {
            text.text = "No current Level";
        }
    }
}
