using UnityEngine;

public class ResetPlayerPref : MonoBehaviour
{
    public void ResetInt(string name)
    {
        PlayerPrefs.SetInt(name, 0);
    }
    public void ResetFloat(string name)
    {
        PlayerPrefs.SetInt(name, 0);
    }
    public void ResetString(string name)
    {
        PlayerPrefs.SetString(name, "");
    }
}
