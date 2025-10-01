using Godot;
using System;

public partial class Area2DMachine : Area2D
{
	
	private int _prodMan = 1;
	private int _prodAuto = 0;
	private nodeRootPrincipal _root;

	public override void _Ready()
	{
		_root = GetTree().Root.GetNode<nodeRootPrincipal>("nodeRootPrincipal");
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");
	}

	public void SetProdMan(int ajout)
	{
		_prodMan += ajout;
	}

	public void SetProdAuto(int ajout)
	{
		_prodAuto += ajout;
	}

	// ✅ Cet événement est appelé SEULEMENT quand tu cliques sur ton Area2D (défini par CollisionShape2D ou CollisionPolygon2D)
	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				AjouterStockMan();
			}
		}
	}

	public void AjouterStockMan()
	{
		_root.addStock(2);
		GD.Print(_root.getStock());
	}
}
