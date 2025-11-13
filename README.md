# 🌱 EcoCode Monitor

<img width="1914" height="1060" alt="image" src="https://github.com/user-attachments/assets/d358c64d-1dfd-4c77-9035-006954312f97" />


> **Sistema de Monitoramento de Impacto Ambiental de Aplicações**  
> Meça, visualize e otimize as emissões de CO₂ do seu código em tempo real.

![License](https://img.shields.io/badge/license-MIT-green)
![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![MongoDB](https://img.shields.io/badge/MongoDB-7.0-green)
![Node.js](https://img.shields.io/badge/Node.js-20.x-green)

---

## 📖 Sobre o Projeto

O **EcoCode Monitor** é um sistema de monitoramento que calcula automaticamente o impacto ambiental de aplicações de software, medindo o consumo de recursos computacionais e convertendo-os em emissões de CO₂.

### 🎯 Objetivo

Conscientizar desenvolvedores sobre o impacto ambiental do código que escrevem, permitindo:
- ✅ Visualizar emissões de CO₂ em tempo real
- ✅ Identificar endpoints e operações mais poluentes
- ✅ Medir o impacto de otimizações de código
- ✅ Rankear aplicações por impacto ambiental

### 🌍 Contexto Ambiental

O Brasil possui uma matriz elétrica limpa (83% renovável), com fator de emissão de **0.0385 kg CO₂/kWh** (ONS, 2023). Mesmo assim, a eficiência energética em software é fundamental para:
- Reduzir custos operacionais
- Diminuir pegada de carbono global
- Liberar recursos computacionais
- Promover desenvolvimento sustentável

---

## 🏗️ Arquitetura
```
┌─────────────────────────────────────────────────────────┐
│                    APLICAÇÕES CLIENTES                   │
│          (C#, Node.js, Python, Java, etc.)              │
└────────────────────┬────────────────────────────────────┘
                     │ HTTP POST
                     ▼
┌─────────────────────────────────────────────────────────┐
│                   API REST (.NET 8)                      │
│  ┌─────────────────────────────────────────────────┐   │
│  │  Controllers → Services → Repository             │   │
│  │  • Recebe métricas                               │   │
│  │  • Calcula CO₂ e Score                          │   │
│  │  • Persiste no banco                             │   │
│  └─────────────────────────────────────────────────┘   │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                  MongoDB 7.0                             │
│         (Armazenamento de métricas)                      │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│              DASHBOARD (Electron + HTML5)                │
│  • Visualização em tempo real                           │
│  • Gráficos e rankings                                  │
│  • Auto-refresh (10s)                                   │
└─────────────────────────────────────────────────────────┘
```

---

## ⚙️ Tecnologias Utilizadas

### Backend
- **C# / ASP.NET Core 8.0** - API REST
- **MongoDB 7.0** - Banco de dados NoSQL
- **Swagger/OpenAPI** - Documentação da API

### Frontend
- **Electron 28** - Desktop application
- **HTML5/CSS3/JavaScript** - Interface responsiva
- **Chart.js** - Visualizações e gráficos

### DevOps
- **Docker** - Containerização do MongoDB
- **Git** - Controle de versão

---

## 📊 Metodologia de Cálculo

### Base Científica

O sistema utiliza a metodologia **SCI (Software Carbon Intensity)** da [Green Software Foundation](https://greensoftware.foundation/) como base teórica.

### Fórmula de Cálculo
```
1. Índice de Impacto = (CPU% / 10) × (Duração ms / 100) × (Memória MB / 1000)

2. CO₂ por requisição = Índice × 0.001 (fator de conversão calibrado)

3. CO₂ Total = CO₂ por req × Número de requisições

4. Energia (Wh) = CO₂ Total (g) / 0.0385 (fator de emissão Brasil/ONS)

5. Score (A-E):
   A: < 1mg CO₂/req    (Excelente)
   B: 1-10mg           (Bom)
   C: 10-100mg         (Regular)
   D: 100mg-1g         (Ruim)
   E: > 1g             (Crítico)
```

### Fontes Oficiais

- **Fator de Emissão**: ONS (Operador Nacional do Sistema Elétrico) - 2023
- **Metodologia**: Green Software Foundation - SCI Specification
- **PUE**: Uptime Institute - Global Data Center Survey

### Limitações

⚠️ **Importante**: Este sistema prioriza **comparação relativa** entre implementações. Os valores absolutos são estimativas educacionais e não substituem ferramentas profissionais como Intel RAPL ou Cloud Carbon Footprint.

---

## 🚀 Instalação

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Node.js 20+](https://nodejs.org/)
- [Git](https://git-scm.com/)

### 1️⃣ Clonar o Repositório
```bash
git clone https://github.com/seu-usuario/ecocode-monitor.git
cd ecocode-monitor
```

### 2️⃣ Iniciar MongoDB
```bash
docker run -d \
  --name mongodb \
  -p 27017:27017 \
  -e MONGO_INITDB_ROOT_USERNAME=admin \
  -e MONGO_INITDB_ROOT_PASSWORD=password123 \
  -v mongodb_data:/data/db \
  mongo:7.0
```

### 3️⃣ Iniciar a API
```bash
cd ErroLoggerAPI
dotnet restore
dotnet run
```

API disponível em: `http://localhost:5000`  
Swagger: `http://localhost:5000/swagger`

### 4️⃣ Iniciar o Dashboard
```bash
cd ElectronDashboard
npm install
npm start
```

---

## 📱 Uso Básico

### Enviar Métrica (cURL)
```bash
curl -X POST http://localhost:5000/api/MetricasAmbientais \
  -H "Content-Type: application/json" \
  -d '{
    "aplicacaoNome": "MeuSistema",
    "endpoint": "/api/vendas",
    "ambiente": "production",
    "cpuUsagePercent": 45.0,
    "memoriaUsadaMB": 512,
    "duracaoMs": 200,
    "numeroRequisicoes": 100,
    "tipoOperacao": "Processing"
  }'
```

### Cliente C# Simples
```csharp
using System.Net.Http;
using System.Net.Http.Json;

var client = new HttpClient();

var metrica = new
{
    aplicacaoNome = "MeuSistema",
    endpoint = "/api/vendas",
    ambiente = "production",
    cpuUsagePercent = 45.0,
    memoriaUsadaMB = 512,
    duracaoMs = 200,
    numeroRequisicoes = 100,
    tipoOperacao = "Processing"
};

await client.PostAsJsonAsync(
    "http://localhost:5000/api/MetricasAmbientais", 
    metrica
);
```

---

## 📊 Endpoints da API

### Métricas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/MetricasAmbientais` | Registra nova métrica |
| `GET` | `/api/MetricasAmbientais` | Lista métricas (paginado) |
| `GET` | `/api/MetricasAmbientais/{id}` | Busca métrica por ID |
| `GET` | `/api/MetricasAmbientais/ranking` | Ranking de endpoints |
| `GET` | `/api/MetricasAmbientais/relatorio` | Relatório consolidado |
| `DELETE` | `/api/MetricasAmbientais/{id}` | Deleta métrica |

### Filtros Disponíveis
```
?pagina=1
&tamanhoPagina=20
&aplicacao=NomeApp
&ambiente=production
&scoreMinimo=C
&dataInicio=2024-01-01
&dataFim=2024-12-31
```

---

## 📊 Dashboard

### Funcionalidades

✅ **Visualização em Tempo Real**
- Tabela de métricas com auto-refresh (10s)
- Cards de resumo (Energia, CO₂, Score médio)
- Badges coloridos por score (A-E)

✅ **Ranking**
- Top endpoints mais poluentes
- Visualização por aplicação
- Gráficos de barras

✅ **Filtros Avançados**
- Por aplicação
- Por ambiente
- Por score mínimo
- Por período

---

## 🧪 Casos de Teste

### Cenário: Evolução de Otimizações

Demonstração de como otimizações progressivas reduzem emissões:

| Versão | CPU | Memória | Score | Redução CO₂ |
|--------|-----|---------|-------|-------------|
| v1.0 (Legado) | 85% | 4GB | E 🔴 | baseline |
| v2.0 (Queries otimizadas) | 65% | 2GB | D 🟠 | ↓ 50% |
| v3.0 (Com cache) | 45% | 1GB | C 🟡 | ↓ 70% |
| v4.0 (Async) | 30% | 512MB | B 🟢 | ↓ 85% |
| v5.0 (Otimizado) | 10% | 256MB | A 🟢 | ↓ 95% |

**Scripts de teste disponíveis em:** `/tests/cenarios-teste.json`

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Fork o projeto
2. Crie uma branch: `git checkout -b feature/nova-funcionalidade`
3. Commit suas mudanças: `git commit -m 'Adiciona nova funcionalidade'`
4. Push para a branch: `git push origin feature/nova-funcionalidade`
5. Abra um Pull Request

---

## 📚 Referências

### Artigos Acadêmicos

1. **Green Software Foundation** (2023). Software Carbon Intensity (SCI) Specification.  
   https://github.com/Green-Software-Foundation/sci

2. **Pereira, R., et al.** (2017). Energy efficiency across programming languages.  
   ACM SIGPLAN International Conference on Software Language Engineering.

3. **Hähnel, M., et al.** (2012). Measuring energy consumption for short code paths using RAPL.  
   ACM SIGMETRICS Performance Evaluation Review.

### Fontes Oficiais

- **ONS** - Fator de Emissão de CO₂ do SIN (2023): 0.0385 tCO₂/MWh
- **EPE** - Balanço Energético Nacional 2023
- **Uptime Institute** - Global Data Center Survey

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👨‍💻 Autor

**Lucas** - Atividade Prática Supervisionada (APS)  
Curso: Ciência da Computação  
Tema: Monitoramento de Impacto Ambiental de Software

---

## 🌟 Agradecimentos

- Green Software Foundation pela metodologia SCI
- ONS pelos dados oficiais de emissão
- Comunidade open source

---

## 📞 Contato

- GitHub: [@noirith]([https://github.com/seu-usuario](https://github.com/noirith))

---

<p align="center">
  <strong>🌱 Código Eficiente é Código Sustentável 🌱</strong>
</p>
