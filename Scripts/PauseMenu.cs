using Godot;
using System;

public partial class PauseMenu : Control
{
	private HSlider volumeSlider;

	private Button continueButton;
	private Button quitButton;
	
	
	public override void _Ready()
	{
		volumeSlider = GetNode<HSlider>("HSlider");
		continueButton = GetNode<Button>("Continue");
		quitButton = GetNode<Button>("Quit");
        
        

	}

	public override void _Process(double delta)
	{
		if (continueButton.IsPressed())
		{
			GetTree().Paused = false;
			Visible = false;
			Input.MouseMode = Input.MouseModeEnum.Confined;
		}

		if (quitButton.IsPressed())
		{
			GetTree().Quit();
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("Escape"))
		{
			if (Visible)
			{
				GetTree().Paused = false;
				Visible = false;
				Input.MouseMode = Input.MouseModeEnum.Confined;
			}
			else
			{
				GetTree().Paused = true;
				Visible = true;
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		}
	}

	public void _on_h_slider_value_changed(float value)
	{
		// GD.Print(value);
		int masterIndex = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(masterIndex,Mathf.LinearToDb(value));
	}
}
