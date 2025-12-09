using Godot;
using System;

public partial class TxtbtnSignatureQuitter : TextureButton
{
	private nodeRootPrincipal _root ;	
	
	
	public override void _Ready()
	{
		
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		//prend la root node de prinipal
		this.Pressed+=FermerTout;
	}
	public override void _Process(double delta)
	{
		
	}
	
	// a finir
	private void FermerTout()
	{
		if (_root == null) return;
		
		_root._sceneAmelioration.Hide();
		
		Sprite2D sprMenuTel = _root.GetNodeOrNull<Sprite2D>("sprMenuTel");
		if (sprMenuTel != null)
		{
			sprMenuTel.Hide();
		}	
		
		SceneController sceneController = _root.GetNodeOrNull<SceneController>("res://scenes/SceneController.cs");
		if (sceneController != null)
		{
			//sceneController.HideAllPanels(); 
			sceneController.SelectAuditPanel("NONE");
		}						
	
	}
}
