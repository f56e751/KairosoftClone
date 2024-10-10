using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject studentPrefab;
    private Vector3 entrancePosition; // 정문 위치
    private int currentSeason = 1;  // 현재 계절을 나타내는 변수 (1부터 12까지)
    private float seasonDuration = 10f;  // 각 계절의 지속 시간 (초 단위)
    private float seasonTimer = 0f;  // 계절의 경과 시간을 추적하는 타이머
    public int studentNum = 10;
    public int professorNum = 3;
    private int maxStudentNum => CalculateMaxStudentNum(); // 건물, 교수 수에 따른 최대 학생 수용량, fameStudentNum이 아무리 크더라도 이 값에 의해 전체 학생 수의 제한이 생김
    private int fameStudentNum => CalculateFameStudentNum(); // fame에 따른 최대 학생 수
    private float tuitionFee = 1000;
    private float professorSalary = 1500;
    public float gold = 0; 
    public float fame => CalculateFame();

    private StudentPoolManager studentPoolManager;
    private List<GameObject> currentStudents = new List<GameObject>();


    // action for UIManager
    public event Action<int> OnStudentCountChanged;
    public event Action<float> OnGoldChanged;
    public event Action<int> OnProfessorCountChanged;
    public event Action<float> OnFameChanged;
    public event Action<int> OnSeasonChanged;

    void Start()
    {
        studentPoolManager = FindObjectOfType<StudentPoolManager>();
        EnterStudent();
    }

    void Update()
    {
        seasonTimer += Time.deltaTime;  // 매 프레임마다 경과 시간 업데이트

        if (seasonTimer >= seasonDuration)
        {
            StartCoroutine(NextSeason());  // 다음 계절로 전환
        }
    }

    

    private float CalculateFame()
    {
        return (studentNum * 1f) + (professorNum * 5f);
    }

    private int CalculateMaxStudentNum()
    {   
        // 빌딩 수, 교수 수에 의해 결정되도록 함 
        return (int) professorNum * 10 ;
    }

    private int CalculateFameStudentNum()
    {
        return (int) fame * 10;
    }

    private IEnumerator NextSeason()
    {
        seasonTimer = 0f;  // 타이머 초기화
        currentSeason = (currentSeason % 12) + 1;  // 다음 계절로 업데이트 (12 이후에는 다시 1로)

        // 3월(3번째 계절)과 9월(9번째 계절)에 등록금 수입 추가
        if (currentSeason == 3 || currentSeason == 9)
        {
            float tuitionIncome = studentNum * tuitionFee;
            UpdateGold(tuitionIncome);
            Debug.Log($"Tuition income received: {tuitionIncome}");

            int maxStudentCapacity = Mathf.Min(fameStudentNum, maxStudentNum);
            int newStudentsToEnroll = maxStudentCapacity - studentNum;

            if (newStudentsToEnroll > 0)
            {
                IncreaseStudentCount(newStudentsToEnroll);
                EnterNewStudents(newStudentsToEnroll);
            }
            else if (newStudentsToEnroll < 0)
            {
                studentNum = maxStudentCapacity;
                OnStudentCountChanged?.Invoke(studentNum);  // 학생 수 변경 알림
            }
        }

        // 계절 변경에 따른 로직 수행
        OnSeasonChanged?.Invoke(currentSeason); 
        Debug.Log("Now entering season: " + currentSeason);

        // 교수 월급 빠져나감
        UpdateGold(- professorNum * professorSalary); 

        // 이전 계절의 학생들이 나가도록 처리하고, 모든 학생들이 풀에 반환될 때까지 기다림
        yield return ExitStudents();

        // 새로운 계절에 학생들이 다시 들어오도록 처리
        EnterStudent();
    }

    

    // 학생 수 증가 메서드
    public void IncreaseStudentCount(int amount)
    {
        studentNum += amount;
        OnStudentCountChanged?.Invoke(studentNum);  // 이벤트 호출
        OnFameChanged?.Invoke(fame);
    }

    // 골드 업데이트 메서드
    public void UpdateGold(float amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);  // 이벤트 호출
        OnFameChanged?.Invoke(fame);
    }

    // 교수 수 증가 메서드
    public void IncreaseProfessorCount(int amount)
    {
        professorNum += amount;
        OnProfessorCountChanged?.Invoke(professorNum);  // 이벤트 호출
        OnFameChanged?.Invoke(fame);
    }



    // public void UpdateFame(float amount)
    // {
    //     fame += amount;
    //     OnFameCountChanged?.Invoke(professorNum);  // 이벤트 호출
    // }

    private void EnterStudent()
    {   
        (int x, int z) = BuildManager.Instance.entrancePosition;
        entrancePosition = new Vector3(x, 0, z);

        for (int i = 0; i < studentNum; i++)
        {
            GameObject student = studentPoolManager.GetStudent();

            float randomOffsetX = UnityEngine.Random.Range(-0.5f, 4f);
            float randomOffsetZ = UnityEngine.Random.Range(-0.5f, 4f);
            student.transform.position = entrancePosition + new Vector3(randomOffsetX, 0, randomOffsetZ);

            student.transform.rotation = Quaternion.identity;
            currentStudents.Add(student);  // 현재 계절에 학교에 있는 학생 리스트에 추가
            // Debug.Log($"currentStudents size is {currentStudents.Count} students");
            
            // UnitMover 컴포넌트가 존재하는지 확인
            UnitMover unitMover = student.GetComponent<UnitMover>();
            if (unitMover != null)
            {   
                // 랜덤한 타겟 위치로 학생을 이동시킴
                // (int targetX, int targetZ) = GetRandomTargetPosition();
                (int targetX, int targetZ) = BuildManager.Instance.GetRandomBuildingCenter();
                unitMover.StartMoveUnit((targetX, targetZ));
            }
            else
            {
                Debug.LogError("UnitMover component not found on student prefab!");
            }
        }
    }

    private void EnterNewStudents(int newStudentCount)
    {
        for (int i = 0; i < newStudentCount; i++)
        {
            GameObject student = studentPoolManager.GetStudent();

            float randomOffsetX = UnityEngine.Random.Range(-0.5f, 4f);
            float randomOffsetZ = UnityEngine.Random.Range(-0.5f, 4f);
            student.transform.position = entrancePosition + new Vector3(randomOffsetX, 0, randomOffsetZ);

            // student.transform.position = entrancePosition + new Vector3(0, 0, i * 0.1f);
            student.transform.rotation = Quaternion.identity;
            currentStudents.Add(student);

            UnitMover unitMover = student.GetComponent<UnitMover>();
            if (unitMover != null)
            {
                (int targetX, int targetZ) = BuildManager.Instance.GetRandomBuildingCenter();
                unitMover.StartMoveUnit((targetX, targetZ));
            }
        }

        Debug.Log($"New students enrolled: {newStudentCount}");
    }

    private IEnumerator ExitStudents()
    {
        List<Coroutine> exitCoroutines = new List<Coroutine>();

        foreach (GameObject student in currentStudents)
        {
            UnitMover unitMover = student.GetComponent<UnitMover>();
            if (unitMover != null)
            {
                unitMover.StartMoveUnit(((int)entrancePosition.x, (int)entrancePosition.z));  // 정문 위치로 이동

                // 학생이 정문에 도달했을 때 풀에 반환하는 코루틴 시작
                Coroutine coroutine = StartCoroutine(ReturnStudentToPoolWhenArrived(student, unitMover));
                exitCoroutines.Add(coroutine);
            }
        }

        // 모든 학생들이 풀에 반환될 때까지 기다림
        foreach (Coroutine coroutine in exitCoroutines)
        {
            yield return coroutine;
        }

        currentStudents.Clear();  // 현재 계절에 학교에 있는 학생 리스트 초기화
    }

    private IEnumerator ReturnStudentToPoolWhenArrived(GameObject student, UnitMover unitMover)
    {
        while (!unitMover.isArrived)
        {
            yield return null;  // 학생이 정문에 도달할 때까지 기다림
        }

        studentPoolManager.ReturnStudent(student);  // 정문에 도달하면 학생을 풀에 반환
    }

    private (int, int) GetRandomTargetPosition()
    {
        // 학교 안에서 랜덤한 위치를 계산
        int randomX = UnityEngine.Random.Range(BuildManager.Instance.minXAxisPosition, BuildManager.Instance.maxXAxisPosition);
        int randomZ = UnityEngine.Random.Range(BuildManager.Instance.minZAxisPosition, BuildManager.Instance.maxZAxisPosition);
        return (randomX, randomZ);
    }
}