using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        GOAL, CAPTUR, HEAL
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected State nextState;
    protected NavMeshAgent agent;
    protected Transform player;

    public State(GameObject _npc, NavMeshAgent _agent, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        player = _player;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() 
    {
        stage = EVENT.UPDATE;
    }
    
    public virtual void Update() { stage = EVENT.UPDATE; }
    
    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        else if(stage == EVENT.UPDATE) Update();
        else if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        
        return this;
    }
}
