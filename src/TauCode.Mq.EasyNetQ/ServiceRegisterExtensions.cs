using EasyNetQ.DI;
using EasyNetQ.Logging;
using TauCode.Mq.EasyNetQ;

// ReSharper disable once CheckNamespace
namespace EasyNetQ;

/// <summary>
///     Register loggers based of Microsoft.Extensions.Logging
/// </summary>
public static class ServiceRegisterExtensions
{
    /// <summary>
    ///     Enables serilog logging support for EasyNetQ. It should be already registered in DI.
    /// </summary>
    /// <param name="serviceRegister">The register</param>
    /// <param name="logger">Serilog logger to register in DI</param>
    public static IServiceRegister EnableSerilogLogging(this IServiceRegister serviceRegister, Serilog.ILogger? logger)
    {
        if (logger != null)
        {
            serviceRegister
                .Register(typeof(Serilog.ILogger), logger)
                .Register(typeof(ILogger<>), typeof(SerilogLoggerAdapter<>));
        }

        return serviceRegister;
    }
}