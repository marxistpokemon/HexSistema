Sistema de geração de terreno em tiles para Unity. Ainda bastante indefinido.

## Capacidades

- Seria legal ter suporte para vários tipos de tiles: triangulares, quadrados, hexagonais, poligonais (tanto faz o número de faces). Já incluir um modelo básico (com UV) para cada um desses e um sistema de coordenadas facilmente adaptável (q, r, anotação).

- Diferentes formas de perturbar a distribuição e forma da malha de tiles, permitindo malhas irregulares. Na verdade, isso envolveria gerar os polígonos a partir de uma distribuição de vértices, o inverso do que é feito agora.

- Criar geradores modulares nos quais você possa adicionar “camadas” de funcionalidade. Coisas como: adicionar rios, fazer duas parametrizações de Perlin para um terreno, carregar um height-map de uma textura, etc.

- Permitir dois modos de renderização: 1) usando o *Terrain* da Unity e 2) usando tiles independentes em 3D;

## Ferramentas

Algumas coisas sobre o modo de funcionamento do sistema, que ainda não estão lá, mas que seriam interessantes.

- Conjunto (bem documentado) de funções helper prontas para desenhar Gizmos das relações lógicas entre os tiles;

- *Inspector* próprio na Unity, onde você pode parametrizar / gerar e carregar mapas na Scene, para depois editar e acrescentar objetos em cima;

- Importação / exportação de XML tanto para 1) carregamento dentro da *Scene* quanto para 2) carregar/salvar os terrenos dentro de um jogo rodando;