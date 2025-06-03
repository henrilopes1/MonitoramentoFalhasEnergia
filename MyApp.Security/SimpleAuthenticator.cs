using MyApp.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace MyApp.Security
{
    public class SimpleAuthenticator : IUserAuthenticator
    {
        private readonly Dictionary<string, string> _usuarios = new()
        {
            { "admin", "Senha123" },
            { "operador", "Oper@2023" }
        };

        public bool Autenticar(string usuario, string senha)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Usuário e senha não podem ser vazios.");

            return _usuarios.ContainsKey(usuario) && _usuarios[usuario] == senha;
        }
    }
}
