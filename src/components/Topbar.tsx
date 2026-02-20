export default function Topbar() {
  const nome = localStorage.getItem("nome");

  return (
    <div style={styles.topbar}>
      <h3 style={styles.title}>Dashboard</h3>

      <div style={styles.user}>
        👤 {nome}
      </div>
    </div>
  );
}

const styles = {
  topbar: {
    backgroundColor: "white",
    padding: "18px 40px",
    boxShadow: "0 4px 12px rgba(0,0,0,0.04)",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center"
  },
  title: {
    margin: 0,
    fontSize: "18px",
    fontWeight: 600,
    color: "#334155"
  },
  user: {
    backgroundColor: "#f1f5f9",
    padding: "6px 14px",
    borderRadius: "20px",
    fontSize: "14px",
    color: "#475569"
  }
};
