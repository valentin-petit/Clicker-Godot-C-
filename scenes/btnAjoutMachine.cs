using Godot;
using System;

public partial class btnAjoutMachine : Button
{
	private nodeRootPrincipal _root;
	
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
		if(_root.getArgent() >= COUT_NOUV_MACHINE){
			_root.subArgent(COUT_NOUV_MACHINE);		
			
			// instaciation d'une machine de tupe area2 comme dans la scene prod
			Area2DMachine nouvMachine = (Area2DMachine)_machineScene.Instantiate();	
			
			//permet au noeud de ne pas recevoir d'évènement et annule donc l'évènement cliquable même si on l'enlèvera plus tard
			nouvMachine.InputPickable = false;
			
			// positionnement (temporaire pcq la ca marche pas si j'en mets plus qu'une)
			nouvMachine.Position = new Vector2(300, 800);	
			nouvMachine.Scale = new Vector2(1.2f, 1.2f); 	
			
			_root.AddChild(nouvMachine);
		}
		
	}
}
