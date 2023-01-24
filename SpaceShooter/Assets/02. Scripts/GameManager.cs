using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    public List<GameObject> monster_pool = new List<GameObject>();
    public int max_monsters = 10;
    public GameObject monster;
    public float create_time = 3;
    bool is_game_over;
    public bool IsGameOver
    {
        get { return is_game_over; }
        set
        {
            is_game_over = value;
            if (is_game_over == true)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    public static GameManager instance = null;

    public TMP_Text score_text;
    int total_score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CreateMonsterPool();

        Transform spawn_point_group = GameObject.Find("SpawnPointGroup")?.transform;

        foreach (Transform point in spawn_point_group)
        {
            points.Add(point);
        }

        InvokeRepeating("CreateMonster", 2, create_time);

        total_score = PlayerPrefs.GetInt("TotalScore", 0);
        DisplayScore(0);
    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(points[idx].position, 
                                                   points[idx].rotation);

        _monster?.SetActive(true);
    }

    void CreateMonsterPool()
    {
        for (int i =0; i <max_monsters; i++)
        {
            var _monster = Instantiate<GameObject>(monster);

            _monster.name = $"Monster_{i:00}";
            _monster.SetActive(false);

            monster_pool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach(var _monster in monster_pool)
        {
            if(_monster.activeSelf == false)
            {
                return _monster;
            }
        }

        return null;
    }

    public void DisplayScore(int score)
    {
        total_score += score;
        score_text.text = $"<color=#00ff00>SCORE: </color><color=#ff0000>{total_score:#,##0}</color>";
        PlayerPrefs.SetInt("TotalScore", total_score);
    }
}
