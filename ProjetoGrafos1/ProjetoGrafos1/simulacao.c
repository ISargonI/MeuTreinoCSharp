#include "simulacao.h"
#include "conectividade.h"
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

static void imprimir_estado_conectividade(const Grafo *g, const char *rotulo) {
    ResultadoComponentes *comp = conectividade_componentes(g);
    if (!comp) return;

    int ativos = grafo_contar_vertices_ativos(g);
    double frac_maior = (ativos > 0) ? (100.0 * (double)comp->maior_componente / (double)ativos) : 0.0;

    printf("  %-26s | Vertices: %4d | Comps: %3d | Maior comp: %4d (%.1f%%)\n",
           rotulo, ativos, comp->num_componentes, comp->maior_componente, frac_maior);

    conectividade_liberar_componentes(comp);
}

void simulacao_remover_vertice(Grafo *g, const char *nome_usuario) {
    if (!g || !nome_usuario) return;

    int id = grafo_buscar_vertice(g, nome_usuario);
    if (id == -1) {
        printf("\n[Simulacao] Usuario '%s' nao encontrado.\n", nome_usuario);
        return;
    }

    printf("\n=== SIMULACAO: REMOCAO DO USUARIO '%s' (grau %d) ===\n",
           nome_usuario, grafo_grau_vertice(g, id));

    imprimir_estado_conectividade(g, "Antes da remocao");

    /* Remove temporariamente */
    g->vertices[id].ativo = false;
    imprimir_estado_conectividade(g, "Apos remover usuario");

    /* Restaura */
    g->vertices[id].ativo = true;
}

void simulacao_remover_aresta(Grafo *g, const char *nome_u, const char *nome_v) {
    if (!g || !nome_u || !nome_v) return;

    int u = grafo_buscar_vertice(g, nome_u);
    int v = grafo_buscar_vertice(g, nome_v);
    if (u == -1 || v == -1) {
        printf("\n[Simulacao] Vertice '%s' ou '%s' nao encontrado.\n", nome_u, nome_v);
        return;
    }

    int id_aresta = -1;
    for (int i = 0; i < g->num_arestas; i++) {
        if ((g->arestas[i].u == u && g->arestas[i].v == v) ||
            (g->arestas[i].u == v && g->arestas[i].v == u)) {
            id_aresta = i;
            break;
        }
    }

    if (id_aresta == -1) {
        printf("\n[Simulacao] Aresta (%s -- %s) nao existe.\n", nome_u, nome_v);
        return;
    }

    printf("\n=== SIMULACAO: REMOCAO DA ARESTA (%s -- %s) ===\n", nome_u, nome_v);
    imprimir_estado_conectividade(g, "Antes da remocao");

    /* Remove temporariamente */
    g->arestas[id_aresta].ativa = false;
    imprimir_estado_conectividade(g, "Apos remover aresta");

    /* Restaura */
    g->arestas[id_aresta].ativa = true;
}

void simulacao_comparar_estrategias(Grafo *g, int k_remocoes) {
    if (!g || k_remocoes <= 0) return;

    int V = g->num_vertices;
    if (k_remocoes >= V) k_remocoes = V - 1;

    printf("\n=== COMPARACAO DE REMOCOES (RF09): ALEATORIA vs ORIENTADA POR GRAU (k=%d) ===\n", k_remocoes);
    imprimir_estado_conectividade(g, "Grafo Original");

    /* 1. Remocao Aleatoria */
    srand(42); /* Semente fixa para reprodutibilidade no relatorio */
    int *indices = (int *)malloc((size_t)V * sizeof(int));
    if (!indices) return;

    for (int i = 0; i < V; i++) indices[i] = i;
    for (int i = V - 1; i > 0; i--) {
        int j = rand() % (i + 1);
        int tmp = indices[i];
        indices[i] = indices[j];
        indices[j] = tmp;
    }

    for (int i = 0; i < k_remocoes; i++) {
        g->vertices[indices[i]].ativo = false;
    }
    imprimir_estado_conectividade(g, "Apos remocao ALEATORIA");
    grafo_restaurar(g);
    free(indices);

    /* 2. Remocao Orientada por Maior Grau (recalculando grau a cada passo) */
    for (int passo = 0; passo < k_remocoes; passo++) {
        int melhor_v = -1;
        int max_grau = -1;
        for (int i = 0; i < V; i++) {
            if (g->vertices[i].ativo) {
                int grau = grafo_grau_vertice(g, i);
                if (grau > max_grau) {
                    max_grau = grau;
                    melhor_v = i;
                }
            }
        }
        if (melhor_v != -1) {
            g->vertices[melhor_v].ativo = false;
        }
    }
    imprimir_estado_conectividade(g, "Apos remocao por GRAU");
    grafo_restaurar(g);
}
