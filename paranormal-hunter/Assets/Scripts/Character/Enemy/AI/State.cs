using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter;
using Hunter.Character;

[CreateAssetMenu(fileName = "States", menuName = "Utility Based AI/States", order = 1)]
public class State : ScriptableObject
{
    public UtilityBasedAI[] utilityBasedAI;
}

public class CurrentState : State
{

}

public class NextState : State
{
    
}

public class FindState : State
{

}
