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
        NativeDialog.OpenDialog("Jazyk není podporovaný!",
            "Zkontrolujte, jestli používáte Google TTS.",
            "Ok",
            () => { }
            );
    }
}
