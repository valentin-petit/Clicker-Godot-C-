using Godot;
using System;
using System.Collections.Generic;

public partial class AuditSecurite : Node2D
{
	private const string ID_THEME = "S"; 
	
	public override void _Ready()
	{
		// faire le système d'affichages des certaines phrases
		Console.WriteLine($"Audit de Sécurité prêt. Clé : {ID_THEME}");
		
		//test des audits random
		InitializeAuditLabels(ID_THEME);
	}
	
	private void InitializeAuditLabels(string key)
	{
		// Construction des noms des Labels : lblS1 et lblS2
		string lblName1 = $"lbl{key}1";
		string lblName2 = $"lbl{key}2";

		// Récupération des Labels par leur nom construit
		Label lbl1 = GetNodeOrNull<Label>(lblName1); 
		Label lbl2 = GetNodeOrNull<Label>(lblName2);

		// Appel de la méthode statique de la Fabrique pour obtenir 2 phrases
		List<string> phrases = AuditSceneFactory.GetRandomPhrases(key, 2);
		
		if (lbl1 != null && lbl2 != null && phrases.Count >= 2)
		{
			lbl1.Text = phrases[0];
			lbl2.Text = phrases[1];
			GD.Print($"Labels {lblName1} et {lblName2} mis à jour pour l'audit {key}.");
		}
		else
		{
			if (lbl1 == null || lbl2 == null)
			{
				GD.PrintErr($"ERREUR DE LABEL : Les Labels ({lblName1} ou {lblName2}) sont introuvables. Vérifiez le nom et le chemin dans la scène.");
			}
			if (phrases.Count < 2)
			{
				GD.PrintErr($"ERREUR DE FABRIQUE : Moins de 2 phrases trouvées pour la clé : {key}.");
			}
		}
	}
}
