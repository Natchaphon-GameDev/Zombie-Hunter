using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private ModalWindowManager myModalWindow;
    [SerializeField] private ModalWindowManager myModalWindowEnd;
    [SerializeField] private TextMeshProUGUI bulletText;
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject particle;
    public Transform player;
    public Transform npc;
    public GameObject enemyPrefab;

    public bool isInRoom_Main;
    public bool isInRoom_Right;
    public bool isInRoom_Top;
    public bool isInRoom_Left;
    public bool isInRoom_Bot;

    public static bool isGameStart = default;

    public bool showLineAgent = false;

    [NonSerialized] public bool isQuitting = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        isGameStart = false;
        State.isGameEnd = false;
        PlayerController.isWalking = false;
        PlayerController.isReload = false;
    }

    public void StartGame()
    {
        isGameStart = true;
    }

    public void OpenPreStartWindow()
    {
        myModalWindow.OpenWindow();
    }
    
    public void ClosePreStartWindow()
    {
        myModalWindow.CloseWindow();
    }
    
    private void OpenEndStartWindow()
    {
        myModalWindowEnd.OpenWindow();
    }
    
    public void RestartGame()
    {
        isQuitting = true;
        Application.LoadLevel(0);
    }

    public void QuitGame()
    {
        isQuitting = true;
        Application.Quit();
    }

    private void Update()
    {
        if (State.isGameEnd)
        {
            if (player != null)
            {
                PlayParticle();
            }
            bulletText.gameObject.SetActive(false);
            Invoke(nameof(OpenEndStartWindow), 1f);
        }
    }

    public void UpdateAmmo(string text)
    {
        bulletText.text = text;
    }

    public GameObject GetParticle()
    {
        return particle;
    }

    private void PlayParticle()
    {
        Instantiate(particle, player.transform.position, particle.transform.rotation);
    }
}
