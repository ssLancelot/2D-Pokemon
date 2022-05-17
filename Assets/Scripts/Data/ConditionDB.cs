using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDB
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.ID = conditionId;
        }
    }
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn ,
            new Condition()
            {
                Name = " Poison",
                StartMessage =" has been poisoned ",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHp(pokemon.MaxHp/8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself due to poison");
                }

            }

        },
        {
            ConditionID.brn ,
            new Condition()
            {
                Name = " Burn",
                StartMessage =" has been Burned ",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHp(pokemon.MaxHp/16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself due to burn");
                }

            }

        },
        {
            ConditionID.par ,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage =" has been Paralyzed ",
               OnBeforeMove = (Pokemon pokemon) =>
               {
                 if(  Random.Range(1,5) == 1)
                   {
                       pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}'s paralyzed and can't move");
                       return false;
                   }

                   return true;
               }

            }

        },
        {
            ConditionID.frz ,
            new Condition()
            {
                Name = "Freeze",
                StartMessage =" has been frozen ",
               OnBeforeMove = (Pokemon pokemon) =>
               {
                 if(  Random.Range(1,5) == 1)
                   {
                       pokemon.CureStatus();
                       pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}'s is not frozen anymore");
                       return true;
                   }

                   return false;
               }

            }

        },
        {
            ConditionID.slp ,
            new Condition()
            {
                Name = "Sleep",
                StartMessage =" has fallen asleep ",
                Onstart= (Pokemon pokemon) =>
                {
                           //Sleep for 1-3 turn
                           pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} moves");
                },


               OnBeforeMove = (Pokemon pokemon) =>
               {
                   if(pokemon.StatusTime <=0)
                   {
                       pokemon.CureStatus();
                         pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke Up");
                       return true;
                   }

                   pokemon.StatusTime --;
                   pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is sleeping");
                   return false;
               }

            }

        },
        //Volatile Status Conditions
        {
            ConditionID.confusion ,
            new Condition()
            {
                Name = "Confusion",
                StartMessage =" has been confused ",
                Onstart= (Pokemon pokemon) =>
                {
                           //Confused for 1-4 turns
                           pokemon.VolatileStatusTime = Random.Range(1,5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} moves");
                },


               OnBeforeMove = (Pokemon pokemon) =>
               {
                   if(pokemon.VolatileStatusTime <=0)
                   {
                       pokemon.CureVolatileStatus();
                         pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} kicked out of confusion!");
                       return true;
                   }

                   pokemon.VolatileStatusTime --;
                   // 50% chance to do a move
                   if (Random.Range(1, 3) == 1)
                       return true;
                   //Hurt by confusion
                   pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused");
                   pokemon.UpdateHp(pokemon.MaxHp / 8);
                   pokemon.StatusChanges.Enqueue($"It hurt itself due to confusion");
                   return false;
               }

            }
        }
    };
    public static float  GetStatusBonus(Condition condition)
    {
        if (condition == null)
            return 1;
        else if (condition.ID == ConditionID.slp || condition.ID == ConditionID.frz)
            return 2f;
        else if (condition.ID == ConditionID.par || condition.ID == ConditionID.psn || condition.ID == ConditionID.brn)
            return 1.5f;
        return 1f;
    }
}
public enum ConditionID
{
    none, psn, brn, slp, par, frz,
    confusion
}
