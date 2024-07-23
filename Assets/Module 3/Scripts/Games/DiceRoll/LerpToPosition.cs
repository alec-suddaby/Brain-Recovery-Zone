using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPosition : MonoBehaviour
{
    [System.Serializable]
    public struct DiceFinishPosition{
        public ReplayDice dice;
        public Transform position;
    }

    public List<DiceFinishPosition> diceFinishPositions;

    public float slerpSpeed = 5f;

    private bool forcePosition;

    public void MoveDiceToPosition(bool move){
        forcePosition = move;

        foreach(DiceFinishPosition finishPosition in diceFinishPositions){
            finishPosition.dice.SetState(true);
        }
    }

    void FixedUpdate(){
        if(!forcePosition){
            return;
        }

        foreach(DiceFinishPosition finishPosition in diceFinishPositions){
            if(!finishPosition.dice.gameObject.activeSelf){
                continue;
            }
            finishPosition.dice.transform.position = Vector3.Slerp(finishPosition.dice.transform.position, finishPosition.position.position, slerpSpeed * Time.fixedDeltaTime);
            
            Quaternion finishRotation = finishPosition.position.rotation;
            finishRotation *=  new Quaternion(-finishPosition.dice.targetFace.localRotation.x, finishPosition.dice.targetFace.localRotation.y, -finishPosition.dice.targetFace.localRotation.z, finishPosition.dice.targetFace.localRotation.w);

            finishPosition.dice.transform.rotation = Quaternion.Slerp(finishPosition.dice.transform.rotation, finishRotation, slerpSpeed * Time.fixedDeltaTime);
        }
    }
}
