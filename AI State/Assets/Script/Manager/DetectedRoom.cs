using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedRoom : MonoBehaviour
{
    [SerializeField] private SpawnManager.Room room;
    private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandleSpawnRoom();
        }
    }

    private void HandleSpawnRoom()
    {
        GameManager.Instance.isInRoom_Main = false;
        GameManager.Instance.isInRoom_Left = false;
        GameManager.Instance.isInRoom_Right = false;
        GameManager.Instance.isInRoom_Top = false;
        GameManager.Instance.isInRoom_Bot = false;
        
        switch (room)
        {
            case SpawnManager.Room.MainRoom:
                GameManager.Instance.isInRoom_Main = true;
                break;
            case SpawnManager.Room.LeftRoom:
                GameManager.Instance.isInRoom_Left = true;
                break;
            case SpawnManager.Room.RightRoom:
                GameManager.Instance.isInRoom_Right = true;
                break;
            case SpawnManager.Room.TopRoom:
                GameManager.Instance.isInRoom_Top = true;
                break;
            case SpawnManager.Room.BotRoom:
                GameManager.Instance.isInRoom_Bot = true;
                break;
        }
    }
}
