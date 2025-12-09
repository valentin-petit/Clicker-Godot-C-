using Godot;
using System;
using System.Collections.Generic;

public partial class SceneController : Node2D
{
	// utilisation de CanvasItem qui est parent à Node2D car on peut pas faire de Node2D.Visible directement (pour afficher les différents audits)
	private readonly Dictionary<string, CanvasItem> AuditPanels = new Dictionary<string, CanvasItem>(); // gère l'affichage

	
	public override void _Ready()
	{
		// Associe l'ID à son "Node2D" d'audit
		AuditPanels.Add("S", GetNode<CanvasItem>("AuditSecurite")); 
		AuditPanels.Add("Q", GetNode<CanvasItem>("AuditQualite"));
		AuditPanels.Add("F", GetNode<CanvasItem>("AuditFiabilite"));
		
		// rend invisible chaque panel (par sécu)
		foreach (var panel in AuditPanels.Values)
		{
			panel.Visible = false;
		}
	}
	
	// méthode qui affiche le panel correspondant à l'ID choisis soit le bouton cliqué sur le menutel
	public CanvasItem GetSceneNode(string auditTypeKey)
	{
		if (AuditPanels.ContainsKey(auditTypeKey))
		{
			return AuditPanels[auditTypeKey];
		}
		GD.PrintErr($"Clé d'audit non reconnue : {auditTypeKey}");
		return null;
	}
	
	//fait comme avant mais non pas au lancement de l'audit mais plutot lorsqu'il change de catégorie (voir si bloqué qu'une catégorie par achat d'audit)
	public void SelectAuditPanel(string auditKey)
	{
		foreach (var panel in AuditPanels.Values)
		{
			panel.Visible = false;
		}

		if (AuditPanels.ContainsKey(auditKey)) 
		{
			AuditPanels[auditKey].Visible = true;
		
		}
	}
}
