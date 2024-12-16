using UnityEngine;

public interface IControllable
{
    void Move(Vector3 direction);
    void Jump(bool isJumping);
    void Sprint();
    void Fire();
}
