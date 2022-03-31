using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Movements : MonoBehaviour
{
    public GameObject ObjectMove;
    public GameObject ObjectRef;
    [SerializeField] Btn_DataCenter _Btn_DataCenter;

    public void ApplyPosition()
    {
        ObjectRef = _Btn_DataCenter.lastBtnUse;
        ObjectMove.transform.position = new Vector2(ObjectRef.transform.position.x, ObjectMove.transform.position.y);
    }
}
