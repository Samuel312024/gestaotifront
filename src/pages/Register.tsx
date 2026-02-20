import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const [nome, setNome] = useState("");
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const navigate = useNavigate();

  async function handleRegister(e: React.FormEvent) {
    e.preventDefault();

    try {
      const response = await fetch(
        "https://localhost:44367/api/auth/register",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            nome,
            email,
            senha,
            role: "User"
          }),
        }
      );

      if (!response.ok) {
        throw new Error("Erro ao cadastrar");
      }

      alert("Cadastro realizado com sucesso!");
      navigate("/"); // volta para login
    } catch (error) {
      alert("Erro ao criar conta");
    }
  }

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <h2 style={styles.title}>Criar Conta</h2>
        <p style={styles.subtitle}>Preencha os dados para se cadastrar</p>

        <form onSubmit={handleRegister} style={styles.form}>
          <input
            type="text"
            placeholder="Nome completo"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            style={styles.input}
            required
          />

          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            style={styles.input}
            required
          />

          <input
            type="password"
            placeholder="Senha"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            style={styles.input}
            required
          />

          <button type="submit" style={styles.button}>
            Criar Conta
          </button>
        </form>

        <p
          style={{ textAlign: "center", marginTop: "15px", cursor: "pointer", color: "#1e3a8a" }}
          onClick={() => navigate("/")}
        >
          Já tenho conta
        </p>
      </div>
    </div>
  );
}

const styles = {
  container: {
    height: "100vh",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    background: "linear-gradient(135deg, #0a1a3a, #1e3a8a)",
  } as React.CSSProperties,

  card: {
    backgroundColor: "white",
    padding: "40px",
    borderRadius: "10px",
    width: "350px",
    boxShadow: "0 10px 25px rgba(0,0,0,0.2)",
  } as React.CSSProperties,

  title: {
    textAlign: "center",
    marginBottom: "10px",
    color: "#0a1a3a",
  } as React.CSSProperties,

  subtitle: {
    textAlign: "center",
    marginBottom: "30px",
    color: "#555",
    fontSize: "14px",
  } as React.CSSProperties,

  form: {
    display: "flex",
    flexDirection: "column",
    gap: "15px",
  } as React.CSSProperties,

  input: {
    padding: "10px",
    borderRadius: "5px",
    border: "1px solid #ccc",
    fontSize: "14px",
  } as React.CSSProperties,

  button: {
    padding: "10px",
    borderRadius: "5px",
    border: "none",
    backgroundColor: "#1e3a8a",
    color: "white",
    fontWeight: "bold",
    cursor: "pointer",
  } as React.CSSProperties,
};
