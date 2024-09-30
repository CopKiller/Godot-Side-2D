using Godot;

namespace Side2D.Cryptography;

public sealed class KeyManager
{
    private readonly Crypto _crypto;
    
    internal X509Certificate Certificate { get; set; }
    internal CryptoKey Key { get; set; }
    
    public KeyManager(Crypto crypto)
    {
        var file = FileAccess.Open("res://Environment/.env", FileAccess.ModeFlags.Read);
        DotNetEnv.Env.LoadContents(file.GetAsText());
        
        file.Close();
        file.Dispose();
        
        _crypto = crypto;
    }

    public void GenerateAndSave()
    {
        Clear();
        Create();
        
        Key = _crypto.GenerateRsa(4096); // Gera uma chave RSA
        Certificate = _crypto.GenerateSelfSignedCertificate(Key, DotNetEnv.Env.GetString("COMPANY_CERTIFICATE"));
        Certificate.Save("user://.crt");
        Key.Save("user://.key"); // Salva no sistema de arquivos do Godot
    }
    
    private void Clear()
    {
        Certificate?.Dispose();
        Certificate = null!;
        Key?.Dispose();
        Key = null!;
    }

    private void Create()
    {
        Certificate = new X509Certificate();
        Key = new CryptoKey();
    }

    public void Load()
    {
        Certificate = new X509Certificate();
        Key = new CryptoKey();
        
        var certificateError = Certificate.Load("user://.crt");
        var keyError = Key.Load("user://.key"); // Carrega a chave salva

        if (certificateError == Error.Ok && keyError == Error.Ok)
        {
            GD.Print("Certificate and key loaded successfully.");
            return;
        }
        
        GD.PrintErr($"Error loading certificate or key. {(certificateError != Error.Ok ? certificateError : keyError)}");
        GD.PrintErr("Generating new key and certificate.");
        GenerateAndSave();
    }
    
    
}