using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class State
{
   public enum StateStatus
   {
      Heal,
      Wander,
      CleverHide,
      Seek,
      Flee,
      Pursuit,
      Evade,
      Hide,
      Attack,
   }
   
   public enum EventState
   {
      Enter,
      Update,
      Exit
   }

   public StateStatus stateName;
   protected EventState stateEvent;
   protected GameObject npc;
   protected Transform player;
   protected State nextState;
   protected NavMeshAgent agent;
   protected TextMeshProUGUI statusText;

   public static bool isGameEnd;

   public State(GameObject npc, NavMeshAgent agent,Transform player, TextMeshProUGUI statusText)
   {
      this.npc = npc;
      this.agent = agent;
      stateEvent = EventState.Enter;
      this.player = player;
      this.statusText = statusText;
   }

   public virtual void Enter()
   {
      stateEvent = EventState.Update;
   }
   
   public virtual void Update()
   {
      stateEvent = EventState.Update;
   }
   
   public virtual void Exit()
   {
      stateEvent = EventState.Exit;
   }

   public State Process()
   {
      if (stateEvent == EventState.Enter)
      {
         Enter();
      }
      else if (stateEvent == EventState.Update)
      {
         Update();
      }
      else if (stateEvent == EventState.Exit)
      {
         Exit();
         return nextState;
      }

      return this;
   }

   public float DistancePlayer()
   {
      return Vector3.Distance(npc.transform.position, player.position);
   }
   
   public bool TargetCanSeeMe()
   {
      var result = false;
      var toAgent = npc.transform.position - player.transform.position;
      var lookingAngle = Vector3.Angle(player.transform.forward, toAgent);
      if (lookingAngle < 30)
      {
         result = true;
      }
      return result;
   }

   public bool CanSeePlayer()
   {
      var ratToTarget = player.transform.position - npc.transform.position;
      if (Physics.Raycast(npc.transform.position, ratToTarget, out var raycastHit))
      {
         if (raycastHit.transform.gameObject.tag == "Player")
         {
            return true;
         }
      }
      return false;
   }
}
