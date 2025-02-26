using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UBSRisk.Interfaces;
using UBSRisk.Useful;

namespace UBSRisk
{
    class Program
    {
        //Já deixa em memória a lista com as opções de setor do cliente
        static readonly List<string> lstClientSector = Enum.GetNames(typeof(TypeClientSector)).ToList();

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Informe a data de referência (MM/dd/yyyy): ");

                //Lê e valida a data de referência
                if (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime referenceDate))
                    throw new Exception("Data inválida. O formato correto é MM/dd/yyyy.");

                Console.Write("Informe o número de operações: ");

                //Lê e valida o número de operações
                if (!int.TryParse(Console.ReadLine(), out int numOfTrades))
                    throw new Exception("O valor informado para o número de operações é inválido.");

                if (numOfTrades < 1)
                    throw new Exception("O número de operações não pode ser zero ou negativo.");

                //Monta a lista de categorias, na ordem de precedência
                List<ICategory> categories = new List<ICategory>
                {
                    new ExpiredCategory(),
                    new HighRiskCategory(),
                    new MediumRiskCategory()
                };

                //Monta a lista da saída com o resultado da categorização
                List<string> resultCategories = new List<string>();

                Console.WriteLine("Informe os dados da operação seguindo o seguinte formato: VALOR_NEGOCIADO SETOR_CLIENTE DATA_PROX_PAGTO");

                //Processa as operações informadas
                for (int i = 0; i < numOfTrades; i++)
                {
                    //Lê a linha da operação
                    string[] tradeInfo = Console.ReadLine().Split(' ');

                    if (tradeInfo.Length != 3)
                        throw new Exception("Informações da operação inválidas. O formato correto é [VALOR_NEGOCIADO SETOR_CLIENTE DATA_PROX_PAGTO]");

                    //Valida os dados informados para operação
                    if (!double.TryParse(tradeInfo[0], out double value))
                        throw new Exception("O valor negociado informado para operação é inválido.");

                    string cliSector = tradeInfo[1];
                    if (!lstClientSector.Any(x => x == cliSector))
                        throw new Exception("O setor do cliente informado para operação é inválido.");

                    if (!DateTime.TryParseExact(tradeInfo[2], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime nextPayDate))
                        throw new Exception("Data do próximo pagamento para operação é inválida. O formato correto é MM/dd/yyyy.");

                    //Após validar, cria e popula um objeto da operação
                    Trade trade = new Trade
                    {
                        Value = value,
                        ClientSector = (TypeClientSector)Enum.Parse(typeof(TypeClientSector), cliSector),
                        NextPaymentDate = nextPayDate
                    };

                    //Categoriza a operação
                    string category = CategorizeTrade(trade, referenceDate, categories);

                    //Alimenta a lista final com a categoria da operação
                    resultCategories.Add(category);
                }

                Console.WriteLine("Segue abaixo o resultado em ordem para a(s) operação(ões) informada(s):");
                resultCategories.ForEach(x => Console.WriteLine(x));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        static string CategorizeTrade(ITrade trade, DateTime referenceDate, List<ICategory> categories)
        {
            foreach (var category in categories)
            {
                if (category.CheckCategory(trade, referenceDate))
                {
                    return category.Category;
                }
            }

            //Se a operação não se enquadrar em nenhuma categoria, retorna um valor padrão representando "indefinido"
            return "UNDEFINED";
        }
    }
}