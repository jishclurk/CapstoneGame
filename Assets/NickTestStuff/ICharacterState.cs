using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState
{

    /// <summary>
    /// Changes the current state based on input or surroundings
    /// </summary>
    void UpdateState();

    /// <summary>
    /// Character moves to a certain position.
    /// </summary>
    void ToMoveState();

    /// <summary>
    /// Character attacks a certain target. If the character is out of range, 
    /// it will move towards the target to be within range.
    /// </summary>
    void ToAttackState();

    /// <summary>
    /// Character has nothing to do.
    /// </summary>
    void ToIdleState();

}
