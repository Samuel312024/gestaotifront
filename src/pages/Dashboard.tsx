import { useEffect, useState } from "react";
import { getStats } from "../services/chamadoService";
import "./dashboard.css";

export default function Dashboard() {
  const [stats, setStats] = useState<any>(null);

  useEffect(() => {
    async function load() {
      const data = await getStats();
      setStats(data);
    }
    

    load();
  }, []);

  if (!stats) return <p>Carregando...</p>;

  return (
    <div className="dashboard-container">
      <h1 className="dashboard-title">Dashboard</h1>

      <div className="cards-grid">
        <div className="card">
          <h2>{stats.total}</h2>
          <p>Chamados</p>
        </div>

        <div className="card">
          <h2>{stats.pendentes}</h2>
          <p>Pendentes</p>
        </div>

        <div className="card">
          <h2>{stats.emAtendimento}</h2>
          <p>Em Atendimento</p>
        </div>

        <div className="card">
          <h2>{stats.fechados}</h2>
          <p>Resolvidos</p>
        </div>
      </div>
    </div>
  );
}
