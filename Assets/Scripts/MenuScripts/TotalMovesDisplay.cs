using TMPro;
using UnityEngine;

public class TotalMovesDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private void Start()
    {
        text.text = PlayerPrefs.GetInt("Moves").ToString();
    }
}