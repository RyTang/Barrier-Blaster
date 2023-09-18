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

        int i = 0;
        foreach (FieldInfo variable in properties) {
            if (i == 2){
                break;
            }
            UiScoreRow row = Instantiate(uiRow, uiDisplayArea).GetComponent<UiScoreRow>();
            row.variable.SetText(variable.Name);
            string text = variable.GetValue(gameData).ToString();
            if (text.Length > 1 && text.Contains(".")) {  // Only remove decimals if more than 1 character
                text = text.Substring(0, text.IndexOf(".", 0));
            }
            row.score.SetText(text);
            i++;
        }
    }
}
