import { useEffect, useState } from "react";
import { listarUsuarios, editarUsuario, listarRoles, type FormUsuario, type Usuario } from "../services/usuarioService";

export default function Usuarios() {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [roles, setRoles] = useState<string[]>([]);
  const [editandoId, setEditandoId] = useState<number | null>(null);
  const [form, setForm] = useState<FormUsuario>({
    nome: "",
    email: "",
    senha: "",
    role: "User",
  });

  useEffect(() => {
    loadUsuarios();
    loadRoles();
  }, []);

  async function loadRoles() {
  const data = await listarRoles();
  setRoles(data);
}

  async function loadUsuarios() {
    try {
      const data = await listarUsuarios();
      setUsuarios(data);
    } catch (err) {
      console.error(err);
      alert("Erro ao carregar usuários");
    }
  }

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  }

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
  e.preventDefault();

 if (editandoId) {
  await editarUsuario(editandoId, {
    nome: form.nome,
    email: form.email,
    role: form.role,
    ativo: true
  });
}



  setForm({ nome: "", email: "", senha: "", role: "User" });
  loadUsuarios();
}


  function handleEditarUsuario(usuario: Usuario) {
  setForm({
    nome: usuario.nome,
    email: usuario.email,
    senha: "",
    role: usuario.role,
  });

  setEditandoId(usuario.id);
}


  return (
    <div style={{
        maxWidth: "1100px",
        margin: "0 auto",
        padding: "40px",
      }}>

      <h1>Usuários</h1>

      <form onSubmit={handleSubmit} style={{ marginBottom: "20px" }}>
        <input name="nome" placeholder="Nome" value={form.nome} onChange={handleChange} style={{ marginRight: "10px" }} />
        <input name="email" placeholder="Email" value={form.email} onChange={handleChange} style={{ marginRight: "10px" }} />
        <input name="senha" placeholder="Senha" type="password" value={form.senha} onChange={handleChange} style={{ marginRight: "10px" }} />
        <select
          name="role"
          value={form.role}
          onChange={handleChange}
        >
          {roles.map(r => (
            <option key={r} value={r}>{r}</option>
          ))}
        </select>

       <button type="submit">
        {editandoId ? "Salvar" : "Criar"}
       </button>

      </form>

      <table style={styles.table}>
        <thead>
          <tr>
            <th style={{ textAlign: "left", padding: "12px" }}>Nome</th>
            <th style={{ textAlign: "left", padding: "12px" }}>Email</th>
            <th style={{ textAlign: "left", padding: "12px" }}>Role</th>
            <th style={{ textAlign: "left", padding: "12px" }}>Ações</th>
          </tr>
        </thead>
        <tbody>
          {usuarios.map(u => (
            <tr key={u.id}>
              <td>{u.nome}</td>
              <td>{u.email}</td>
              <td>{u.role}</td>
              <td>
                <button style={styles.action} onClick={() => handleEditarUsuario(u)}>Editar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

const styles = {
  button: { padding: "10px 15px", borderRadius: "8px", border: "none", backgroundColor: "#1677ff", color: "white", cursor: "pointer" },
  table: {
  width: "100%",
  borderCollapse: "collapse" as const,
  backgroundColor: "#111",
  borderRadius: "12px",
  overflow: "hidden"
}
,
  action: { padding: "5px 10px", borderRadius: "6px", border: "none", backgroundColor: "#52c41a", color: "white", cursor: "pointer" },
};
