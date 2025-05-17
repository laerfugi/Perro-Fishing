using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

// This script should be placed on an object that is in every scene to handle audio
// Audio can be added within the inspector and then played by any script referencing the instance
public class AudioManager : MonoBehaviour
{
    private AudioSource backgroundAudioSource;
    private AudioSource fullSoundAudioSource;
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Modifier
    {
        public ModifierType type;
        public float value;
        public float minValue;
        public float maxValue;
    }

    public enum ModifierType
    {
        Volume,
        Pitch,
        RandomPitch
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public List<Modifier> modifiers;
    }

    public List<Sound> sounds;
    public List<(AudioSource, float)> audioSources;
    private Dictionary<string, Sound> soundDictionary;
    private AudioSource audioSource;
    public bool isSoundEnabled;
    public bool isMusicEnabled;

    // PlayerPrefs save settings key
    private const string SoundEnabledKey = "SoundEnabled";
    private const string MusicEnabledKey = "MusicEnabled";
    private const string SoundVolumeKey = "SoundVolume";
    private const string MusicVolumeKey = "MusicVolume";

    [Header("Current Settings")]
    [SerializeField][Range(0f, 1f)] public float soundVolume = 1f;
    [SerializeField][Range(0f, 1f)] public float musicVolume = 1f;

    // Keep alive between scenes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSounds();
            LoadSettings();
            PlayBackgroundMusic("BGM");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSounds()
    {
        soundDictionary = new Dictionary<string, Sound>();
        foreach (var sound in sounds)
        {
            soundDictionary[sound.name] = sound;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
        fullSoundAudioSource = gameObject.AddComponent<AudioSource>();

        backgroundAudioSource.loop = true;
        backgroundAudioSource.playOnAwake = false;

        fullSoundAudioSource.loop = false;
        fullSoundAudioSource.playOnAwake = false;
    }

    public void AddAudioSource(AudioSource currSource)
    {
        audioSources.Add((currSource, currSource.volume));
    }

    // For sounds that need to be spammed and overlap each other
    public void PlaySound(string soundName)
    {
        if (!isSoundEnabled) return;

        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            if (sound.clip != null)
            {
                audioSource.pitch = GetPitchValue(sound);
                audioSource.volume = soundVolume * GetModifierValue(sound, ModifierType.Volume);
                audioSource.PlayOneShot(sound.clip);
            }
        }
    }

    public void PlayBackgroundMusic(string soundName)
    {
        if (!isMusicEnabled) return;

        if (backgroundAudioSource.isPlaying) return;

        if (soundDictionary.TryGetValue(soundName, out Sound sound) && sound.clip != null)
        {
            backgroundAudioSource.clip = sound.clip;
            backgroundAudioSource.volume = musicVolume * GetModifierValue(sound, ModifierType.Volume);
            backgroundAudioSource.Play();
        }
    }

    // For sounds that need to completely play before playing another
    public void PlayFullSound(string soundName)
    {
        if (!isSoundEnabled) return;

        if (fullSoundAudioSource.isPlaying) return;

        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            if (sound.clip != null)
            {
                fullSoundAudioSource.clip = sound.clip;
                fullSoundAudioSource.pitch = GetPitchValue(sound);
                fullSoundAudioSource.volume = soundVolume * GetModifierValue(sound, ModifierType.Volume);
                fullSoundAudioSource.Play();
            }
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
    }

    public void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled;
        SaveSettings();
        if (!isMusicEnabled)
        {
            StopBackgroundMusic();
        }
        else
        {
            PlayBackgroundMusic("BGM");
        }
    }

    public void ToggleSound()
    {
        isSoundEnabled = !isSoundEnabled;
        SaveSettings();
    }

    // Clamps volume between 0 and 1
    public void SetSoundVolume(float volume)
    {
        soundVolume = Mathf.Clamp01(volume);
        //if (audioSources.Count > 0)
        //{
        //    foreach ((AudioSource aud, float vol) in audioSources) { aud.volume = vol * volume; }
        //}
        SaveSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        backgroundAudioSource.volume = musicVolume;
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt(SoundEnabledKey, isSoundEnabled ? 1 : 0);
        PlayerPrefs.SetInt(MusicEnabledKey, isMusicEnabled ? 1 : 0);
        PlayerPrefs.SetFloat(SoundVolumeKey, soundVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        isSoundEnabled = PlayerPrefs.GetInt(SoundEnabledKey, 1) == 1;
        isMusicEnabled = PlayerPrefs.GetInt(MusicEnabledKey, 1) == 1;
        soundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 1f);
        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    }

    private float GetModifierValue(Sound sound, ModifierType type)
    {
        Modifier modifier = sound.modifiers?.Find(m => m.type == type);
        return modifier != null ? modifier.value : 1f; // Default value of 1
    }

    // Get pitch from modifiers if random
    private float GetPitchValue(Sound sound)
    {
        Modifier randomPitchModifier = sound.modifiers?.Find(m => m.type == ModifierType.RandomPitch);
        if (randomPitchModifier != null)
        {
            return Random.Range(randomPitchModifier.minValue, randomPitchModifier.maxValue);
        }
        return GetModifierValue(sound, ModifierType.Pitch);
    }
}

#region Utility
//// Custom GUI for Modifiers
//[CustomPropertyDrawer(typeof(AudioManager.Modifier))]
//public class ModifierPropertyDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);

//        // Get references
//        var typeProperty = property.FindPropertyRelative("type");
//        var valueProperty = property.FindPropertyRelative("value");
//        var minValueProperty = property.FindPropertyRelative("minValue");
//        var maxValueProperty = property.FindPropertyRelative("maxValue");

//        // Layout for type dropdown
//        var typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//        // Layout for value
//        var valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);

//        // Draw the layout for type dropdown
//        EditorGUI.PropertyField(typeRect, typeProperty);

//        // If random pitch
//        if ((AudioManager.ModifierType)typeProperty.enumValueIndex == AudioManager.ModifierType.RandomPitch)
//        {
//            // Layout for the min and max fields
//            var minRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight + 2), position.width / 2 - 2, EditorGUIUtility.singleLineHeight);
//            var maxRect = new Rect(position.x + position.width / 2 + 2, position.y + (EditorGUIUtility.singleLineHeight + 2), position.width / 2 - 2, EditorGUIUtility.singleLineHeight);

//            // Draw both the min and max fields
//            EditorGUI.PropertyField(minRect, minValueProperty, new GUIContent("Min"));
//            EditorGUI.PropertyField(maxRect, maxValueProperty, new GUIContent("Max"));
//        }
//        else
//        {
//            // Draw the layout for any other modifier
//            EditorGUI.PropertyField(valueRect, valueProperty);
//        }

//        EditorGUI.EndProperty();
//    }

//    // Add extra padding to the bottom of everything
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        var typeProperty = property.FindPropertyRelative("type");
//        if ((AudioManager.ModifierType)typeProperty.enumValueIndex == AudioManager.ModifierType.RandomPitch)
//        {
//            return EditorGUIUtility.singleLineHeight * 2;
//        }
//        return EditorGUIUtility.singleLineHeight * 2 + 2;
//    }
//}

//[CustomEditor(typeof(AudioManager))]
//public class AudioManagerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        AudioManager audioManager = (AudioManager)target;

//        // Add a button to force save the settings set in inspector
//        if (GUILayout.Button("Force Save Volume Settings"))
//        {
//            audioManager.SetSoundVolume(audioManager.soundVolume);
//            audioManager.SetMusicVolume(audioManager.musicVolume);
//        }
//    }
//}
#endregion