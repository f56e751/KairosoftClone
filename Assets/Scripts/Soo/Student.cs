using System.Collections.Generic;
using UnityEngine;

// ScriptableObject를 상속받음
[CreateAssetMenu(fileName = "NewStudent", menuName = "ScriptableObjects/Student")]

public class Student : ScriptableObject
{
    public int studentID;
    public string studentName;
    public string major; // 학과이름
    public int majorID; // 학과
    public string status; // 재학/졸업 상태
    public List<int> Stats; // 스탯리스트 (반드시, 과학, 예술, 논리, 수리, 언어의 순서대로 기입해야 한다)

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