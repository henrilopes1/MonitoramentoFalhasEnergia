using System;

namespace MyApp.Domain.Exceptions
{
    public class ValidacaoException : Exception
    {
        public ValidacaoException(string mensagem) : base(mensagem) { }
    }
}
