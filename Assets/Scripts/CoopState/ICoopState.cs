﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoopState {

   
  
    void UpdateState();

    void ToIdleState();


    void ToAttackState();


    void ToMoveState();


    /// <summary>
    /// Character will cast an ability.
    /// </summary>
    void ToCastState();

}