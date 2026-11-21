#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "grafo.h"
#include "conectividade.h"
#include "comunidades.h"
#include "simulacao.h"
#include "relatorio.h"

static Grafo *carregar_csv(const char *caminho) {
    FILE *fp = fopen(caminho, "r");
    if (!fp) {
        perror("Erro ao abrir arquivo CSV");
        return NULL;
    }

    Grafo *g = grafo_criar();
    if (!g) {
        fclose(fp);
        return NULL;
    }

    char linha[256];
    char p1[MAX_NOME], p2[MAX_NOME];

    /* Pula o cabecalho (pessoa1,pessoa2) */
    if (fgets(linha, sizeof(linha), fp) == NULL) {
        fclose(fp);
        grafo_liberar(g);
        return NULL;
    }

    while (fgets(linha, sizeof(linha), fp)) {
        if (sscanf(linha, " %63[^,\r\n],%63[^,\r\n]", p1, p2) == 2) {
            grafo_adicionar_aresta(g, p1, p2);
        }
    }

    fclose(fp);
    return g;
}

static void limpar_buffer(void) {
    int c;
    while ((c = getchar()) != '\n' && c != EOF);
}

int main(void) {
    char arquivo_csv[128] = "amizades.csv";
    Grafo *g = carregar_csv(arquivo_csv);
    if (!g) {
        printf("Nao foi possivel carregar '%s'. Verifique se o arquivo esta na pasta do projeto.\n", arquivo_csv);
        return 1;
    }

    ResultadoComponentes *comp = conectividade_componentes(g);
    ResultadoTarjan *tarjan = conectividade_analisar_tarjan(g);
    ResultadoComunidades *com = NULL;

    int opcao = -1;
    while (opcao != 0) {
        printf("\n==================================================\n");
        printf("  PROJETO GRAFOS - REDES SOCIAIS (%s)\n", arquivo_csv);
        printf("==================================================\n");
        printf("  1. Resumo do grafo, graus e lista de adjacencia (RF02)\n");
        printf("  2. Componentes conexas (RF03 / RF04)\n");
        printf("  3. Articulacoes, pontes e biconexas (RF05 / RF06 / RF07)\n");
        printf("  4. Detectar comunidades - Girvan-Newman (RF08)\n");
        printf("  5. Simular remocao de um usuario (RF09)\n");
        printf("  6. Simular remocao de uma aresta (RF09)\n");
        printf("  7. Comparar remocao aleatoria vs por grau (RF09)\n");
        printf("  8. Gerar relatorio TXT e visualizacao DOT (RF10)\n");
        printf("  9. Carregar outro arquivo CSV (RF01)\n");
        printf("  0. Sair\n");
        printf("Escolha uma opcao: ");

        if (scanf("%d", &opcao) != 1) {
            limpar_buffer();
            opcao = -1;
            continue;
        }
        limpar_buffer();

        switch (opcao) {
            case 1:
                grafo_imprimir_resumo(g);
                grafo_imprimir_graus(g, 20);
                grafo_imprimir_lista_adjacencia(g, 10);
                break;

            case 2:
                conectividade_imprimir_componentes(g, comp, 20);
                break;

            case 3:
                conectividade_imprimir_tarjan(g, tarjan, 15);
                break;

            case 4: {
                int alvo, max_rem;
                printf("Numero alvo de comunidades (ex: 3): ");
                scanf("%d", &alvo);
                printf("Maximo de arestas a remover (ex: 3 p/ Facebook, 10 p/ grafo pequeno): ");
                scanf("%d", &max_rem);
                limpar_buffer();

                comunidades_liberar(com);
                com = comunidades_girvan_newman(g, alvo, max_rem);
                comunidades_imprimir(g, com, 15);
                break;
            }

            case 5: {
                char usuario[MAX_NOME];
                printf("Nome/ID do usuario a remover: ");
                scanf("%63s", usuario);
                limpar_buffer();
                simulacao_remover_vertice(g, usuario);
                break;
            }

            case 6: {
                char u[MAX_NOME], v[MAX_NOME];
                printf("Primeiro usuario da aresta: ");
                scanf("%63s", u);
                printf("Segundo usuario da aresta: ");
                scanf("%63s", v);
                limpar_buffer();
                simulacao_remover_aresta(g, u, v);
                break;
            }

            case 7: {
                int k;
                printf("Quantidade de usuarios (k) a remover na comparacao (ex: 15): ");
                scanf("%d", &k);
                limpar_buffer();
                simulacao_comparar_estrategias(g, k);
                break;
            }

            case 8: {
                int max_v;
                printf("Maximo de vertices para exportar no grafo.dot (0 = todos, ex: 50 p/ Facebook): ");
                scanf("%d", &max_v);
                limpar_buffer();

                if (relatorio_gerar_txt("relatorio_metricas.txt", g, comp, tarjan, com)) {
                    printf("Relatorio salvo em 'relatorio_metricas.txt'.\n");
                }
                if (relatorio_exportar_dot("grafo.dot", g, tarjan, com, max_v)) {
                    printf("Visualizacao salva em 'grafo.dot' (abra no Edotor ou GraphvizOnline).\n");
                }
                break;
            }

            case 9: {
                char novo_csv[128];
                printf("Nome do novo arquivo CSV (ex: exemplo.csv): ");
                scanf("%127s", novo_csv);
                limpar_buffer();

                Grafo *novo_g = carregar_csv(novo_csv);
                if (novo_g) {
                    conectividade_liberar_componentes(comp);
                    conectividade_liberar_tarjan(tarjan);
                    comunidades_liberar(com);
                    grafo_liberar(g);

                    g = novo_g;
                    strncpy(arquivo_csv, novo_csv, sizeof(arquivo_csv) - 1);
                    arquivo_csv[sizeof(arquivo_csv) - 1] = '\0';

                    comp = conectividade_componentes(g);
                    tarjan = conectividade_analisar_tarjan(g);
                    com = NULL;
                    printf("Arquivo '%s' carregado com sucesso!\n", arquivo_csv);
                }
                break;
            }

            case 0:
                printf("Encerrando...\n");
                break;

            default:
                printf("Opcao invalida.\n");
                break;
        }
    }

    conectividade_liberar_componentes(comp);
    conectividade_liberar_tarjan(tarjan);
    comunidades_liberar(com);
    grafo_liberar(g);
    return 0;
}
