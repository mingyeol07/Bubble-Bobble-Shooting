using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class CircleData
{
    public int x;
    public int y;
    public string color;

    public CircleData(int x, int y, string color)
    {
        this.x = x;
        this.y = y;
        this.color = color;
    }
}

[Serializable]
public class StageData
{
   public List<CircleData> circles = new List<CircleData>();
}

[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/ItemDataReader", order = int.MaxValue)]
public class StageDataReader : DataReaderBase
{
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")][SerializeField] public List<CircleData> DataList = new List<CircleData>();

    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        int x = 0, y = 0;
        string color = "";

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "X":
                    {
                        x = int.Parse(list[i].value);
                        break;
                    }
                case "Y":
                    {
                        y = int.Parse(list[i].value);
                        break;
                    }
                case "Color":
                    {
                        color = list[i].value;
                        break;
                    }
            }
        }

        DataList.Add(new CircleData(x, y, color));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StageDataReader))]
public class ItemDataReaderEditor : Editor
{
    StageDataReader data;

    void OnEnable()
    {
        data = (StageDataReader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 읽기(API 호출)"))
        {
            UpdateStats(UpdateMethodOne);
            data.DataList.Clear();
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(data.associatedSheet, data.associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        for (int i = data.START_ROW_LENGTH; i <= data.END_ROW_LENGTH; ++i)
        {
            data.UpdateStats(ss.rows[i], i);
        }

        EditorUtility.SetDirty(target);
    }
}
#endif