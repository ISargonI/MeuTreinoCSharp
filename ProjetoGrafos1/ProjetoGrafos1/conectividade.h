#ifndef CONECTIVIDADE_H
#define CONECTIVIDADE_H

#include "grafo.h"
#include <stdbool.h>

/* Resultado das componentes conexas (RF03 e RF04) */
typedef struct {
    int num_componentes;
    int maior_componente;
    int *comp_id;           /* comp_id[v] = id da componente (0..k-1), ou -1 se inativo */
    int *tamanho_comp;      /* tamanho_comp[c] = qtd de vertices na componente c */
} ResultadoComponentes;

/* Resultado da analise de Tarjan (RF05, RF06 e RF07) */
typedef struct {
    bool *eh_articulacao;   /* tamanho: num_vertices */
    int num_articulacoes;

    int *pontes_id;         /* ids das arestas que sao pontes */
    int num_pontes;

    int num_biconexas;
    int **biconexas_vertices; /* matriz com os vertices de cada componente biconexa */
    int *biconexas_tamanhos;  /* quantidade de vertices em cada componente biconexa */
} ResultadoTarjan;

/* RF03 e RF04: Componentes Conexas */
ResultadoComponentes *conectividade_componentes(const Grafo *g);
void conectividade_liberar_componentes(ResultadoComponentes *res);
bool conectividade_eh_conexo(const Grafo *g);
void conectividade_imprimir_componentes(const Grafo *g, const ResultadoComponentes *res, int limite_vertices);

/* RF05, RF06 e RF07: Articulacoes, Pontes e Componentes Biconexas */
ResultadoTarjan *conectividade_analisar_tarjan(const Grafo *g);
void conectividade_liberar_tarjan(ResultadoTarjan *res);
void conectividade_imprimir_tarjan(const Grafo *g, const ResultadoTarjan *res, int limite_itens);

#endif /* CONECTIVIDADE_H */
