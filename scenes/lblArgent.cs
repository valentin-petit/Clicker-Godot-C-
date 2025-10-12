using Godot;
using System;

public partial class lblArgent : Label
{
	private nodeRootPrincipal _root;
	
	public override void _Ready()
	{		
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		
		Text = "Argent : " + _root.getArgent().ToString("F2");	
	}
	public override void _Process(double delta)
	{		
		Text = "Argent : " + _root.getArgent().ToString("F2");		
	}
}
