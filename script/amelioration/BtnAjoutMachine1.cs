using Godot;
using System;

public partial class BtnAjoutMachine1 : Button
{
	private nodeRootPrincipal _root;
	
	// pour faire les équilibrages plus tard
	private const float COUT_NOUV_MACHINE = 500f;
	
	//test d'ajout de la scène prod pour ajouter différente fois la machine
	private PackedScene _machineScene = GD.Load<PackedScene>("res://scenes/production.tscn");
	
	public override void _Ready()
	{
		// récupération de code venant de ControlVente.cs pour trouver root
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");				
		
		this.Pressed+=nouvMachine;
	}
	
	public override void _Process(double delta)
	{
		
	}
	private void nouvMachine()
	{
		if(_root.getArgent() >= COUT_NOUV_MACHINE)
		{
			_root.subArgent(COUT_NOUV_MACHINE);
			
			_root._machineCountCol1++;
			
			Control machineContainer = (Control)_machineScene.Instantiate();  
			
			Area2D machineArea = machineContainer.GetNode<Area2D>("Area2DMachine");
			
			if (machineArea != null)
			{
				machineArea.InputPickable = false;
			}			
			
			if (_root.colonne1 != null)
			{
				//_root.colonne1.AddChild(machineContainer);
				//GD.Print($"Nouvelle machine {_root._machineCountCol1} ajoutée à colonne1 !");				
				_root.AddNewMachine(_root.colonne1, _root._machineCountCol1);            
				GD.Print($"Nouvelle machine {_root._machineCountCol1} ajoutée à colonne1 !");
			}
			else
			{
				GD.PrintErr("Erreur : La référence à 'colonne1' est manquante dans nodeRootPrincipal.");
			}
		} 
		else		
		{
			GD.Print("Achat impossible : Argent insuffisant.");
		}
		
	}
}
