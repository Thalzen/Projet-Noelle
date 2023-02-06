// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Fungus 
{
    /// <summary>
    /// The block will execute when the user clicks or taps on the clickable object.
    /// </summary>
    [EventHandlerInfo("Sprite",
                      "Object Clicked",
                      "The block will execute when the user clicks or taps on the clickable object.")]
    [AddComponentMenu("")]
    public class ObjectClicked : EventHandler
    {   
        public class ObjectClickedEvent
        {
            public Clickable2D _clickableObject;
            public ObjectClickedEvent(Clickable2D clickableObject)
            {
                _clickableObject = clickableObject;
            }
        }

        [Tooltip("Object that the user can click or tap on")]
        [SerializeField] protected Clickable2D clickableObject;

        [Tooltip("Wait for a number of frames before executing the block.")]
        [SerializeField] protected int waitFrames = 1;

        protected EventDispatcher eventDispatcher;

        private Noelle _noelle;
        private NavMeshAgent _agent;

        private void Start()
        {
            _noelle = FindObjectOfType<Noelle>();
            _agent = _noelle.GetComponent<NavMeshAgent>();
        }

        protected virtual void OnEnable()
        {
            eventDispatcher = FungusManager.Instance.EventDispatcher;

            eventDispatcher.AddListener<ObjectClickedEvent>(OnObjectClickedEvent);
        }

        protected virtual void OnDisable()
        {
            eventDispatcher.RemoveListener<ObjectClickedEvent>(OnObjectClickedEvent);

            eventDispatcher = null;
        }

        void OnObjectClickedEvent(ObjectClickedEvent evt)
        {
            OnObjectClicked(evt._clickableObject);
        }

        /// <summary>
        /// Executing a block on the same frame that the object is clicked can cause
        /// input problems (e.g. auto completing Say Dialog text). A single frame delay 
        /// fixes the problem.
        /// </summary>
        protected virtual IEnumerator DoExecuteBlock(int numFrames)
        {
            while (Vector3.Distance(clickableObject.transform.position, _noelle.transform.position) > clickableObject.activateDistance)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (Vector3.Distance(clickableObject.transform.position, _noelle.transform.position) <= clickableObject.activateDistance)
            {
                _noelle.EnterDialogue();
                _agent.SetDestination(_noelle.transform.position);
                _noelle.TargetPos = _noelle.transform.position;
                _noelle._animator.SetFloat("Distance", 0f);

                if (numFrames == 0)
                {
                    ExecuteBlock();
                    yield break;
                }

                int count = Mathf.Max(waitFrames, 1);
                while (count > 0)
                {
                    count--;
                    yield return new WaitForEndOfFrame();
                }

                ExecuteBlock();
            }
                
        }
            

        #region Public members

        /// <summary>
        /// Called by the Clickable2D object when it is clicked.
        /// </summary>
        public virtual void OnObjectClicked(Clickable2D clickableObject)
        {
            if (clickableObject == this.clickableObject)
            {
                StartCoroutine(DoExecuteBlock(waitFrames));
            }
        }

        public override string GetSummary()
        {
            if (clickableObject != null)
            {
                return clickableObject.name;
            }

            return "None";
        }

        #endregion
    }
}
