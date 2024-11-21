using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ScriptableObject�� ��ӹ���
[CreateAssetMenu(fileName = "NewFoundation", menuName = "ScriptableObjects/Foundation")]
public class Foundation : ScriptableObject
{
    public int FoundationID;     // â�� ID
    public string FoundationName;   // â�� �̸�
    public string FoundationDescription; // â�� ���� 
    public int FoundationCost;             // â�� �ݾ�
    public int Reward;                 // ���� �ݾ�
    public float SuccessProbability; // ���� Ȯ��
    public int StartDate;       // �����ϴ� ��¥
    public int EndDate;         // ������ ��¥
    public List<Student> Students;    // ĳ���� �̹���


    //â�� ������ �������̶��
    public int FoundationLevel;
    //public List<string> RequiredMajor; // ���� ���� ���̵� �޴� ���� �������� �𸣰ڴ�. 
    public List<int> RequiredMajorID; // ���ȵ���� ������ ���߾���.
    public List<int> RequiredStat0; //���񸶴� ������� 5���� ������ ����
    public List<int> RequiredStat1; 
    public List<int> RequiredStat2; 
    public List<int> RequiredStat3; 
    public List<int> RequiredStat4; 
    public List<int> RequiredStat5; 
    public List<int> RequiredStat6;



}

