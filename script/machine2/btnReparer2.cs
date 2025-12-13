using Godot;
using System;

public partial class btnReparer2 : TextureButton
{
	private Machine2Container _machineContainer; 
	
	public override void _Ready()
	{
		
		_machineContainer = GetNode<Machine2Container>("../"); 
		
		Pressed += OnCliquerReparer;
	}

	private void OnCliquerReparer()
	{
		if (_machineContainer != null)
		{
			_machineContainer.Reparer(); 
		}
	}
}
