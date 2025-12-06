using Godot;
using System;

public partial class BtnAmelPub : Button
{
	private nodeRootPrincipal _root;
	private ControlVente _sceneVente;

	private const float COUT_AMELIO_PUB = 1000f;
	private const float AUGMENTE_PUB = 0.1f;

	private CanvasLayer _overlayLayer;
	private Control _overlayUI;
	private TextureRect _image;
	private Timer _timer;

	public override void _Ready()
	{
		// Références
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		if (_root == null)
			GD.PushError("nodeRootPrincipal introuvable via CurrentScene.");

		_sceneVente = _root?.FindChild("ControlVente") as ControlVente;
		if (_sceneVente == null)
			GD.PushWarning("ControlVente introuvable sous root.");

		Pressed += achatPub;

		// Overlay au-dessus de tout
		_overlayLayer = new CanvasLayer { Layer = 100 };
		GetTree().CurrentScene.AddChild(_overlayLayer);

		// Cadre UI plein écran
		_overlayUI = new Control
		{
			MouseFilter = Control.MouseFilterEnum.Ignore,
			Visible = true
		};
		_overlayUI.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		_overlayUI.SetOffsetsPreset(Control.LayoutPreset.FullRect);
		_overlayUI.Size = GetViewportRect().Size; // taille initiale
		_overlayLayer.AddChild(_overlayUI);

		// Image
		var tex = GD.Load<Texture2D>("res://image/imagePub1.jpeg");
		if (tex == null)
			GD.PushError("Texture introuvable: res://image/imagePub1.jpeg");

		_image = new TextureRect
		{
			Texture = tex,
			StretchMode = TextureRect.StretchModeEnum.KeepAspect, // bandes noires OK, pas de dépassement
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			MouseFilter = Control.MouseFilterEnum.Ignore,
			Visible = false
		};
		_image.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		_image.SetOffsetsPreset(Control.LayoutPreset.FullRect);
		_overlayUI.AddChild(_image);

		// Timer 2s
		_timer = new Timer { WaitTime = 2.0, OneShot = true };
		_timer.Timeout += OnTimerTimeout;
		AddChild(_timer);
	}

	private void achatPub()
	{
		GD.Print("Bouton cliqué ! Tentative d'achat...");

		if (_root != null && _root.getArgent() < COUT_AMELIO_PUB)
		{
			GD.Print($"Pas assez d'argent ! Tu as {_root.getArgent()} mais il faut {COUT_AMELIO_PUB}");
			return;
		}

		GD.Print("Achat validé ! Affichage de l'image.");
		if (_root != null)
			_root.subArgent(COUT_AMELIO_PUB);

		if (_sceneVente != null)
			_sceneVente._coefficientAmeliorationPub += AUGMENTE_PUB; // FIX CS0131

		ShowImageFor2Seconds();
	}

	public void ShowImageFor2Seconds()
	{
		if (_image == null || _image.Texture == null)
		{
			GD.PushError("Impossible d’afficher: TextureRect ou texture null.");
			return;
		}

		// Synchroniser la taille au viewport au moment de l’affichage
		_overlayUI.Size = GetViewportRect().Size;
		_image.Visible = true;
		_timer.Start();
	}

	private void OnTimerTimeout()
	{
		_image.Visible = false;
	}
}
