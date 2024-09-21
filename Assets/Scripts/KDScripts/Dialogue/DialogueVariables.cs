using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueVariables : MonoBehaviour, IDataPersistence
{
    private Dictionary<string, Ink.Runtime.Object> inkVariables;
    public void SetUpVariables(TextAsset loadGlobalsJSON)
    {
        // compile GLOBALS story
        Story globalVariablesStory = new Story(loadGlobalsJSON.text);

        // initialize dictionary
        inkVariables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            inkVariables.Add(name, value);
        }
    }

    public void StartListening(Story story)
    {
        // important that VariablesToStory is called before assigning the listener
        VarsToStory(story);
        story.variablesState.variableChangedEvent += inkVarChanged;
    }
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= inkVarChanged;
    }
    private void inkVarChanged(string name, Ink.Runtime.Object value)
    {
        if(inkVariables.ContainsKey(name))
        {
            inkVariables.Remove(name);
            inkVariables.Add(name, value);
        }
    }
    private void VarsToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> var in inkVariables) 
        {
            story.variablesState.SetGlobal(var.Key, var.Value);
        }
    }
    public void LoadData(GameData data)
    {
        //throw new System.NotImplementedException();
    }
    public void SaveData(GameData data)
    {
        //throw new System.NotImplementedException();
    }
}
