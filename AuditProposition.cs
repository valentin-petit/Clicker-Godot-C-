using System.Collections.Generic;

// structure pour faire les différents audits
	public class AuditProposition
	{
		// Propriétés qui correspondent à vos Labels
		public string Objectif { get; set; }
		public string But { get; set; }
		public string StatutActuel { get; set; }
		public string Action { get; set; }
		public string Cout { get; set; } // Gardons string pour le format "$12 500"
		public string Impact { get; set; }

		public AuditProposition(string objectif, string but, string statutActuel, string action, string cout, string impact)
		{
			Objectif = objectif;
			But = but;
			StatutActuel = statutActuel;
			Action = action;
			Cout = cout;
			Impact = impact;
		}
	}
