using UnityEngine;
using UnityEngine.UI;

public class AudioCheck : MonoBehaviour
{
    public Toggle audioToggle;
    private bool isMuted;
    void Start()
    {
        isMuted = PlayerPrefs.GetInt("isMuted", 1) == 1;
        AudioListener.pause = isMuted;

        if (audioToggle != null)
        {
            audioToggle.isOn = !isMuted;
            audioToggle.onValueChanged.AddListener(ToggleAudio);

        }
    }
    public void ToggleAudio(bool isOn)
    {
        isMuted = !isOn;
        AudioListener.pause = isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}
