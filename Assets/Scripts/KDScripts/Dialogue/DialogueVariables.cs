using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueVariables : MonoBehaviour, IDataPersistence
{
    private Dictionary<string, Ink.Runtime.Object> variables;
    //private Story globalVariablesStory;
    public void SetUpVariables(TextAsset loadGlobalsJSON)
    {
        //Debug.Log("Setupvariables");
        // compile GLOBALS story
        Story globalVariablesStory = new Story(loadGlobalsJSON.text);

        // initialize dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            //Debug.Log("Initialize global dialogue variable: " + name + " = " + value);
        }
    }

    public void StartListening(Story story)
    {
        // important that VariablesToStory is called before assigning the listener
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">variable name</param>
    /// <param name="value">value of variable</param>
    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        //Debug.Log("Variable changed: " + name + " = " + value);
        if(variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables) 
        {
            //Debug.Log("Variable to story: " + variable.Key + " = " + variable.Value);
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }

}

    public void LoadData(GameData data)
    {
        //throw new System.NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        //data.inkVariables.Clear();
        //foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        //{
        //    data.inkVariables[variable.Key] = variable.Value;
        //}
    }
}
