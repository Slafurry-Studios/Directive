using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogData", menuName = "Dialog/Dialog Data")]
public class DialogData : ScriptableObject
{
    public List<DialogEntry> dialogEntries;
}

[System.Serializable]
public class DialogEntry
{
    [TextArea(2, 5)]
    public string text;

    public TextColor color;
}

public enum TextColor
{
    White,
    Purple,
    Green,
    Red
}