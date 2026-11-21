#include "comunidades.h"
#include "conectividade.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* =========================================================================
 * ALGORITMO DE BRANDES PARA CENTRALIDADE DE INTERMEDIACAO DE ARESTAS
 * ========================================================================= */

double *comunidades_calcular_betweenness(const Grafo *g) {
    if (!g) return NULL;

    int V = g->num_vertices;
    int E = g->num_arestas;

    double *betweenness = (double *)calloc((size_t)(E > 0 ? E : 1), sizeof(double));
    int *dist = (int *)malloc((size_t)V * sizeof(int));
    double *sigma = (double *)malloc((size_t)V * sizeof(double));
    double *delta = (double *)malloc((size_t)V * sizeof(double));
    int *fila = (int *)malloc((size_t)V * sizeof(int));

    if (!betweenness || !dist || !sigma || !delta || !fila) {
        free(betweenness);
        free(dist);
        free(sigma);
        free(delta);
        free(fila);
        return NULL;
    }

    /* Executa uma BFS a partir de cada vertice ativo s */
    for (int s = 0; s < V; s++) {
        if (!g->vertices[s].ativo) continue;

        for (int i = 0; i < V; i++) {
            dist[i] = -1;
            sigma[i] = 0.0;
            delta[i] = 0.0;
        }

        dist[s] = 0;
        sigma[s] = 1.0;

        int ini = 0, fim = 0;
        fila[fim++] = s;

        /* Fase 1: BFS para calcular distancias e numero de caminhos minimos (sigma) */
        while (ini < fim) {
            int u = fila[ini++];
            NoAdj *atual = g->vertices[u].adj;
            while (atual) {
                int v = atual->destino;
                if (g->arestas[atual->id_aresta].ativa && g->vertices[v].ativo) {
                    if (dist[v] < 0) {
                        dist[v] = dist[u] + 1;
                        fila[fim++] = v;
                    }
                    if (dist[v] == dist[u] + 1) {
                        sigma[v] += sigma[u];
                    }
                }
                atual = atual->prox;
            }
        }

        /* Fase 2: Acumulacao reversa das dependencias (do mais distante para s) */
        for (int idx = fim - 1; idx > 0; idx--) {
            int w = fila[idx];
            NoAdj *atual = g->vertices[w].adj;
            while (atual) {
                int v = atual->destino;
                int id_e = atual->id_aresta;
                if (g->arestas[id_e].ativa && g->vertices[v].ativo) {
                    if (dist[v] == dist[w] - 1) {
                        double contrib = (sigma[v] / sigma[w]) * (1.0 + delta[w]);
                        betweenness[id_e] += contrib;
                        delta[v] += contrib;
                    }
                }
                atual = atual->prox;
            }
        }
    }

    /* Grafo nao dirigido: cada par (s, t) foi contado duas vezes */
    for (int i = 0; i < E; i++) {
        betweenness[i] /= 2.0;
    }

    free(dist);
    free(sigma);
    free(delta);
    free(fila);
    return betweenness;
}

/* =========================================================================
 * CALCULO DA MODULARIDADE (Q)
 * ========================================================================= */

double comunidades_calcular_modularidade(const Grafo *g, const int *comp_id, int num_comps) {
    if (!g || !comp_id || num_comps <= 0 || g->num_arestas == 0) return 0.0;

    int V = g->num_vertices;
    int E = g->num_arestas;
    double m = 0.0;

    double *e_interno = (double *)calloc((size_t)num_comps, sizeof(double));
    double *a_grau = (double *)calloc((size_t)num_comps, sizeof(double));
    if (!e_interno || !a_grau) {
        free(e_interno);
        free(a_grau);
        return 0.0;
    }

    /* Considera todas as arestas originais entre vertices ativos */
    for (int i = 0; i < E; i++) {
        int u = g->arestas[i].u;
        int v = g->arestas[i].v;
        if (!g->vertices[u].ativo || !g->vertices[v].ativo) continue;

        m += 1.0;
        int cu = comp_id[u];
        int cv = comp_id[v];
        if (cu >= 0 && cu < num_comps) a_grau[cu] += 1.0;
        if (cv >= 0 && cv < num_comps) a_grau[cv] += 1.0;
        if (cu >= 0 && cu == cv) {
            e_interno[cu] += 1.0;
        }
    }

    double Q = 0.0;
    if (m > 0.0) {
        for (int c = 0; c < num_comps; c++) {
            double frac_interna = e_interno[c] / m;
            double frac_esperada = a_grau[c] / (2.0 * m);
            Q += frac_interna - (frac_esperada * frac_esperada);
        }
    }

    free(e_interno);
    free(a_grau);
    return Q;
}

/* =========================================================================
 * ALGORITMO DE GIRVAN-NEWMAN
 * ========================================================================= */

ResultadoComunidades *comunidades_girvan_newman(Grafo *g, int alvo_comunidades, int max_remocoes) {
    if (!g) return NULL;

    /* Salva estado original das arestas para restaurar ao final */
    bool *estado_original = (bool *)malloc((size_t)g->num_arestas * sizeof(bool));
    if (!estado_original) return NULL;
    for (int i = 0; i < g->num_arestas; i++) {
        estado_original[i] = g->arestas[i].ativa;
    }

    ResultadoComponentes *comp_inicial = conectividade_componentes(g);
    int comps_originais = comp_inicial->num_componentes;

    printf("\n=== EXECUTANDO GIRVAN-NEWMAN (RF08) ===\n");
    printf("Componentes conexas originais: %d\n", comps_originais);
    printf("Alvo de comunidades: %d | Maximo de remocoes: %d\n", alvo_comunidades, max_remocoes);

    ResultadoComponentes *melhor_particao = comp_inicial;
    double melhor_Q = comunidades_calcular_modularidade(g, comp_inicial->comp_id, comp_inicial->num_componentes);
    int remocoes_melhor = 0;

    for (int passo = 1; passo <= max_remocoes; passo++) {
        double *btw = comunidades_calcular_betweenness(g);
        if (!btw) break;

        /* Encontra a aresta ativa de maior centralidade */
        int melhor_aresta = -1;
        double max_btw = -1.0;
        for (int i = 0; i < g->num_arestas; i++) {
            if (g->arestas[i].ativa &&
                g->vertices[g->arestas[i].u].ativo &&
                g->vertices[g->arestas[i].v].ativo) {
                if (btw[i] > max_btw) {
                    max_btw = btw[i];
                    melhor_aresta = i;
                }
            }
        }
        free(btw);

        if (melhor_aresta == -1 || max_btw <= 0.0) {
            break;
        }

        /* Remove temporariamente a aresta de maior centralidade */
        g->arestas[melhor_aresta].ativa = false;
        int u = g->arestas[melhor_aresta].u;
        int v = g->arestas[melhor_aresta].v;

        ResultadoComponentes *atual = conectividade_componentes(g);
        double Q_atual = comunidades_calcular_modularidade(g, atual->comp_id, atual->num_componentes);

        printf("  Passo %2d: removida (%s -- %s) [btw=%.2f] -> %d comunidades | Modularidade Q = %.4f\n",
               passo, g->vertices[u].nome, g->vertices[v].nome, max_btw,
               atual->num_componentes, Q_atual);

        /* Guarda a particao se melhorou a modularidade ou atingiu o alvo */
        if (Q_atual >= melhor_Q || atual->num_componentes >= alvo_comunidades) {
            if (melhor_particao != atual) {
                conectividade_liberar_componentes(melhor_particao);
            }
            melhor_particao = atual;
            melhor_Q = Q_atual;
            remocoes_melhor = passo;
        } else {
            conectividade_liberar_componentes(atual);
        }

        if (melhor_particao->num_componentes >= alvo_comunidades) {
            break;
        }
    }

    /* Restaura o grafo original */
    for (int i = 0; i < g->num_arestas; i++) {
        g->arestas[i].ativa = estado_original[i];
    }
    free(estado_original);

    /* Empacota o resultado final */
    ResultadoComunidades *res = (ResultadoComunidades *)malloc(sizeof(ResultadoComunidades));
    res->num_comunidades = melhor_particao->num_componentes;
    res->modularidade = melhor_Q;
    res->arestas_removidas = remocoes_melhor;

    res->comunidade_id = (int *)malloc((size_t)g->num_vertices * sizeof(int));
    res->tamanho_comunidade = (int *)malloc((size_t)res->num_comunidades * sizeof(int));
    memcpy(res->comunidade_id, melhor_particao->comp_id, (size_t)g->num_vertices * sizeof(int));
    memcpy(res->tamanho_comunidade, melhor_particao->tamanho_comp, (size_t)res->num_comunidades * sizeof(int));

    conectividade_liberar_componentes(melhor_particao);
    return res;
}

void comunidades_liberar(ResultadoComunidades *res) {
    if (!res) return;
    free(res->comunidade_id);
    free(res->tamanho_comunidade);
    free(res);
}

void comunidades_imprimir(const Grafo *g, const ResultadoComunidades *res, int limite_vertices) {
    if (!g || !res) return;

    printf("\n=== COMUNIDADES DETECTADAS (GIRVAN-NEWMAN) ===\n");
    printf("Total de comunidades : %d\n", res->num_comunidades);
    printf("Arestas removidas    : %d\n", res->arestas_removidas);
    printf("Modularidade (Q)     : %.4f\n", res->modularidade);

    for (int c = 0; c < res->num_comunidades; c++) {
        printf("  Comunidade %d (%d vertices): ", c + 1, res->tamanho_comunidade[c]);
        int impressos = 0;
        for (int i = 0; i < g->num_vertices; i++) {
            if (res->comunidade_id[i] == c) {
                if (limite_vertices > 0 && impressos >= limite_vertices) {
                    printf("... (+%d)", res->tamanho_comunidade[c] - impressos);
                    break;
                }
                printf("%s ", g->vertices[i].nome);
                impressos++;
            }
        }
        printf("\n");
    }
}
