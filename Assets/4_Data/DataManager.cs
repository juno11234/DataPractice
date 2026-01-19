using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    // 검색을 빠르게 하기 위해 List 대신 Dictionary 사용
    public Dictionary<int, MonsterData> MonsterDict = new Dictionary<int, MonsterData>();

    void Awake()
    {
        Instance = this;
        LoadAllData();
    }

    void LoadAllData()
    {
        // 1. TSVReader를 이용해 리스트로 쫙 불러옴
        List<MonsterData> monsterList = TSVReader.Read<MonsterData>("MonsterData"); // 확장자 .tsv 생략

        // 2. 검색하기 쉽게 Dictionary에 옮겨 담음
        foreach (var monster in monsterList)
        {
            if (!MonsterDict.ContainsKey(monster.ID))
            {
                MonsterDict.Add(monster.ID, monster);
            }
        }
        
        Debug.Log($"총 {MonsterDict.Count}마리의 몬스터 데이터를 로드했습니다.");
    }

    public MonsterData GetMonster(int id)
    {
        if (MonsterDict.ContainsKey(id))
            return MonsterDict[id];
        
        Debug.LogWarning($"ID {id} 몬스터가 없습니다.");
        return null;
    }
}