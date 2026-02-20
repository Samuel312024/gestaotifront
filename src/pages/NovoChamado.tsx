import { useState } from "react";
import { abrirChamado } from "../services/chamadoService";
import { useNavigate } from "react-router-dom";

export default function NovoChamado() {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    titulo: "",
    descricao: "",
    prioridade: "Baixa"
  });

  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState("");

  function handleChange(e: any) {
    setForm({
      ...form,
      [e.target.name]: e.target.value
    });
  }

  async function handleSubmit(e: any) {
    e.preventDefault();
    setErro("");
    setLoading(true);

    try {
      await abrirChamado(form);
      navigate("/chamados");
    } catch (err: any) {
      setErro("Erro ao abrir chamado.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={styles.container}>
      <h1 style={styles.title}>Abrir Novo Chamado</h1>

      <form onSubmit={handleSubmit} style={styles.form}>
        <label>Título</label>
        <input
          name="titulo"
          value={form.titulo}
          onChange={handleChange}
          required
          style={styles.input}
        />

        <label>Descrição</label>
        <textarea
          name="descricao"
          value={form.descricao}
          onChange={handleChange}
          required
          style={styles.textarea}
        />

        <label>Prioridade</label>
        <select
          name="prioridade"
          value={form.prioridade}
          onChange={handleChange}
          style={styles.input}
        >
          <option>Baixa</option>
          <option>Média</option>
          <option>Alta</option>
          <option>Crítica</option>
        </select>

        {erro && <p style={styles.error}>{erro}</p>}

        <button type="submit" disabled={loading} style={styles.button}>
          {loading ? "Enviando..." : "Abrir Chamado"}
        </button>
      </form>
    </div>
  );
}

const styles = {
  container: {
    padding: "40px",
    maxWidth: "600px",
    margin: "0 auto"
  },
  title: {
    marginBottom: "30px",
    fontSize: "28px"
  },
  form: {
    display: "flex",
    flexDirection: "column" as const,
    gap: "15px",
    backgroundColor: "white",
    padding: "30px",
    borderRadius: "15px",
    boxShadow: "0 10px 30px rgba(0,0,0,0.08)"
  },
  input: {
    padding: "12px",
    borderRadius: "8px",
    border: "1px solid #ccc"
  },
  textarea: {
    padding: "12px",
    borderRadius: "8px",
    border: "1px solid #ccc",
    minHeight: "120px"
  },
  button: {
    marginTop: "10px",
    padding: "14px",
    borderRadius: "10px",
    border: "none",
    backgroundColor: "#2563eb",
    color: "white",
    fontWeight: 600,
    cursor: "pointer"
  },
  error: {
    color: "red"
  }
};
