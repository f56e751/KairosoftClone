using System.Collections.Generic;
using UnityEngine;

// ScriptableObject�� ��ӹ���
[CreateAssetMenu(fileName = "NewStudent", menuName = "ScriptableObjects/Student")]

public class Student : ScriptableObject
{
    public int studentID;
    public string studentName;
    public string major; // �а��̸�
    public int majorID; // �а�
    public string status; // ����/���� ����
    public List<int> Stats; // ���ȸ���Ʈ (�ݵ��, ����, ����, ��, ����, ����� ������� �����ؾ� �Ѵ�)

    public Student(int studentID, string studentName, string major, int majorID, string status, List<int> stats)
    {
        this.studentID = studentID;
        this.studentName = studentName;
        this.major = major;
        this.majorID = majorID;
        this.status = status;
        this.Stats = stats;
    }


}