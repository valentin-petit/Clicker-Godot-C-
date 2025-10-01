using Godot;
using System;

public partial class btnBureau : Button
{
	private nodeRootPrincipal _root;
	public override void _Ready()
	{
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		this.Pressed+=OuvrirBureau;
	}
	public override void _Process(double delta)
	{
		
	}
	private void OuvrirBureau()
	{
		//_ameliorationScene.Show();
		_root._sceneAmelioration.Show();
		
	}
}
