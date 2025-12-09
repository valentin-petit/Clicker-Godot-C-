using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AuditSceneFactory : Node2D
{
	
	// Dico avec toutes les variantes de fiches d'audit
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
					cout: "500"
				),
				
			
				new AuditProposition(
					objectif: "remplirS2",
					but: "remplirS2",
					statutActuel: "S2remplir",
					action: "remplirS2",
					cout: "remplirS2"
				)
				
			}
		},
		{"Q", new List<AuditProposition> 
			{
				new AuditProposition(
					objectif: "remplirQ1",
					but: "remplirQ1",
					statutActuel: "Q1remplir",
					action: "remplirQ1",
					cout: "remplirQ1"
				),							
							
				new AuditProposition(
					objectif: "remplirQ2",
					but: "remplirQ2",
					statutActuel: "remplirQ2",
					action: "remplirQ2",
					cout: "remplirQ2"
				)
			} 
		
		},
		{"F", new List<AuditProposition> 
			{
				new AuditProposition(
					objectif: "remplirF1",
					but: "remplirF1",
					statutActuel: "F1remplir",
					action: "remplirF1",
					cout: "remplirF1"
				),
															
				new AuditProposition(
					objectif: "remplirF2",
					but: "remplirF2",
					statutActuel: "remplirF2",
					action: "remplirF2",
					cout: "remplirF2"
				)
			} 
		}
	};
							

	// méthode pour obtenir une propal aléatoirement
	
	public static List<AuditProposition> GetRandomPropositions(string auditTypeKey, int count)
	{
		if (ThemePropositions.ContainsKey(auditTypeKey)) // verifie si la clée existe : S, Q, F
		{
			var propositions = ThemePropositions[auditTypeKey]; // récupère toutes les propales du theme choisis
			var random = new Random(); 
						
			if (propositions.Count > 0)
			{
				// génères un nombre entre 0 et le nombre de propositions -1
				int randomIndex = random.Next(propositions.Count); 
			
				// sélectionne l'élément à l'index choisi audessus dans la liste de propal et le retourne 
				return new List<AuditProposition> { propositions[randomIndex] };
			}
		}
		return new List<AuditProposition>(); // si la clée exist epas (devrait pas arriver) alors retourne une liste vide

	}
}	
