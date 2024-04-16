using Godot;
using System;

[GlobalClass]
public partial class MovementState : Resource
{
    [Export] public int id;
    [Export] public float acceleration;
    [Export] public float movementSpeedModifier;

    // eventually fovModifier can be based on players current movementSpeed
    [Export] public float fovModifier;
}
