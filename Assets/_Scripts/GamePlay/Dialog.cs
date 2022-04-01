using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [TextArea]
    [SerializeField] List<string> _lines;

    public List<string> Lines
    {
        get { return _lines; }
    }
}
