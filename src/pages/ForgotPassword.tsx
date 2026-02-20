import { useState } from "react";
// import "./auth.css";
import "../styles/auth.css";


export default function ForgotPassword() {
  const [email, setEmail] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    await fetch("https://localhost:7092/api/auth/forgot-password", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email })
    });

    alert("Se o email existir, você receberá instruções.");
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h2>Recuperar Senha</h2>
        <p>Digite seu email para receber o link de redefinição.</p>

        <form onSubmit={handleSubmit}>
          <input
            type="email"
            placeholder="Seu email"
            value={email}
            onChange={e => setEmail(e.target.value)}
            required
          />

          <button type="submit">Enviar Link</button>
        </form>
      </div>
    </div>
  );
}
