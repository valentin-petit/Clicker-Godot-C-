using Godot;
using System;
using System.Collections.Generic;

public partial class AuditSecurite : Node2D
{
	private const string ID_THEME = "S"; 
	private Label lblObjectif;
	private Label lblBut;
	private Label lblStatutActuel;
	private Label lblAction;
	private Label lblCout;
	
	private nodeRootPrincipal _root;
	
	
	public override void _Ready()
	{
		_root = GetNode<nodeRootPrincipal>("/root/NodeRootPrincipal");
		
		lblObjectif = GetNode<Label>("sprSecuF1/lblObjectif");
		lblBut = GetNode<Label>("sprSecuF1/lblBut");
		lblStatutActuel = GetNode<Label>("sprSecuF1/lblStatutActuel");		
		
		lblAction = GetNode<Label>("sprSecuF1/lblAction");
		lblCout = GetNode<Label>("sprSecuF1/lblCout");
		

		InitializeAuditData(ID_THEME);
		
	}
	
	public void InitializeAuditData(string key)
	{
		//prend une propal parmis celles référencé dans AuditSceneFactory
		List<AuditProposition> propositions = AuditSceneFactory.GetRandomPropositions(key, 1);

		if (propositions.Count >= 1)
		{
			AuditProposition proposition = propositions[0];

			// remplissage des labels par la propal
			lblObjectif.Text = proposition.Objectif;
			lblBut.Text = proposition.But;
			lblStatutActuel.Text = proposition.StatutActuel;
			lblAction.Text = proposition.Action;
			lblCout.Text = proposition.Cout;
			
			GD.Print($"audit {key} chargé.");
		}
		else
		{
			GD.PrintErr($"ERREUR : Aucune proposition trouvée pour {key}.");
		}
	}			
	
	private void _on_txtbtn_signature_quitter_pressed()
	{				
		_root._sceneAmelioration.Hide();
	}
}	
