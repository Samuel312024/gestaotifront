using System.Linq;
using System.Text.RegularExpressions;

namespace integracoes.Tools
{
    public static class CpfHelper
    {
        public static string SomenteNumeros(string cpf)
        {
            return Regex.Replace(cpf ?? "", @"\D", "");
        }

        public static bool CpfValido(string cpf)
        {
            cpf = SomenteNumeros(cpf);

            if (cpf.Length != 11)
                return false;

            // Rejeita CPFs com números repetidos
            if (cpf.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
