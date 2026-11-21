#ifndef RELATORIO_H
#define RELATORIO_H

#include "grafo.h"
#include "conectividade.h"
#include "comunidades.h"

/* Gera um arquivo de texto estruturado com todas as metricas do grafo (RF10) */
bool relatorio_gerar_txt(const char *caminho_saida,
                         const Grafo *g,
                         const ResultadoComponentes *comp,
                         const ResultadoTarjan *tarjan,
                         const ResultadoComunidades *com);

/* Exporta o grafo no formato Graphviz (.dot) para visualizacao (RF02 / RF10).
 * Destaca vertices de articulacao (vermelho) e pontes (tracejado vermelho).
 * Em grafos grandes, 'max_vertices' limita o subgrafo exportado para evitar travar o visualizador. */
bool relatorio_exportar_dot(const char *caminho_saida,
                            const Grafo *g,
                            const ResultadoTarjan *tarjan,
                            const ResultadoComunidades *com,
                            int max_vertices);

#endif /* RELATORIO_H */
