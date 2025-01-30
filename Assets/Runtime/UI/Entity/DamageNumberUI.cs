using MyTool;
using Services.ObjectPools;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumberUI : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private MyObject myObject;
    public float lifeSpan;

    public void SetDamage(int damage)
    {
        if(damage == 0)
        {
            myObject.Recycle();
            return;
        }

        if (damage > 0)
            tmp.text = damage.ToString().ColorText("red");
        else
            tmp.text = $"+{-damage}".ColorText("green");      //伤害小于0视为治疗
        StartCoroutine(Delay());
    }

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        myObject = GetComponent<MyObject>();
        tmp.text = string.Empty;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(lifeSpan);
        myObject.Recycle(); 
    }
}
