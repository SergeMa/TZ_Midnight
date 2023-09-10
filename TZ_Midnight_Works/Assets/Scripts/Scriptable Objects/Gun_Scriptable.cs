using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun")]
public class Gun_Scriptable : ScriptableObject
{
    public string GunName = "Default Gun";

    public int Damage_Per_Shot;
    public int Bullets_In_Magazine;
    public float Time_Between_Shots;
    public float Reload_Time;
}
