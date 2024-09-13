using System;
using System.Collections.Generic;
using Godot;
using Side2D.scripts.Host;

namespace Side2D.scripts;

public class SceneManager
{
    private Node _currentScene = ApplicationHost.Instance.GetSceneTree().CurrentScene;

    private readonly Dictionary<Type, StringName> Scenes = new()
    {
        { typeof(Root), "res://scenes/Root.tscn" },
        //{typeof(MainMenu), "res://scenes/TitleScreen.tscn"},
        { typeof(Game), "res://scenes/Game/Game.tscn" },
    };
    
    public void LoadScene<T>() where T : Node
    {
        if (!Scenes.ContainsKey(typeof(T)))
        {
            GD.PrintErr("Scene not found in SceneManager");
            return;
        }
        
        SetScene<T>();
    }
    
    private void SetScene<T>() where T : Node
    {
        var tree = ApplicationHost.Instance.GetSceneTree();
        var newScene = GD.Load<PackedScene>(Scenes[typeof(T)]).Instantiate();
        tree.Root.AddChild(newScene);
        
        _currentScene.QueueFree();
        tree.SetCurrentScene(newScene);
    }
}