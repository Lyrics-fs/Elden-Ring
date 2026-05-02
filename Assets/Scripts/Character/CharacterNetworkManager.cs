using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 保存角色需要通过 Netcode 同步的基础状态。
/// </summary>
public class CharacterNetworkManager : NetworkBehaviour
{
    [Header("Position")]
    private NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>
        (Vector3.zero, 
         NetworkVariableReadPermission.Everyone,
         NetworkVariableWritePermission.Owner);
    private NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>
        (Quaternion.identity,
         NetworkVariableReadPermission.Everyone,
         NetworkVariableWritePermission.Owner);

    [SerializeField] private float NetworkPositionSmoothTime = 0.1f;
    [SerializeField] private float NetworkRotationSmoothTime = 0.1f;

    private Vector3 network_position_velocity;

    public Vector3 GetNetworkPosition()
    {
        return NetworkPosition.Value;
    }

    public void SetNetworkPosition(Vector3 network_position)
    {
        NetworkPosition.Value = network_position;
    }

    public Quaternion GetNetworkRotation()
    {
        return NetworkRotation.Value;
    }

    public void SetNetworkRotation(Quaternion network_rotation)
    {
        NetworkRotation.Value = network_rotation;
    }

    public float GetNetworkPositionSmoothTime()
    {
        return NetworkPositionSmoothTime;
    }

    public float GetNetworkRotationSmoothTime()
    {
        return NetworkRotationSmoothTime;
    }

    public Vector3 SmoothNetworkPosition(Vector3 current_position)
    {
        return Vector3.SmoothDamp
            (current_position,
             NetworkPosition.Value,
             ref network_position_velocity,
             NetworkPositionSmoothTime);
    }
}
