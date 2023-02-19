using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public enum Room
    {
        MainRoom,
        LeftRoom,
        RightRoom,
        TopRoom,
        BotRoom
    }
    
    public Room room;
    [SerializeField] private float spawnPerSec;
    private GameObject enemy;
    [SerializeField] private Transform[] spawnPoints;
    private AI_Npc npc;
    
    private void Start()
    {
        enemy = GameManager.Instance.enemyPrefab;
        npc = GameManager.Instance.npc.GetComponent<AI_Npc>();
    }

    public void StartSpawn()
    {
        InvokeRepeating(nameof(SpawnHandle),1f,spawnPerSec);
    }
    
    private void SpawnHandle()
    {
        if (!GameManager.isGameStart)
        {
            return;
        }
       
        switch (room)
        {
            case Room.MainRoom:
                if (GameManager.Instance.isInRoom_Main)
                {
                    return;
                }
                
                SpawnEnemy();
                break;
            case Room.LeftRoom:
                if (GameManager.Instance.isInRoom_Left)
                {
                    return;
                }
                
                SpawnEnemy();
                break;
            case Room.RightRoom:
                if (GameManager.Instance.isInRoom_Right)
                {
                    return;
                }
                
                SpawnEnemy();
                break;
            case Room.TopRoom:
                if (GameManager.Instance.isInRoom_Top)
                {
                    return;
                }
                
                SpawnEnemy();
                break;
            case Room.BotRoom:
                if (GameManager.Instance.isInRoom_Bot)
                {
                    return;
                }

                SpawnEnemy();
                break;
        }
    }
    
    private void SpawnEnemy()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var enemyInstance = Instantiate(enemy, spawnPoint.position, enemy.transform.rotation);
            enemyInstance.GetComponent<AI_Enemy>().player = GameManager.Instance.player;
            npc.enemys.Add(enemyInstance.transform);
        }
    }
}
