using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomEditor(typeof(SoundDataBase))]
public class SoundDataBaseEditor : Editor
{
    public VisualTreeAsset visualTree;
    public override VisualElement CreateInspectorGUI()
    {
        
        var root = new VisualElement();
        
        visualTree.CloneTree(root);
        
        var MultiColumnList = root.Q<MultiColumnListView>("MultiColumnList");
        var so = new SerializedObject(serializedObject.targetObject);
        MultiColumnList.Bind(so);
        return root;
    }

  
}
