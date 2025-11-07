using System.Net.Sockets;
using System.ComponentModel;
using crm.BusinessLogic.Services;
using crm.BusinessLogic.IServices;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;
using crm.BusinessLogic.Dtos;
using System.Threading.Tasks;
using static crm.Presentation.Helpers.UserHelpers;
using ConsoleTables;

namespace crm.Presentation.Helpers;

public static class BaseHelpers
{
    
    public static Task<string> Read(CancellationToken token = default) => Task.FromResult(Console.ReadLine()!);

    public static Task Write(string message, CancellationToken token = default)
    {
        Console.Write(message);

        return Task.CompletedTask;
    }

    public static async Task<Guid> GetEntityId(string entityName, CancellationToken token = default)
    {
        await Write($"\n{entityName} ID: ", token);

        return Guid.Parse(await Read(token));
    }

    public static async Task<T?> GetStringAsync<T>(string entityName, CancellationToken token = default)
    {
        var targetType = typeof(T);

        var _converter = TypeDescriptor.GetConverter(targetType);

        for (int i = 0; i < 3; i++)
        {
            await Write($"\n{entityName}: ", token);

            string value = await Read(token);

            if (_converter != null && _converter.IsValid(value))
            {
                return (T)_converter.ConvertFromString(value)!;
            }

            await Write("\nIncompatible type!\n", token);
        }
        throw new InvalidOperationException("Fuck you line");
    }
    
    

}