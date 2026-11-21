#ifndef SIMULACAO_H
#define SIMULACAO_H

#include "grafo.h"

/* Remove temporariamente um usuario especifico, recalcula conectividade e restaura o grafo */
void simulacao_remover_vertice(Grafo *g, const char *nome_usuario);

/* Remove temporariamente uma aresta especifica, recalcula conectividade e restaura o grafo */
void simulacao_remover_aresta(Grafo *g, const char *nome_u, const char *nome_v);

/* Compara o impacto na rede entre:
 * 1) Remocao aleatoria de k usuarios
 * 2) Remocao orientada por maior grau (hubs) de k usuarios
 * Restaura o grafo original ao final de cada teste. */
void simulacao_comparar_estrategias(Grafo *g, int k_remocoes);

#endif /* SIMULACAO_H */
