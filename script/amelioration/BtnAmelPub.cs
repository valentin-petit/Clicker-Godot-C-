using Godot;
using System;
using System.Collections.Generic;

public partial class BtnAmelPub : Button
{
	private nodeRootPrincipal _root;
	private ControlVente _sceneVente;

	private const float COUT_AMELIO_PUB = 1000f;
	private const float AUGMENTE_PUB = 0.1f;

	private CanvasLayer _overlayLayer;
	private Node2D _overlayNode2D;
	private Sprite2D _sprite;
	private Timer _timer;

	// Liste des textures possibles
	private List<Texture2D> _textures = new List<Texture2D>();
	private int _count;

	public override void _Ready()
	{
		_count = 0;

		// Références
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		if (_root == null)
			GD.PushError("nodeRootPrincipal introuvable via CurrentScene.");

		_sceneVente = _root?.FindChild("ControlVente") as ControlVente;
		if (_sceneVente == null)
			GD.PushWarning("ControlVente introuvable sous root.");

		Pressed += achatPub;

		// CanvasLayer au-dessus de tout
		_overlayLayer = new CanvasLayer { Layer = 100 };
		GetTree().CurrentScene.AddChild(_overlayLayer);

		// Node2D qui contiendra le sprite
		_overlayNode2D = new Node2D { Visible = true };
		_overlayLayer.AddChild(_overlayNode2D);

		// Charger plusieurs textures
		_textures.Add(GD.Load<Texture2D>("res://image/imagePub1.png"));
		_textures.Add(GD.Load<Texture2D>("res://image/imagePub2.png"));
		_textures.Add(GD.Load<Texture2D>("res://image/imagePub3.png"));
		// Ajoute autant d’images que tu veux

		// Sprite2D configuré
		_sprite = new Sprite2D
		{
			Texture = _textures[0],
			Visible = false,
			Centered = true
		};
		_overlayNode2D.AddChild(_sprite);

		// Timer 2s
		_timer = new Timer { WaitTime = 2.0, OneShot = true };
		_timer.Timeout += OnTimerTimeout;
		AddChild(_timer);
	}

	private void UpdateSpriteScaleAndPosition()
	{
		if (_sprite == null || _sprite.Texture == null)
			return;

		var vpSize = GetViewportRect().Size;
		float viewportWidth = vpSize.X;
		float viewportHeight = vpSize.Y;

		var texSize = _sprite.Texture.GetSize();
		float texWidth = texSize.X;
		float texHeight = texSize.Y;

		if (texWidth <= 0 || texHeight <= 0)
			return;

		float scale = viewportWidth / texWidth;
		_sprite.Scale = new Vector2(scale, scale);
		_sprite.Position = new Vector2(viewportWidth / 2f, viewportHeight / 2f);
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
			_sceneVente._coefficientAmeliorationPub += AUGMENTE_PUB;

		// Incrémente le compteur et change l’image
		_count++;
		int index = _count % _textures.Count; // boucle sur la liste
		_sprite.Texture = _textures[index];

		ShowImageFor2Seconds();
	}

	public void ShowImageFor2Seconds()
	{
		if (_sprite == null || _sprite.Texture == null)
		{
			GD.PushError("Impossible d’afficher: Sprite ou texture null.");
			return;
		}

		UpdateSpriteScaleAndPosition();
		_sprite.Visible = true;
		_timer.Start();
	}

	private void OnTimerTimeout()
	{
		if (_sprite != null)
			_sprite.Visible = false;
	}
}
