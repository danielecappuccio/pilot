
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WSA_10_0)
using UnityEngine.Windows.Speech;
#endif

/// <summary>
///  Turns speech input into a UnityEvent.
/// </summary>
/// <remarks>
///  This behaviour only works under windows and Universal Windows Platform 
///  (so also on HoloLens).
/// </remarks>
public class WindowsSpeechRecognitionBehaviour : MonoBehaviour
{
    [System.Serializable]
    public class OnKeywordEvent : UnityEvent { }

    [System.Serializable]
    public class VoiceCommand
    {
        public string keyWord;

        public OnKeywordEvent command;
    }

    public VoiceCommand[] voiceCommands;

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WSA_10_0)

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, OnKeywordEvent> keywords = 
        new Dictionary<string, OnKeywordEvent>();

    private void KeywordRecognizer_OnPhraseRecognized(
        PhraseRecognizedEventArgs args)
    {
        OnKeywordEvent keywordEvent;
        // if the keyword recognized is in our dictionary, call that Action.
        if (this.keywords.TryGetValue(args.text, out keywordEvent))
        {
            keywordEvent.Invoke();
        }
    }

    private void Awake()
    {
        foreach(VoiceCommand voiceCommand in this.voiceCommands)
        {
            //Create keywords for keyword recognizer
            this.keywords.Add(voiceCommand.keyWord, voiceCommand.command);
        }

        this.keywordRecognizer = 
            new KeywordRecognizer(this.keywords.Keys.ToArray());
    }

    private void OnEnable()
    {
        this.keywordRecognizer.OnPhraseRecognized += 
            this.KeywordRecognizer_OnPhraseRecognized;
        this.keywordRecognizer.Start();
    }

    private void OnDisable()
    {
        this.keywordRecognizer.Stop();
        this.keywordRecognizer.OnPhraseRecognized -= 
            this.KeywordRecognizer_OnPhraseRecognized;
    }
#else // Empty dummy implementation
    private void Awake()
    {
    }

    private void Start()
    {
        Debug.LogWarning("[vlUnitySDK] The WindowsSpeechRecognitionBehaviour only works for Windows and Windows Store applications");
    }

    private void Update()
    {
    }
#endif
}