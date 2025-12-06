using Godot;
using System;

public partial class BtnAjoutMachine3 : Button
{
	private nodeRootPrincipal _root;

	private const float COUT_NOUV_MACHINE = 500f;

	private PackedScene _machineScene = GD.Load<PackedScene>("res://scenes/machine3.tscn");

	public override void _Ready()
	{
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");

		Pressed += nouvMachine;
	}

	private void ShowPopupNbMax()
	{
		var dialog = new AcceptDialog
		{
			Title = "Attention",
			DialogText = "Nb max atteint"
		};

		// Ajout à la scène
		GetTree().CurrentScene.AddChild(dialog);

		// Affiche centré
		dialog.PopupCentered();
	}

	private void nouvMachine()
	{
		if (_root.getArgent() >= COUT_NOUV_MACHINE)
		{
			if (_root._machineCountCol3 >= 3)
			{
				ShowPopupNbMax();
				GD.Print("Ajout impossible : vous avez déjà 3 machines de type 3 !");
				return;
			}

			_root.subArgent(COUT_NOUV_MACHINE);
			_root._machineCountCol3++;

			Control machineContainer = (Control)_machineScene.Instantiate();

			Area2D machineArea = machineContainer.GetNode<Area2D>("Area2DMachine");
			if (machineArea != null)
				machineArea.InputPickable = false;

			if (_root.colonne3 != null)
			{
				_root.AddNewMachine(_root.colonne3, _root._machineCountCol3);
				GD.Print($"Nouvelle machine {_root._machineCountCol3} ajoutée à colonne3 !");
			}
			else
			{
				GD.PrintErr("Erreur : La référence à 'colonne3' est manquante dans nodeRootPrincipal.");
			}
		}
		else
		{
			GD.Print("Achat impossible : Argent insuffisant.");
		}
	}
}
