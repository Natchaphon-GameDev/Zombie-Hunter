using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private GameObject particle;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float movementSpeed = default;
    [SerializeField] private float rotateSpeed = default;
    [SerializeField] private LayerMask layerMask = default;
    [SerializeField] private RectTransform reloadImg = default;

    private RaycastHit hit;
    private int ammo = default;
    [SerializeField] private int ammoDefault = 2;
    [SerializeField] private float reloadTime = 2f;
    
    public static bool isReload; //กำลังโหลด วิ้งเข้าใส่
    public static bool isWalking;
    public static bool isShooting;

    private AI_Enemy aiEnemy;
    private AI_Boss aiBoss;

    private float currentHitDis;
    
    private Vector3 moveVector;

    private void Start()
    {
        particle = GameManager.Instance.GetParticle();
        ammo = ammoDefault;
    }

    private void OnDestroy()
    {
        if (!GameManager.Instance.isQuitting)
        {
            Instantiate(particle, transform.position, particle.transform.rotation);
        }
        //Show GameOver Panel
    }

    private void Update()
    {
        if (!GameManager.isGameStart)
        {
            return;
        }
        if (State.isGameEnd)
        {
            return;
        }
        
        if (ammo <= 0)
        {
            reloadImg.gameObject.SetActive(true);
            isReload = true;
            lineRenderer.enabled = false;
            StartCoroutine(Reloading());
        }
        
        GameManager.Instance.UpdateAmmo($"Bullet : {ammo} / {ammoDefault}");

        HandleGravity();
        HandleMovement();
        HandleRotate();
        
        if (isReload) {return;}

        HandleAttack();
        
    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        isReload = false;
        ammo = ammoDefault;
        reloadImg.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    private void HandleGravity()
    {
        moveVector = Vector3.zero;
 
        if (characterController.isGrounded == false)
        {
            moveVector += Physics.gravity;
        }
 
        characterController.Move(moveVector * Time.deltaTime);
    }

    private void HandleMovement()
    {
        var verticalInput = Input.GetAxis("Vertical");

        var movementVector = transform.forward * verticalInput;
        if (verticalInput > 0)
        {
            characterController.Move(Vector3.ClampMagnitude(movementVector, 1.0f) * movementSpeed * Time.deltaTime);
        }
        else if (verticalInput < 0)
        {
            characterController.Move(Vector3.ClampMagnitude(movementVector, 1.0f) * (movementSpeed - 0.5f) * Time.deltaTime);
        }
        
        if (verticalInput != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void HandleRotate()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        
        characterController.transform.Rotate(Vector3.up * horizontalInput * rotateSpeed * (100f * Time.deltaTime));
    }

    private void HandleAttack()
    {
        if (isWalking)
        {
            lineRenderer.enabled = false;
            
            if (aiEnemy != null)
            {
                aiEnemy.isCanDie = false;
            }
            if (aiBoss != null)
            {
                aiBoss.isCanDie = false;
            }

            return;
        }

        var forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(aimPoint.position, forward, Color.green);
        if (Physics.Raycast(aimPoint.position , forward, out hit,Mathf.Infinity,layerMask))
        {
            
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                if (aiEnemy != null)
                {
                    aiEnemy.isCanDie = false;
                }
                if (aiBoss != null)
                {
                    aiBoss.isCanDie = false;
                }
                
                aiEnemy = hit.collider.gameObject.GetComponent<AI_Enemy>();
                aiEnemy.isCanDie = true;
            }
            else if (hit.collider.gameObject.CompareTag("Boss"))
            {
                if (aiEnemy != null)
                {
                    aiEnemy.isCanDie = false;
                }
                if (aiBoss != null)
                {
                    aiBoss.isCanDie = false;
                }
                
                aiBoss = hit.collider.gameObject.GetComponent<AI_Boss>();
                aiBoss.isCanDie = true;
            }
            else
            {
                if (aiEnemy != null)
                {
                    aiEnemy.isCanDie = false;
                }
                if (aiBoss != null)
                {
                    aiBoss.isCanDie = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && ammo > 0 && !isShooting)
            {
                isShooting = true;
                ammo--;
                Instantiate(particle, hit.point, particle.transform.rotation);
                
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    aiEnemy.isDie = true;
                }
                else if (hit.collider.gameObject.CompareTag("Boss"))
                {
                    aiBoss.currentHealth--;

                    if (aiBoss.currentHealth <= 0)
                    {
                        aiBoss.isDie = true;
                        return;
                    }

                    aiBoss.damaged = true;
                    StartCoroutine(Wait());

                }
            }

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, aimPoint.position);
            lineRenderer.SetPosition(1, hit.point);

            // Debug.Log($"{hit.distance} {hit.collider.gameObject.name}");
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(.3f);
        aiBoss.damaged = false;
    }
}