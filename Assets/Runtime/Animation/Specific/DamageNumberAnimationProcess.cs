using UnityEngine;

public class DamageNumberAnimationProcess : GameObjectAnimationProcess
{
    public int damage;

    public DamageNumberAnimationProcess(Effect effect, Transform parent, Vector3 position, int damage) :
        base(effect, "DamageNumberUI", parent, position)
    {
        this.damage = damage;
    }

    public override void Play()
    {
        base.Play();
        DamageNumberUI ui = myObject.GetComponent<DamageNumberUI>();
        ui.SetDamage(damage);
    }
}
