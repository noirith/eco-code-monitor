# 🌱 EcoCode Monitor

Lightweight platform for estimating and visualizing the environmental impact of software systems based on computational resource consumption.

EcoCode Monitor was created to explore concepts related to sustainable software engineering, observability and carbon-aware development, helping developers understand how infrastructure and code efficiency can affect energy consumption and estimated CO₂ emissions.

<img width="1918" height="1056" alt="image" src="https://github.com/user-attachments/assets/25e88ae5-83f9-4478-9b11-3fd423c247b9" />

---

## ✨ Features

- Real-time environmental metrics monitoring
- Estimated CO₂ emission calculation
- Endpoint and application ranking
- Dashboard visualization
- Historical metrics persistence
- Energy efficiency scoring system
- Comparative analysis between implementations
- REST API for metric ingestion

---

## 🛠️ Tech Stack

### Backend
- ASP.NET Core 8
- MongoDB
- Swagger / OpenAPI

### Dashboard
- Electron
- HTML5 / CSS3 / JavaScript
- Chart.js

### Infrastructure
- Docker
- Docker Compose

---

## 🚀 Running Locally

### Prerequisites

- .NET 8 SDK
- Docker
- Node.js 20+
- Git

---

## 📦 Clone Repository

```bash
git clone https://github.com/noirith/eco-code-monitor.git
cd eco-code-monitor
```

---

## ⚙️ Start MongoDB

```bash
docker run -d \
  --name mongodb \
  -p 27017:27017 \
  -e MONGO_INITDB_ROOT_USERNAME=admin \
  -e MONGO_INITDB_ROOT_PASSWORD=password123 \
  -v mongodb_data:/data/db \
  mongo:7.0
```

---

## 🖥️ Run Backend API

```bash
cd EcoCodeMonitorAPI
dotnet restore
dotnet run
```

API should be available at:

```txt
http://localhost:5000
```

Swagger:

```txt
http://localhost:5000/swagger
```

---

## 📊 Run Dashboard

```bash
cd ElectronDashboard
npm install
npm start
```

---

## 📡 Example Payload

```json
{
  "applicationName": "OrdersAPI",
  "endpoint": "/api/orders",
  "environment": "production",
  "cpuUsagePercent": 45.0,
  "memoryUsageMB": 512,
  "durationMs": 200,
  "requestCount": 100,
  "operationType": "Processing"
}
```

---

## 🌍 Environmental Context

EcoCode Monitor uses concepts inspired by the SCI (Software Carbon Intensity) specification from the Green Software Foundation.

The project focuses on educational and comparative analysis, allowing developers to visualize how software optimization can reduce estimated environmental impact.

Examples of analysis:
- CPU consumption
- Memory usage
- Request duration
- Request volume
- Relative efficiency between implementations

---

## 🧠 Project Goals

This project was created to study and experiment with:

- Sustainable software engineering
- Green IT concepts
- Carbon-aware development
- Observability
- Infrastructure monitoring
- Environmental metrics visualization
- Performance optimization
- Distributed systems telemetry

---

## 📌 Roadmap

- [ ] Error and anomaly detection
- [ ] Real-time metrics streaming
- [ ] CI/CD integration
- [ ] Dockerized full stack
- [ ] Carbon footprint reports
- [ ] Cloud provider comparison
- [ ] Grafana integration
- [ ] Prometheus exporter
- [ ] Authentication and multi-tenant support
- [ ] Advanced dashboard analytics
- [ ] AI-assisted optimization suggestions

---

## 📁 Project Structure

```txt
eco-code-monitor/
├── EcoCodeMonitorAPI/
├── ElectronDashboard/
├── docs/
├── tests/
├── docker-compose.yml
└── README.md
```

---

## 📊 Scientific References

- Green Software Foundation — SCI Specification
- ONS (Brazilian National Electric System Operator)
- Uptime Institute — Global Data Center Survey
- Research on energy-efficient software engineering

---

## 🚧 Project Status

Work in progress.

This repository is part of my studies and experiments involving sustainable software engineering, observability and infrastructure efficiency.

---

## 📄 License

MIT
