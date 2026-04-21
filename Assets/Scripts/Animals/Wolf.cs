using UnityEditor.Build;
using UnityEngine;

public class Wolf : Creature

{
    void Awake()
    {
        SetEatStrategy(new CarnivoreEatStrategy());
        SetTargetStrategy(new CarnivoreTargetStrategy());
    }



}
