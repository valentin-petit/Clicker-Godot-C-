using Godot;
using System;

public partial class TextureButtonAcceuil : TextureButton
{
	
	public override void _Ready()
	{
		this.Pressed+=FermerBureau;
	}
	public override void _Process(double delta)
	{
		
	}
	private async void FermerBureau()
	{
		await ToSignal(GetTree().CreateTimer(0.23f), "timeout");

		this.GetParent().GetParent().QueueFree();


	
	}
	}
