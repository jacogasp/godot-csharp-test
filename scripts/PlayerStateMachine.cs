using Godot;

public partial class PlayerStateMachine
{
  PlayerState currentState = new IdleState();

  public void Ready(Player player)
  {
    currentState.Enter(player);
    currentState.HandleInput(player);
  }

  public void PhysicsProcess(Player player, double delta)
  {
    var nextState = currentState.HandleInput(player);
    if (nextState == currentState)
    {
      return;
    }
    currentState = nextState;
    currentState.Enter(player);
    currentState.HandleInput(player);
    currentState.Update(player, delta);
  }
}
