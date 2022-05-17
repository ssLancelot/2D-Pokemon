using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask _solidObjecktLayer;
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] LayerMask _grassLayer;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] LayerMask _fovLayer;

    public static GameLayers i { get; set; }
    private void Awake()
    {
        i = this;
    }
    public LayerMask GrassLayer
    {
        get => _grassLayer;
    }
    public LayerMask InteractableLayer
    {
        get => _interactableLayer;
    }
     public LayerMask SolidObcejtLayer
    {
        get => _solidObjecktLayer;
    }
    public LayerMask PlayerLayer
    {
        get => _playerLayer;
    }
    public LayerMask FovLayer
    {
        get => _fovLayer;
    }
}
