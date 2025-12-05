using Godot;
using System;
using System.Collections.Generic;

public partial class SceneController : Node2D
{
	private readonly Dictionary<string, CanvasItem> AuditPanels = new Dictionary<string, CanvasItem>();
	
	public override void _Ready()
	{
		AuditPanels.Add("S", GetNode<CanvasItem>("AuditSecurite")); 
		AuditPanels.Add("Q", GetNode<CanvasItem>("AuditQualite"));
		AuditPanels.Add("F", GetNode<CanvasItem>("AuditFiabilite"));
		
		foreach (var panel in AuditPanels.Values)
		{
			panel.Visible = false;
		}
	}
	
	// Méthode pour return la scène de l'id (Factory Method)
	public CanvasItem GetSceneNode(string auditTypeKey)
	{
		if (AuditPanels.ContainsKey(auditTypeKey))
		{
			return AuditPanels[auditTypeKey];
		}
		GD.PrintErr($"Clé d'audit non reconnue : {auditTypeKey}");
		return null;
	}
	
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
