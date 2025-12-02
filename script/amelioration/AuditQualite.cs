using Godot;
using System;

public partial class AuditQualite : Node2D
{
	private const string ID_THEME = "Q"; 
	
	public override void _Ready()
	{
		// faire le système d'affichages des certaines phrases
		Console.WriteLine($"Audit de Qualité prêt. Clé : {ID_THEME}");
	}
}
