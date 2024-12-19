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
            Tabuleiro tab = new Tabuleiro(8, 8);
            
         try
            {
                tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(0, 0));
                tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(1, 3));
                tab.colocarPeca(new Rei(tab, Cor.Preta), new Posicao(0, 2));

                tab.colocarPeca(new Torre(tab, Cor.Branca), new Posicao(3, 5));
                tab.colocarPeca(new Torre(tab, Cor.Branca), new Posicao(2, 3));
                tab.colocarPeca(new Rei(tab, Cor.Branca), new Posicao(1, 2));

                Tela.imprimirTabuleiro(tab);
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