#ifndef COMUNIDADES_H
#define COMUNIDADES_H

#include "grafo.h"

/* Resultado da deteccao de comunidades via Girvan-Newman (RF08) */
typedef struct {
    int num_comunidades;
    int *comunidade_id;     /* comunidade_id[v] = id da comunidade (0..k-1) */
    int *tamanho_comunidade;/* qtd de vertices em cada comunidade */
    double modularidade;    /* Modularidade Q da particao obtida */
    int arestas_removidas;  /* Quantas arestas foram removidas ate esta particao */
} ResultadoComunidades;

/* Calcula a centralidade de intermediacao (edge betweenness) de todas as arestas ativas.
 * Retorna um vetor alocado de tamanho g->num_arestas (deve ser liberado com free). */
double *comunidades_calcular_betweenness(const Grafo *g);

/* Calcula a modularidade Q de uma particao usando as arestas originais do grafo */
double comunidades_calcular_modularidade(const Grafo *g, const int *comp_id, int num_comps);

/* Executa Girvan-Newman:
 * - alvo_comunidades: para quando atingir essa quantidade de comunidades (ex: 3)
 * - max_remocoes: limite maximo de arestas removidas (evita demora excessiva em grafos grandes)
 * Restaura o grafo original ao final da execucao. */
ResultadoComunidades *comunidades_girvan_newman(Grafo *g, int alvo_comunidades, int max_remocoes);

void comunidades_liberar(ResultadoComunidades *res);
void comunidades_imprimir(const Grafo *g, const ResultadoComunidades *res, int limite_vertices);

#endif /* COMUNIDADES_H */
