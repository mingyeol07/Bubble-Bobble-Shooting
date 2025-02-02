// # Systems
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;


// # Unity
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleSheetLoader : MonoBehaviour
{
    // https://docs.google.com/spreadsheets/d/1uwkcHHwz0n8yGP1jpJAqxrc4s_CkFZlsxVq1qcj4i8g/edit?usp=sharing


    const string googleSheetURL = "https://docs.google.com/spreadsheets/d/1uwkcHHwz0n8yGP1jpJAqxrc4s_CkFZlsxVq1qcj4i8g/export?format=tsv&range=A";
    private int[] stage_A = { 2 };
    private int[] stage_D = { 31, 10, 22 };

    private int[] sheet = { 0, 1717451476, 137674187 };

    // googleSheetURL + stage1_D.ToString();

    private string sheetData;
    public CircleData[] CircleDatas;


    private IEnumerator SetSheet(int stageNumber)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(googleSheetURL + /*stage_A[stageNumber].ToString()+*/ "2:D" + stage_D[stageNumber].ToString() + "&gid=" + sheet[stageNumber].ToString()))
        {
            yield return www.SendWebRequest();
            if (www.isDone)
                sheetData = www.downloadHandler.text;
        }
    }

    public IEnumerator SetListCircleInSheet(int stageNumber)
    {
        yield return StartCoroutine(SetSheet(stageNumber));

        string[] rows = sheetData.Split("\n");
        CircleData[] circles = new CircleData[rows.Length];

        for (int i =0; i <rows.Length; i++)
        {
            string[] columns = rows[i].Split("\t");
            circles[i] = (new CircleData(int.Parse(columns[1]), int.Parse(columns[2]), GetColorType(columns[3].Trim())));
        }

        CircleDatas = circles;

        yield return null;
    }

    private ColorType GetColorType(string color)
    {
        ColorType colorType = ColorType.None;

        switch (color.ToLower())  // 소문자로 변환
        {
            case "red":
                colorType = ColorType.Red;
                break;
            case "blue":
                colorType = ColorType.Blue;
                break;
            case "green":
                colorType = ColorType.Green;
                break;
            case "black":
                colorType = ColorType.Black;
                break;
            case "white":
                colorType = ColorType.White;
                break;
            case "yellow":
                colorType = ColorType.Yellow;
                break;
            case "purple":
                colorType = ColorType.Purple;
                break;
            case "orange":
                colorType = ColorType.Orange;
                break;
        }

        return colorType;
    }
}

public struct CircleData
{
    public int x, y;
    public ColorType color;

    public CircleData(int x, int y, ColorType color)
    {
        this.x = x;
        this.y = y;
        this.color = color;
    }
}

