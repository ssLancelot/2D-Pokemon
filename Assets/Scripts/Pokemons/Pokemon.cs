using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonsBase _base;
    [SerializeField] int _level;

    public Pokemon(PokemonsBase pBase , int pLevel)
    {
        _base = pBase;
        _level = pLevel;

        Init();
    }
    public PokemonsBase Base
    {
        get
        {
            return _base;
        }
    }
    public int Exp { get; set; }
    public int Level { get { return _level; } }
    public int HP { get; set; }
    public List<Move> Moves { get; set; }
    public Move CurrentMove { get; set; }   
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Queue<string> StatusChanges { get; private set; } 
    public bool HpChanged { get; set; }
    public Condition Status { get; private set; }
    public Condition VolatileStatus { get;private set; }
    public int VolatileStatusTime { get; set; }
    public int StatusTime { get; set; }
    public event System.Action OnStatusChanged;
    public void Init()
    {
        //Generate Moves
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }
            if (Moves.Count >= 4)
                break;
        }
        Exp = _base.GetExpForLevel(Level);
        CalculateStats();
        HP = MaxHp;

        StatusChanges = new Queue<string>();
        ResetStatBost();
        Status = null;
        VolatileStatus = null;
    }
    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defanse, Mathf.FloorToInt((Base.Defanse * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefanse, Mathf.FloorToInt((Base.SpDefanse * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);

        MaxHp = Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10 +Level;
    }
    void ResetStatBost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack,0 },
            {Stat.Defanse,0 },
            {Stat.SpAttack,0 },
            {Stat.SpDefanse,0 },
            {Stat.Speed,0 },
            {Stat.Accuracy,0 },
            {Stat.Evasion,0 },

        };
    }
    int GetStat(Stat stat)
    {
        int statValue = Stats[stat];

        // Apply stat boost
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
            statValue = Mathf.FloorToInt(statValue * boostValues[boost]);
        else
            statValue = Mathf.FloorToInt(statValue / boostValues[-boost]);
        return statValue;
    }
    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (var statboost in statBoosts)
        {
            var stat = statboost.stat;
            var boost = statboost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            if (boost > 0)
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            else
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");
            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]} ");
        }
    }

    public bool CheckForLevelUp()
    {
        if(Exp > Base.GetExpForLevel(Level + 1)){
            ++_level;
            return true;
        }
        return false;
    }
    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }
    public int Defanse
    {
        get { return GetStat(Stat.Defanse); }
    }
    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); }
    }
    public int SpDefanse
    {
        get { return GetStat(Stat.SpDefanse); }
    }
    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }
    public int MaxHp { get; private set; }
    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);
        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false

        };
        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAttack : attacker.Attack;
        float defanse = (move.Base.Category == MoveCategory.Special) ? SpDefanse : Defanse;
        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defanse) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHp(damage);
        return damageDetails;

    }
    public void UpdateHp(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHp);
        HpChanged = true;
    }
    public void SetStatus(ConditionID conditionID)
    {
        if (Status != null) return;


        Status = ConditionDB.Conditions[conditionID];
        Status?.Onstart?.Invoke(this); 
        StatusChanges.Enqueue($"{Base.Name} {Status.StartMessage}");

        OnStatusChanged?.Invoke();
    }
    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }
    public void SetVolatileStatus(ConditionID conditionID)
    {
        if (VolatileStatus != null) return;


        VolatileStatus = ConditionDB.Conditions[conditionID];
        VolatileStatus?.Onstart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {VolatileStatus.StartMessage}");
    }
    public void CureVolatileStatus()
    {
        VolatileStatus = null;
    }
    public Move GetRandomMove()
    {
        var moveWhitPP = Moves.Where(x => x.PP > 0).ToList();    


        int r = Random.Range(0, moveWhitPP.Count);
        return moveWhitPP[r];
    }
    public bool OnBeforeMove()
    {
        bool canPerformMove = true;
        if (Status?.OnBeforeMove != null)
        {
           if(!Status.OnBeforeMove(this))
                canPerformMove = false;
        }
        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (!VolatileStatus.OnBeforeMove(this))
                canPerformMove = false;
        }
        return canPerformMove;
    }
    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }
    public void OnBattleOver()
    {
        VolatileStatus = null;
        ResetStatBost();
    }
}
public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
