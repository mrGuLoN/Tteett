using System.Collections;
using System.Collections.Generic;
using Underground_Laboratory_Generator.Scripts;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LaboratoryGenerator))]
public class CreateLaboratoryOnEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var lab = (LaboratoryGenerator)target;
        if (GUILayout.Button("Generate Stage"))
        {
            lab.StartGeneration();
        }
    }
}
