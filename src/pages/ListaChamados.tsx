import { useEffect, useState } from "react";
import {
  listarChamados,
  assumirChamado,
  fecharChamado,
} from "../services/chamadoService";
import { getUserRole } from "../utils/auth";
import { useNavigate } from "react-router-dom";


export default function ListaChamados() {
  const [chamados, setChamados] = useState<any[]>([]);
  const role = getUserRole(); // 🔥 pegamos a role

  async function load() {
    const data = await listarChamados();
    setChamados(data);
  }

  const navigate = useNavigate();


  useEffect(() => {
    load();
  }, []);

  async function handleAssumir(id: number) {
    try {
      await assumirChamado(id);
      load();
    } catch (error: any) {
      alert(error.message);
    }
  }

  async function handleFechar(id: number) {
    try {
      await fecharChamado(id);
      load();
    } catch (error: any) {
      alert(error.message);
    }
  }

  return (
    <div style={{ padding: 40 }}>
      <h1>Lista de Chamados</h1>
      <button
      onClick={() => navigate("/chamados/novo")}
      style={{ marginBottom: 20 }}
    >
    + Novo Chamado
  </button>
      {chamados.map((c) => (
        <div key={c.id} className="card" style={{ marginBottom: 20 }}>
          <h3>{c.titulo}</h3>
          <p>{c.descricao}</p>
          <p>Status: {c.status}</p>

          {/* 👨‍🔧 Técnico ou Admin podem assumir */}
          {(role === "Tecnico" || role === "Admin") && (
            <button onClick={() => handleAssumir(c.id)}>
              Assumir
            </button>
          )}

          {/* 👑 Apenas Admin pode fechar */}
          {role === "Admin" && (
            <button onClick={() => handleFechar(c.id)}>
              Fechar
            </button>
          )}
        </div>
      ))}
    </div>
  );
}
