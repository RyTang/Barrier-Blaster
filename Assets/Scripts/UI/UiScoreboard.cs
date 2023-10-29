using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System;
using UnityEngine;

public class UiScoreboard : MonoBehaviour
{
    public GameObject uiRow;
    public Transform uiDisplayArea;
    public GameData gameData;


    public void OnEnable() {

        // Idea taken from https://stackoverflow.com/questions/8151888/c-sharp-iterate-through-class-properties
        Type type = typeof(GameData);
        FieldInfo[] properties = type.GetFields();

        foreach (FieldInfo variable in properties) {
            UiScoreRow row = Instantiate(uiRow, uiDisplayArea).GetComponent<UiScoreRow>();
            
            string pattern = @"(?<=[a-z])(?=[A-Z])";
            string[] parts = Regex.Split(variable.Name, pattern);
            
            List<string> adjusted_parts = new List<string>();
            foreach (string part in parts){
                adjusted_parts.Add(char.ToUpper(part[0]) + part.Substring(1));
            }

            string variableName = string.Join(" ", adjusted_parts);

            row.variable.SetText(variableName);
            string text = variable.GetValue(gameData).ToString();
            if (text.Length > 1 && text.Contains(".")) {  // Only remove decimals if more than 1 character
                text = text.Substring(0, text.IndexOf(".", 0));
            }
            row.score.SetText(text);
        }
    }
}
