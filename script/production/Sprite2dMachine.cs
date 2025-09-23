using Godot;
using System;

public partial class Sprite2dMachine : Sprite2D
{
	private int _prodMan=1;
	private int _prodAuto=0;
	private nodeRootPrincipal _root;

	public override void _Ready()
	{
		_root = GetTree().Root.GetNode<nodeRootPrincipal>("nodeRootPrincipal");
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");
	}

	public override void _Process(double delta)
	{
		}
	public void setProdMan(int ajout)
	{
		this._prodMan+=ajout;
	}
	public void setProdAuto(int ajout)
	{
		this._prodAuto+=ajout;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				
				ajouterStockMan();
			}
		}
	}
	public void ajouterStockMan()
	{
		
		_root.addStock(2);
		GD.Print(_root.getStock());
	}
	
	
}
