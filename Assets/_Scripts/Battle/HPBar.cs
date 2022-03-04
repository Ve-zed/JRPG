using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject _health;


    public void SetHP(float hpNormalized)
    {
        _health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHPSmooth(float newHp)
    {
        float curHp = _health.transform.localScale.x;
        float changeAmt = curHp - newHp;

        while(curHp - newHp > Mathf.Epsilon)
        {
            curHp -= changeAmt * Time.deltaTime;
            _health.transform.localScale = new Vector3(curHp, 1f);
            yield return null;
        }
        _health.transform.localScale = new Vector3(newHp, 1f);
    }



}
