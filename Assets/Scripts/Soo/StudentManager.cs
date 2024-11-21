using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    public Dictionary<int, Student> studentDict = new Dictionary<int, Student>();

    // �л� �߰�
    public void AddStudent(Student student)
    {
        if (!studentDict.ContainsKey(student.studentID))
        {
            studentDict.Add(student.studentID, student);
        }
    }

    // �л� �˻�
    public Student GetStudentByID(int studentID)
    {
        if (studentDict.ContainsKey(studentID))
        {
            return studentDict[studentID];
        }
        return null;
    }

    //���Ŀ� �����ϴ� ��, ���� �����ϴ� �� ����� �۾��մϴ�
}
