#include "conectividade.h"
#include <stdio.h>
#include <stdlib.h>

/* =========================================================================
 * RF03 e RF04: COMPONENTES CONEXAS (BFS ITERATIVA)
 * ========================================================================= */

ResultadoComponentes *conectividade_componentes(const Grafo *g) {
    if (!g) return NULL;

    ResultadoComponentes *res = (ResultadoComponentes *)malloc(sizeof(ResultadoComponentes));
    if (!res) return NULL;

    int V = g->num_vertices;
    res->comp_id = (int *)malloc((size_t)V * sizeof(int));
    res->tamanho_comp = (int *)calloc((size_t)V, sizeof(int));
    int *fila = (int *)malloc((size_t)V * sizeof(int));

    if (!res->comp_id || !res->tamanho_comp || !fila) {
        free(res->comp_id);
        free(res->tamanho_comp);
        free(fila);
        free(res);
        return NULL;
    }

    for (int i = 0; i < V; i++) {
        res->comp_id[i] = -1;
    }

    res->num_componentes = 0;
    res->maior_componente = 0;

    for (int i = 0; i < V; i++) {
        if (!g->vertices[i].ativo || res->comp_id[i] != -1) {
            continue;
        }

        int cid = res->num_componentes++;
        int ini = 0, fim = 0;
        int tam = 0;

        res->comp_id[i] = cid;
        fila[fim++] = i;

        while (ini < fim) {
            int u = fila[ini++];
            tam++;

            NoAdj *atual = g->vertices[u].adj;
            while (atual) {
                int v = atual->destino;
                if (g->arestas[atual->id_aresta].ativa &&
                    g->vertices[v].ativo &&
                    res->comp_id[v] == -1) {
                    res->comp_id[v] = cid;
                    fila[fim++] = v;
                }
                atual = atual->prox;
            }
        }

        res->tamanho_comp[cid] = tam;
        if (tam > res->maior_componente) {
            res->maior_componente = tam;
        }
    }

    free(fila);
    return res;
}

void conectividade_liberar_componentes(ResultadoComponentes *res) {
    if (!res) return;
    free(res->comp_id);
    free(res->tamanho_comp);
    free(res);
}

bool conectividade_eh_conexo(const Grafo *g) {
    ResultadoComponentes *res = conectividade_componentes(g);
    if (!res) return false;
    bool conexo = (res->num_componentes == 1);
    conectividade_liberar_componentes(res);
    return conexo;
}

void conectividade_imprimir_componentes(const Grafo *g, const ResultadoComponentes *res, int limite_vertices) {
    if (!g || !res) return;

    printf("\n=== COMPONENTES CONEXAS (RF03 / RF04) ===\n");
    printf("Grafo conexo?          : %s\n", (res->num_componentes == 1) ? "SIM" : "NAO");
    printf("Total de componentes   : %d\n", res->num_componentes);
    printf("Maior componente       : %d vertices\n", res->maior_componente);

    for (int c = 0; c < res->num_componentes; c++) {
        printf("  Componente %d (%d vertices): ", c + 1, res->tamanho_comp[c]);
        int impressos = 0;
        for (int i = 0; i < g->num_vertices; i++) {
            if (res->comp_id[i] == c) {
                if (limite_vertices > 0 && impressos >= limite_vertices) {
                    printf("... (+%d)", res->tamanho_comp[c] - impressos);
                    break;
                }
                printf("%s ", g->vertices[i].nome);
                impressos++;
            }
        }
        printf("\n");
    }
}

/* =========================================================================
 * RF05, RF06 e RF07: TARJAN (ARTICULACOES, PONTES E COMPONENTES BICONEXAS)
 * ========================================================================= */

typedef struct {
    const Grafo *g;
    ResultadoTarjan *res;
    int *disc;
    int *low;
    int *pai;
    int tempo;

    /* Pilha de arestas para extrair componentes biconexas */
    int *pilha_arestas;
    int topo_pilha;

    /* Controle dinamico das componentes biconexas */
    int cap_biconexas;
    int *marca_vertice; /* evita duplicar vertices ao extrair uma biconexa */
    int marca_atual;
} TarjanCtx;

static void registrar_biconexa(TarjanCtx *ctx, int id_aresta_parada) {
    ResultadoTarjan *res = ctx->res;

    if (res->num_biconexas == ctx->cap_biconexas) {
        int nova_cap = (ctx->cap_biconexas == 0) ? 16 : ctx->cap_biconexas * 2;
        res->biconexas_vertices = (int **)realloc(res->biconexas_vertices, (size_t)nova_cap * sizeof(int *));
        res->biconexas_tamanhos = (int *)realloc(res->biconexas_tamanhos, (size_t)nova_cap * sizeof(int));
        ctx->cap_biconexas = nova_cap;
    }

    int cap_v = 16;
    int qtd_v = 0;
    int *lista_v = (int *)malloc((size_t)cap_v * sizeof(int));
    ctx->marca_atual++;

    while (ctx->topo_pilha > 0) {
        int id_e = ctx->pilha_arestas[--ctx->topo_pilha];
        int u = ctx->g->arestas[id_e].u;
        int v = ctx->g->arestas[id_e].v;

        if (ctx->marca_vertice[u] != ctx->marca_atual) {
            ctx->marca_vertice[u] = ctx->marca_atual;
            if (qtd_v == cap_v) {
                cap_v *= 2;
                lista_v = (int *)realloc(lista_v, (size_t)cap_v * sizeof(int));
            }
            lista_v[qtd_v++] = u;
        }
        if (ctx->marca_vertice[v] != ctx->marca_atual) {
            ctx->marca_vertice[v] = ctx->marca_atual;
            if (qtd_v == cap_v) {
                cap_v *= 2;
                lista_v = (int *)realloc(lista_v, (size_t)cap_v * sizeof(int));
            }
            lista_v[qtd_v++] = v;
        }

        if (id_e == id_aresta_parada) {
            break;
        }
    }

    int idx = res->num_biconexas++;
    res->biconexas_vertices[idx] = lista_v;
    res->biconexas_tamanhos[idx] = qtd_v;
}

static void dfs_tarjan(TarjanCtx *ctx, int u) {
    const Grafo *g = ctx->g;
    ctx->disc[u] = ctx->low[u] = ++ctx->tempo;
    int filhos = 0;

    NoAdj *atual = g->vertices[u].adj;
    while (atual) {
        int v = atual->destino;
        int id_e = atual->id_aresta;

        if (!g->arestas[id_e].ativa || !g->vertices[v].ativo) {
            atual = atual->prox;
            continue;
        }

        if (ctx->disc[v] == 0) {
            filhos++;
            ctx->pai[v] = u;
            ctx->pilha_arestas[ctx->topo_pilha++] = id_e;

            dfs_tarjan(ctx, v);

            if (ctx->low[v] < ctx->low[u]) {
                ctx->low[u] = ctx->low[v];
            }

            /* Condicao de Vertice de Articulacao (RF05) e Componente Biconexa (RF07) */
            if ((ctx->pai[u] == -1 && filhos > 1) ||
                (ctx->pai[u] != -1 && ctx->low[v] >= ctx->disc[u])) {
                if (!ctx->res->eh_articulacao[u]) {
                    ctx->res->eh_articulacao[u] = true;
                    ctx->res->num_articulacoes++;
                }
                registrar_biconexa(ctx, id_e);
            }

            /* Condicao de Ponte (RF06) */
            if (ctx->low[v] > ctx->disc[u]) {
                ctx->res->pontes_id[ctx->res->num_pontes++] = id_e;
            }
        } else if (v != ctx->pai[u] && ctx->disc[v] < ctx->disc[u]) {
            /* Aresta de retorno */
            ctx->pilha_arestas[ctx->topo_pilha++] = id_e;
            if (ctx->disc[v] < ctx->low[u]) {
                ctx->low[u] = ctx->disc[v];
            }
        }

        atual = atual->prox;
    }
}

ResultadoTarjan *conectividade_analisar_tarjan(const Grafo *g) {
    if (!g) return NULL;

    int V = g->num_vertices;
    int E = g->num_arestas;

    ResultadoTarjan *res = (ResultadoTarjan *)calloc(1, sizeof(ResultadoTarjan));
    if (!res) return NULL;

    res->eh_articulacao = (bool *)calloc((size_t)V, sizeof(bool));
    res->pontes_id = (int *)malloc((size_t)(E > 0 ? E : 1) * sizeof(int));

    TarjanCtx ctx;
    ctx.g = g;
    ctx.res = res;
    ctx.disc = (int *)calloc((size_t)V, sizeof(int));
    ctx.low = (int *)calloc((size_t)V, sizeof(int));
    ctx.pai = (int *)malloc((size_t)V * sizeof(int));
    ctx.pilha_arestas = (int *)malloc((size_t)(E > 0 ? E : 1) * sizeof(int));
    ctx.marca_vertice = (int *)calloc((size_t)V, sizeof(int));
    ctx.tempo = 0;
    ctx.topo_pilha = 0;
    ctx.cap_biconexas = 0;
    ctx.marca_atual = 0;

    for (int i = 0; i < V; i++) {
        ctx.pai[i] = -1;
    }

    for (int i = 0; i < V; i++) {
        if (g->vertices[i].ativo && ctx.disc[i] == 0) {
            dfs_tarjan(&ctx, i);
            /* Se sobraram arestas na pilha, formam uma componente biconexa */
            if (ctx.topo_pilha > 0) {
                registrar_biconexa(&ctx, -1);
            }
        }
    }

    free(ctx.disc);
    free(ctx.low);
    free(ctx.pai);
    free(ctx.pilha_arestas);
    free(ctx.marca_vertice);

    return res;
}

void conectividade_liberar_tarjan(ResultadoTarjan *res) {
    if (!res) return;
    free(res->eh_articulacao);
    free(res->pontes_id);
    for (int i = 0; i < res->num_biconexas; i++) {
        free(res->biconexas_vertices[i]);
    }
    free(res->biconexas_vertices);
    free(res->biconexas_tamanhos);
    free(res);
}

void conectividade_imprimir_tarjan(const Grafo *g, const ResultadoTarjan *res, int limite_itens) {
    if (!g || !res) return;

    printf("\n=== VERTICES DE ARTICULACAO (RF05) ===\n");
    printf("Total encontrados: %d\n", res->num_articulacoes);
    int cont = 0;
    for (int i = 0; i < g->num_vertices; i++) {
        if (res->eh_articulacao[i]) {
            if (limite_itens > 0 && cont >= limite_itens) {
                printf("... (+%d)\n", res->num_articulacoes - cont);
                break;
            }
            printf("  - %s (grau %d)\n", g->vertices[i].nome, grafo_grau_vertice(g, i));
            cont++;
        }
    }

    printf("\n=== PONTES / CONEXOES CRITICAS (RF06) ===\n");
    printf("Total encontradas: %d\n", res->num_pontes);
    for (int i = 0; i < res->num_pontes; i++) {
        if (limite_itens > 0 && i >= limite_itens) {
            printf("... (+%d)\n", res->num_pontes - i);
            break;
        }
        int id_e = res->pontes_id[i];
        int u = g->arestas[id_e].u;
        int v = g->arestas[id_e].v;
        printf("  - (%s -- %s)\n", g->vertices[u].nome, g->vertices[v].nome);
    }

    printf("\n=== COMPONENTES BICONEXAS (RF07) ===\n");
    printf("Total encontradas: %d\n", res->num_biconexas);
    for (int i = 0; i < res->num_biconexas; i++) {
        if (limite_itens > 0 && i >= limite_itens) {
            printf("... (+%d componentes)\n", res->num_biconexas - i);
            break;
        }
        printf("  Biconexa %d (%d vertices): ", i + 1, res->biconexas_tamanhos[i]);
        int max_v = (limite_itens > 0 && res->biconexas_tamanhos[i] > limite_itens)
                    ? limite_itens : res->biconexas_tamanhos[i];
        for (int j = 0; j < max_v; j++) {
            printf("%s ", g->vertices[res->biconexas_vertices[i][j]].nome);
        }
        if (max_v < res->biconexas_tamanhos[i]) {
            printf("... (+%d)", res->biconexas_tamanhos[i] - max_v);
        }
        printf("\n");
    }
}
