using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonsBase : ScriptableObject
{
    [SerializeField] string _name;
    [TextArea]
    [SerializeField] string _description;
    [SerializeField] Sprite _frontSprite;
    [SerializeField] Sprite _backSprite;
    [SerializeField] PokemonType _type1;
    [SerializeField] PokemonType _type2;
    [SerializeField] GrowthRate _growthRate;

    [SerializeField] int _maxHp;
    [SerializeField] int _attack;
    [SerializeField] int _defanse;
    [SerializeField] int _spAttack;
    [SerializeField] int _spDefanse;
    [SerializeField] int _speed;


    [SerializeField] int _expYield;


    [SerializeField] int _catchRate =255;

    [SerializeField] List<LearnableMove> _learnableMoves;
    public int GetExpForLevel(int level)
    {
        if(_growthRate == GrowthRate.Fast)
        {
            return 4*(level * level*level) /5;
        }
        else if(_growthRate == GrowthRate.Medium)
        {
            return level * level * level;
        }
        return -1;
    }
    public string Name
    {
        get { return _name; }
    }
    public string Description
    {
        get { return _description; }
    }
    public Sprite FrontSprite
    {
        get { return _frontSprite; }
    }
    public Sprite BackSprite
    {
        get { return _backSprite; }
    }
    public PokemonType Type1
    {
        get { return _type1; }
    }
    public PokemonType Type2
    {
        get { return _type2; }
    }
    public int MaxHp
    {
        get { return _maxHp; }
    }
    public int Attack
    {
        get { return _attack; }
    }
    public int Defanse
    {
        get { return _defanse; }
    }
    public int SpAttack
    {
        get { return _spAttack; }
    }
    public int SpDefanse
    {
        get { return _spDefanse; }
    }
    public int Speed
    {
        get { return _speed; }
    }
    public List<LearnableMove> LearnableMoves
    {
        get { return _learnableMoves; }
    }
    public int CatchRate => _catchRate;
    public int ExpYield => _expYield;
    public GrowthRate GrowthRate => _growthRate;
}
[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase _moveBase;
    [SerializeField] int _level;
    public MoveBase Base
    {
        get { return _moveBase; }
    }
    public int Level
    {
        get { return _level; }
    }
}
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon

}
public enum GrowthRate
{
    Fast , Medium
}
public enum Stat
{
    Attack,
    Defanse,
    SpAttack,
    SpDefanse,
    Speed,

    Accuracy,
    Evasion
}
public class TypeChart
{
    static float[][] chart =
     {      //             Nor Fire  Wat  Ele  Gra  Ice  FIG  POI
    /*Nor*/ new float[] { 1f,  1f,   1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f, 0.5f, 0f,  1f,  1f, 0.5f },
   /*Fire*/ new float[] { 1f, 0.5f, 0.5f, 1f,  2f,  2f,  1f,  1f,  1f,  1f,  1f,  2f, 0.5f, 1f, 0.5f, 1f,  2f },
  /*Wat*/   new float[] { 1f,  2f,  0.5f, 2f, 0.5f, 1f,  1f,  1f,  2f,  1f,  1f,  1f,  2f,  1f, 0.5f, 1f,  1f },
    /*Ele*/ new float[] { 1f,  1f,   2f, 0.5f,0.5f, 2f,  1f,  1f,  0f,  2f,  1f,  1f,  1f,  1f, 0.5f, 1f,  1f },
   /*Gra*/  new float[] { 1f, 0.5f,  2f,  2f, 0.5f, 1f,  1f, 0.5f, 2f, 0.5f, 1f, 0.5f, 2f,  1f, 0.5f, 1f, 0.5f },
  /*ICE*/   new float[] { 1f, 0.5f, 0.5f, 1f,  2f, 0.5f, 1f,  1f,  2f,  2f,  1f,  1f,  1f,  1f,  2f,  1f, 0.5f },  
 /*fight*/  new float[] { 2f,  1f,   1f,  1f,  1f,  2f,  1f, 0.5f, 1f, 0.5f, 0.5f, 0.5f, 2f, 0f, 1f,  2f,  2f },
    /*POI*/ new float[] { 1f,  1f,   1f,  1f,  2f,  1f,  1f, 0.5f, 0.5f, 1f, 1f,  1f, 0.5f, 0.5f, 1f, 1f,  0f },
/*ground*/  new float[] { 1f,  2f,   1f,  2f, 0.5f, 1f,  1f,  2f,  1f,  0f,  1f, 0.5f, 2f,  1f,  1f,  1f,  2f },
    /*fly*/ new float[] { 1f,  1f,   1f, 0.5f, 2f,  1f,  2f,  1f,  1f,  1f,  1f,  2f, 0.5f, 1f,  1f,  1f, 0.5f },
  /*phys*/  new float[] { 1f,  1f,   1f,  1f,  1f,  1f,  2f,  2f,  1f,  1f, 0.5f, 1f,  1f,  1f,  1f,  0f, 0.5f },
   /*bug*/  new float[] { 1f, 0.5f,  1f,  1f,  2f, 1f, 0.5f, 0.5f, 1f, 0.5f, 2f,  1f,  1f, 0.5f, 1f,  2f, 0.5f },
 /*Rock*/   new float[] { 1f,  2f,   1f,  1f,  1f,  2f, 0.5f, 1f, 0.5f, 2f,  1f,  2f,  1f,  1f,  1f,  1f, 0.5f },
 /*Ghost*/  new float[] { 0f,  1f,   1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,  2f, 1f, 0.5f, 0.5f },
 /*Dragon*/ new float[] { 1f,  1f,   1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  2f,  1f,  0.5f},
  /*Dark*/  new float[] { 1f,  1f,   1f,  1f,  1f,  1f, 0.5f, 1f,  1f,  1f,  2f,  1f,  1f,  2f, 1f, 0.5f, 0.5f },
 /*Steel*/  new float[] { 1f, 0.5f, 0.5f, 0.5f, 1f, 2f,  1f,  1f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,  1f,  0.5f },
 /*Fairy*/  new float[] { 1f, 0.5f,   1f,  1f,  1f, 1f,  2f,  0.5f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,  1f,  0.5f }
     };
    public static float GetEffectiveness(PokemonType attacktType, PokemonType defanseType)
    {
        if (attacktType == PokemonType.None || defanseType == PokemonType.None)
            return 1f;
        int row = (int)attacktType -1;
        int col = (int)defanseType -1;
        return chart[row][col];
    }
}