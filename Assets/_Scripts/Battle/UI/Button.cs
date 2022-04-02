using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
        public Text text;
    public void OnPointerButton()
    {
        text.color = Color.blue;
    }
    public void OnExitButton()
    {
        text.color = Color.black;
    }
}
