using UnityEngine;

public interface IPlayerFactory
{
    GameObject CreatePlayer(Vector3 position, Quaternion rotation);
}
