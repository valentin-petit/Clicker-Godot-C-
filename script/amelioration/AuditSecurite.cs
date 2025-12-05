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
	private Label lblImpact;
	
	private nodeRootPrincipal _root;
	
	public override void _Ready()
	{
		_root = GetNode<nodeRootPrincipal>("/root/NodeRootPrincipal");
		
		lblObjectif = GetNode<Label>("sprSecuF1/lblObjectif");
		lblBut = GetNode<Label>("sprSecuF1/lblBut");
		lblStatutActuel = GetNode<Label>("sprSecuF1/lblStatutActuel");		
		
		lblAction = GetNode<Label>("sprSecuF1/lblAction");
		lblCout = GetNode<Label>("sprSecuF1/lblCout");
		lblImpact = GetNode<Label>("sprSecuF1/lblImpact");
		//test des audits random
		InitializeAuditData(ID_THEME);
	}
	
	private void InitializeAuditData(string key)
	{
		// Appelez la NOUVELLE méthode statique de la Fabrique pour obtenir 1 proposition
		// Nous voulons 1 seule proposition complète à la fois
		List<AuditProposition> propositions = AuditSceneFactory.GetRandomPropositions(key, 1);

		if (propositions.Count >= 1)
		{
			AuditProposition proposition = propositions[0];

			// 2. Remplir les Labels avec les données de la proposition
			lblObjectif.Text = proposition.Objectif;
			lblBut.Text = proposition.But;
			lblStatutActuel.Text = proposition.StatutActuel;
			lblAction.Text = proposition.Action;
			lblCout.Text = proposition.Cout;
			lblImpact.Text = proposition.Impact;
			
			GD.Print($"Proposition d'audit {key} chargée.");
		}
		else
		{
			GD.PrintErr($"ERREUR DE FABRIQUE : Aucune proposition trouvée pour la clé : {key}.");
		}
	}
	
	private void _on_btn_investir_pressed()
	{
		string coutNettoye = lblCout.Text.Replace(" ", ""); 

		if (int.TryParse(coutNettoye, out int coutInt))
		{
			float coutFloat = (float)coutInt;
			
			GD.Print($"Coût d'investissement : {coutFloat} (en float)");

			if(_root != null && _root.getArgent() >= coutFloat) 
			{
				_root.subArgent(coutFloat); 
				
				GD.Print("Investissement réussi ! Argent dépensé et impact appliqué.");
			}        
			else 
			{
				GD.PrintErr("Fonds insuffisants. Impossible de payer cette recommandation.");
			}
		}
		else
		{
			GD.PrintErr("Erreur critique : Problème de format de coût. Le coût n'est pas un nombre valide.");
		}
	}
}	
