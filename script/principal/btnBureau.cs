using Godot;
using System;

public partial class btnBureau : Button
{
	public override void _Ready()
	{
		this.Pressed+=OuvrirBureau;
	}
	public override void _Process(double delta)
	{
		
	}
	private void OuvrirBureau()
	{
		GetTree().ChangeSceneToFile("res://scenes/amelioration.tscn");
		
	}
}
