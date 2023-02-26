using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SO_Vector3 : ScriptableObject
{
    public Vector3 vectorThree;
    public int squadNumber;

    private void Awake()
    {
        squadNumber = 0;
    }

    public void addSquadNumber()
    {
        squadNumber++;
    }
}
