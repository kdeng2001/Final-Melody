using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Bo_MusicianBase))]
public class Bo_MusicianEditor : Editor
{

    Bo_MusicianBase muse;
    #region Serialized Properties

    SerializedProperty name;
    SerializedProperty battleSprite;
    SerializedProperty maxHP;
    SerializedProperty attack;
    SerializedProperty defense;
    SerializedProperty speed;
    #endregion

    private void OnEnable()
    {

        name = serializedObject.FindProperty("name");
        battleSprite = serializedObject.FindProperty("battleSprite");
        maxHP = serializedObject.FindProperty("maxHP");
        attack = serializedObject.FindProperty("attack");
        defense = serializedObject.FindProperty("defense");
        speed = serializedObject.FindProperty("speed");

        muse = (Bo_MusicianBase)target;

    }

    public override void OnInspectorGUI()
    {
        var styleLabel = new GUIStyle();
        styleLabel.fontStyle = FontStyle.Bold;
        styleLabel.normal.textColor = Color.white;
        styleLabel.alignment = TextAnchor.MiddleCenter;

        serializedObject.Update();
        EditorGUILayout.PropertyField(name);

        EditorGUILayout.PropertyField(battleSprite);


        
        Texture2D texture = AssetPreview.GetAssetPreview(muse.BattleSprite);

     
        if (texture != null)
        {
            
            Rect spriteSheetRect = GUILayoutUtility.GetRect(texture.width * 1.5f, texture.height * 1.5f, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            EditorGUI.DrawTextureTransparent(spriteSheetRect, texture);

        }
      

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Stats", styleLabel);

        EditorGUILayout.PropertyField(maxHP);
        EditorGUILayout.PropertyField(attack);
        EditorGUILayout.PropertyField(defense);
        EditorGUILayout.PropertyField(speed);

        serializedObject.ApplyModifiedProperties();

    }
}
