using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AuditSceneFactory : Node2D
{
	
	// Dico avec toutes les variantes de phrases pour les différentes fiches d'audit
	private static readonly Dictionary<string, List<AuditProposition>> ThemePropositions = new Dictionary<string, List<AuditProposition>>()
	{
		{"S", new List<AuditProposition> // Propositions de Sécurité
			{
				// Exemple de votre proposition complète :
				new AuditProposition(
					objectif: "Vérifier la conformité des protecteurs (carters, barrières, verrouillages) et tester la fonctionnalité des interrupteurs de sécurité.",
					but: "Évaluer le respect des règles de sécurité et garantir l'intégrité de la production.",
					statutActuel: "Non-conforme : 3 interrupteurs de sécurité sont facilement contournables.",
					action: "Remplacement des 3 interrupteurs de sécurité par des modèles RFID.",
					cout: "500",
					impact: "Réduction du Risque : Le taux d'accident annuel prévisionnel passe de 20% à 7%."
				),
				// Ajoutez d'autres propositions "S" ici...
			
				new AuditProposition(
					objectif: "remplir",
					but: "remplir",
					statutActuel: "remplir",
					action: "remplir",
					cout: "remplir",
					impact: "remplir"
				)
				
			}
		},
		{"Q", new List<AuditProposition> { /* ... Vos propositions de Qualité ... */ } },
		{"F", new List<AuditProposition> { /* ... Vos propositions de Fiabilité ... */ } }
	};
							

	// Méthode pour obtenir 2 phrases aléatoires
	
	public static List<AuditProposition> GetRandomPropositions(string auditTypeKey, int count)
	{
		if (ThemePropositions.ContainsKey(auditTypeKey))
		{
			var propositions = ThemePropositions[auditTypeKey];
			var random = new Random(); // System.Random
			
			var selected = propositions.OrderBy(x => random.NextDouble())
									   .Take(count)
									   .ToList();		
			return selected;
		}
		return new List<AuditProposition>();
	}
}
