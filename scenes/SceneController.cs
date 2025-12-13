using Godot;
using System;
using System.Collections.Generic;

public partial class SceneController : Node2D
{
	// utilisation de CanvasItem qui est parent à Node2D car on peut pas faire de Node2D.Visible directement (pour afficher les différents audits)
	private readonly Dictionary<string, CanvasItem> AuditPanels = new Dictionary<string, CanvasItem>(); // gère l'affichage
	private readonly Dictionary<string, Node> AuditNodes = new Dictionary<string, Node>(); // Gère l'accès aux scripts
	
	
	private nodeRootPrincipal _root; 
	//private bool _isInvestmentApplied = false;
	private readonly Dictionary<string, bool> _investmentsApplied = new Dictionary<string, bool>();
	
	public override void _Ready()
	{
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		
		//stock les nodes d'audit
		AuditNodes.Add("S", GetNode<Node>("AuditSecurite"));
		AuditNodes.Add("Q", GetNode<Node>("AuditQualite"));
		AuditNodes.Add("F", GetNode<Node>("AuditFiabilite"));
		
		// remplit les panels avec chaque key / scenes
		foreach (var entry in AuditNodes)
		{
			AuditPanels.Add(entry.Key, entry.Value as CanvasItem);
		}
		
		ConnectAllAuditSignals(); 

		// Initialiser l'état d'investissement pour chaque audit à false
		foreach (var key in AuditNodes.Keys)
		{
			_investmentsApplied.Add(key, false);
		}

		// cache tous les panels (audit) à l'initialisation
		foreach (var panel in AuditPanels.Values)
		{
			panel.Visible = false;
		}
	}
	
	// connecte les signaux de chaque audit à la même méthode centrale
	private void ConnectAllAuditSignals()
	{
		//pour chaque node Audit on lui récupe sa clée et son Node2D et en fonction de la clé on affichera le bon Audit
		foreach (var entry in AuditNodes)
		{
			string key = entry.Key; 
			Node auditNode = entry.Value;
			
			// Connexion spécifique basée sur le type de script (cast nécessaire)
			if (key == "S")
			{				
				(auditNode as AuditSecurite).InvestmentToggled += OnAuditInvestmentToggled;
			}
			else if (key == "Q")
			{
				(auditNode as AuditQualite).InvestmentToggled += OnAuditInvestmentToggled;
			}
			else if (key == "F")
			{
				(auditNode as AuditFiabilite).InvestmentToggled += OnAuditInvestmentToggled;
			}
		}
	}

	// déclenché par la page choisit au dessus et réceptionne le signal + agit en fonction de l'etat actuel de la checkbox
	public void OnAuditInvestmentToggled(bool estCoche, AuditProposition proposition, string auditKey)
	{
		bool estActuellementApplique = _investmentsApplied.TryGetValue(auditKey, out bool isApplied) && isApplied;
		GD.Print($"[SCENE_CTRL] Signal reçu pour {auditKey}. Coche: {estCoche}. État persistant actuel: {estActuellementApplique}");
		if (estCoche && !estActuellementApplique)
		{
			ApplyInvestment(proposition, auditKey); 
			_investmentsApplied[auditKey] = true;
		}
		else if (!estCoche && estActuellementApplique)
		{
			CancelInvestment(proposition, auditKey); 
			_investmentsApplied[auditKey] = false; 
			
		}
		else if (estCoche && estActuellementApplique)
		{
			GD.PrintErr($"WARNING : L'investissement pour la clé '{auditKey}' était déjà appliqué.");
		}
		else if (!estCoche && !estActuellementApplique)
		{
			GD.PrintErr($"WARNING : L'investissement pour la clé '{auditKey}' n'était pas appliqué.");
		}		
	}

	private void ApplyInvestment(AuditProposition proposition, string auditKey)
	{
		if (_root == null)
		{
			//utile pour debug
			GD.PrintErr("ERREUR: _root est null dans ApplyInvestment.");
			return;
		}
		// prend le cout et le soustrait à notre argent
		if (float.TryParse(proposition.Cout, out float cout) && _root.getArgent() > cout)
		{
			_root.subArgent(cout); 
			
		}else{
			GD.PrintErr($"ERREUR DE PARSING: Cout non converti: '{proposition.Cout}'");

		}

		//méthode à compléter, c'est ici qu'on va enlever des % d'accidents/pannes/ defectueux
		if (float.TryParse(proposition.ImpactVariable, out float valeurImpact))
		{
			switch (auditKey)
			{
				case "S": 
					//_root.subAccident(valeurImpact); 
					break;
				case "Q": 
					//_root.subDefectueux(valeurImpact); 
					break;
				case "F":
					//_root.subPanne(valeurImpact);
					break;
			}
		}else{
			GD.PrintErr($"ERREUR DE PARSING: Impact non converti: '{proposition.ImpactVariable}'");
		}
		_investmentsApplied[auditKey] = true;
	}
	
	//idem au contraire
	private void CancelInvestment(AuditProposition proposition, string auditKey)
	{
		if (_root == null) return;
		
		if (float.TryParse(proposition.Cout, out float cout))
		{
			_root.addArgent(cout); 
		}
		
		if (float.TryParse(proposition.ImpactVariable, out float valeurImpact))
		{
			switch (auditKey)
			{
				case "S": 
					//root.add
					break;
				case "Q": 
					//root.add
					break;
				case "F": 
					//root.add
					break;
			}
		}
		_investmentsApplied[auditKey] = false;
	}
	
	// Méthode pour obtenir le Node Scene
	public CanvasItem GetSceneNode(string auditTypeKey)
	{
		if (AuditPanels.ContainsKey(auditTypeKey))
		{
			return AuditPanels[auditTypeKey];
		}
		GD.PrintErr($"Clé d'audit non reconnue : {auditTypeKey}");
		return null;
	}
	
	// Méthode qui affiche le panel et déclenche son initialisation
	public void SelectAuditPanel(string auditKey)
	{		
		ResetAuditInvestmentStatus(auditKey);
		// cache tous les panels
		foreach (var panel in AuditPanels.Values)
		{
			panel.Visible = false; 
		}

		if (AuditPanels.ContainsKey(auditKey))  
		{
			// affiche le bon panel
			AuditPanels[auditKey].Visible = true;
			
			// initialise les données en fonction de son type
			if (AuditNodes.TryGetValue(auditKey, out Node auditNode))
			{				
				if (auditKey == "S")
				{
					(auditNode as AuditSecurite)?.InitializeAuditData(auditKey);
				}
				else if (auditKey == "Q")
				{
					(auditNode as AuditQualite)?.InitializeAuditData(auditKey);
				}
				else if (auditKey == "F")
				{
					(auditNode as AuditFiabilite)?.InitializeAuditData(auditKey);
				}
			}
		}
	}
	// méthode qui permet de reset le fait d'investir en cochant la checkbox
	public void ResetAuditInvestmentStatus(string auditKey)
	{
		if (_investmentsApplied.ContainsKey(auditKey))
		{	
			_investmentsApplied[auditKey] = false;
		}	
	}
}	
	
