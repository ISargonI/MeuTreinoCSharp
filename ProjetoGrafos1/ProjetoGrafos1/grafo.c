#include "grafo.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* Funcao de hash djb2 para strings */
static unsigned int hash_nome(const char *str) {
    unsigned long hash = 5381;
    int c;
    while ((c = (unsigned char)*str++) != 0) {
        hash = ((hash << 5) + hash) + (unsigned long)c;
    }
    return (unsigned int)(hash % HASH_TAM);
}

Grafo *grafo_criar(void) {
    Grafo *g = (Grafo *)malloc(sizeof(Grafo));
    if (!g) {
        fprintf(stderr, "Erro: falha ao alocar memoria para o grafo.\n");
        return NULL;
    }

    g->num_vertices = 0;
    g->cap_vertices = 64;
    g->vertices = (Vertice *)malloc((size_t)g->cap_vertices * sizeof(Vertice));

    g->num_arestas = 0;
    g->cap_arestas = 128;
    g->arestas = (Aresta *)malloc((size_t)g->cap_arestas * sizeof(Aresta));

    if (!g->vertices || !g->arestas) {
        free(g->vertices);
        free(g->arestas);
        free(g);
        fprintf(stderr, "Erro: falha ao alocar vetores iniciais do grafo.\n");
        return NULL;
    }

    for (int i = 0; i < HASH_TAM; i++) {
        g->tabela_hash[i] = NULL;
    }

    return g;
}

void grafo_liberar(Grafo *g) {
    if (!g) return;

    /* Libera listas de adjacencia */
    for (int i = 0; i < g->num_vertices; i++) {
        NoAdj *atual = g->vertices[i].adj;
        while (atual) {
            NoAdj *prox = atual->prox;
            free(atual);
            atual = prox;
        }
    }

    /* Libera tabela hash */
    for (int i = 0; i < HASH_TAM; i++) {
        HashNo *atual = g->tabela_hash[i];
        while (atual) {
            HashNo *prox = atual->prox;
            free(atual);
            atual = prox;
        }
    }

    free(g->vertices);
    free(g->arestas);
    free(g);
}

int grafo_buscar_vertice(const Grafo *g, const char *nome) {
    if (!g || !nome) return -1;

    unsigned int h = hash_nome(nome);
    HashNo *atual = g->tabela_hash[h];
    while (atual) {
        if (strcmp(atual->nome, nome) == 0) {
            return atual->id;
        }
        atual = atual->prox;
    }
    return -1;
}

int grafo_obter_ou_criar_vertice(Grafo *g, const char *nome) {
    if (!g || !nome || nome[0] == '\0') return -1;

    int id_existente = grafo_buscar_vertice(g, nome);
    if (id_existente != -1) {
        return id_existente;
    }

    /* Redimensiona vetor de vertices se necessario */
    if (g->num_vertices == g->cap_vertices) {
        int nova_cap = g->cap_vertices * 2;
        Vertice *novo_vet = (Vertice *)realloc(g->vertices, (size_t)nova_cap * sizeof(Vertice));
        if (!novo_vet) {
            fprintf(stderr, "Erro: falha ao redimensionar vertices.\n");
            return -1;
        }
        g->vertices = novo_vet;
        g->cap_vertices = nova_cap;
    }

    int novo_id = g->num_vertices++;
    g->vertices[novo_id].id = novo_id;
    strncpy(g->vertices[novo_id].nome, nome, MAX_NOME - 1);
    g->vertices[novo_id].nome[MAX_NOME - 1] = '\0';
    g->vertices[novo_id].ativo = true;
    g->vertices[novo_id].adj = NULL;

    /* Insere na tabela hash */
    HashNo *hno = (HashNo *)malloc(sizeof(HashNo));
    if (!hno) {
        fprintf(stderr, "Erro: falha ao alocar no da tabela hash.\n");
        return -1;
    }
    strncpy(hno->nome, nome, MAX_NOME - 1);
    hno->nome[MAX_NOME - 1] = '\0';
    hno->id = novo_id;

    unsigned int h = hash_nome(nome);
    hno->prox = g->tabela_hash[h];
    g->tabela_hash[h] = hno;

    return novo_id;
}

/* Verifica se ja existe aresta entre u e v para evitar duplicatas */
static bool aresta_existe(const Grafo *g, int u, int v) {
    NoAdj *atual = g->vertices[u].adj;
    while (atual) {
        if (atual->destino == v) {
            return true;
        }
        atual = atual->prox;
    }
    return false;
}

static bool inserir_no_adj(Grafo *g, int origem, int destino, int id_aresta) {
    NoAdj *no = (NoAdj *)malloc(sizeof(NoAdj));
    if (!no) return false;
    no->destino = destino;
    no->id_aresta = id_aresta;
    no->prox = g->vertices[origem].adj;
    g->vertices[origem].adj = no;
    return true;
}

bool grafo_adicionar_aresta(Grafo *g, const char *nome_u, const char *nome_v) {
    if (!g || !nome_u || !nome_v) return false;

    int u = grafo_obter_ou_criar_vertice(g, nome_u);
    int v = grafo_obter_ou_criar_vertice(g, nome_v);
    if (u == -1 || v == -1) return false;

    /* Ignora lacos (self-loops) e arestas duplicadas */
    if (u == v || aresta_existe(g, u, v)) {
        return false;
    }

    /* Redimensiona vetor de arestas se necessario */
    if (g->num_arestas == g->cap_arestas) {
        int nova_cap = g->cap_arestas * 2;
        Aresta *novo_vet = (Aresta *)realloc(g->arestas, (size_t)nova_cap * sizeof(Aresta));
        if (!novo_vet) {
            fprintf(stderr, "Erro: falha ao redimensionar arestas.\n");
            return false;
        }
        g->arestas = novo_vet;
        g->cap_arestas = nova_cap;
    }

    int id_aresta = g->num_arestas++;
    g->arestas[id_aresta].u = u;
    g->arestas[id_aresta].v = v;
    g->arestas[id_aresta].ativa = true;

    /* Grafo nao dirigido: insere nos dois sentidos */
    if (!inserir_no_adj(g, u, v, id_aresta) || !inserir_no_adj(g, v, u, id_aresta)) {
        fprintf(stderr, "Erro: falha ao alocar no de adjacencia.\n");
        return false;
    }

    return true;
}

int grafo_contar_vertices_ativos(const Grafo *g) {
    if (!g) return 0;
    int cont = 0;
    for (int i = 0; i < g->num_vertices; i++) {
        if (g->vertices[i].ativo) cont++;
    }
    return cont;
}

int grafo_contar_arestas_ativas(const Grafo *g) {
    if (!g) return 0;
    int cont = 0;
    for (int i = 0; i < g->num_arestas; i++) {
        int u = g->arestas[i].u;
        int v = g->arestas[i].v;
        if (g->arestas[i].ativa && g->vertices[u].ativo && g->vertices[v].ativo) {
            cont++;
        }
    }
    return cont;
}

int grafo_grau_vertice(const Grafo *g, int id_v) {
    if (!g || id_v < 0 || id_v >= g->num_vertices || !g->vertices[id_v].ativo) {
        return 0;
    }

    int grau = 0;
    NoAdj *atual = g->vertices[id_v].adj;
    while (atual) {
        if (g->arestas[atual->id_aresta].ativa && g->vertices[atual->destino].ativo) {
            grau++;
        }
        atual = atual->prox;
    }
    return grau;
}

double grafo_densidade(const Grafo *g) {
    int V = grafo_contar_vertices_ativos(g);
    if (V < 2) return 0.0;

    int E = grafo_contar_arestas_ativas(g);
    return (2.0 * (double)E) / ((double)V * (double)(V - 1));
}

void grafo_restaurar(Grafo *g) {
    if (!g) return;
    for (int i = 0; i < g->num_vertices; i++) {
        g->vertices[i].ativo = true;
    }
    for (int i = 0; i < g->num_arestas; i++) {
        g->arestas[i].ativa = true;
    }
}

void grafo_imprimir_resumo(const Grafo *g) {
    if (!g) return;
    int V = grafo_contar_vertices_ativos(g);
    int E = grafo_contar_arestas_ativas(g);
    double d = grafo_densidade(g);
    double grau_medio = (V > 0) ? (2.0 * (double)E) / (double)V : 0.0;

    printf("=== RESUMO DO GRAFO ===\n");
    printf("Vertices ativos : %d\n", V);
    printf("Arestas ativas  : %d\n", E);
    printf("Grau medio      : %.4f\n", grau_medio);
    printf("Densidade       : %.6f\n", d);
    printf("=======================\n");
}

void grafo_imprimir_graus(const Grafo *g, int limite) {
    if (!g) return;
    int total = g->num_vertices;
    int max_exibir = (limite > 0 && limite < total) ? limite : total;

    printf("\n--- Grau dos Vertices (exibindo %d de %d) ---\n", max_exibir, total);
    for (int i = 0; i < max_exibir; i++) {
        if (g->vertices[i].ativo) {
            printf("Vertice %-10s (id %d): grau = %d\n",
                   g->vertices[i].nome, i, grafo_grau_vertice(g, i));
        }
    }
}

void grafo_imprimir_lista_adjacencia(const Grafo *g, int limite) {
    if (!g) return;
    int total = g->num_vertices;
    int max_exibir = (limite > 0 && limite < total) ? limite : total;

    printf("\n--- Lista de Adjacencia (exibindo %d de %d) ---\n", max_exibir, total);
    for (int i = 0; i < max_exibir; i++) {
        if (!g->vertices[i].ativo) continue;
        printf("%s -> ", g->vertices[i].nome);
        NoAdj *atual = g->vertices[i].adj;
        while (atual) {
            if (g->arestas[atual->id_aresta].ativa && g->vertices[atual->destino].ativo) {
                printf("%s ", g->vertices[atual->destino].nome);
            }
            atual = atual->prox;
        }
        printf("\n");
    }
}
