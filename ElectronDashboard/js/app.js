/**
 * EcoCode Monitor Dashboard
 * Sistema de monitoramento de impacto ambiental de aplicações
 */

// Configurações globais
const CONFIG = {
    API_URL: 'http://localhost:5000/api/MetricasAmbientais',
    ITENS_POR_PAGINA: 10,
    AUTO_REFRESH_INTERVAL: 10000 // 10 segundos
};

// Estado da aplicação
const State = {
    paginaAtual: 1,
    filtrosAtivos: {},
    autoRefreshInterval: null
};

// Classe principal do Dashboard
class DashboardController {
    constructor() {
        console.log('🌱 Inicializando EcoCode Monitor Dashboard...');
        this.init();
    }

    // Inicialização
    init() {
        console.log('📡 API URL:', CONFIG.API_URL);

        // Carregar dados iniciais
        this.carregarMetricas();
        this.carregarAplicacoes();
        this.carregarResumo();

        // Iniciar auto-refresh
        this.iniciarAutoRefresh();

        console.log('✅ Dashboard inicializado com sucesso!');
    }

    // ========== AUTO-REFRESH ==========
    iniciarAutoRefresh() {
        State.autoRefreshInterval = setInterval(() => {
            console.log('🔄 Auto-refresh executado em', new Date().toLocaleTimeString());

            // Atualizar resumo
            this.carregarResumo();

            // Atualizar a tab ativa
            const tabAtiva = document.querySelector('.tab-content.active');
            if (tabAtiva && tabAtiva.id === 'tab-metricas') {
                this.carregarMetricas();
            } else if (tabAtiva && tabAtiva.id === 'tab-ranking') {
                this.carregarRanking();
            }
        }, CONFIG.AUTO_REFRESH_INTERVAL);

        console.log(`✅ Auto-refresh ativado (${CONFIG.AUTO_REFRESH_INTERVAL/1000} segundos)`);
    }

    // ========== FILTROS ==========
    toggleFiltros() {
        const filtros = document.getElementById('filtros');
        filtros.style.display = filtros.style.display === 'none' ? 'grid' : 'none';
    }

    async carregarAplicacoes() {
        try {
            const response = await fetch(`${CONFIG.API_URL}/aplicacoes`);

            if (!response.ok) {
                console.error('Erro ao carregar aplicações:', response.status);
                return;
            }

            const aplicacoes = await response.json();
            console.log('📱 Aplicações carregadas:', aplicacoes);

            const select = document.getElementById('filtro-aplicacao');

            // Limpar opções existentes (exceto "Todas")
            while (select.options.length > 1) {
                select.remove(1);
            }

            // Adicionar cada aplicação
            aplicacoes.forEach(app => {
                const option = document.createElement('option');
                option.value = app;
                option.textContent = app;
                select.appendChild(option);
            });

        } catch (error) {
            console.error('❌ Erro ao carregar aplicações:', error);
        }
    }

    aplicarFiltros() {
        State.filtrosAtivos = {
            aplicacao: document.getElementById('filtro-aplicacao').value,
            ambiente: document.getElementById('filtro-ambiente').value,
            dataInicio: document.getElementById('filtro-data-inicio').value,
            dataFim: document.getElementById('filtro-data-fim').value
        };
        State.paginaAtual = 1;
        this.carregarMetricas();
    }

    // ========== MÉTRICAS ==========
    async carregarMetricas() {
        const tabela = document.getElementById('metricasTabela');
        tabela.innerHTML = '<tr><td colspan="11" class="loading">Carregando dados</td></tr>';

        try {
            const url = this.construirUrlMetricas();
            console.log('📊 Carregando métricas de:', url);

            const response = await fetch(url);

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            const metricas = await response.json();
            console.log('✅ Métricas carregadas:', metricas.length);

            this.renderizarTabela(metricas);
            this.atualizarPaginacao(metricas.length);

        } catch (error) {
            console.error('❌ Erro ao carregar métricas:', error);
            tabela.innerHTML = '<tr><td colspan="11" style="text-align: center; color: #ef4444;">Erro ao carregar dados. Verifique se a API está rodando.</td></tr>';
        }
    }

    construirUrlMetricas() {
        let url = `${CONFIG.API_URL}?pagina=${State.paginaAtual}&tamanhoPagina=${CONFIG.ITENS_POR_PAGINA}`;

        if (State.filtrosAtivos.aplicacao)
            url += `&aplicacao=${encodeURIComponent(State.filtrosAtivos.aplicacao)}`;
        if (State.filtrosAtivos.ambiente)
            url += `&ambiente=${encodeURIComponent(State.filtrosAtivos.ambiente)}`;
        if (State.filtrosAtivos.dataInicio)
            url += `&dataInicio=${State.filtrosAtivos.dataInicio}`;
        if (State.filtrosAtivos.dataFim)
            url += `&dataFim=${State.filtrosAtivos.dataFim}`;

        return url;
    }

    renderizarTabela(metricas) {
        const tabela = document.getElementById('metricasTabela');

        if (!metricas || metricas.length === 0) {
            tabela.innerHTML = '<tr><td colspan="11" style="text-align: center;">Nenhuma métrica encontrada</td></tr>';
            return;
        }

        tabela.innerHTML = metricas.map(m => {
            // ========== FORMATAR CO₂ ==========
            const co2 = m.emissaoCO2Gramas || 0;
            let co2Formatado;

            if (co2 < 0.01) {
                // Menos de 0.01g → mostra em miligramas
                co2Formatado = `${(co2 * 1000).toFixed(1)} mg`;
            } else if (co2 < 1) {
                // Entre 0.01g e 1g → 2 casas decimais
                co2Formatado = `${co2.toFixed(2)} g`;
            } else {
                // Mais de 1g → 1 casa decimal
                co2Formatado = `${co2.toFixed(1)} g`;
            }

            // ========== FORMATAR ENERGIA ==========
            const energia = m.energiaConsumidaWh || 0;
            let energiaFormatada;

            if (energia < 1) {
                // Menos de 1Wh → mostra em miliWatt-hora (mWh)
                energiaFormatada = `${(energia * 1000).toFixed(1)} mWh`;
            } else {
                // Mais de 1Wh → 2 casas decimais
                energiaFormatada = `${energia.toFixed(2)} Wh`;
            }

            return `
            <tr>
                <td>${m.aplicacaoNome || 'N/A'}</td>
                <td>${m.endpoint || 'N/A'}</td>
                <td>${m.ambiente || 'N/A'}</td>
                <td>${(m.cpuUsagePercent || 0).toFixed(2)}%</td>
                <td>${m.memoriaUsadaMB || 0}</td>
                <td>${m.duracaoMs || 0}</td>
                <td>${m.numeroRequisicoes || 0}</td>
                <td>${energiaFormatada}</td>
                <td>${co2Formatado}</td>
                <td><span class="score-badge score-${m.carbonScore || 'C'}">${m.carbonScore || 'N/A'}</span></td>
                <td>${m.dataHora || 'N/A'}</td>
            </tr>
        `;
        }).join('');
    }

    // ========== PAGINAÇÃO ==========
    atualizarPaginacao(qtdItens) {
        const btnAnterior = document.getElementById('btn-anterior');
        const btnProxima = document.getElementById('btn-proxima');
        const paginaInfo = document.getElementById('paginacao-info');

        if (btnAnterior) btnAnterior.disabled = State.paginaAtual === 1;
        if (btnProxima) btnProxima.disabled = qtdItens < CONFIG.ITENS_POR_PAGINA;
        if (paginaInfo) paginaInfo.textContent = `Página ${State.paginaAtual}`;
    }

    paginaAnterior() {
        if (State.paginaAtual > 1) {
            State.paginaAtual--;
            this.carregarMetricas();
        }
    }

    proximaPagina() {
        State.paginaAtual++;
        this.carregarMetricas();
    }

    // ========== RESUMO ==========
    async carregarResumo() {
        try {
            const url = `${CONFIG.API_URL}/relatorio`;
            console.log('📈 Carregando relatório de:', url);

            const response = await fetch(url);

            if (!response.ok) {
                console.error('Erro ao carregar resumo:', response.status);
                return;
            }

            const relatorio = await response.json();
            console.log('✅ Relatório carregado:', relatorio);

            // Atualizar cards com verificação de valores
            const energia = document.getElementById('resumo-energia');
            const co2 = document.getElementById('resumo-co2');
            const arvores = document.getElementById('resumo-arvores');
            const requisicoes = document.getElementById('resumo-requisicoes');
            const registros = document.getElementById('resumo-registros');
            const score = document.getElementById('resumo-score');
            const endpointPior = document.getElementById('resumo-endpoint-pior');

            if (energia) energia.textContent = (relatorio.totalEnergiaKWh || 0).toFixed(4) + ' kWh';
            if (co2) co2.textContent = (relatorio.totalCO2Kg || 0).toFixed(4) + ' kg';
            if (arvores) arvores.textContent = `${(relatorio.equivalenciaArvoresDias || 0).toFixed(4)} árvores/dia necessárias`;
            if (requisicoes) requisicoes.textContent = (relatorio.totalRequisicoes || 0).toLocaleString('pt-BR');
            if (registros) registros.textContent = `${relatorio.totalRegistros || 0} registros`;
            if (score) score.textContent = (relatorio.mediaScore || 0).toFixed(2);
            if (endpointPior) endpointPior.textContent = `Pior: ${relatorio.endpointMaisPoluente || 'N/A'}`;

        } catch (error) {
            console.error('❌ Erro ao carregar resumo:', error);
        }
    }
    
}

// Criar instância global do Dashboard
let Dashboard;

// Inicializar quando o DOM estiver pronto
document.addEventListener('DOMContentLoaded', () => {
    console.log('🚀 DOM Loaded - Criando Dashboard...');
    Dashboard = new DashboardController();
});

// Fallback: se DOMContentLoaded já passou
if (document.readyState === 'loading') {
    console.log('⏳ Aguardando DOM...');
} else {
    console.log('🚀 DOM já carregado - Criando Dashboard...');
    Dashboard = new DashboardController();
}