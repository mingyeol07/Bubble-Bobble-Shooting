using UnityEngine;

public abstract class DataReaderBase : ScriptableObject
{
    [Header("시트의 주소")] 
    public string associatedSheet = "";

    [Header("스프레드 시트의 시트 이름")]
    public string associatedWorksheet = "";

    [Header("읽기 시작할 행 번호")] 
    public int START_ROW_LENGTH = 2;

    [Header("읽을 마지막 행 번호")] 
    public int END_ROW_LENGTH = -1;
}