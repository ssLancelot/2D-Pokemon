using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pokemon> _wildPokemons;

    public Pokemon GetRandomWildPokemon()
    {
        var wildpokemon= _wildPokemons[Random.Range(0, _wildPokemons.Count)];
        wildpokemon.Init();
        return wildpokemon;
    }
}
