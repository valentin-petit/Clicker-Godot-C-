using Godot;
using System;
using System.Collections.Generic;

public partial class AuditFiabilite : Node2D
{
		//chk
	[Signal]
	public delegate void InvestmentToggledEventHandler(bool isChecked, AuditProposition proposition, string auditKey);
	private AuditProposition _currentProposition;
	private CheckBox _chkInvestir;
	
	private const string ID_THEME = "F"; 
	private Label lblObjectif;
	private Label lblBut;
	private Label lblStatutActuel;
	private Label lblAction;
	private Label lblCout;
	
	private nodeRootPrincipal _root;
	
	public override void _Ready()
	{
		_root = GetNode<nodeRootPrincipal>("/root/NodeRootPrincipal");
		
		lblObjectif = GetNode<Label>("sprFiaF1/lblObjectif");
		lblBut = GetNode<Label>("sprFiaF1/lblBut");
		lblStatutActuel = GetNode<Label>("sprFiaF1/lblStatutActuel");		
		
		lblAction = GetNode<Label>("sprFiaF1/lblAction");
		lblCout = GetNode<Label>("sprFiaF1/lblCout");

		//InitializeAuditData(ID_THEME);
		
		//chk
		_chkInvestir = GetNode<CheckBox>("sprFiaF1/chkInvestir");      
		_chkInvestir.Toggled += OnChkInvestirToggled;
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
			
			_chkInvestir.ButtonPressed = false; // chk
			
			GD.Print($"audit {key} chargé.");
		}
		else
		{
			GD.PrintErr($"ERREUR : Aucune proposition trouvée pour {key}.");
		}
	}
	
		//chk
	private void OnChkInvestirToggled(bool estCoche)
	{
		EmitSignal(SignalName.InvestmentToggled, estCoche, _currentProposition, ID_THEME);
	}
	
	private void _on_txtbtn_signature_quitter_pressed()
	{				
		_root._sceneAmelioration.Hide();
	}
		
}
