using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlant
{
    Dictionary<PlantState, Action> StateActions { get; set; }
}
