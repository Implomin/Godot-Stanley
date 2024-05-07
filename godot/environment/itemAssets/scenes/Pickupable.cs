using Godot;
using System;

public partial class Pickupable : RigidBody3D
{
	public bool isPickedUp = false;
    public override void _Ready()
    {
		this.SetCollisionLayerValue(4,true);
    }
    public override void _PhysicsProcess(double delta)
    {
        if(isPickedUp){
			this.SetCollisionLayerValue(4,true);
		}
		else
		{
			this.SetCollisionLayerValue(4,false);
		}
    }
}
