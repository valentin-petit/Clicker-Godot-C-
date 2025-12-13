using Godot;
using System;

public partial class AuditFiabilite : Node2D
{
	private const string ID_THEME = "F"; 
	
	public override void _Ready()
	{
		// faire le système d'affichages des certaines phrases
		Console.WriteLine($"Audit de Fiabilité prêt. Clé : {ID_THEME}");
	}
}
