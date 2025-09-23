using Godot;
using System;

public partial class btnQuitter : Button
{
	public override void _Ready()
	{
		this.Pressed+=FermerBureau;
	}
	public override void _Process(double delta)
	{
		
	}
	private void FermerBureau()
	{
		GetTree().ChangeSceneToFile("res://scenes/principal.tscn");
		
	}
}
