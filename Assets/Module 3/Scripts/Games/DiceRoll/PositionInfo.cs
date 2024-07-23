using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PositionInfo{
    [SerializeField]
    private Vector3 position;
    public Vector3 GetPosition{ get => position; }
    [SerializeField]
    private Quaternion rotation;
    public Quaternion GetRotation{ get => rotation; }[SerializeField]
    private Vector3 velocity;
    public Vector3 GetVelocity{ get => velocity; }
    [SerializeField]
    private Vector3 angularVelocity;
    public Vector3 GetAngularVelocity{ get => angularVelocity; }
    [SerializeField]
    private Quaternion topFaceRotationOffset;
    public Quaternion GetTopFaceRotationOffset{ get => topFaceRotationOffset; }
    [SerializeField]
    private bool collision;
    public bool WasCollision {get => collision; }

    public PositionInfo(Vector3 pos, Quaternion rot, Vector3 vel, Vector3 angVel, Quaternion topFaceRotOffset, bool col){
        position = pos;
        rotation = rot;
        velocity = vel;
        angularVelocity = angVel;
        topFaceRotationOffset = topFaceRotOffset;
        collision = col;
    }
}
