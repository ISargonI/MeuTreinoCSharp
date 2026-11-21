#include "relatorio.h"
#include <stdio.h>
#include <stdlib.h>

static bool eh_ponte(const ResultadoTarjan *tarjan, int id_aresta) {
    if (!tarjan) return false;
    for (int i = 0; i < tarjan->num_pontes; i++) {
        if (tarjan->pontes_id[i] == id_aresta) return true;
    }
    return false;
}

bool relatorio_gerar_txt(const char *caminho_saida,
                         const Grafo *g,
                         const ResultadoComponentes *comp,
                         const ResultadoTarjan *tarjan,
                         const ResultadoComunidades *com) {
    if (!g || !caminho_saida) return false;

    FILE *fp = fopen(caminho_saida, "w");
    if (!fp) {
        perror("Erro ao criar arquivo de relatorio");
        return false;
    }

    int V = grafo_contar_vertices_ativos(g);
    int E = grafo_contar_arestas_ativas(g);
    double grau_medio = (V > 0) ? (2.0 * (double)E) / (double)V : 0.0;

    fprintf(fp, "============================================================\n");
    fprintf(fp, "   RELATORIO TECNICO - ANALISE DE REDES SOCIAIS EM GRAFOS   \n");
    fprintf(fp, "============================================================\n\n");

    fprintf(fp, "1. METRICAS GERAIS (RF02)\n");
    fprintf(fp, "   - Vertices ativos : %d\n", V);
    fprintf(fp, "   - Arestas ativas  : %d\n", E);
    fprintf(fp, "   - Grau medio      : %.4f\n", grau_medio);
    fprintf(fp, "   - Densidade       : %.6f\n\n", grafo_densidade(g));

    if (comp) {
        fprintf(fp, "2. CONECTIVIDADE E COMPONENTES CONEXAS (RF03 / RF04)\n");
        fprintf(fp, "   - Grafo conexo?        : %s\n", (comp->num_componentes == 1) ? "SIM" : "NAO");
        fprintf(fp, "   - Total de componentes : %d\n", comp->num_componentes);
        fprintf(fp, "   - Maior componente     : %d vertices\n\n", comp->maior_componente);
    }

    if (tarjan) {
        fprintf(fp, "3. PONTOS CRITICOS E BICONECTIVIDADE (RF05 / RF06 / RF07)\n");
        fprintf(fp, "   - Vertices de articulacao : %d\n", tarjan->num_articulacoes);
        for (int i = 0; i < g->num_vertices; i++) {
            if (tarjan->eh_articulacao[i]) {
                fprintf(fp, "       * Usuario %s (grau %d)\n",
                        g->vertices[i].nome, grafo_grau_vertice(g, i));
            }
        }

        fprintf(fp, "\n   - Pontes (conexoes criticas): %d\n", tarjan->num_pontes);
        int lim_pontes = (tarjan->num_pontes > 30) ? 30 : tarjan->num_pontes;
        for (int i = 0; i < lim_pontes; i++) {
            int id_e = tarjan->pontes_id[i];
            fprintf(fp, "       * (%s -- %s)\n",
                    g->vertices[g->arestas[id_e].u].nome,
                    g->vertices[g->arestas[id_e].v].nome);
        }
        if (tarjan->num_pontes > lim_pontes) {
            fprintf(fp, "       ... (+%d pontes omitidas)\n", tarjan->num_pontes - lim_pontes);
        }

        fprintf(fp, "\n   - Componentes biconexas : %d\n\n", tarjan->num_biconexas);
    }

    if (com) {
        fprintf(fp, "4. DETECCAO DE COMUNIDADES - GIRVAN-NEWMAN (RF08)\n");
        fprintf(fp, "   - Comunidades detectadas : %d\n", com->num_comunidades);
        fprintf(fp, "   - Arestas removidas      : %d\n", com->arestas_removidas);
        fprintf(fp, "   - Modularidade (Q)       : %.4f\n", com->modularidade);
        for (int c = 0; c < com->num_comunidades; c++) {
            fprintf(fp, "       * Comunidade %d: %d vertices\n", c + 1, com->tamanho_comunidade[c]);
        }
        fprintf(fp, "\n");
    } else {
        fprintf(fp, "4. DETECCAO DE COMUNIDADES - GIRVAN-NEWMAN (RF08)\n");
        fprintf(fp, "   - Ainda nao executado nesta sessao.\n\n");
    }

    fclose(fp);
    return true;
}

bool relatorio_exportar_dot(const char *caminho_saida,
                            const Grafo *g,
                            const ResultadoTarjan *tarjan,
                            const ResultadoComunidades *com,
                            int max_vertices) {
    if (!g || !caminho_saida) return false;

    FILE *fp = fopen(caminho_saida, "w");
    if (!fp) {
        perror("Erro ao criar arquivo .dot");
        return false;
    }

    const char *cores[] = {
        "#AED6F1", "#A9DFBF", "#F9E79F", "#F5B7B1",
        "#D2B4DE", "#FAD7A0", "#A3E4D7", "#D5DBDB"
    };
    int num_cores = 8;

    int limite = (max_vertices > 0 && max_vertices < g->num_vertices) ? max_vertices : g->num_vertices;

    fprintf(fp, "graph RedeSocial {\n");
    fprintf(fp, "    layout=neato;\n");
    fprintf(fp, "    overlap=false;\n");
    fprintf(fp, "    node [shape=circle, style=filled, fontname=\"Arial\"];\n\n");

    /* Exporta vertices */
    for (int i = 0; i < limite; i++) {
        if (!g->vertices[i].ativo) continue;

        const char *cor = "#AED6F1";
        if (com && com->comunidade_id[i] >= 0) {
            cor = cores[com->comunidade_id[i] % num_cores];
        }

        if (tarjan && tarjan->eh_articulacao[i]) {
            fprintf(fp, "    \"%s\" [fillcolor=\"%s\", color=\"red\", penwidth=2.5];\n",
                    g->vertices[i].nome, cor);
        } else {
            fprintf(fp, "    \"%s\" [fillcolor=\"%s\"];\n",
                    g->vertices[i].nome, cor);
        }
    }

    fprintf(fp, "\n");

    /* Exporta arestas entre os vertices dentro do limite */
    for (int i = 0; i < g->num_arestas; i++) {
        if (!g->arestas[i].ativa) continue;
        int u = g->arestas[i].u;
        int v = g->arestas[i].v;
        if (u >= limite || v >= limite) continue;
        if (!g->vertices[u].ativo || !g->vertices[v].ativo) continue;

        if (eh_ponte(tarjan, i)) {
            fprintf(fp, "    \"%s\" -- \"%s\" [color=\"red\", style=\"dashed\", penwidth=2.0];\n",
                    g->vertices[u].nome, g->vertices[v].nome);
        } else {
            fprintf(fp, "    \"%s\" -- \"%s\";\n",
                    g->vertices[u].nome, g->vertices[v].nome);
        }
    }

    fprintf(fp, "}\n");
    fclose(fp);
    return true;
}
