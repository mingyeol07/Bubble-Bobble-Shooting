using UnityEngine;

public abstract class DataReaderBase : ScriptableObject
{
    [Header("��Ʈ�� �ּ�")] 
    public string associatedSheet = "";

    [Header("�������� ��Ʈ�� ��Ʈ �̸�")]
    public string associatedWorksheet = "";

    [Header("�б� ������ �� ��ȣ")] 
    public int START_ROW_LENGTH = 2;

    [Header("���� ������ �� ��ȣ")] 
    public int END_ROW_LENGTH = -1;
}