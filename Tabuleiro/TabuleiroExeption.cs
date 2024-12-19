using System;
using System.Collections.Generic;


namespace SistemaJogoDeXadrez.tabuleiro
{
    public class TabuleiroExeption : Exception
    {
        public TabuleiroExeption(string msg) : base(msg)
        {
            
        }
    }
}