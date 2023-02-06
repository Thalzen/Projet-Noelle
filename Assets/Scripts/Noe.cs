using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Noe : MonoBehaviour
{
    [SerializeField] private float scaleRatio = 0f;
    [SerializeField] private float perspectiveScale;
    private NavMeshAgent _agent;
    private Vector2 stuckDistanceCheck;
    [SerializeField] private Vector3 TargetPos;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _noelle;
     private Vector3 _scale;
    //private static Noe _noe;
    
    


    void Start()
    {   
        Vector3 _scale = transform.localScale;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        TargetPos = new Vector2(transform.position.x, transform.position.y);
        _noelle = FindObjectOfType<Noelle>().transform;

        //_noe = this;

    }

    
    void Update()
    {
        RescalePlayerDistance();
        // if (SceneManager.GetActiveScene().buildIndex != 1)
        // {
        //     _noe.transform.localScale = new Vector3(2.7f, 2.7f, 2.7f);
        //     _noe.GetComponent<NavMeshAgent>().speed = 2.5f;
        //     _noe.GetComponent<NavMeshAgent>().acceleration = 30f;
        //     _noe.GetComponent<SpriteRenderer>().color =Color.HSVToRGB(299f/360,35f/100,63f/100);
        //     _noe.GetComponent<NavMeshAgent>().stoppingDistance = 2f;
        // }
        // else
        // {
        //     _noe.transform.localScale = new Vector3(1f, 1f, 1f);
        //     _noe.GetComponent<NavMeshAgent>().speed = 1.2f;
        //     _noe.GetComponent<NavMeshAgent>().acceleration = 15f;
        //     _noe.GetComponent<SpriteRenderer>().color =Color.HSVToRGB(237f/360,37f/100,100f/100);
        // }
       
        TargetPos = new Vector3(_noelle.position.x, _noelle.position.y, transform.position.y);
        
        
        _agent.SetDestination(new Vector3(TargetPos.x, TargetPos.y, transform.position.z));
        
        UpdateAnimation();
        
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
}
