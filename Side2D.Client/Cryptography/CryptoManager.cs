using System;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using Godot;
using Godot.NativeInterop;
using Side2D.Environment;

namespace Side2D.Cryptography;

public enum ConfigSection
{
    User,
    Network,
    Graphics
}

public enum ConfigKey
{
    Username,
    Password,
    ServerIp,
    ServerPort,
    Resolution,
    Fullscreen
}

public delegate Variant LoadDataDelegate(ConfigSection section, ConfigKey key, bool decrypt = false);
public delegate void SaveDataDelegate(ConfigSection section, ConfigKey key, string value, bool encrypt = false);
public delegate void CreateDefault();

public sealed class CryptoManager
{
    public static SaveDataDelegate Save;
    public static LoadDataDelegate Load;
    public static CreateDefault CreateDefault;
    
    private const string ConfigFilePath = "user://userdata.cfg";
    private ConfigFile _configFile;
    
    private Crypto _crypto;
    private KeyManager _keyManager;
    
    public CryptoManager()
    {
        _configFile = new ConfigFile();
        var error = _configFile.Load(ConfigFilePath);
        
        if (error != Error.Ok)
            _configFile.Save(ConfigFilePath);
        
        _crypto = new Crypto();
        _keyManager = new KeyManager(_crypto);
        _keyManager.Load();
        
        Save += SaveData;
        Load += LoadData;
        CreateDefault += _keyManager.GenerateAndSave;
    }
    
    private void SaveData(ConfigSection section, ConfigKey key, string value, bool encrypt = false)
    {
        if (value == "")
        {
            _configFile.SetValue(section.ToString(), key.ToString(), value);
        }
        else
        {
            if (encrypt)
            {
                var packedByte = value.ToUtf8Buffer();
                byte[] encryptedValue = _crypto.Encrypt(_keyManager.Key, packedByte);
                _configFile.SetValue(section.ToString(), key.ToString(), encryptedValue);
            }
            else
                _configFile.SetValue(section.ToString(), key.ToString(), value);
        }
        
        _configFile.Save(ConfigFilePath);
    }
    
    private Variant LoadData(ConfigSection section, ConfigKey key, bool decrypt = false)
    {
        var value = _configFile.GetValue(section.ToString(), key.ToString(), "");
        
        if (!decrypt) return value;
        
        var valueByteArray = value.AsByteArray();
        
        if (valueByteArray.Length == 0) return Variant.CreateFrom("");
        
        byte[] decryptedValue = _crypto.Decrypt(_keyManager.Key, valueByteArray);
        
        return Variant.CreateFrom(decryptedValue.GetStringFromUtf8());
    }
}

