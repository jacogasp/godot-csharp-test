using System.Runtime.InteropServices;
using Godot;


public abstract class PlayerState
{
  public abstract void Enter(Player player);
  public abstract PlayerState HandleInput(Player player);
  public abstract void Update(Player player, double delta);
}

public class IdleState : PlayerState
{
  public override void Enter(Player player)
  {
    var animation = player.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    animation.Play("idle");
    var velocity = player.Velocity;
    velocity.X = 0;
    player.Velocity = velocity;
    GD.Print("enter idle");
  }

  public override PlayerState HandleInput(Player player)
  {
    if (Input.IsActionJustPressed("jump"))
    {
      return new JumpingState();
    }
    if (Mathf.Abs(Input.GetAxis("ui_left", "ui_right")) > 0)
    {
      return new RunningState();
    }
    return this;
  }
  public override void Update(Player player, double delta)
  {

  }
}

class RunningState : PlayerState
{
  public override void Enter(Player player)
  {
    var animation = player.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    animation?.Play("run");
    GD.Print("enter run");
  }

  public override PlayerState HandleInput(Player player)
  {
    var axis = Input.GetAxis("ui_left", "ui_right");
    if (Mathf.Abs(axis) < 0.01)
    {
      return new IdleState();
    }
    if (Input.IsActionJustPressed("jump"))
    {
      return new JumpingState();
    }
    return this;
  }

  public override void Update(Player player, double delta)
  {
    float axis = Input.GetAxis("ui_left", "ui_right");
    var velocity = player.Velocity;
    velocity.X = axis * player.Speed;
    player.Velocity = velocity;
    var animation = player.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    if (axis < 0)
    {
      animation.FlipH = true;
    }
    else if (axis > 0)
    {
      animation.FlipH = false;
    }
  }
}

class JumpingState : PlayerState
{
  public override void Enter(Player player)
  {
    var animation = player.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    animation.Play("jump");
    var velocity = player.Velocity;
    velocity.Y = player.JumpVelocity;
    player.Velocity = velocity;
    GD.Print("enter jump!");
  }

  public override PlayerState HandleInput(Player player)
  {
    if (player.IsOnFloor())
    {
      var axis = Input.GetAxis("ui_left", "ui_right");
      if (Mathf.Abs(axis) > 0.01)
      {
        return new RunningState();
      }
      else
      {
        return new IdleState();
      }
    }
    return this;
  }

  public override void Update(Player player, double delta)
  {

  }
}
