using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ScriptableObject를 상속받음
[CreateAssetMenu(fileName = "NewFoundation", menuName = "ScriptableObjects/Foundation")]
public class Foundation : ScriptableObject
{
    public int FoundationID;     // 창업 ID
    public string FoundationName;   // 창업 이름
    public string FoundationDescription; // 창업 설명 
    public int FoundationCost;             // 창업 금액
    public int Reward;                 // 보상 금액
    public float SuccessProbability; // 성공 확률
    public int StartDate;       // 시작하는 날짜
    public int EndDate;         // 끝나는 날짜
    public List<Student> Students;    // 캐릭터 이미지


    //창업 정보가 고정적이라면
    public int FoundationLevel;
    //public List<string> RequiredMajor; // 글자 말고 아이디를 받는 편인 나을지도 모르겠다. 
    public List<int> RequiredMajorID; // 스탯들과의 순서만 맞추었다.
    public List<int> RequiredStat0; //과목마다 순서대로 5개씩 스탯을 저장
    public List<int> RequiredStat1; 
    public List<int> RequiredStat2; 
    public List<int> RequiredStat3; 
    public List<int> RequiredStat4; 
    public List<int> RequiredStat5; 
    public List<int> RequiredStat6;



}

