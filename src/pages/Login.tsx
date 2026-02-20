import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";


export default function Login() {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      navigate("/dashboard");
    }
  }, [navigate]);

  async function handleLogin(e: React.FormEvent) {
    e.preventDefault();

    try {
      const response = await fetch(
        "https://localhost:44367/api/auth/login",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ email, senha }),
        }
      );

      if (!response.ok) {
        throw new Error("Erro no login");
      }

      const data = await response.json();
      localStorage.setItem("token", data.token);

      navigate("/dashboard");
    } catch (error) {
      alert("Email ou senha inválidos");
    }
  }

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <h2 style={styles.title}>Gestão TI</h2>
        <p style={styles.subtitle}>Faça login para continuar</p>

        <form onSubmit={handleLogin} style={styles.form}>
          <input
            type="email"
            placeholder="Digite seu email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            style={styles.input}
            required
          />

          <input
            type="password"
            placeholder="Digite sua senha"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            style={styles.input}
            required
          />

          <button type="submit" style={styles.button}>
            Entrar
          </button>
        </form>
        <div style={{ textAlign: "center", marginTop: "15px" }}>
        <span
          style={{ cursor: "pointer", color: "#1e3a8a", display: "block", marginBottom: "6px" }}
          onClick={() => navigate("/forgot-password")}
        >
          Esqueci minha senha
        </span>

        <span
          style={{ cursor: "pointer", color: "#2563eb", fontWeight: "500" }}
          onClick={() => navigate("/register")}
        >
          Criar Cadastro
        </span>
      </div>

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
