#ifndef GRAFO_H
#define GRAFO_H

#include <stdbool.h>

#define MAX_NOME 64
#define HASH_TAM 16381 /* Numero primo para boa distribuicao na tabela hash */

/* No da lista de adjacencia */
typedef struct NoAdj {
    int destino;        /* Indice do vertice vizinho */
    int id_aresta;      /* Indice da aresta no vetor global de arestas */
    struct NoAdj *prox;
} NoAdj;

/* Estrutura de um vertice (usuario) */
typedef struct {
    int id;
    char nome[MAX_NOME];
    bool ativo;         /* Usado na simulacao de remocoes (RF09) */
    NoAdj *adj;
} Vertice;

/* Estrutura de uma aresta nao dirigida (amizade) */
typedef struct {
    int u;
    int v;
    bool ativa;         /* Usado em Girvan-Newman (RF08) e simulacao (RF09) */
} Aresta;

/* Entrada da tabela hash para mapear nome -> id em O(1) medio */
typedef struct HashNo {
    char nome[MAX_NOME];
    int id;
    struct HashNo *prox;
} HashNo;

/* Estrutura principal do Grafo */
typedef struct {
    Vertice *vertices;
    int num_vertices;
    int cap_vertices;

    Aresta *arestas;
    int num_arestas;
    int cap_arestas;

    HashNo *tabela_hash[HASH_TAM];
} Grafo;

/* Criacao e destruicao */
Grafo *grafo_criar(void);
void grafo_liberar(Grafo *g);

/* Manipulacao de vertices e arestas */
int grafo_buscar_vertice(const Grafo *g, const char *nome);
int grafo_obter_ou_criar_vertice(Grafo *g, const char *nome);
bool grafo_adicionar_aresta(Grafo *g, const char *nome_u, const char *nome_v);

/* Metricas basicas (RF02 / Etapa 1) */
int grafo_contar_vertices_ativos(const Grafo *g);
int grafo_contar_arestas_ativas(const Grafo *g);
int grafo_grau_vertice(const Grafo *g, int id_v);
double grafo_densidade(const Grafo *g);

/* Utilitarios para simulacao e visualizacao */
void grafo_restaurar(Grafo *g);
void grafo_imprimir_resumo(const Grafo *g);
void grafo_imprimir_graus(const Grafo *g, int limite);
void grafo_imprimir_lista_adjacencia(const Grafo *g, int limite);

#endif /* GRAFO_H */
