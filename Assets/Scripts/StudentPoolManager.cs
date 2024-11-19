using System.Collections.Generic;
using UnityEngine;

public class StudentPoolManager : MonoBehaviour
{
    // [SerializeField] private GameObject studentPrefab;
    [SerializeField] private string studentPrefabFolderPath = "Students_prefab"; // Resources 폴더 내 경로
    private Dictionary<string, GameObject> studentPrefabs = new Dictionary<string, GameObject>();
    private Queue<GameObject> studentPool = new Queue<GameObject>();
    private List<GameObject> activeStudents = new List<GameObject>(); // 활성화된 학생들을 추적하는 리스트
    private int initialPoolSize = 10;

    void Awake()
    {
        LoadPrefabs();
    }
    void Start()
    {   
        
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateStudent();
        }
    }

    GameObject GetRandomPrefab()
    {
        int index = Random.Range(0, studentPrefabs.Count);
        foreach (var prefab in studentPrefabs.Values)
        {
            if (index == 0)
                return prefab;
            index--;
        }
        return null;
    }


    void LoadPrefabs()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>(studentPrefabFolderPath);
        if (prefabs.Length == 0)
        {
            Debug.LogError($"No prefabs found in Resources/{studentPrefabFolderPath}");
            return;
        }
        Debug.Log($"prefabs.Length: {prefabs.Length}");

        foreach (GameObject prefab in prefabs)
        {   
            studentPrefabs[prefab.name] = prefab;
        }
    }

    GameObject CreateStudent()
    {   
        // GameObject student = Instantiate(studentPrefab);

        if (studentPrefabs.Count == 0)
        {
            Debug.LogError("No prefabs loaded. Ensure the path is correct and prefabs exist.");
            return null;
        }

        GameObject randomPrefab = GetRandomPrefab();
        GameObject student = Instantiate(randomPrefab);
        student.AddComponent<UnitMover>();
        UnitMover unitMover = student.GetComponent<UnitMover>();
        unitMover.unitToMove = student;
        unitMover.yAxis = 0;
        unitMover.speed = 3;


        student.SetActive(false);
        studentPool.Enqueue(student);
        return student;
    }

    public GameObject GetStudent()
    {
        if (studentPool.Count == 0)
        {
            CreateStudent();
        }
        GameObject student = studentPool.Dequeue();
        student.SetActive(true);
        activeStudents.Add(student);
        return student;
    }

    public void ReturnStudent(GameObject student)
    {
        student.SetActive(false);
        activeStudents.Remove(student);
        studentPool.Enqueue(student);
    }

    public void IncreasePoolSize(int newSize)
    {
        int currentTotalSize = studentPool.Count + activeStudents.Count;
        while (currentTotalSize < newSize)
        {
            CreateStudent();
            currentTotalSize++;
        }
    }
}
