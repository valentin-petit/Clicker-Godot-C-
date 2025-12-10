using Godot;
using System;
using System.Collections.Generic;

public partial class AuditSecurite : Node2D
{
	//chk
	[Signal]
	public delegate void InvestmentToggledEventHandler(bool isChecked, AuditProposition proposition, string auditKey);
	private AuditProposition _currentProposition;
	private CheckBox _chkInvestir;
	
		
	private const string ID_THEME = "S"; 
	private Label lblObjectif;
	private Label lblBut;
	private Label lblStatutActuel;
	private Label lblAction;
	private Label lblCout;
	
	private nodeRootPrincipal _root;	
	
	public override void _Ready()
	{
		_root = GetNode<nodeRootPrincipal>("/root/NodeRootPrincipal");
		
		lblObjectif = GetNode<Label>("sprSecuF1/lblObjectif");
		lblBut = GetNode<Label>("sprSecuF1/lblBut");
		lblStatutActuel = GetNode<Label>("sprSecuF1/lblStatutActuel");		
		
		lblAction = GetNode<Label>("sprSecuF1/lblAction");
		lblCout = GetNode<Label>("sprSecuF1/lblCout");
		
		//chk
		_chkInvestir = GetNode<CheckBox>("sprSecuF1/chkInvestir");      
		_chkInvestir.Toggled += OnChkInvestirToggled;	
		
		//test résolution bug coché permanent
		this.VisibilityChanged += OnVisibilityChanged;	
	}
	
	public void InitializeAuditData(string key)
	{
		//prend une propal parmis celles référencé dans AuditSceneFactory
		List<AuditProposition> propositions = AuditSceneFactory.GetRandomPropositions(key, 1);

		if (propositions.Count >= 1)
		{									
			AuditProposition proposition = propositions[0];
			_currentProposition = proposition; // chk
			
			// remplissage des labels par la propal
			lblObjectif.Text = proposition.Objectif;
			lblBut.Text = proposition.But;
			lblStatutActuel.Text = proposition.StatutActuel;
			lblAction.Text = proposition.Action;
			lblCout.Text = proposition.Cout;
			
			GD.Print($"audit {key} chargé.");    
			GD.Print($"[AUDIT_ENTREE {key}] CheckBox UI NE SERA PAS réinitialisée ici.");
		}
		else
		{
			GD.PrintErr($"ERREUR : Aucune proposition trouvée pour {key}.");
		}
	}	
	private void OnVisibilityChanged()
	{
		// L'exécution doit avoir lieu uniquement lorsque la scène devient visible
		if (IsVisibleInTree())
		{
			// On utilise la même logique de déconnexion/reconnexion pour éviter les signaux
			_chkInvestir.Toggled -= OnChkInvestirToggled;
			
			// 🚨 Réinitialisation forcée de l'état UI
			_chkInvestir.ButtonPressed = false;
			
			_chkInvestir.Toggled += OnChkInvestirToggled;
			
			GD.Print($"[AUDIT_VISIBLE {ID_THEME}] CheckBox UI réinitialisée par événement de visibilité. État: {_chkInvestir.ButtonPressed}");
		}
	}
	
	//chk
	private void OnChkInvestirToggled(bool estCoche)
	{		
		// Émission du signal (pour que le SceneController gère l'investissement/annulation)
		EmitSignal(SignalName.InvestmentToggled, estCoche, _currentProposition, ID_THEME);					
	}
	
	private void _on_txtbtn_signature_quitter_pressed()
	{								
		_root._sceneAmelioration.Hide();
		(GetParent() as SceneController)?.ResetAuditInvestmentStatus(ID_THEME);
	}
}	
