import { useEffect, useState } from "react";
import { meusChamados } from "../services/chamadoService";

export default function MeusChamados() {
  const [chamados, setChamados] = useState<any[]>([]);

  useEffect(() => {
    async function load() {
      const data = await meusChamados();
      setChamados(data);
    }

    load();
  }, []);

  return (
    <div style={{ padding: 40 }}>
      <h1>Meus Chamados</h1>

      {chamados.map((c) => (
        <div key={c.id} className="card" style={{ marginBottom: 20 }}>
          <h3>{c.titulo}</h3>
          <p>{c.descricao}</p>
          <p>Status: {c.status}</p>
        </div>
      ))}
    </div>
  );
}
