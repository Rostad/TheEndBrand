using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState3D {

    void Enter();
    void Update(Vector3 movementInput, float deltaTime);
    CharacterStateSwitch3D HandleCollisions(CollisionFlags collisionFlags);
    void Exit();
	
}
