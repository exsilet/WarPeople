using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "StaticData/Skill")]
public class SkillStaticData : ScriptableObject
{
    public Sprite UIIcon;
    public string Label;
    public string Description;
    public int CountLimit;
    [SerializeField] private int _count;
    public SkillTypeId Type;
    [Range(0, 10)] public int Damage;
    [Range(0, 10)] public int Defence;
    [Range(0, 10)] public int Evasion;
    [Range(0, 10)] public int Counterstrike;
    public bool IsDefault;

    public int Count => _count;
}