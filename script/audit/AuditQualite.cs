using Godot;
using System;
using System.Collections.Generic;

public partial class AuditQualite : Node2D
{
	private const string ID_THEME = "Q"; 
	private Label lblObjectif;
	private Label lblBut;
	private Label lblStatutActuel;
	private Label lblAction;
	private Label lblCout;
	
	private nodeRootPrincipal _root;
	
	public override void _Ready()
	{
		_root = GetNode<nodeRootPrincipal>("/root/NodeRootPrincipal");
		
		lblObjectif = GetNode<Label>("sprQuaF1/lblObjectif");
		lblBut = GetNode<Label>("sprQuaF1/lblBut");
		lblStatutActuel = GetNode<Label>("sprQuaF1/lblStatutActuel");		
		
		lblAction = GetNode<Label>("sprQuaF1/lblAction");
		lblCout = GetNode<Label>("sprQuaF1/lblCout");

		InitializeAuditData(ID_THEME);
	}
	
	private void InitializeAuditData(string key)
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
	
	public void _on_chk_investir_pressed()
	{
				//recupe le cout de l'investissement chargé dans le string de lblCout et enlève les espaces pour en faire un int
		string coutNettoye = lblCout.Text.Replace(" ", ""); 
		//si coutNettoye peut etre traduis en Int, le faire
		if (int.TryParse(coutNettoye, out int coutInt))
		{
			float coutFloat = (float)coutInt; // le cast en float
			
			GD.Print($"Coût d'investissement : {coutFloat}");// message evenement
			
			if(_root != null && _root.getArgent() >= coutFloat) //si le control principal existe et argent > investissement
			{
				_root.subArgent(coutFloat); // on paye
				
				GD.Print("Investissement réussi ! Argent dépensé et impact appliqué.");
			}        
			else 
			{
				GD.PrintErr("Fonds insuffisants. Impossible de payer cette recommandation.");
			}
		}
		else
		{
			GD.PrintErr("Erreur critique : Problème de format de coût. Le coût n'est pas un nombre valide."); //on a pas pu faire de int (normalement y a pas de raison)
		}
	}
}
