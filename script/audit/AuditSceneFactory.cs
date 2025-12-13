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
					cout: "500",
					impactVariable: "-10" // créer une variable de panne dans le root pour pouvoir la récup
				),
				
			
				new AuditProposition(
					objectif: "Évaluer et renforcer la formation des opérateurs sur la consignation des énergies (LOTO).",
					but: "Minimiser le risque d'accidents graves lors des interventions de maintenance en assurant une procédure de mise hors tension fiable.",
					statutActuel: "Manque de formation LOTO : 40% du personnel de maintenance et de production n'a pas suivi de recyclage depuis 12 mois.",
					action: "Mise en œuvre d'un programme de formation pratique et théorique obligatoire pour l'ensemble du personnel de maintenance et de production.",
					cout: "500",
					impactVariable: "-5"

				)
				
			}
		},
		{"Q", new List<AuditProposition> 
			{
				new AuditProposition(
					objectif: "Vérification et étalonnage des instruments de mesure critiques (pieds à coulisse, micromètres) sur les lignes de production.",
					but: "Assurer la précision des contrôles dimensionnels pour détecter les dérives de production avant la fabrication de pièces défectueuses en série.",
					statutActuel: "Non-conforme : 20% des outils de mesure n'ont pas été étalonnés depuis plus de six mois.",
					action: "Programme d'étalonnage systématique et achat de 5 nouveaux instruments de référence certifiés.",
					cout: "500",
					impactVariable: "-10"
				),							
							
				new AuditProposition(
					objectif: "Standardisation et renforcement du contrôle visuel et tactile des chaussettes en fin de ligne (inspection finale).",
					but: "Améliorer la capacité des opérateurs à détecter les défauts de surface (boulochage, fils tirés) et de structure avant l'emballage, réduisant les retours clients.",
					statutActuel: "Variation importante : Les critères d'acceptation et de rejet dépendent de l'opérateur, entraînant un taux élevé de 'faux positifs' et de 'faux négatifs' en inspection.",
					action: "Création de gabarits d'inspection standardisés, mise en place d'un 'mur des défauts' visuel, et formation certifiante des opérateurs à l'inspection selon les nouveaux standards.",
					cout: "500",
					impactVariable: "-10"
				)
			} 
		
		},
		{"F", new List<AuditProposition> 
			{
				new AuditProposition(
					objectif: "Mise en œuvre d'un plan de maintenance préventive (MP) pour les systèmes de tricotage et de motorisation.",
					but: "Réduire les pannes imprévues et l'usure des composants critiques (cames, moteurs) en basant l'entretien sur le temps de fonctionnement.",
					statutActuel: "Maintenance corrective : La maintenance est effectuée après la panne, entraînant un temps d'arrêt machine imprévu élevé (MTTR élevé).",
					action: "Déploiement d'un calendrier de MP strict (lubrification, remplacement des pièces d'usure) pour les 10 machines les plus critiques.",
					cout: "500",
					impactVariable: "-10"
					
					
				),
															
				new AuditProposition(
					objectif: "Audit de l'état des systèmes pneumatiques et des alimentations électriques des machines.",
					but: "Assurer la stabilité des systèmes d'énergie pour prévenir les pannes causées par les fluctuations de pression d'air ou les pics électriques.",
					statutActuel: "Pression d'air et électricité instables : Fuites dans le réseau pneumatique et filtres encrassés causant des arrêts machine aléatoires.",
					action: "Réparation des fuites d'air comprimé, remplacement des filtres pneumatiques et installation de régulateurs de tension pour stabiliser l'alimentation électrique.",
					cout: "500",
					impactVariable: "-5"
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
