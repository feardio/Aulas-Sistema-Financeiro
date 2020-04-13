using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
namespace Sistema_Financeiro
{
    public static class Uteis
    {
        public static void MontaMenu()
        {
            MontaHeader("CONTROLE FINANCEIRO");
            WriteLine("Selecione uma opção a baixo:");
            WriteLine("------------------------------------");
            WriteLine("1 - Listar");
            WriteLine("2 - Cadastrar");
            WriteLine("3 - Editar");
            WriteLine("4 - Excluir");
            WriteLine("5 - Relatíro");
            WriteLine("6 - Sair");
            Write("Opção: ");
        }

        public static void MontaHeader(string titulo, char cod = '=',int len = 30)
        {
            WriteLine(new string(cod,len) + " " + titulo + " " + new string(cod, len) + "\n");
            
        }
    }
}
