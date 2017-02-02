using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState : ICharacterState {

    //this will contain extra methods that only will be available to player state. Casting is an idea I had (the state of selecting where to throw a grenade, for example)

    /// <summary>
    /// Character will cast an ability.
    /// </summary>
    void ToCastState();

}
