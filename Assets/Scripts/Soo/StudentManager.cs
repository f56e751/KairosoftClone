using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    public Dictionary<int, Student> studentDict = new Dictionary<int, Student>();

    // 학생 추가
    public void AddStudent(Student student)
    {
        if (!studentDict.ContainsKey(student.studentID))
        {
            studentDict.Add(student.studentID, student);
        }
    }

    // 학생 검색
    public Student GetStudentByID(int studentID)
    {
        if (studentDict.ContainsKey(studentID))
        {
            return studentDict[studentID];
        }
        return null;
    }

    //추후에 삭제하는 것, 값을 변경하는 것 등등을 작업합니다
}
