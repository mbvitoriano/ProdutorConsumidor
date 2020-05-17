using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace ProdutorXConsumidor
{

    class Program
    {
        private static readonly Object meuLock = new Object(); //Instancia um Object pois o Lock precisa ser utlizado com um objeto       
        static string[] buffer;
        static Thread Produz;
        static Thread Consome;
        static ConsoleKeyInfo tecla = new ConsoleKeyInfo();  //Será utilizado para identificar a tecla pressionada pelo usuário, e inclui-la ao Buffer 
        static bool mutex = false; // Variável que atuará como controle para o while do Lock (Chamei de mutex pela funcionalidade semelhante)        

        static void Main(string[] args)
        {
            buffer = new string[10] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " }; // Iniciando com espaços para poder imprimir ele vazio no console.          

            Thread.Sleep(3000);
            Console.Clear();

            Produz = new Thread(Produtor);    //| Iniciando as Threads com os métodos de Produção e Consumo        
            Consome = new Thread(Consumidor); //|

            Produz.Start();
            Thread.Sleep(1000); //Utilizando o Sleep para garantir que a Thread de Produção inicie primeiro.     
            Consome.Start();

            Produz.Join(); // | O Join vai garantir que o uma Thread termine antes que outra possa ser iniciada
            Consome.Join();// |

        }

        static void Produtor()
        {
            while (!mutex)
            {
                lock (meuLock) /* Adquire a "trava" do trecho de código em seu interior, e só libera a outras operações quando
                               as intruções dinalizarem */
                {
                    Console.CursorVisible = true;   /* Como na Thread de consumo não há a interação do usuário, desativei o cursor para
                                                    melhorar a visibiladade da atuação do consumidor, portanto aqui eu reabilito ele.*/

                    Console.CursorSize = 100; // Faz com que o cursor apresente a forma de um bloco de texto, pois atribui sua altura a 100% da altura da linha

                    int posicao = 2; //Variável para controlar a posição do cursor de texto de forma dinâmica.

                    for (int i = 0; i < 10; i++)
                    {
                        Console.SetCursorPosition(0, 0); //Reseta a posição do cursor para que o texto seja sobrescrito, evitando o uso de Console.Clear()

                        Console.Write("\nPressione um caractere (Letras ou números) para incluir no Buffer\n\n");
                        Console.Write($"| {buffer[0]} | {buffer[1]} | {buffer[2]} | {buffer[3]} | {buffer[4]} | {buffer[5]} | {buffer[6]} | {buffer[7]} | {buffer[8]} | {buffer[9]} |");

                        Console.SetCursorPosition(posicao, 3);
                        /* Altera a posição do cursor para que ele esteja ao centro da primeira posição do buffer, note que ele está na coluna "posição" e linha 2
                        apos o usuário entrar com um caractere para o buffer ele será deslocado para a pŕoxima posição vazia do buffer*/

                        tecla = Console.ReadKey(true); // Utilizando o ReadKey para armazenar qual tecla o usuário apertou, sem a necessidade de pressionar enter.                                 

                        buffer[i] = tecla.Key.ToString().ToUpper(); // Coloca a tecla pressionada na posição i do buffer

                        posicao = posicao + 4; // Incrementa em 4 a coluna para que o cursor vá para a próxima posição vazia do buffer                     
                                                
                    }
                    Console.Clear();
                }
                Thread.Sleep(500);  // Aguarda um tempo entre a finalização do input e a exibição da mensagem "Buffer Cheio! Acionando o Consumidor!".                                       

            }
        }

        static void Consumidor()
        {

            while (!mutex)
            {
                lock (meuLock)
                {
                    int posicao = 2; // Mesma aplicação de manipulação do cursor

                    Console.CursorVisible = false; // Esconde o cursor para que a exibição das mensagens fique mais limpa
                    Console.CursorSize = 100; // Faz com que o cursor apresente a forma de um bloco de texto, pois atribui sua altura a 100% da altura da linha

                    Console.WriteLine("\nBuffer Cheio! Acionando o Consumidor!\n");
                    Thread.Sleep(2000); //Aguarda para que o programa não execute muito rápido e a mensagem possa ser lida

                    for (int i = 0; i < 10; i++)
                    {
                        Console.SetCursorPosition(0, 0); //Reseta a posição do cursor para que o texto seja sobrescrito, evitando o uso de Console.Clear()

                        Console.WriteLine("\nO Consumidor está esvaziando o buffer!\n");

                        buffer[i] = " "; //Para cada posição do buffe insere um caractere de espaço;

                        Console.Write($"| {buffer[0]} | {buffer[1]} | {buffer[2]} | {buffer[3]} | {buffer[4]} | {buffer[5]} | {buffer[6]} | {buffer[7]} | {buffer[8]} | {buffer[9]} |");

                        Console.CursorVisible = true; // Mostra o cursor para dar a impressão de que ele está apagando as posições do buffer

                        Console.SetCursorPosition(posicao, 3); //Mesma lógica do Produtor
                        posicao = posicao + 4;

                        Thread.Sleep(800);
                    }
                    Console.Clear();
                }
            }
        }
    }
}
