using Godot;
using System;
using System.Collections.Generic;

public partial class AuditFiabilite : Node2D
{
	private const string ID_THEME = "F"; 
	private Label lblObjectif;
	private Label lblBut;
	private Label lblStatutActuel;
	private Label lblAction;
	private Label lblCout;
	
	private nodeRootPrincipal _root;
	
	public override void _Ready()
	{
		_root = GetNode<nodeRootPrincipal>("/root/NodeRootPrincipal");
		
		lblObjectif = GetNode<Label>("sprFiaF1/lblObjectif");
		lblBut = GetNode<Label>("sprFiaF1/lblBut");
		lblStatutActuel = GetNode<Label>("sprFiaF1/lblStatutActuel");		
		
		lblAction = GetNode<Label>("sprFiaF1/lblAction");
		lblCout = GetNode<Label>("sprFiaF1/lblCout");

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
		
}
