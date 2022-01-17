using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeletePlayerPrefabs : MonoBehaviour
{
    [MenuItem("Tools/Reset Player Pref")]
    public static void ResetPlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }
}
