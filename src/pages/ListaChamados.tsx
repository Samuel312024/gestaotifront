import { useEffect, useState } from "react";
import {
  listarChamados,
  assumirChamado,
  fecharChamado,
} from "../services/chamadoService";

export default function ListaChamados() {
  const [chamados, setChamados] = useState<any[]>([]);

  async function load() {
    const data = await listarChamados();
    setChamados(data);
  }

  useEffect(() => {
    load();
  }, []);

  async function handleAssumir(id: number) {
    await assumirChamado(id);
    load();
  }

  async function handleFechar(id: number) {
    await fecharChamado(id);
    load();
  }

  return (
    <div style={{ padding: 40 }}>
      <h1>Lista de Chamados</h1>

      {chamados.map((c) => (
        <div key={c.id} className="card" style={{ marginBottom: 20 }}>
          <h3>{c.titulo}</h3>
          <p>{c.descricao}</p>
          <p>Status: {c.status}</p>

          <button onClick={() => handleAssumir(c.id)}>
            Assumir
          </button>

          <button onClick={() => handleFechar(c.id)}>
            Fechar
          </button>
        </div>
      ))}
    </div>
  );
}
