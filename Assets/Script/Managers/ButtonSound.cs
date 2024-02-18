using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void ToggleButtonClickSound(bool isOn)
    {
        // Om isOn är falskt, stäng av ljudet
        if (!isOn)
        {
            audioSource.Stop();
        }
    }
}
