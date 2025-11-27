using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AuditSceneFactory : Node2D
{
	// Association des ID_THEME avec les différentes scènes pour afficher la bonne scène avec une même méthode (Factory)
	private readonly Dictionary<string, string> AuditScenePaths = new Dictionary<string, string>()
	{
		{"S", "res://scenes/AuditSecurite.tscn"},
		{"Q", "res://scenes/AuditQualite.tscn"},
		{"F", "res://scenes/AuditFiabilite.tscn"}
	};

	// Dico avec toutes les variantes de phrases pour les différentes fiches d'audit
	private readonly Dictionary<string, List<string>> ThemePhrases = new Dictionary<string, List<string>>()
	{
		{"S", new List<string> {"Sécurité 1", "Sécurité 2", "Sécurité 3", "Sécurité 4", "Sécurité 5", "Sécurité 6", "Sécurité 7", "Sécurité 8", "Sécurité 9", "Sécurité 10"}},
		{"Q", new List<string> {"Qualité 1", "Qualité 2", "Qualité 3", "Qualité 4", "Qualité 5", "Qualité 6", "Qualité 7", "Qualité 8", "Qualité 9", "Qualité 10"}},
		{"F", new List<string> {"Fiabilité 1", "Fiabilité 2", "Fiabilité 3", "Fiabilité 4", "Fiabilité 5", "Fiabilité 6", "Fiabilité 7", "Fiabilité 8", "Fiabilité 9", "Fiabilité 10"}}
	};

	// Méthode pour return la scène de l'id (Factory Method)
	public string GetScenePath(string auditTypeKey)
	{
		if (AuditScenePaths.ContainsKey(auditTypeKey))
		{
			return AuditScenePaths[auditTypeKey];
		}
		Console.Error.WriteLine($"Clé d'audit non reconnue : {auditTypeKey}");
		return null;
	}

	// Méthode pour obtenir 2 phrases aléatoires
	
	public static List<string> GetRandomPhrases(string auditTypeKey, int count)
	{
		/*
		if (ThemePhrases.ContainsKey(auditTypeKey))
		{
			var phrases = ThemePhrases[auditTypeKey];
			var rng = new RandomNumberGenerator();
			rng.Randomize();

			var selected = phrases.OrderBy(x => rng.Randf()) 
								   .Take(count)               
								   .ToList();                 
	
			return selected;
		}
		// Retourne une liste vide si la clé n'existe pas
		*/
		return new List<string>();
		}
		
		
}
