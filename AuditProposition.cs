using System.Collections.Generic;

// structure pour faire les différents audits
	public class AuditProposition
	{
		//variables qui mettent en lien les éléments de la scène et les strings du dico
		public string Objectif { get; set; }
		public string But { get; set; }
		public string StatutActuel { get; set; }
		public string Action { get; set; }
		public string Cout { get; set; } 
		public string ImpactVariable {get; set;}

		public AuditProposition(string objectif, string but, string statutActuel, string action, string cout, string impactVariable)
		{
			Objectif = objectif;
			But = but;
			StatutActuel = statutActuel;
			Action = action;
			Cout = cout;
			ImpactVariable = impactVariable;
		}
	}
