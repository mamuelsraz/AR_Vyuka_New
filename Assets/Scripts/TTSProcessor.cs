using pingak9;
using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using UnityEngine;

public class TTSProcessor : MonoBehaviour
{
    private void Start()
    {
        TextToSpeech.Instance.onLanguageNotAvailable += LanguageNotSupported;
    }

    void LanguageNotSupported()
    {
        NativeDialog.OpenDialog("Jazyk nen� podporovan�!",
            "Zkontrolujte, jestli pou��v�te Google TTS.",
            "Ok",
            () => { }
            );
    }
}
