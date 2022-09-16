using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomToolsGUIStyles
{
    public static GUIStyle CustomLabel(bool center, bool bold, bool wordWrap)
    {
        TextAnchor anchor = center ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
        FontStyle style = bold ? FontStyle.Bold : FontStyle.Normal;
        return new GUIStyle(GUI.skin.label) { alignment = anchor, fontStyle = style, wordWrap = wordWrap};
    }
    public static GUIStyle CustomColorLabel(bool center, bool bold, bool wordWrap, Color color)
    {
        TextAnchor anchor = center ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
        FontStyle style = bold ? FontStyle.Bold : FontStyle.Normal;
        return new GUIStyle(GUI.skin.label) { alignment = anchor, fontStyle = style, wordWrap = wordWrap, normal = { textColor = color }, hover = { textColor = color } };
    }
}
