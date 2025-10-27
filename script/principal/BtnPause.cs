using Godot;
using System;

public partial class BtnPause : Button
{
	private nodeRootPrincipal _root;
	public override void _Ready()
	{
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		this.Pressed+=MettrePause;
	}
	public override void _Process(double delta)
	{
		
	}
	private void MettrePause()
	{
		PackedScene ac = GD.Load<PackedScene>("res://scenes/menuPause.tscn");
		Control _sceneAcceuille = (Control)ac.Instantiate();
		_sceneAcceuille.Size = GetViewport().GetVisibleRect().Size;

		_root.AddChild(_sceneAcceuille);
		
	}

}
