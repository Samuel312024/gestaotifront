export default function Usuarios() {
  return (
    <div>
      <h1 style={{ marginBottom: "20px" }}>Usuários</h1>

      <button style={styles.button}>+ Novo Usuário</button>

      <table style={styles.table}>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Email</th>
            <th>Role</th>
            <th>Ações</th>
          </tr>
        </thead>

        <tbody>
          <tr>
            <td>João</td>
            <td>joao@email.com</td>
            <td>Admin</td>
            <td>
              <button style={styles.action}>Editar</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  );
}

const styles = {
  button: {
    marginBottom: "20px",
    padding: "10px 15px",
    borderRadius: "8px",
    border: "none",
    backgroundColor: "#1677ff",
    color: "white",
    cursor: "pointer"
  },
  table: {
    width: "100%",
    backgroundColor: "white",
    borderCollapse: "collapse" as const,
    borderRadius: "12px",
    overflow: "hidden"
  },
  action: {
    padding: "5px 10px",
    borderRadius: "6px",
    border: "none",
    backgroundColor: "#52c41a",
    color: "white",
    cursor: "pointer"
  }
};
