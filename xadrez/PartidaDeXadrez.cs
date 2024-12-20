using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SistemaJogoDeXadrez.tabuleiro;
using SistemaJogoDeXadrez.xadrez;
using tabuleiro;

namespace xadrez
{
    public class PartidaDeXadrez
    {
        private Tabuleiro tab;
        public int turno {get; private set;}
        public Cor jogadorAtual {get; private set;}
        public bool terminada {get; private set;}
        public bool xeque {get; private set;}
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public Tabuleiro GetTab(){
            return tab;
        }

        private void SetTab(Tabuleiro tab){
            tab = this.tab;
        }

        public PartidaDeXadrez(){
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);

            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada){
            Peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimentos();

            if(pecaCapturada != null){
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }

            tab.colocarPeca(p, origem);
        }

        public void realizaJogada(Posicao origem, Posicao destino){
            Peca pecaCapturada = executaMovimento(origem, destino);

            if(estaEmXeque(jogadorAtual)){
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroExeption("Você não pode se colocar em xeque!");
            }

            if (estaEmXeque(adversaria(jogadorAtual))){
                xeque = true;
            }
            else {
                xeque = false;
            }
            turno++;
            mudaJogador();
        }

        public void validarPosicaoDeOrigem(Posicao pos){
            if(tab.peca(pos) == null){
                throw new TabuleiroExeption("Não existe peça na posição de origem escolhida!");
            }
            if(jogadorAtual != tab.peca(pos).cor){
                throw new TabuleiroExeption("A peça de origem escolhida não é sua");
            }
            if(!tab.peca(pos).existeMovimentosPossiveis()){
                throw new TabuleiroExeption("Não há movimentos possiveis para a peça de origem escolhida");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino){
            if(!tab.peca(origem).podeMoverPara(destino)){
                throw new TabuleiroExeption("Posição de destino invalida!");
            }
        }

        public void mudaJogador(){
            if(jogadorAtual == Cor.Branca){
                jogadorAtual = Cor.Preta;
            }else {
                jogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor){
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca x in capturadas)
            {
                if(x.cor == cor){
                    aux.Add(x);
                }
            }

            return aux;
        }

         public HashSet<Peca> PecasEmJogo(Cor cor){
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca x in pecas) { // Aqui estava o problema: usar "pecas" ao invés de "capturadas"
                if (x.cor == cor) {
                    aux.Add(x);
                }
            }

            aux.ExceptWith(pecasCapturadas(cor)); // Remove as capturadas
            return aux;
        }


        private Cor adversaria(Cor cor){
            if(cor == Cor.Branca){
                return Cor.Preta;
            }else {
                return Cor.Branca;
            }
        }

        private Peca rei (Cor cor){
            foreach(Peca x in PecasEmJogo(cor))
            {
                if(x is Rei){
                    return x;
                }
            }

            return null;
        }

        public bool estaEmXeque(Cor cor){

            Peca R = rei(cor);
            if (R == null){
                throw new TabuleiroExeption($"Não tem rei da cor {cor} No tabuleiro!");
            }

            foreach (Peca x in PecasEmJogo(adversaria(cor))){
                bool[,] mat = x.movimentosPossiveis();
                if(mat[R.posicao.linha, R.posicao.coluna]){
                    return true;
                }
            }

            return false;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca){
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas(){
            colocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('c', 2, new Torre(tab, Cor.Branca));
            colocarNovaPeca('d', 2, new Torre(tab, Cor.Branca));
            colocarNovaPeca('e', 2, new Torre(tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));

            colocarNovaPeca('c', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('d', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('e', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Rei(tab, Cor.Preta));
        }
    }
}