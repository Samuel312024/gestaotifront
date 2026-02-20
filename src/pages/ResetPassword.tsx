import { useSearchParams } from "react-router-dom";
import { useState } from "react";

export default function ResetPassword() {
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token");

  const [senha, setSenha] = useState("");

  if (!token) {
    return <h2>Link inválido ou expirado</h2>;
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    await fetch("https://localhost:44367/api/auth/reset-password", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        token,
        novaSenha: senha,
      }),
    });

    alert("Senha alterada com sucesso!");
  }

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="password"
        placeholder="Nova senha"
        value={senha}
        onChange={(e) => setSenha(e.target.value)}
      />

      <button type="submit">Alterar senha</button>
    </form>
  );
}
