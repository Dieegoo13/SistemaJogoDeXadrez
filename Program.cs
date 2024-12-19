using System;
using SistemaJogoDeXadrez.tabuleiro;
using SistemaJogoDeXadrez.xadrez;
using tabuleiro;
using xadrez;

namespace SistemaJogoDeXadrez 
{
    internal class Program
    {
        static void Main(string[] args)
        {
         try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();
               while(!partida.terminada)
               {
                 Console.Clear();
                 Tela.imprimirTabuleiro(partida.GetTab());

                 Console.WriteLine();
                 Console.WriteLine("Origem: ");
                 Posicao origem = Tela.LerPosicaoXadrez().toPosicao();

                 
                 bool[,] posicoesPossiveis = partida.GetTab().peca(origem).movimentosPossiveis();
                 
                 Console.Clear();
                 Tela.imprimirTabuleiro(partida.GetTab(), posicoesPossiveis);
                 
                 Console.Write("Destino:");
                 Posicao destino = Tela.LerPosicaoXadrez().toPosicao();

                 partida.executaMovimento(origem, destino);
               }
        
            }
           catch(TabuleiroExeption e)
           {
                Console.WriteLine(e.Message);
           } 

            // PosicaoXadrez pos = new PosicaoXadrez('c', 7);
            // Console.WriteLine(pos);

            // Console.WriteLine(pos.toPosicao());
        }
    }
}