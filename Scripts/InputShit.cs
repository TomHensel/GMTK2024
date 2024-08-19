using Godot;
using System;

public partial class InputShit : Control
{
	//[Export] private Godot.Collections.Array<Node> nodesNeedingInput;

	//[Export] private Player player;
	
	

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		
		// GD.Print(@event.AsText());

		//player.handInput(@event);
		
		// foreach (var node in nodesNeedingInput)
		// {
		// 	GD.Print(node.ToString());
		// 	node._Input(@event);
		// }
	}
}
