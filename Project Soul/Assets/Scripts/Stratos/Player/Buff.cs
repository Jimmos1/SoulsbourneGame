using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff 
{
    public int _id;
    public string _name;
    public string _description;
    public bool _canStack;
    public float _duration;
    public float _expireTime; // _expireTime => { _expireTime = Time.time + _duration; }

    public GameObject _source; //who buffed me?
    public Dictionary<string, int> _stats = new Dictionary<string, int>();

    //public Sprite _icon; //for future UI use

    public Buff(int id, string name, string description, bool canStack,
                float duration, Dictionary<string, int> stats)
    {
        this._id = id;
        this._name = name;
        this._description = description;
        this._duration = duration;
        this._stats = stats;
        this._canStack = canStack;
    }
    public Buff(Buff buff)
    {
        this._id = buff._id;
        this._name = buff._name;
        this._description = buff._description;
        this._duration = buff._duration;
        this._stats = buff._stats;
        this._canStack = buff._canStack;
    }
}
