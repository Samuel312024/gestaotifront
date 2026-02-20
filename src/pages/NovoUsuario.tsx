import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { criarUsuario } from "../services/usuarioService";

export default function NovoUsuario() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    nome: "",
    email: "",
    senha: "",
    role: "User",
  });

  // Atualiza o estado quando o usuário digita algo
  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  }

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    await criarUsuario(form);
    navigate("/usuarios");
  }

  return (
    <form onSubmit={handleSubmit}>
      <input
        name="nome"
        placeholder="Nome"
        value={form.nome}
        onChange={handleChange}
      />
      <input
        name="email"
        placeholder="Email"
        value={form.email}
        onChange={handleChange}
      />
      <input
        name="senha"
        placeholder="Senha"
        type="password"
        value={form.senha}
        onChange={handleChange}
      />
      <select name="role" value={form.role} onChange={handleChange}>
        <option>User</option>
        <option>Tecnico</option>
        <option>Admin</option>
      </select>

      <button type="submit">Salvar</button>
    </form>
  );
}
