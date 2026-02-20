import { useState } from "react";

export default function CreateUser() {
  const [nome, setNome] = useState("");
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [role, setRole] = useState("User");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    await fetch("https://localhost:7092/api/auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`
      },
      body: JSON.stringify({
        nome,
        email,
        senha,
        role
      })
    });

    alert("Usuário criado com sucesso");
  };

  return (
    <div className="container">
      <h2>Criar Novo Usuário</h2>

      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Nome"
          value={nome}
          onChange={e => setNome(e.target.value)}
        />

        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={e => setEmail(e.target.value)}
        />

        <input
          type="password"
          placeholder="Senha"
          value={senha}
          onChange={e => setSenha(e.target.value)}
        />

        <select value={role} onChange={e => setRole(e.target.value)}>
          <option value="User">Usuário</option>
          <option value="Admin">Administrador</option>
        </select>

        <button type="submit">Criar</button>
      </form>
    </div>
  );
}
