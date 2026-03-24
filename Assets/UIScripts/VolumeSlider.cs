using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    // objects for the audio mixer and slider
    public AudioMixer mixer;
    public Slider volumeSlider;

    // name for where the inputted slider value will be saved in memory
    const string VOLUME_KEY = "MasterVolumeValue";

    // sets the initial value of the volume slider and allows the volume to be changed
    void Start()
    {
        float savedValue = LoadVolume();
        volumeSlider.value = savedValue;
        SetVolume(savedValue);

        volumeSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    // sets necessary variables to the inputted slider value
    void OnSliderChanged(float value)
    {
        SetVolume(value);
        SaveVolume(value);
    }

    // sets the level that ingame music will be played at
    public void SetVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        mixer.SetFloat("MasterVolume", volume);
    }

    // saves the player's volume choice into the game's files
    public void SaveVolume(float value)
    {
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    // loads the volume based on the previous value set by the player
    public float LoadVolume()
    {
        return PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
    }
}