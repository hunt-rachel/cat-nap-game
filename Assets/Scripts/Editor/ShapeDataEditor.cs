using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]

public class ShapeDataEditor : Editor
{
    private ShapeData sd => target as ShapeData; 

    //allows us to create editor for making game shapes
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ClearBoardBtn();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        if(sd.shapeBoard != null && sd.columnCount > 0 && sd.rowCount > 0)
        {
            DrawBoardTable();
        } 
        
        serializedObject.ApplyModifiedProperties();

        if(GUI.changed)
        {
            EditorUtility.SetDirty(sd);
        }
    }

    private void ClearBoardBtn()
    {
        if(GUILayout.Button("Clear Board"))
        {
            sd.Clear();
        }
    }

    private void DrawColumnsInputFields()
    {
        var colsTemp = sd.columnCount;
        var rowsTemp = sd.rowCount;

        sd.columnCount = EditorGUILayout.IntField("Columns", sd.columnCount);
        sd.rowCount = EditorGUILayout.IntField("Rows", sd.rowCount);

        if((sd.columnCount != colsTemp || sd.rowCount != rowsTemp) && 
            (sd.columnCount > 0 && sd.rowCount > 0))
        {
            sd.CreateShapeBoard();
        }
    }

    //creates board interface for development
    private void DrawBoardTable()
    {
        //interface for overall table
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        //header for editor
        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        //interface for rows of grid
        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 35;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        //interface for boxes that will be filled to create shapes
        var boxStyle = new GUIStyle(EditorStyles.miniButtonMid);
        boxStyle.fixedWidth = 25;
        boxStyle.fixedHeight = 25;
        boxStyle.normal.background = Texture2D.grayTexture; //not selected
        boxStyle.onNormal.background = Texture2D.whiteTexture; //selected

        
        //initialising editor layout
        for(int row = 0; row < sd.rowCount; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for(int col = 0; col < sd.columnCount; col++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(sd.shapeBoard[row].column[col], boxStyle);
                sd.shapeBoard[row].column[col] = data;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
