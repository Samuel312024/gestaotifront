import { Link, useLocation, useNavigate } from "react-router-dom";

export default function Sidebar() {
  const location = useLocation();
  const navigate = useNavigate();

  const handleLogout = () => {
    // Remove o token
    localStorage.removeItem("token");

    // Opcional: limpar tudo
    // localStorage.clear();

    // Redireciona para login
    navigate("/login");
  };

  const menu = [
    { name: "Dashboard", path: "/dashboard" },
    { name: "Chamados", path: "/chamados" },
    { name: "Usuários", path: "/usuarios" }
  ];

  return (
    <div style={styles.sidebar}>
      <div>
        <h2 style={styles.logo}>Gestão TI</h2>

        <nav style={styles.nav}>
          {menu.map((item) => (
            <Link
              key={item.path}
              to={item.path}
              style={{
                ...styles.link,
                ...(location.pathname === item.path
                  ? styles.activeLink
                  : {})
              }}
            >
              {item.name}
            </Link>
          ))}
        </nav>
      </div>

      <button style={styles.logout} onClick={handleLogout}>
        Sair
      </button>
    </div>
  );
}


const styles = {
  sidebar: {
    width: "250px",
    background: "linear-gradient(180deg, #0f172a, #1e293b)",
    color: "white",
    display: "flex",
    flexDirection: "column" as const,
    justifyContent: "space-between",
    padding: "25px 20px"
  },
  logo: {
    marginBottom: "40px",
    fontSize: "20px",
    fontWeight: 600
  },
  nav: {
    display: "flex",
    flexDirection: "column" as const,
    gap: "12px"
  },
  link: {
    color: "#cbd5e1",
    textDecoration: "none",
    padding: "10px 15px",
    borderRadius: "10px",
    transition: "all 0.2s ease",
    fontWeight: 500
  },
  activeLink: {
    backgroundColor: "#334155",
    color: "white"
  },
  logout: {
    backgroundColor: "#ef4444",
    border: "none",
    padding: "10px",
    borderRadius: "10px",
    color: "white",
    cursor: "pointer",
    fontWeight: 500
  }
};
