using Modelo;
using System;
using System.Collections.Generic;
using static System.Console;
using Persistencia;
using System.Data.SqlClient;
using ConsoleTables;

namespace Sistema_Financeiro
{
    class Program
    {
        private List<Conta> contas;
        private List<Categoria> categorias;

        private ContaDAL conta;
        private CategoriaDAL categoria;

        public Program()
        {
            string strConn = Db.Conexao.GetStringConnection();
            this.conta = new ContaDAL(new SqlConnection(strConn));
            this.categoria = new CategoriaDAL(new SqlConnection(strConn));
        }

        static void Main(string[] args)
        {
            
            int opc;

            Program p = new Program();
            ConsoleTable table;
            do
            {
                Title = "CONTROLE FINANCEIRO";
                Uteis.MontaMenu();
                opc = Convert.ToInt32(ReadLine());
                
                if (!(opc >= 1 && opc <= 6))
                {
                    Clear();
                    BackgroundColor = ConsoleColor.Red;
                    ForegroundColor = ConsoleColor.White;
                    Uteis.MontaHeader("INFOMRE UMA OPÇÃO VALIDA!", 'X', 30);
                    ResetColor();
                }
                else
                {
                    Clear();
                    switch (opc) 
                    {
                        case 1:
                            Title = "LISTAGEM DE CONTAS - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("LISTAGEM DE CONTAS");

                            p.contas = p.conta.ListarTodos();
                            ListarContas(p);
                            ReadLine();
                            Clear();
                            break;

                        case 2:
                            Title = "NOVA CONTA - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("CADASTRANDO UMA NOVA CONTA");

                            CadastrarConta(p);
                            
                            ReadLine();
                            Clear();

                            break;

                        case 3:
                            Title = "EDITAR CONTAS - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("EDITANDO UMA CONTA");
                            ReadLine();
                            Clear();
                            break;

                        case 4:
                            Title = "EXCLUIR CONTAS - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("EXCLUINDO UMA CONTA");
                            ReadLine();
                            Clear();
                            break;

                        case 5:
                            Title = "RELATORIOS - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("RELATORIO POR DATA DE VENCIMENTO");

                            Write("Informe a data inicial (dd/mm/aaaa): ");
                            string data_inicial = ReadLine();

                            Write("Informe a data final (dd/mm/aaaa): ");
                            string data_final = ReadLine();


                            ListarContas(p, data_inicial, data_final);
                            


                            ReadLine();
                            Clear();
                            break;
                       
                    }
                    
                }


            } while (opc != 6);

            ReadLine();
        }

        static void ListarContas(Program p, string data_inicial = "", string data_final = "")
        {
            p.contas = p.conta.ListarTodos(data_inicial, data_final);

            ConsoleTable table = new ConsoleTable("ID", "Descrição", "Tipo", "Valor", "Data Vencimento");

            foreach (var c in p.contas)
            {
                table.AddRow(c.Id, c.Descricao, c.Tipo.Equals('R') ? "Receber" : "Pagar", String.Format("{0:c}", c.Valor), String.Format("{0:dd/MM/yyyy}", c.DataVencimento));
            }
            table.Write();
       
        }
            
        static void CadastrarConta(Program p)
        {
            ConsoleTable table;
            string descricao = "";
            do
            {
                Write("Informe a descrição da conta: ");
                descricao = ReadLine();

                if (descricao.Equals(""))
                {
                    BackgroundColor = ConsoleColor.Red;
                    ForegroundColor = ConsoleColor.White;
                    Uteis.MontaHeader("INFOMRE UMA DESCRIÇÃO PARA A CONTA!", '*', 30);
                    ResetColor();
                }

            } while (descricao.Equals(""));

            Write("Informe o valor da conta: ");
            double valor = Convert.ToDouble(ReadLine());

            Write("Informe o Tipo da conta(R para Receber e P para Pagar): ");
            char tipo = Convert.ToChar(ReadLine());

            Write("Informe a data de vencimento conta(dd/mm/aaaa): ");
            DateTime dataVencimento = DateTime.Parse(ReadLine());

            WriteLine("Selecione uma Categoria plea ID: \n");
            p.categorias = p.categoria.ListarTodos();
            table = new ConsoleTable("ID", "Nome");

            foreach (var c in p.categorias)
            {
                table.AddRow(c.Id, c.Nome);
            }
            table.Write();

            Write("Escolha a Categoria:");
            int cat_id = Convert.ToInt32(ReadLine());
            Categoria categoria_cadastro = p.categoria.GetCategoria(cat_id);

            Conta conta = new Conta()
            {
                Descricao = descricao,
                Valor = valor,
                Tipo = tipo,
                DataVencimento = dataVencimento,
                Categoria = categoria_cadastro
            };

            p.conta.Salvar(conta);
            BackgroundColor = ConsoleColor.DarkGreen;
            ForegroundColor = ConsoleColor.White;
            Uteis.MontaHeader("CONTA SALVA COM SUCESSO!", '+', 30);
            ResetColor();
        }
    }
}
