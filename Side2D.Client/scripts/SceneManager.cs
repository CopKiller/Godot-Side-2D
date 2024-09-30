using System;
using System.Collections.Generic;
using Godot;
using Side2D.Logger;
using Side2D.scripts.MainScripts.Game;
using ApplicationHost = Side2D.Host.ApplicationHost;

namespace Side2D.scripts;

public class SceneManager
{
    public Node CurrentScene { get; private set; }

    private readonly Dictionary<Type, StringName> _scenes = new()
    {
        { typeof(Root), "res://scenes/Root.tscn" },
        {typeof(MainMenu), "res://scenes/Menu/MainMenu.tscn"},
        { typeof(Game), "res://scenes/Game/Game.tscn" },
    };
    
    public void LoadScene<T>() where T : Node
    {
        if (!_scenes.ContainsKey(typeof(T)))
        {
            Log.PrintError($"{nameof(T)} Scene not found in SceneManager");
            return;
        }
        
        var tree = ApplicationHost.Instance.GetSceneTree();
        CurrentScene = tree.CurrentScene;
            
        var scene = GD.Load<PackedScene>(_scenes[typeof(T)]).Instantiate<T>();
        
        tree.ProcessFrame += ProcessFrame;
        return;

        void ProcessFrame()
        {
            tree.Root.AddChild(scene);
            CurrentScene?.QueueFree();
            tree.CurrentScene = scene;
            CurrentScene = scene;
            tree.ProcessFrame -= ProcessFrame;
        }
    }
    
    public void LoadSceneNow<T>() where T : Node
    {
        if (!_scenes.ContainsKey(typeof(T)))
        {
            Log.PrintError($"{nameof(T)} Scene not found in SceneManager");
            return;
        }
        
        var tree = ApplicationHost.Instance.GetSceneTree();
        CurrentScene = tree.CurrentScene;
            
        var scene = GD.Load<PackedScene>(_scenes[typeof(T)]).Instantiate<T>();
        
        tree.Root.AddChild(scene);
        CurrentScene?.QueueFree();
        tree.CurrentScene = scene;
        CurrentScene = scene;
    }
}