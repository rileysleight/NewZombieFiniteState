using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Stack<State> States { get; set;}

    private void Awake()
    {
        States = new Stack<State>();
    }

    private State GetCurrentState()
    {
        return States.Count > 0 ? States.Peek() : null;
    }
    private void Update ()
    {
        if (GetCurrentState() != null)
        {
            GetCurrentState().ActiveAction.Invoke();
        }
    }
    public void PushState(System.Action active, System.Action onEnter, System.Action onExit)
    {
        if (GetCurrentState() != null)
            GetCurrentState().OnExit();

        State state = new State(active, onEnter, onExit);
        States.Push(state);
        GetCurrentState().OnEnter();    
    }
    public void PopState()
    {
        if (GetCurrentState() != null)
        {
            GetCurrentState().OnExit();
            GetCurrentState().ActiveAction = null;
            States.Pop();
            GetCurrentState().OnEnter();
        }
    }
    /*
    public void PushState()
    {
        if (GetCurrentState() != null)
            GetCurrentState().OnExit();
    }*/
}
