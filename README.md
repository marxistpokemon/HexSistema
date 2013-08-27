#HexSistema

Sistema de geração de terreno em tiles para Unity. Ainda bastante indefinido.

![Exemplo de terreno gerado com o HexSistema e tiles cúbicos](http://31.media.tumblr.com/431482703857d7f9cafd99bc475f5937/tumblr_mrg9isgfaw1rr3yu2o1_1280.png)

## Objetivos

- Suporte para **vários tipos de tiles**: triangulares, quadrados, hexagonais, poligonais (tanto faz o número de faces). 

- Incluir um **modelo 3D básico** (com UV) para cada tipo de tile;

- **Sistema adaptável de coordenadas** simples (q, r, anotação).

- **Diferentes formas de perturbar a distribuição e forma da malha de tiles**, permitindo malhas irregulares. Na verdade, isso envolveria gerar os polígonos a partir de uma distribuição de vértices, o inverso do que é feito agora.

- Criar **módulos** para o gerador nos quais você possa adicionar “camadas” de funcionalidade. Coisas como: 
	- adicionar rios; 
	- fazer duas parametrizações de Perlin para um terreno;
	- carregar um height-map de uma textura;

- Permitir **dois modos de renderização**
	1. Usando o *Terrain* da Unity;
	2. Usando tiles independentes em 3D;

## Ferramentas

Algumas coisas sobre o modo de funcionamento do sistema, que ainda não estão lá, mas que seriam interessantes.

- Conjunto (bem documentado) de **funções helper** prontas para **desenhar Gizmos** das relações lógicas entre os tiles;

- ***Inspector* próprio na Unity**, onde você pode parametrizar / gerar e carregar mapas na Scene, para depois editar e acrescentar objetos em cima;

- **Importação / exportação de XML** tanto para 1) carregamento dentro da *Scene* quanto para 2) carregar/salvar os terrenos dentro de um jogo rodando;