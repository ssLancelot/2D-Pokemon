using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject _healt;
    public void SetHp(float hpNormalized)
    {
        _healt.transform.localScale = new Vector3(hpNormalized, 1);
    }
    public IEnumerator SetHpSmooth(float newHP)
    {
        float currentHp = _healt.transform.localScale.x;
        float changeAmt = currentHp - newHP;
        while (currentHp-newHP > Mathf.Epsilon)
        {
            currentHp -= changeAmt*Time.deltaTime;
            _healt.transform.localScale = new Vector3(currentHp, 1);
            yield return null;
        }
        _healt.transform.localScale = new Vector3(newHP, 1);
    }
}
