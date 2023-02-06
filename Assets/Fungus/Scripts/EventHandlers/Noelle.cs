using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Fungus;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class Noelle : MonoBehaviour
{
    [SerializeField]public Vector3 TargetPos;
    [SerializeField] private float scaleRatio = 0f;
    [SerializeField] private float perspectiveScale;
    [SerializeField] public Animator _animator;
    private NavMeshAgent _agent;
    private Vector2 stuckDistanceCheck;
    private SpriteRenderer SR;
    public bool InDialogue;
    public bool cutSceneInProgress;
    public Choices _choices;
    private static Noelle _noelle;
    private Vector3 _scale;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        TargetPos = new Vector3(transform.position.x, transform.position.y,transform.position.z);
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        SR = GetComponent<SpriteRenderer>();
        _choices = FindObjectOfType<Choices>();
        Vector3 _scale = transform.localScale;
        // if (_noelle != null && _noelle != this)
        // {
        //      Destroy(gameObject);
        //      return;
        // }
        _noelle = this;
        //  DontDestroyOnLoad(this);



    }


    void Update()
    {
        
        // if (SceneManager.GetActiveScene().buildIndex != 1)
        // {
        //     _noelle.transform.localScale = new Vector3(2.7f, 2.7f, 2.7f);
        //     _noelle.GetComponent<NavMeshAgent>().speed = 2.5f;
        //     _noelle.GetComponent<NavMeshAgent>().acceleration = 30f;
        //     _noelle.GetComponent<SpriteRenderer>().color =Color.HSVToRGB(299f/360,35f/100,63f/100);
        // }
        // else
        // {
        //     _noelle.transform.localScale = new Vector3(1f, 1f, 1f);
        //     _noelle.GetComponent<NavMeshAgent>().speed = 1.2f;
        //     _noelle.GetComponent<NavMeshAgent>().acceleration = 15f;
        //     _noelle.GetComponent<SpriteRenderer>().color =Color.HSVToRGB(237f/360,40f/100,100f/100);
        //}
        if (InDialogue != true)
        {
            Vector2 mousePos  =Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //TargetPos = new Vector2(Noelle.position.x, Noelle.position.y);
            if (Input.GetMouseButtonDown(0))
            {
                TargetPos = new Vector2(mousePos.x, mousePos.y);
            
            
            }
            _agent.SetDestination(new Vector3(TargetPos.x, TargetPos.y, transform.position.z));
            UpdateAnimation();
        }

        RescalePlayerDistance();


        //transform.position = Vector2.MoveTowards(transform.position, TargetPos, Time.deltaTime * MoveSpeed);




    }

    private void UpdateAnimation()
    {
        float distance = Vector2.Distance(transform.position, TargetPos);
        if (Vector2.Distance(stuckDistanceCheck, transform.position) == 0)
        {
            _animator.SetFloat("Distance",0f); return;
            
        }
        _animator.SetFloat("Distance",distance);
        if (distance > 0.01f)
        {
            Vector3 direction = transform.position - new Vector3(TargetPos.x,TargetPos.y,transform.position.z);
            float angle = Mathf.Atan2(direction.x,direction.y) * Mathf.Rad2Deg;
            _animator.SetFloat("Angle",angle);
            stuckDistanceCheck = transform.position;
        }
        
    }
    private void RescalePlayerDistance()
    {
        _scale.x = perspectiveScale * (scaleRatio - transform.position.y);
        _scale.y = perspectiveScale * (scaleRatio - transform.position.y);
        transform.localScale = _scale;
    }

    public void ExitDialogue()
    {
        InDialogue = false;
        cutSceneInProgress = false;
        _choices.choice = Choices.Action.Walk;
        _choices.UpdateChoiceTextBox(null);
        _choices.gameObject.SetActive(true);

    }

    public void EnterDialogue()
    {
        InDialogue = true;
        cutSceneInProgress = true;
        
        _choices.choice = Choices.Action.Walk;
        _choices.UpdateChoiceTextBox(null);
        _choices.gameObject.SetActive(false);
        
    }

    public void ChangeSize()
    {
        
    }
}
