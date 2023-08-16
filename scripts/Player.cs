using Godot;
using System;
using System.Data;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public float JumpVelocity = -400.0f;

	private readonly PlayerStateMachine stateMachine = new();

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		stateMachine.Ready(this);
	}
	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;
		velocity.Y += gravity * (float)delta;
		Velocity = velocity;
		stateMachine.PhysicsProcess(this, delta);
		MoveAndSlide();
	}

}
