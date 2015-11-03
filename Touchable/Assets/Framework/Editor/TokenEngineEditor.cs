using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TokenEngine))]
public class TokenEngineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TokenEngine tEngine = (TokenEngine) target;

        DrawDefaultInspector(); 
    }
}