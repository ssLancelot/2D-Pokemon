using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string _name;
    [TextArea]
    [SerializeField] string _description;
    [SerializeField] PokemonType _type;
    [SerializeField] int _power;
    [SerializeField] int _accuary;
    [SerializeField] bool _alwaysHits;

    [SerializeField] int _pp;
    [SerializeField] int _priority;
    [SerializeField] MoveCategory _category;
    [SerializeField] MoveEfeckts _effeckts;
    [SerializeField] List<SecondaryEffects> _secondaries;
    [SerializeField] Movetarget _target;

    public string Name
    {
        get { return _name; }
    }
    public string Description
    {
        get { return _description; }
    }
    public PokemonType Type
    {
        get { return _type; }
    }
    public int Power
    {
        get { return _power; }
    }
    public int Accuary
    {
        get { return _accuary; }
    }
    public bool AlwaysHits
    {
        get { return _alwaysHits; }
    }
    public int PP
    {
        get { return _pp; }
    }
    public int Priority
    {
            get { return _priority; }
    }
    public MoveCategory Category
    {
        get { return _category; }
    }
    public MoveEfeckts Efeckts
    {
        get { return _effeckts; }
    }
    public List< SecondaryEffects> Secondaries
    {
        get { return _secondaries; }
    }
    public Movetarget Target
    {
        get { return _target; }
    }
}
[System.Serializable]
public class MoveEfeckts
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;
    [SerializeField] ConditionID volatileStatus;

    public List<StatBoost> Boosts
    {
        get { return boosts; }
    }
    public ConditionID Status
    {
        get { return status; }
    }
    public ConditionID VolatileStatus
    {
        get { return volatileStatus; }
    }
}

[System.Serializable]
public class SecondaryEffects : MoveEfeckts
{
    [SerializeField] int _chance;
    [SerializeField] Movetarget _target;

    public int Chance
    {
        get { return _chance; }
    }
    public Movetarget Target
    {
        get { return _target; }
    }
}


[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}
public enum MoveCategory
{
    Physical, Special,Status
}
public enum Movetarget
{
    Foe , Self
}
