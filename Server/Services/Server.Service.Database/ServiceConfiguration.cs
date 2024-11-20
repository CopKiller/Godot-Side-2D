using System.ComponentModel.Design;
using Core.Service.Interfaces;

namespace Server.Service.Database;

public class ServiceConfiguration : IServiceConfiguration
{
    public bool Enabled { get; set; } = true;
    public bool NeedUpdate { get; set; } = false;
    public int UpdateIntervalMs { get; set; } = 1000;
}